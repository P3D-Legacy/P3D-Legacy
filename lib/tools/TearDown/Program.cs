using System;
using System.IO;
using System.Linq;

namespace TearDown
{
    class Program
    {
        private const string SOLUTION_FILENAME = "P3D.sln";
        private const string P3D_DIR = "p3d/";
        private const string PROJECT_HOOK = "    <Content Include=\"Pokemon3D.ico\" />";

        static void Main(string[] args)
        {
            Console.WriteLine("Starting teardown");

            // delete all binaries from /p3d
            // delete generated files/folders from /p3d:
            // - log.dat
            // - GameModes/
            // - ContentPacks/
            // move (copy and delete) the folder /p3d/save to /lib/save
            // check for all files in /p3d/content if they exist in the .vbproj file and add them, if they don't

            Console.WriteLine("Find solution folder...");

            // find solution folder
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            string solutionFolder = null;

            do
            {
                var files = Directory.GetFiles(dir);
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    if (fileName == SOLUTION_FILENAME)
                    {
                        solutionFolder = dir;
                    }
                }

                if (solutionFolder == null)
                {
                    // go one folder up and try again
                    dir = new DirectoryInfo(dir).Parent.FullName;
                }

            } while (solutionFolder == null);


            Console.WriteLine($"Found solution folder at {solutionFolder}");

            // delete all binaries from /p3d
            var p3dFolder = Path.Combine(solutionFolder, P3D_DIR);
            var binaries = GetFiles(p3dFolder, new[] { "*.exe", "*.dll", "*.xnb" }, SearchOption.AllDirectories);
            foreach (var file in binaries)
            {
                var normalized = file.Replace("/", "\\");
                // do not delete files in working /bin or /obj folders
                if (!normalized.Contains("\\bin\\") &&
                    !normalized.Contains("\\obj\\"))
                {
                    File.Delete(file);
                    Console.WriteLine($"Deleted binary at {file}");
                }
            }

            // delete generated files/folders from /p3d:
            // - log.dat
            // - GameModes/
            // - ContentPacks/
            var logFile = Path.Combine(p3dFolder, "log.dat");
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
                Console.WriteLine($"Deleted log file at {logFile}");
            }
            var gameModesFolder = Path.Combine(p3dFolder, "GameModes");
            if (Directory.Exists(gameModesFolder))
            {
                Directory.Delete(gameModesFolder, true);
                Console.WriteLine($"Deleted GameModes folder at {gameModesFolder}");
            }
            var contentPacksFolder = Path.Combine(p3dFolder, "ContentPacks");
            if (Directory.Exists(contentPacksFolder))
            {
                Directory.Delete(contentPacksFolder, true);
                Console.WriteLine($"Deleted ContentPacks folder at {contentPacksFolder}");
            }

            // move (copy and delete) the folder /p3d/save to /lib/save
            var saveFolder = Path.Combine(p3dFolder, "save");
            if (Directory.Exists(saveFolder))
            {
                var targetSaveFolder = Path.Combine(solutionFolder, "lib/save");

                Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(saveFolder, targetSaveFolder, true);
                Directory.Delete(saveFolder, true);

                Console.WriteLine($"Moved save folder to {targetSaveFolder}");
            }

            // check for all files in /p3d/content if they exist in the .vbproj file and add them, if they don't
            {
                var projFile = Path.Combine(p3dFolder, "p3d.vbproj");
                var contentFolder = Path.Combine(p3dFolder, "Content");
                var contentUri = new Uri(contentFolder);
                var files = GetFiles(contentFolder, new[] { "*.dat", "*.poke", "*.trainer", "*.png", "*.mp3", "*.wav" }, SearchOption.AllDirectories);

                var projContent = File.ReadAllText(projFile);
                var startIndex = projContent.IndexOf(PROJECT_HOOK);
                var changes = false;
                var checkedCount = 0;

                Console.WriteLine($"Checking {files.Length} content files for inclusion in .vbproj");

                foreach (var file in files)
                {
                    var fileUri = new Uri(file);
                    var relative = contentUri.MakeRelativeUri(fileUri).ToString();

                    var entry = GetProjectContentFileEntry(relative);
                    if (!projContent.Contains(entry))
                    {
                        Console.WriteLine($"Content file {relative} not found in project file.");
                        projContent = projContent.Insert(startIndex, entry + Environment.NewLine);
                        changes = true;
                    }

                    checkedCount++;
                    if (checkedCount % 500 == 0)
                    {
                        Console.WriteLine($"Done checking {checkedCount}...");
                    }
                }

                Console.WriteLine($"Done checking {checkedCount}...");

                if (changes)
                {
                    Console.WriteLine("Changes found in Content directory, applying to vbproj.");
                    File.WriteAllText(projFile, projContent);
                }
            }

            Console.WriteLine("Teardown complete");
            Console.ReadLine();
        }

        private static string GetProjectContentFileEntry(string contentFile)
        {
            contentFile = contentFile
                .Replace("/", "\\")
                .Replace("%20", " ");
            contentFile = Uri.EscapeDataString(contentFile)
                .Replace("%5C", "\\")
                .Replace("%20", " ");
            return
                $"    <Content Include=\"{contentFile}\">" + Environment.NewLine +
                $"      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>" + Environment.NewLine +
                $"    </Content>";
        }

        private static string[] GetFiles(string dir, string[] filters, SearchOption searchOption)
        {
            return filters.SelectMany(filter => Directory.GetFiles(dir, filter, searchOption)).ToArray();
        }
    }
}
