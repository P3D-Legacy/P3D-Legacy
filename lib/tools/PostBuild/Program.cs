using System;
using System.IO;
using System.Linq;

namespace PostBuild
{
    class Program
    {
        private const string SOLUTION_FILENAME = "P3D.sln";
        private const string OUTPUT_DIR = "p3d/bin/Debug";

        static void Main(string[] args)
        {
            // find solution folder
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            string solutionFile = null;
            string solutionFolder = null;

            do
            {
                var files = Directory.GetFiles(dir);
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    if (fileName == SOLUTION_FILENAME)
                    {
                        solutionFile = file;
                        solutionFolder = dir;
                    }
                }

                if (solutionFile == null)
                {
                    // go one folder up and try again
                    dir = new DirectoryInfo(dir).Parent.FullName;
                }

            } while (solutionFile == null);

            var libFolder = Path.Combine(solutionFolder, "lib");
            var buildFolder = Path.Combine(libFolder, "build");

            // create lib/build
            if (!Directory.Exists(buildFolder))
            {
                Console.WriteLine($"Created build folder at {buildFolder}");
                Directory.CreateDirectory(buildFolder);
            }

            var binFolder = Path.Combine(solutionFolder, "p3d/bin/Debug");
            if (Directory.Exists(binFolder))
            {
                var copiedCount = 0;
                var binFolderUri = new Uri(binFolder);
                var binaries = GetFiles(binFolder, new[] { "*.exe", "*.dll", "*.xnb" }, SearchOption.AllDirectories);
                foreach (var binary in binaries)
                {
                    var binaryUri = new Uri(binary);
                    var relative = binFolderUri.MakeRelativeUri(binaryUri).ToString();
                    relative = relative.Remove(0, "Debug\\".Length);

                    var targetFile = Path.Combine(buildFolder, relative);
                    var targetDir = Path.GetDirectoryName(targetFile);

                    // create dir
                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    File.Copy(binary, targetFile, true);
                    copiedCount++;
                }

                Console.WriteLine($"Copied {copiedCount} files to the build directory.");
            }
        }

        private static string[] GetFiles(string dir, string[] filters, SearchOption searchOption)
        {
            return filters.SelectMany(filter => Directory.GetFiles(dir, filter, searchOption)).ToArray();
        }
    }
}
