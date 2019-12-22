using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Data.SQLite;

namespace Estou_a_ver
{
    public class conDB
    {

        public static void InsertDB(List<hash> Lista)
        {
            string sql;
            bool verify;
            string bdName = "", sha256Name = "";
            //seleciona a base de dados
            SQLiteConnection mdbConnection = new SQLiteConnection("Data Source=filessha256.sqlite;Version=3;");

            //verifica se existe o ficheiro
            verify = File.Exists(@"C:\Users\Frias\Desktop\Estou a ver\Estou a ver\bin\Debug\netcoreapp3.1\filessha256.sqlite");

            if (verify == false)
            {
                SQLiteConnection.CreateFile("filessha256.sqlite");
                mdbConnection.Open();

                sql = " CREATE TABLE filessha256 ( \"ID\" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, \"nameFile\"	TEXT, \"sha256\" TEXT );";

                SQLiteCommand command = new SQLiteCommand(sql, mdbConnection);
                command.ExecuteNonQuery();

                mdbConnection.Close();
            }

            int num = Lista.Count;

            Console.WriteLine(num.ToString());

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
                    sql = "INSERT INTO filessha256 (nameFile, sha256) values ('" + Lista[i].nameFile + "', '" + Lista[i].hash256 + "') ";

                    SQLiteCommand command2 = new SQLiteCommand(sql, mdbConnection);

                    command2.ExecuteNonQuery();
                }
                mdbConnection.Close();
            }

        }

        public static bool CreatUser(string UserName, string hash, string salt)
        {
            string sql;


            string rdUser = "";
            SQLiteConnection mdbConnection = new SQLiteConnection("Data Source=filessha256.sqlite;Version=3;");
            sql = "select NameUser FROM User where User.NameUser = '" + UserName + "'";

            SQLiteCommand cmd = new SQLiteCommand(sql, mdbConnection);
            mdbConnection.Open();
            SQLiteDataReader rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                rdUser = (string)rd["NameUser"];
            }
            mdbConnection.Close();


            while (rdUser.Trim() == UserName.Trim())
            {
                Console.WriteLine("O utilizador com o username \"" + UserName + "\" já existe!");
                Console.WriteLine("Por favor introduza o novo username!");
                UserName = Console.ReadLine();
            }

            sql = "INSERT INTO User (NameUser, Salt, Rep) values ('" + UserName + "', '" + salt + "',  '" + hash + "') ";

            SQLiteCommand command2 = new SQLiteCommand(sql, mdbConnection);
            mdbConnection.Open();
            command2.ExecuteNonQuery();
            mdbConnection.Close();

            Console.WriteLine("Utilizador registado com sucesso");
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
            Funcoes obj = new Funcoes();
            passSHA = obj.GerarSha256(password);

            //Junta as duas string
            string final_sha_salt = passSHA + rdSalt;

            string autentication;
            //calcula o sha da pass com o salt
            Funcoes sha = new Funcoes();
            autentication = sha.GerarSha256(final_sha_salt);

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
                Console.WriteLine("Login com sucesso!");
                return true;
            }
            else
            {
                Console.WriteLine("Password errada!");
                return false;
            }
        }
    }
}