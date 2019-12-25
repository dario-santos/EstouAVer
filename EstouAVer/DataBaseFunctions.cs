using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using EstouAVer.Tables;

namespace EstouAVer
{
    public static class DataBaseFunctions
    {
        public static string userLog;

        public static void InsertDB(List<Hash> Lista)
        {
            string sql;
            bool verify;
            string bdName = "", sha256Name = "";
            //seleciona a base de dados
            SQLiteConnection mdbConnection = new SQLiteConnection("Data Source=filessha256.sqlite;Version=3;");

            //verifica se existe o ficheiro
            verify = File.Exists(Directories.database);

            if (verify == false)
            {
                SQLiteConnection.CreateFile("filessha256.sqlite");
                mdbConnection.Open();

                sql = " CREATE TABLE filessha256 ( \"ID\" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, \"nameFile\"	TEXT, \"sha256\" TEXT );";

                SQLiteCommand command = new SQLiteCommand(sql, mdbConnection);
                command.ExecuteNonQuery();

                mdbConnection.Close();
                mdbConnection.Dispose();
                command.Dispose();
            }

            //  Console.WriteLine(Lista[0].nameFile + " " + Lista[0].hash256);
            for (int i = 0; i < Lista.Count; i++)
            {
                string sql3 = "select nameFile, sha256 FROM filessha256 WHERE nameFile = '" + Lista[i].nameFile + "' AND sha256 = '" + Lista[i].hash256 + "' ";
                mdbConnection.Open();
                SQLiteCommand cmd = new SQLiteCommand(sql3, mdbConnection);

                SQLiteDataReader rd = cmd.ExecuteReader();


                while (rd.Read())
                {
                    bdName = (string)rd["nameFile"];

                    sha256Name = (string)rd["sha256"];

                    Console.WriteLine("|" + bdName + "|" + "  " + "|" + sha256Name + "|");

                }

                if (bdName != Lista[i].nameFile || sha256Name != Lista[i].hash256)
                {

                    DataBase.InsertDB("INSERT INTO filessha256 (nameFile, sha256) values ('" + Lista[i].nameFile + "', '" + Lista[i].hash256 + "') ");
                
                }

                mdbConnection.Close();
                mdbConnection.Dispose();
            }

        }


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
    }
}