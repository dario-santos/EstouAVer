using EstouAVer.Tables;
using System;
using System.Data.SQLite;

namespace EstouAVer
{
    public class AjudanteParaBD
    {
        //seleciona a base de dados
        public static SQLiteConnection mdbConnection { get; }  = new SQLiteConnection("Data Source=" + Directories.database + ";Version=3;");

        public static readonly string BD_NAME = "EstouAVer.sqlite";

        // Table Names
        public static readonly string TABLE_USER           = "User";
        public static readonly string TABLE_DIRECTORY      = "Directory";
        public static readonly string TABLE_FILESSHA256    = "filessha256";

        // Table USER - Columns
        public static readonly string USER_USERNAME = "UserName";
        public static readonly string USER_SALT     = "Salt";
        public static readonly string USER_REP      = "Rep";

        // Table DIRECTORY - Columns
        public static readonly string DIRECTORY_ID            = "ID";
        public static readonly string DIRECTORY_NAMEDIRECTORY = "nameDirectory";
        public static readonly string DIRECTORY_PATH          = "path";
        public static readonly string DIRECTORY_NAMEUSER      = "NameUser";

        // Table FILESSHA256 - Columns
        public static readonly string FILESSHA256_ID       = "ID";
        public static readonly string FILESSHA256_NAMEFILE = "nameFile";
        public static readonly string FILESSHA256_SHA256   = "sha256";

        // Table Create Statements
        // USER table create statement
        private static readonly string CREATE_TABLE_USER = "CREATE TABLE " + TABLE_USER + "("
            + USER_USERNAME + " TEXT NOT NULL,"
            + USER_SALT     + " TEXT,"
            + USER_REP      + " TEXT,"
            + "PRIMARY KEY(" + USER_USERNAME + ")"
            + ");";

        // DIRECTORY table create statement
        private static readonly string CREATE_TABLE_DIRECTORY = "CREATE TABLE " + TABLE_DIRECTORY + "("
            + DIRECTORY_ID            + " INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, "
            + DIRECTORY_NAMEDIRECTORY + " TEXT, "
            + DIRECTORY_PATH          + " TEXT, "
            + DIRECTORY_NAMEUSER      + " TEXT "
            + ");";

        // FILESSHA256 table create statement
        private static readonly string CREATE_TABLE_FILESSHA256 = "CREATE TABLE " + TABLE_FILESSHA256 + "("
            + FILESSHA256_ID        + " INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, "
            + FILESSHA256_NAMEFILE  + " TEXT, "
            + FILESSHA256_SHA256    + " TEXT "
            + ");";

        public AjudanteParaBD() {}

        public static void OnCreate()
        {
            SQLiteConnection.CreateFile(Directories.database);

            using SQLiteCommand command = mdbConnection.CreateCommand();
            mdbConnection.Open();

            command.CommandText = CREATE_TABLE_USER;
            command.ExecuteNonQuery();

            command.CommandText = CREATE_TABLE_DIRECTORY;
            command.ExecuteNonQuery();

            command.CommandText = CREATE_TABLE_FILESSHA256;
            command.ExecuteNonQuery();
            
            mdbConnection.Close();            
        }

        public static User SelectUserWithUsername(string username)
        {
            string sql = "SELECT * FROM " + TABLE_USER + " WHERE " + USER_USERNAME + " = @UserName;";

            SQLiteCommand selectSQL = new SQLiteCommand(sql, mdbConnection);
            selectSQL.Parameters.AddWithValue("@UserName", username);

            try
            {
                mdbConnection.Open();

                var rd = selectSQL.ExecuteReader();
                User u = rd.Read() ? new User((string)rd[USER_USERNAME], (string)rd[USER_REP], (string)rd[USER_SALT]) : null;

                mdbConnection.Close();
                return u;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static int InsertUser(User user)
        {
            string sql = "INSERT INTO " + TABLE_USER + " (" + USER_USERNAME + " , " + USER_REP + " , " + USER_SALT + " ) VALUES (@UserName, @Rep, @Salt)";
            
            using var insertSQL = new SQLiteCommand(sql, mdbConnection);
            
            insertSQL.Parameters.AddWithValue("@UserName", user.username);
            insertSQL.Parameters.AddWithValue("@Rep", user.rep);
            insertSQL.Parameters.AddWithValue("@Salt", user.salt);

            int id = -1;

            try
            {
                mdbConnection.Open();
                id = insertSQL.ExecuteNonQuery();
            }
            catch {}
            
            mdbConnection.Close();
            return id;
        }
    }
}
