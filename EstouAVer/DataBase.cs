using System.Data.SQLite;

namespace EstouAVer
{
    class DataBase
    {
        //seleciona a base de dados
        private static SQLiteConnection mdbConnection = new SQLiteConnection("Data Source=filessha256.sqlite;Version=3;");

        public static void InsertDB(string query)
        {
            using (SQLiteCommand command = mdbConnection.CreateCommand())
            {
                mdbConnection.Open();
                command.CommandText = query;
                command.ExecuteNonQuery();
                mdbConnection.Close();
            }
        }

        public static SQLiteDataReader SelectDB(string query)
        {
            using (SQLiteCommand command = mdbConnection.CreateCommand())
            {
                mdbConnection.Open();
                
                command.CommandText = query;
                return command.ExecuteReader();
            }
        }
    }
}

