using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.CodeAnalysis;

namespace P3D.SourceGenerator
{
    [Generator(LanguageNames.VisualBasic)]
    public class MetaGenerator : ISourceGenerator
    {
        private const string FileValidationFileName = "FileValidation.vb";

        private readonly string[] _excludedFolder = { "bin", "obj", "Effects", "GUI", "Items", "Localization", "SkyDomeResource", "Songs", "Sounds", "Textures" };
        private readonly string[] _includedExtensions = { ".dat", ".poke", ".lua", ".trainer" };

        private MD5 _md5;

        public void Initialize(GeneratorInitializationContext context)
        {
            _md5 = MD5.Create();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var fileValidationFilePath = context.Compilation.SyntaxTrees.First(a => a.FilePath.EndsWith(FileValidationFileName)).FilePath;
            var fileValidationDirectory = Path.GetDirectoryName(Path.Combine(fileValidationFilePath)) ?? string.Empty;
            var projectDirectory = new DirectoryInfo(Path.Combine(fileValidationDirectory, ".."));
            var contentDirectory = new DirectoryInfo(Path.Combine(fileValidationDirectory, "..", "Content"));
            var metaFilePath = Path.Combine(projectDirectory.FullName, "meta");
            var fileValidationHashPath = Path.Combine(fileValidationDirectory, "FileValidationHash.vb");

            var filesToCompute = new List<FileInfo>();
            var metaFileOutput = new StringBuilder();
            var measuredSize = 0L;

            foreach (var fileSystemEntry in Directory.EnumerateFiles(contentDirectory.FullName, "*.*", SearchOption.AllDirectories))
            {
                if (_excludedFolder.Any(folderName => fileSystemEntry.Contains(Path.Combine("Content", folderName)))) continue;
                if (!_includedExtensions.Contains(Path.GetExtension(fileSystemEntry))) continue;
                
                filesToCompute.Add(new FileInfo(fileSystemEntry));
            }

            foreach (var file in filesToCompute.OrderBy(info => info.FullName))
            {
                var fileSize = file.Length;
                measuredSize += fileSize;

                metaFileOutput.AppendFormat(metaFileOutput.Length == 0 ? "{0}:{1}" : ",{0}:{1}", file.FullName.Remove(0, projectDirectory.FullName.Length + 1), ComputeMd5Hash(file.OpenRead()));
            }

            File.WriteAllText(metaFilePath, metaFileOutput.ToString());

            var metaHash = ComputeMd5Hash(File.OpenRead(metaFilePath));
            var metaResult = Convert.ToBase64String(Encoding.UTF8.GetBytes(metaHash));
            
            File.WriteAllText(fileValidationHashPath, $@"
Namespace Security

    Public MustInherit Class FileValidationHash
        '
        ' Dev notes:
        ' This would be auto generated via P3D.SourceGenerator.
        ' The fields are here so that the error would go away and also act as a fall back if generation fails.
        ' Actual code will be read from FileValidation.Generated.vb which generates a FileValidation class with
        ' the same details as here.
        '
        Protected Shared EXPECTEDSIZE As Integer = {measuredSize}
        Protected Shared METAHASH As String = ""{metaResult}""

#Region ""Generated File Hashes""
        ' {string.Join($"{Environment.NewLine}        ' ", metaFileOutput.ToString().Split(','))}
#End Region
    End Class

End Namespace
");
            
            context.AddSource("FileValidation.Generated.vb", $@"
Namespace Security

    Public Partial Class FileValidation
        Protected Shadows Shared EXPECTEDSIZE As Integer = {measuredSize}
        Protected Shadows Shared METAHASH As String = ""{metaResult}""
    End Class

End Namespace
");
            
            _md5.Dispose();
        }

        private string ComputeMd5Hash(Stream stream)
        {
            var hash = _md5.ComputeHash(stream);
            var result = new StringBuilder(hash.Length * 2);

            foreach (var b in hash)
            {
                result.Append(b.ToString("X2"));
            }

            return result.ToString();
        }
    }
}