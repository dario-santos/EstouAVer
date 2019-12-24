using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace EstouAVer
{
    class HashCode
    {
        public static IDictionary<string, byte[]> Generate(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Console.WriteLine("The directory specified could not be found.");
                return null;
            }

            var dir = new DirectoryInfo(directory);
            var files = dir.GetFiles();
            IDictionary<string, byte[]> hashValues = new Dictionary<string, byte[]>();

            using (var mySHA256 = SHA256.Create())
            {
                foreach (var fInfo in files)
                {
                    try
                    {
                        var fileStream = fInfo.Open(FileMode.Open);
                        hashValues.Add(fInfo.Name, mySHA256.ComputeHash(fileStream));
                        fileStream.Close();
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine($"I/O Exception: {e.Message}");
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        Console.WriteLine($"Access Exception: {e.Message}");
                    }
                }
            }
            return hashValues;
        }
    }
}