using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace EstouAVer
{
    class Dir
    {
        public static string pedirDirectoria()
        {
            string sql;
            string op = "";
            string p1 = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            do
            {
                Console.WriteLine("Deseja criar uma pasta na raiz do programa sim(s) não(n)");
                op = Console.ReadLine();


                if (op == "s")
                {

                    string pasta;

                    Console.WriteLine("p1 = " + p1);

                    Console.WriteLine("Qual o nome da pasta que recebe os ficheiros a verificar?");
                    pasta = Console.ReadLine();

                    while (Directory.Exists(pasta))
                    {
                        Console.WriteLine("Já existe uma pasta com esse nome, por favor dê outro nome!");
                        pasta = Console.ReadLine();
                    }

                    Directory.CreateDirectory(pasta);
                    Console.WriteLine("A sua pasta foi criada em: ");
                    Console.WriteLine(p1 + "\\" + pasta);


                    SQLiteConnection mdbConnection = new SQLiteConnection("Data Source=filessha256.sqlite;Version=3;");
                    sql = "INSERT INTO Directory (nameDirectory, path, NameUser) VALUES ('" + pasta + "','" + p1 + "\\" + pasta + "', '" + conDB.userLog + "')";

                    SQLiteCommand cmd = new SQLiteCommand(sql, mdbConnection);
                    mdbConnection.Open();
                    cmd.ExecuteNonQuery();
                    mdbConnection.Close();


                    return p1 + "\\" + pasta;
                }


                if (op == "n")
                {

                    Console.WriteLine("Introduza o cominho absoluto da pasta que você deseja verificar existente na sua maquina");
                    Console.WriteLine("Ex: C:/Utilizadores/Utilizador/Documentos/Ficheiros");
                    p1 = Console.ReadLine();
                    Console.ReadLine();

                    Console.WriteLine(p1);

                    while (!Directory.Exists(p1))
                    {

                        Console.WriteLine("Esse caminho não existe na sua maquina");


                        Console.WriteLine("Introduza o caminho absoluto da pasta que você deseja verificar");
                        p1 = Console.ReadLine();

                    }


                    Console.WriteLine(Path.GetDirectoryName(p1));

                    string[] words = p1.Split("\\");

                    int num = words.Length;

                    Console.WriteLine("Word: " + words[num-1]);



                    SQLiteConnection mdbConnection = new SQLiteConnection("Data Source=filessha256.sqlite;Version=3;");
                     sql = "INSERT INTO Directory (nameDirectory, path, NameUser) VALUES ('" + words[num-1] + "','" + p1 + "', '" + conDB.userLog + "')";

                    SQLiteCommand cmd = new SQLiteCommand(sql, mdbConnection);
                    mdbConnection.Open();
                    cmd.ExecuteNonQuery();
                    mdbConnection.Close();

                    return p1;

                }

            } while (op != "s" || op != "n");

            return "0";
        }
    }
}
