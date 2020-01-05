using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace EstouAVer
{
    class HMac
    {
        public static IDictionary<string, string> hmac(string directory, string key)
        {

            if (!Directory.Exists(directory))
            {
                Console.WriteLine("A direcotria não foi encontrada");
                return null;
            }

            var dir = new DirectoryInfo(directory);
            var files = dir.GetFiles();

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            IDictionary<string, string> hashValues = new Dictionary<string, string>();

            using (var mySHA256 = SHA256.Create())
            {
                foreach (var fInfo in files)
                {
                    try
                    {
                        var fileStream = fInfo.Open(FileMode.Open);
                        var hashHAMC = hmacsha256.ComputeHash(fileStream);
                        var hash = mySHA256.ComputeHash(fileStream);

                        hashValues.Add(fInfo.FullName, BitConverter.ToString(hashHAMC).Replace("-", ""));
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


            //EXEMPLO PARA CHAMAR O FUNÇÃO PARA UMA PASTA 
            //var hash = HMac.hmac(@"C:\Users\Frias\Desktop\Trabalho de Grupo SI", "1234");

            //CASO QUEIRAM VER O RESULTADO
            //foreach (var x in hash)
            //{
            //    Console.WriteLine(x);
            //}

        }

    }
}
