using System;
using EstouAVer.Tables;

namespace EstouAVer
{
    public static class DataBaseFunctions
    {
        public static string userLog;

        public static bool Register(User user)
        {
            //Console.Clear();

            if(AjudanteParaBD.SelectUserWithUsername(user.username) != null)
            {
                Console.WriteLine("\nErro! O username \"" + user.username + "\" nao esta disponivel!");
                Console.WriteLine("Por-favor introduza outro username!");

                return false;
            }

            if(AjudanteParaBD.InsertUser(user) != -1)
            {
                Console.WriteLine("Registo efetuado com sucesso!");
                return true;
            }

            Console.WriteLine("Registo efetuado sem sucesso!");
            return false;
        }

        public static bool Login(string username, string password)
        {
            User user = AjudanteParaBD.SelectUserWithUsername(username);

            Console.Clear();
            if (user == null)
            {
                Console.WriteLine("\nErro! O utilizador inserido nao existe.");
                return false;
            }

            //calcula o rep da palavra passe introduzida
            string insertedRep = SHA256Code.GenerateFromText(SHA256Code.GenerateFromText(password) + user.salt);

            if (user.rep.Equals(insertedRep))
            {
                Console.WriteLine("\nLogin efetuado com sucesso!\n");
                userLog = username;
                return true;
            }
            else
            {
                Console.WriteLine("\nDados invalidos! Tente novamente.\n");
                return false;
            }
        }

        public static void VerificarIntegridade(Dir dir)
        {
            TFile tfile = null;
            var files = SHA256Code.GenerateFromDir(dir.path);
            
            foreach (string file in files.Keys)
            {
                tfile = AjudanteParaBD.SelectFileWithPath(file);

                if(tfile == null)
                {
                    Console.WriteLine("Adicionado o ficheiro \'" + file + "\' a base de dados.");
                    AjudanteParaBD.InsertFile(new TFile(file, files[file]));
                }
                else
                {
                    if (!tfile.sha256.Equals(files[file]))
                    {
                        Console.WriteLine("O ficheiro \'" + file + "\' sofreu alteracoes.");
                        AjudanteParaBD.UpdateFile(new TFile(file, files[file]));
                    }
                }
            }
        }
    }
}