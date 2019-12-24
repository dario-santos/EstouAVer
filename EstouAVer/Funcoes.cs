using System;
using System.Security.Cryptography;
using System.Text;

namespace EstouAVer
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

        public static void Ajuda()
        {
            Console.WriteLine("+----------------------------------------------------------+");
            Console.WriteLine("|                         AJUDA                            |");
            Console.WriteLine("+----------------------------------------------------------+");
            Console.WriteLine("*    1 - Ler Diretoria                                     *");
            Console.WriteLine("*        -> Pede ao utilizador para introduzir a diretoria *");
            Console.WriteLine("*        absoluta onde têm os documentos.                  *");
            Console.WriteLine("*        -> Pede ao utilizador para criar uma pasta numa   *");
            Console.WriteLine("*        diretoria definida pelo programa.                 *");
            Console.WriteLine("*        -> Mostra a diretoria onde foi criada a pasta e   *");
            Console.WriteLine("*        onde o utilizador têm que colocar os documentos.  *");
            Console.WriteLine("*    2 - Verificar Diretoria                               *");
            Console.WriteLine("*        -> Verifica se a diretoria especificada no po     *");
            Console.WriteLine("*    3 - Apagar Registos                                   *");
            Console.WriteLine("*        -> Apaga o registo na base de dados do utilizador *");
            Console.WriteLine("*        que estiver logado.                               *");
            Console.WriteLine("*    4 - Ajuda                                             *");
            Console.WriteLine("*        -> Lista as opções do programa.                   *");
            Console.WriteLine("*    5 - Logout                                            *");
            Console.WriteLine("*        -> Termina a sessão do utilizador logado.         *");
            Console.WriteLine("*    0 - SAIR                                              *");
            Console.WriteLine("*        -> Sai do programa.                               *");
            Console.WriteLine("+----------------------------------------------------------+");
        }
    }
}