using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;

namespace EstouAVer
{
    public static class conDB
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
            verify = File.Exists(Directories.sqlPath);

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

        public static bool CreatUser(string userName, string hash, string salt)
        {
            string rdUser = string.Empty;

            var mdbConnection = new SQLiteConnection("Data Source=filessha256.sqlite;Version=3;");
            string sql = "select NameUser FROM User where User.NameUser = '" + userName + "'";

            SQLiteCommand cmd = new SQLiteCommand(sql, mdbConnection);
            mdbConnection.Open();
            SQLiteDataReader rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                rdUser = (string)rd["NameUser"];
            }
            mdbConnection.Close();

            if (rdUser.Trim() == userName.Trim())
            {
                Console.Clear();
                Console.WriteLine("\nERRO! O UTILIZADOR COM O USERNAME \"" + userName + "\" JÁ EXISTE!");
                Console.WriteLine("INTRODUZA UM NOVO USERNAME!");
                return false;
            }

            DataBase.InsertDB("INSERT INTO User (NameUser, Salt, Rep) values ('" + userName + "', '" + salt + "',  '" + hash + "')  ");

            Console.Clear();
            Console.WriteLine("REGISTO EFETUADO COM SUCESSO!");
            return true;
        }

        public static bool Login(string UserName, string password)
        {

            string sql;

            string rdSalt = "";
            SQLiteConnection mdbConnection = new SQLiteConnection("Data Source=filessha256.sqlite;Version=3;");
            sql = "select Salt FROM User where User.NameUser = '" + UserName + "'";

            SQLiteCommand cmd = new SQLiteCommand(sql, mdbConnection);
            mdbConnection.Open();
            SQLiteDataReader rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                rdSalt = (string)rd["Salt"];
            }

            mdbConnection.Close();

            string passSHA;
            //Calcula o SHA da password do user
            passSHA = HashCodeSHA256.GenerateFromText(password);

            //Junta as duas string
            string final_sha_salt = passSHA + rdSalt;

            string autentication;
            //calcula o sha da pass com o salt
            autentication = HashCodeSHA256.GenerateFromText(final_sha_salt);

            string rep = "";
            sql = "select Rep FROM User where User.NameUser = '" + UserName + "'";

            SQLiteCommand cmd2 = new SQLiteCommand(sql, mdbConnection);
            mdbConnection.Open();
            SQLiteDataReader rd1 = cmd2.ExecuteReader();

            while (rd1.Read())
            {
                rep = (string)rd1["Rep"];
            }

            mdbConnection.Close();

            if (rep == autentication)
            {
                Console.Clear();
                Console.WriteLine("\nLOGIN EFETUADO COM SUCESSO!\n");

                userLog = UserName;

                return true;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\nDADOS INVÁLIDOS! VOLTE A TENTAR.\n");
                return false;
            }
        }
    }
}