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
