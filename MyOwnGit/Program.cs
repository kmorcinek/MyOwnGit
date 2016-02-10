using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyOwnGit
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "gitcam")
            {
                var message = args[1];
                message = message.Replace("\"", "");

                // TODO what is current dir?
                // ignore MyOwnGit.exe
                var basePath = Environment.CurrentDirectory + "/../../TestFiles/";
                var gitPath = Path.Combine(basePath, ".git");
                var files = Directory.EnumerateFiles(basePath).ToList();

                var fileWithHashes = new List<FileWithHash>();

                foreach (var file in files)
                {
                    //var fileContent = File.ReadAllText(file);
                    var fileStream = File.OpenRead(file);
                    var hash = ComputeHash(fileStream);
                    var stringHash = BitConverter.ToString(hash);
                    var fileName = Path.GetFileName(file);
                    var fileWithHash = new FileWithHash()
                    {
                        FileName = fileName,
                        Path = file,
                        Hash = stringHash
                    };

                    fileWithHashes.Add(fileWithHash);
                }

                var commitContent = CalculateCommitContent(message, fileWithHashes);
                var objectsPath = Path.Combine(gitPath, "objects");

                WriteToObjects(fileWithHashes, objectsPath);
                WriteCommit(objectsPath, commitContent);
            }
        }

        private static byte[] ComputeHash(Stream fileStream)
        {
            return SHA256.Create().ComputeHash(fileStream);
        }

        private static void WriteCommit(string objectsPath, string commitContent)
        {
            StringReader stringReader = new StringReader(commitContent);
            var streamReader = new StreamReader(commitContent);
            Con
            ComputeHash(streamReader)

        }

        private static void WriteToObjects(IEnumerable<FileWithHash> fileWithHashes, string objectsPath)
        {
            foreach (var fileWithHash in fileWithHashes)
            {
                var hashedFilePath = Path.Combine(objectsPath, fileWithHash.Hash);
                File.Copy(fileWithHash.Path, hashedFilePath);
            }
        }

        private static string CalculateCommitContent(string message, IEnumerable<FileWithHash> fileWithHashes)
        {
            var list = new List<string> { message };
            foreach (var fileWithHash in fileWithHashes)
            {
                list.Add(fileWithHash.GetFileNameDelimiterHash());
            }

            return string.Join(Environment.NewLine, list);
        }
    }

    internal class FileWithHash
    {
        public string FileName { get; set; }
        public string Hash { get; set; }
        public string Path { get; set; }

        public string GetFileNameDelimiterHash()
        {
            return FileName + ";" + Hash;
        }
    }
}

// git init
// git commit -m "msg" // all files with checksum to objects, create commit from "list of hashes-names by alphabet, message, previous commit"
// git reset --hard
// git ls
// git reset --hard hash1
// git branch // 

// Intersting integration tests
// Dodac StyleCop