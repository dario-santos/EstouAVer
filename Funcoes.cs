using System;
using System.Security.Cryptography;
using System.Text;

namespace main
{
    class Funcoes
    {
        //Função para gerar SHA256 de uma string
        public string GerarSha256(string text)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //Função que gera o salt aleatório
        public byte[] GerarSalt()
        {
            Random number = new Random();
            byte[] salt = new byte[32];
            number.NextBytes(salt);
            return salt;
        }
    }
}
