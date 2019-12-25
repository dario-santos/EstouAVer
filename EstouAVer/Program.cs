using EstouAVer.Tables;
using System;
using System.IO;
using System.ServiceProcess;

namespace EstouAVer
{
    class Program
    {
        private static void Main(string[] args)
        {
            StartService();
            
            CreateDataBase();

            FirstMenu();
        }

        private static void StartService() 
        {
            ServiceBase.Run(new VerificationService());
            Console.Clear();
        }
        
        private static void CreateDataBase()
        {
            if (!File.Exists(Directories.database))
                AjudanteParaBD.OnCreate();
        }

        private static void FirstMenu()
        {
            bool tmp;
            string opcao;

            //verificação da introdução do user
            do
            {
                tmp = true;

                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("| Estou-a-Ver: um Monitor para Integridade para Diretorias |");
                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("\n1 - Entrar");
                Console.WriteLine("2 - Registar");
                Console.WriteLine("0 - Sair");
                Console.Write("\nOpcao escolhida: ");
                opcao = Console.ReadLine();

                if (!opcao.Equals("1") && !opcao.Equals("2") && !opcao.Equals("0"))
                {
                    Console.Clear();
                    Console.WriteLine("\nOpcao invalida. Introduza uma das opcoes enunciadas!");
                    tmp = false;
                }
            } while(!tmp);

            //opções depois do user escolher no meu
            switch (int.Parse(opcao))
            {
                case 0:
                    Console.WriteLine("Programa terminado.");
                    break;
                case 1:
                    LoginMenu();
                    break;
                case 2:
                    RegistoMenu();
                    break;
            }
        }

        private static void LoginMenu()
        {
            string username, password;
            bool value;
            do
            {
                //Console.Clear();
                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("|                        Login                             |");
                Console.WriteLine("+----------------------------------------------------------+");
                
                Console.WriteLine("\nIntroduza os dados solicitados!");
                Console.Write("Username: ");
                username = Console.ReadLine();
                Console.Write("Password: ");
                password = Console.ReadLine();

                value = DataBaseFunctions.Login(username, password);
            } while (value == false);

            Console.WriteLine("Bem vindo " +username +"!");
            MainMenu(username);
        }

        private static void MainMenu(string ursname)
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
                Console.WriteLine("0 - Sair");
                Console.Write("\nOpcao escolhida: ");
                opcao = Console.ReadLine();

                if (opcao != "1" && opcao != "2" && opcao != "3" && opcao != "4" && opcao != "5" && opcao != "0")
                {
                    Console.Clear();
                    Console.WriteLine("\nOpcao invalida. Introduza uma das opcoes enunciadas!");
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

                    MainMenu(ursname);

                    break;
                case 2:
                    Console.WriteLine("Verificar Diretoria");
                    break;
                case 3:
                    Console.WriteLine("Apagar Registos");
                    break;
                case 4:
                    Console.Clear();
                    HelpMenu();

                    MainMenu(ursname);
                    break;
                case 5:
                    Console.WriteLine("5 - Logout");
                    break;
            }
        }

        private static void RegistoMenu()
        {
            string username = string.Empty;
            string password = string.Empty;
            string rep      = string.Empty;
            string salt     = string.Empty;

            User user = null;

            do
            {
                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("|                       Registo                            |");
                Console.WriteLine("+----------------------------------------------------------+");
                Console.WriteLine("\nIntroduza os dados solicitados!");
                Console.Write("Username:");

                username = Console.ReadLine();

                //verificação da password
                do
                {
                    Console.Write("Password:");
                    password = Console.ReadLine();

                    if (password.Length < 4)
                        Console.WriteLine("Erro! A password deve conter no minimo 4 caracteres.");
                    
                } while (password.Length < 4);

                //Calcula o SALT
                salt = BitConverter.ToString(Funcoes.GenerateSalt()).Replace("-", "");

                //calcula o REP
                rep = SHA256Code.GenerateFromText(SHA256Code.GenerateFromText(password) + salt);

                user = new User(username, rep, salt);
            } while(!DataBaseFunctions.Register(user));

            LoginMenu();
        }

        private static void HelpMenu()
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
            Console.WriteLine("");
            Console.WriteLine("");
            Console.Write("Pressione qualquer tecla para continuar...");
            Console.ReadLine();
        }

        private static void PrintElementOfDictionary(string name, byte[] array)
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

public class Hash
{
    public string nameFile { get; set; }

    public string hash256 { get; set; }
}



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
