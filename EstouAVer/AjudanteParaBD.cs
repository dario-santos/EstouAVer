using System.Data.SQLite;

namespace EstouAVer
{
    class AjudanteParaBD
    {
        //seleciona a base de dados
        private static SQLiteConnection mdbConnection = new SQLiteConnection("Data Source=" + Directories.database + ";Version=3;");

        internal static readonly string BD_NAME = "EstouAVer.sqlite";

        // Table Names
        internal static readonly string TABLE_USER           = "User";
        internal static readonly string TABLE_DIRECTORY      = "Directory";
        internal static readonly string TABLE_FILESSHA256    = "filessha256";

        // Table USER - Columns
        internal static readonly string USER_NAMEUSER = "NameUser";
        internal static readonly string USER_SALT     = "Salt";
        internal static readonly string USER_REP      = "Rep";

        // Table DIRECTORY - Columns
        internal static readonly string DIRECTORY_ID            = "ID";
        internal static readonly string DIRECTORY_NAMEDIRECTORY = "nameDirectory";
        internal static readonly string DIRECTORY_PATH          = "path";
        internal static readonly string DIRECTORY_NAMEUSER      = "NameUser";

        // Table FILESSHA256 - Columns
        internal static readonly string FILESSHA256_ID       = "ID";
        internal static readonly string FILESSHA256_NAMEFILE = "nameFile";
        internal static readonly string FILESSHA256_SHA256   = "sha256";

        // Table Create Statements
        // USER table create statement
        private static readonly string CREATE_TABLE_USER = "CREATE TABLE " + TABLE_USER + "("
            + USER_NAMEUSER + " TEXT NOT NULL,"
            + USER_SALT     + " TEXT,"
            + USER_REP      + " TEXT,"
            + "PRIMARY KEY(" + USER_NAMEUSER + ")"
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

        private AjudanteParaBD() {}

        internal static void OnCreate()
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


    }
}
