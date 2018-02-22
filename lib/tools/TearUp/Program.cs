using System;
using System.IO;

namespace TearUp
{
    class Program
    {
        private const string SOLUTION_FILENAME = "P3D.sln";
        private const string OUTPUT_DIR = "p3d/";

        static void Main(string[] args)
        {
            Console.WriteLine("Starting tearup");

            // will move all files from lib/build to p3d/
            // will move the /lib/save folder to p3d/save

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

            var libFolder = Path.Combine(solutionFolder, "lib");
            var buildFolder = Path.Combine(libFolder, "build");
            var targetFolder = Path.Combine(solutionFolder, OUTPUT_DIR);

            if (Directory.Exists(buildFolder) && Directory.Exists(targetFolder))
            {
                Console.WriteLine($"Copy files from {buildFolder} to {targetFolder}.");
                var buildFolderUri = new Uri(buildFolder);
                foreach (var file in Directory.GetFiles(buildFolder, "*.*", SearchOption.AllDirectories))
                {
                    var fileUri = new Uri(file);
                    var relative = buildFolderUri.MakeRelativeUri(fileUri).ToString();
                    relative = relative.Remove(0, "build\\".Length);

                    var targetFile = Path.Combine(targetFolder, relative);
                    var targetDir = Path.GetDirectoryName(targetFile);

                    // create dir
                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                        Console.WriteLine($"Created directory at {targetDir}");
                    }

                    File.Copy(file, targetFile, true);
                    Console.WriteLine($"Copied to {targetFile}");
                }
            }
            else
            {
                Console.WriteLine($"Either the build folder at {buildFolder} or the target folder at {targetFolder} does not exist.");
            }

            // copy save folder
            var saveFolder = Path.Combine(libFolder, "save");
            if (Directory.Exists(saveFolder))
            {
                var targetDir = Path.Combine(targetFolder, "save");
                Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(saveFolder, targetDir, true);
            }

            Console.ReadLine();
        }
    }
}
