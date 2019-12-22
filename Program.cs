using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Estou_a_ver
{
    class Program
    {
        static void Main(string[] args)
        {

            //_______________________________________________________INSERIR HASH DOS FICHEIROS NO BASE DE DADOS_______________________________________________________//

            List<hash> List = new List<hash>();

            string directory = @"C:\Users\Frias\Desktop\Estou a ver\FicheirosTxt";
            var hashValues = HashCode.Generate(directory);

            foreach (var hashValue in hashValues)
            {
                hash obj = new hash();

                PrintElementOfDictionary(hashValue.Key, hashValue.Value);

                obj.nameFile = hashValue.Key;
                obj.hash256 = BitConverter.ToString(hashValue.Value).Replace("-", "");

                List.Add(obj);
            }

            conDB.InsertDB(List);

            //_______________________________________________________INSERIR HASH DOS FICHEIROS NO BASE DE DADOS_______________________________________________________//

            Inicio();
        }

        public static void Inicio()
        {
            Console.WriteLine("+----------------------------------------------------------+");
            Console.WriteLine("| Estou-a-Ver: um Monitor para Integridade para Diretorias |");
            Console.WriteLine("+----------------------------------------------------------+");

            Console.WriteLine("\n1 - ENTRAR");
            Console.WriteLine("2 - REGISTAR");
            Console.WriteLine("0 - SAIR");

            bool a;
            string opcao;
            int result;

            //verificação da introdução do user
            do
            {
                a = true;
                opcao = Console.ReadLine();

                if (opcao != "1" && opcao != "2" && opcao != "0")
                {
                    Console.WriteLine("\nOPÇÃO INVÁLIDA.INTRODUZA A OPÇÃO CORRETA!");
                    Console.WriteLine("1 - ENTRAR");
                    Console.WriteLine("2 - REGISTAR");
                    Console.WriteLine("0 - SAIR\n");
                    a = false;
                }
            } while (a != true);

            result = int.Parse(opcao);

            //opções depois do user escolher no meu
            switch (result)
            {
                case 0:
                    Console.WriteLine("Programa terminado");
                    break;
                case 1:
                    Login();
                    break;
                case 2:
                    Registo();
                    break;
            }
        }

        //Login do USER
        public static void Login()
        {
            string username, password;

            Console.Clear();

            Console.WriteLine("Username: "); username = Console.ReadLine();


            Console.WriteLine("Password: ");
            password = Console.ReadLine();

            conDB.Login(username, password);
        }



        //Registo do USER
        public static void Registo()
        {
            Console.WriteLine("+-----------------------------------------------+");
            Console.WriteLine("|                     Registo                   |");
            Console.WriteLine("+-----------------------------------------------+");
            Console.WriteLine("\nIntroduza os dados solicitados!");

            string userName;
            string password;
            string passSHA;
            byte[] salt;
            string passwordSalt_SHA;


            //Pedido ao user do nome e pass onde é verificada
            Console.WriteLine("Username: ");
            userName = Console.ReadLine();

            bool verifica;
            do
            {
                verifica = true;
                Console.WriteLine("Password:");
                password = Console.ReadLine();

                if (password.Length < 4 || password.Length > 16)
                {
                    Console.WriteLine("ERRO! A password deve conter entre 4 e 16 caractéres.");
                    verifica = false;
                }
            } while (verifica != true);

            //Calcula o SHA da password do user
            Funcoes obj = new Funcoes();
            passSHA = obj.GerarSha256(password);

            //Calcula o SALT[32]
            Funcoes gerarsalt = new Funcoes();
            salt = gerarsalt.GerarSalt();

            //converte o salt numa string
            string final_salt = BitConverter.ToString(salt).Replace("-", "");

            //Junta as duas string
            string final_sha_salt = passSHA + final_salt;

            //calcula o sha da pass com o salt
            Funcoes sha = new Funcoes();
            passwordSalt_SHA = sha.GerarSha256(final_sha_salt);

            Console.WriteLine(conDB.CreatUser(userName, passwordSalt_SHA, final_salt));
        }
        public static void PrintElementOfDictionary(string name, byte[] array)
        {

            Console.Write($"{name} : ");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write($"{array[i]:X2}");
                if ((i % 4) == 3) Console.Write(" ");
            }

            Console.WriteLine();
        }

    }
}

public class hash
{
    public string nameFile { get; set; }

    public string hash256 { get; set; }
}