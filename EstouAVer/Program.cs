using EstouAVer.Tables;
using System;
using System.Collections.Generic;
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
            string username = string.Empty; 
            string password = string.Empty;

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

            Console.WriteLine("Bem vindo " + username +"!");
            MainMenu(username);
        }

        private static void RegistoMenu()
        {
            string username = string.Empty;
            string password = string.Empty;
            string rep = string.Empty;
            string salt = string.Empty;

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
                salt = BitConverter.ToString(SHA256Code.GenerateSalt()).Replace("-", "");

                //calcula o REP
                rep = SHA256Code.GenerateFromText(SHA256Code.GenerateFromText(password) + salt);

                user = new User(username, rep, salt);
            } while (!DataBaseFunctions.Register(user));

            LoginMenu();
        }

        private static void MainMenu(string username)
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
                Console.WriteLine("");
                Console.WriteLine("1 - Ler Diretoria");
                Console.WriteLine("2 - Verificar Diretoria");
                Console.WriteLine("3 - Apagar Registos");
                Console.WriteLine("4 - Ajuda");
                Console.WriteLine("5 - Logout");
                Console.WriteLine("");
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
                    Console.WriteLine("Adeus!");
                    break;
                case 1:
                    ReceberDiretoria();
                    MainMenu(username);
                    break;
                case 2:
                    VerificarDiretoria();
                    MainMenu(username);
                    break;
                case 3:
                    Console.WriteLine("Apagar Registos");
                    break;
                case 4:
                    Console.Clear();
                    HelpMenu();
                    MainMenu(username);
                    break;
                case 5:
                    Console.WriteLine("5 - Logout");
                    break;
            }
        }

        private static string ReceberDiretoria()
        {
            Console.WriteLine("+----------------------------------------------------------+");
            Console.WriteLine("|               Ler Directoria                             |");
            Console.WriteLine("+----------------------------------------------------------+");

            string mainPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string sql = string.Empty;
            string option = string.Empty;
            string relativePath = string.Empty;

            do
            {
                Console.WriteLine("Deseja criar uma pasta na raiz do programa sim(S) não(N)");
                option = Console.ReadLine().ToLower();

                if (option.Equals("s"))
                {
                    Console.WriteLine("Qual será o nome da pasta que ira conter os ficheiros a verificar?");
                    relativePath = Console.ReadLine();

                    while (Directory.Exists(relativePath))
                    {
                        Console.WriteLine("Já existe uma pasta com esse nome, por favor dê outro nome!");
                        relativePath = Console.ReadLine();
                    }

                    Console.WriteLine("A sua pasta foi criada em: ");
                    Console.WriteLine(mainPath + "\\" + relativePath);

                    // Adicionar esta directoria à base de dados
                    Dir dir = new Dir(relativePath, mainPath + "\\" + relativePath, DataBaseFunctions.userLog);

                    if (AjudanteParaBD.InsertDirectory(dir) == -1)
                    {
                        Console.WriteLine("Erro ao criar a diretoria");
                        return string.Empty;
                    }

                    Directory.CreateDirectory(relativePath);
                    return mainPath + "\\" + relativePath;
                }

                else if (option.Equals("n"))
                {

                    Console.WriteLine("Introduza o cominho absoluto da diretoria que pretende verificar.");
                    Console.WriteLine("Ex.: C:/Caminho/Completo/Ate/A/Diretoria");
                    Console.Write("Introduzido:");

                    mainPath = Console.ReadLine();

                    while (!Directory.Exists(mainPath))
                    {
                        Console.WriteLine("O caminho introduzido nao existe na sua maquina");
                        Console.WriteLine("Introduza o caminho absoluto da pasta que você deseja verificar");
                        Console.Write("Introduzido:");
                        mainPath = Console.ReadLine();
                    }

                    string[] words = mainPath.Split("\\");

                    // Adicionar esta directoria à base de dados
                    Dir dir = new Dir(words[words.Length - 1], mainPath, DataBaseFunctions.userLog);

                    if (AjudanteParaBD.InsertDirectory(dir) == -1)
                    {
                        Console.WriteLine("Erro ao adicionar a diretoria a base de dados.");
                        return string.Empty;
                    }

                    return mainPath;
                }

            } while (true);
        }

        private static void VerificarDiretoria() 
        {
            // Mostrar lista de diretorias do utilizador
            Console.WriteLine("+----------------------------------------------------------+");
            Console.WriteLine("|               Ler Directoria                             |");
            Console.WriteLine("+----------------------------------------------------------+");

            List<Dir> directories = AjudanteParaBD.SelectDirsWithUsername(DataBaseFunctions.userLog);
            int index = 0;

            if (directories == null)
            {
                Console.WriteLine("Erro durante a leitura da base de dados.");
                return;
            }
            if (directories.Count == 0)
            {
                Console.WriteLine("Erro! Voce nao tem nenhuma diretoria adicionada.");
                return;
            }

            for (int i = 0 ; i < directories.Count ; i++)
                Console.WriteLine(i + " - " + directories[i].path);
            
            do
            {
                Console.Write("Diretoria a verificar: ");
                index = int.Parse(Console.ReadLine());

                if(index > directories.Count - 1 || index < 0)
                    Console.WriteLine("Erro! Escolha uma opcao valida.");

            } while(index > directories.Count - 1 || index < 0);

            Console.WriteLine(string.Empty);
            Console.WriteLine("Diretoria escolhida: " + directories[index]);

            // Chamar a função que trata do resto


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
