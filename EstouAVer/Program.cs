using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace EstouAVer
{
    class Program
    {
        static void Main(string[] args)
        {

            //_______________________________________________________INSERIR HASH DOS FICHEIROS NO BASE DE DADOS_______________________________________________________//

            //List<hash> List = new List<hash>();

            //string directory = @"C:\Users\Frias\Documents\Estou a ver\FicheirosTxt";
            //var hashValues = HashCode.Generate(directory);

            //foreach (var hashValue in hashValues)
            //{
            //    hash obj = new hash();

            //    PrintElementOfDictionary(hashValue.Key, hashValue.Value);

            //    obj.nameFile = hashValue.Key;
            //    obj.hash256 = BitConverter.ToString(hashValue.Value).Replace("-", "");

            //    List.Add(obj);
            //}

            //conDB.InsertDB(List);

            ////_______________________________________________________INSERIR HASH DOS FICHEIROS NO BASE DE DADOS_______________________________________________________//

            ServiceBase.Run(new VerificationService());
            Inicio();
            Console.ReadLine();
        }

        public static void Inicio()
        {
            bool a;
            string opcao;
            int result_a;

            //verificação da introdução do user
            do
            {
                a = true;
                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("| Estou-a-Ver: um Monitor para Integridade para Diretorias |");
                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("\n1 - ENTRAR");
                Console.WriteLine("2 - REGISTAR");
                Console.WriteLine("0 - SAIR");
                opcao = Console.ReadLine();

                if (opcao != "1" && opcao != "2" && opcao != "0")
                {
                    Console.Clear();
                    Console.WriteLine("\nOPÇÃO INVÁLIDA. INTRODUZA A OPÇÃO CORRETA!");
                    a = false;
                }
            } while (a != true);

            //opções depois do user escolher no meu
            result_a = int.Parse(opcao);
            switch (result_a)
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
            bool value;
            do
            {
                Console.Clear();
                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("|                        Login                             |");
                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("\nIntroduza os dados solicitados!");
                Console.WriteLine("Username: ");
                username = Console.ReadLine();
                Console.WriteLine("Password: ");
                password = Console.ReadLine();

                value = conDB.Login(username, password);
            } while (value == false);

            Console.WriteLine("Bem vindo " +username +"!");
            userMenuLog(username);
        }

        public static void userMenuLog(string ursname)
        {
            bool b;
            string opcao;
            int result_b;

            //verificação da introdução do user
            do
            {
                b = true;
                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("|                         MENU                             |");
                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("\n1 - Ler Diretoria");
                Console.WriteLine("2 - Verificar Diretoria");
                Console.WriteLine("3 - Apagar Registos");
                Console.WriteLine("4 - Ajuda");
                Console.WriteLine("5 - Logout");
                Console.WriteLine("0 - SAIR");
                opcao = Console.ReadLine();

                if (opcao != "1" && opcao != "2" && opcao != "3" && opcao != "4" && opcao != "5" && opcao != "0")
                {
                    Console.Clear();
                    Console.WriteLine("\nOPÇÃO INVÁLIDA. INTRODUZA A OPÇÃO CORRETA!");
                    b = false;
                }
            } while (b != true);

            //opções depois do user escolher no meu
            result_b = int.Parse(opcao);
            switch (result_b)
            {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Programa terminado");
                    break;
                case 1:
                   // Dir.pedirDirectoria();
                    Console.WriteLine("Ler Diretoria");

                    Dir.pedirDirectoria();

                    userMenuLog(ursname);

                    break;
                case 2:
                    Console.WriteLine("Verificar Diretoria");
                    break;
                case 3:
                    Console.WriteLine("Apagar Registos");
                    break;
                case 4:
                    Console.Clear();
                    Funcoes.Ajuda();

                    userMenuLog(ursname);
                    break;
                case 5:
                    Console.WriteLine("5 - Logout");
                    break;
            }
        }

        //Registo do USER
        public static void Registo()
        {
            string userName;
            string password;
            string passSHA;
            string passwordSalt_SHA;
            string final_salt;
            byte[] salt;

            //Pedido ao user do nome e pass onde é verificada
            do
            {
                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("|                       Registo                            |");
                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("\nIntroduza os dados solicitados!");
                Console.WriteLine("Username: ");
                userName = Console.ReadLine();

                //verificação da passrword
                bool verifica;
                do
                {
                    verifica = true;
                    Console.WriteLine("Password:");
                    password = Console.ReadLine();

                    if (password.Length < 4)
                    {
                        Console.WriteLine("ERRO! A PASSWORD DEVE CONTER NO MÍNIMO 4 CARACTÉRES.");
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
                final_salt = BitConverter.ToString(salt).Replace("-", "");

                //Junta as duas string
                string final_sha_salt = passSHA + final_salt;

                //calcula o sha da pass com o salt
                Funcoes sha = new Funcoes();
                passwordSalt_SHA = sha.GerarSha256(final_sha_salt);

            }while(conDB.CreatUser(userName, passwordSalt_SHA, final_salt) == false);

            Login();
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