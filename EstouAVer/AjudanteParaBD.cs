using EstouAVer.Tables;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace EstouAVer
{
    public class AjudanteParaBD
    {
        //seleciona a base de dados
        private static string connectionString { get; }  = "Data Source=" + Directories.databaseFrias + ";Version=3;";

        public static readonly string BD_NAME = "EstouAVer.sqlite";

        // Table Names
        public static readonly string TABLE_USER      = "User";
        public static readonly string TABLE_DIRECTORY = "Directory";
        public static readonly string TABLE_FILE      = "File";
        public static readonly string TABLE_FILEHMAC = "FileHMAC";

        // Table USER - Columns
        public static readonly string USER_USERNAME = "username";
        public static readonly string USER_SALT     = "salt";
        public static readonly string USER_REP      = "rep";

        // Table DIRECTORY - Columns
        public static readonly string DIRECTORY_PATH          = "path";
        public static readonly string DIRECTORY_USERNAME      = "username";

        // Table FILESSHA256 - Columns
        public static readonly string FILE_PATH   = "path";
        public static readonly string FILE_SHA256 = "sha256";
        public static readonly string FILE_DIR = "dir";

        //Table FILESHMAC - Colums
        public static readonly string FILE_PATHHMAC = "path";
        public static readonly string FILE_HMAC = "hmac";
        public static readonly string FILE_DIRH = "dir";



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
            + DIRECTORY_PATH          + " TEXT PRIMARY KEY, "
            + DIRECTORY_USERNAME      + " TEXT "
            + ");";

        // FILESSHA256 table create statement
        private static readonly string CREATE_TABLE_FILE = "CREATE TABLE " + TABLE_FILE + "("
            + FILE_PATH + " TEXT PRIMARY KEY, "
            + FILE_SHA256 + " TEXT, "
            + FILE_DIR + " TEXT "
            + ");";

        //FileHmac table creat statement
        private static readonly string CREATE_TABLE_FILEHMAC = "CREAT TABLE " + TABLE_FILEHMAC + "("
            + FILE_PATHHMAC + " TEXT PRIMARY KEY, "
            + FILE_HMAC + "TEXT, "
            + FILE_DIRH + " TEXT, "
            + ");";

        public AjudanteParaBD() {}

        public static void OnCreate()
        {
            SQLiteConnection.CreateFile(Directories.databaseFrias);

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var c1 = new SQLiteCommand(CREATE_TABLE_USER, connection))
                {
                    c1.ExecuteNonQuery();
                }
                using (var c2 = new SQLiteCommand(CREATE_TABLE_DIRECTORY, connection))
                {
                    c2.ExecuteNonQuery();
                }
                using (var c3 = new SQLiteCommand(CREATE_TABLE_FILE, connection))
                {
                    c3.ExecuteNonQuery();
                }
                using (var c4 = new SQLiteCommand(CREATE_TABLE_FILEHMAC, connection))
                {
                    c4.ExecuteNonQuery();
                }
            }
        }

        public static User SelectUserWithUsername(string username)
        {
            using var connection = new SQLiteConnection(connectionString);
            string sql = "SELECT * FROM " + TABLE_USER + " WHERE " + USER_USERNAME + " = @UserName;";

            connection.Open();
            using (var selectSQL = new SQLiteCommand(sql, connection))
            {
                selectSQL.Parameters.AddWithValue("@UserName", username);

                using (var rd = selectSQL.ExecuteReader())
                {
                    try
                    {
                        return rd.Read() ? new User((string)rd[USER_USERNAME], (string)rd[USER_REP], (string)rd[USER_SALT]) : null;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }       
            }
        }

        public static int InsertUser(User user)
        {
            using var connection = new SQLiteConnection(connectionString);
            string sql = "INSERT INTO " + TABLE_USER + " ( " + USER_USERNAME + " , " + USER_REP + " , " + USER_SALT + " ) VALUES (@UserName, @Rep, @Salt)";

            connection.Open();
            using (var insertSQL = new SQLiteCommand(sql, connection))
            {
                insertSQL.Parameters.AddWithValue("@UserName", user.username);
                insertSQL.Parameters.AddWithValue("@Rep", user.rep);
                insertSQL.Parameters.AddWithValue("@Salt", user.salt);

                try
                {
                    return insertSQL.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }



        public static int InsertDirectory(Dir dir)
        {
            using var connection = new SQLiteConnection(connectionString);
            string sql = "INSERT INTO " + TABLE_DIRECTORY + " ( " + DIRECTORY_PATH + " , " + DIRECTORY_USERNAME + " ) VALUES (@Path, @UserName)";

            connection.Open();
            using (var insertSQL = new SQLiteCommand(sql, connection))
            {
                insertSQL.Parameters.AddWithValue("@Path", dir.path);
                insertSQL.Parameters.AddWithValue("@UserName", dir.username);

                try
                {
                    return insertSQL.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        public static List<Dir> SelectDirsWithUsername(string username)
        {
            using var connection = new SQLiteConnection(connectionString);
            string sql = "SELECT * FROM " + TABLE_DIRECTORY + " WHERE " + DIRECTORY_USERNAME + " = @UserName;";

            connection.Open();
            using (var selectSQL = new SQLiteCommand(sql, connection))
            {
                selectSQL.Parameters.AddWithValue("@UserName", username);

                using (var rd = selectSQL.ExecuteReader())
                {
                    try
                    {
                        var directories = new List<Dir>();
                        while (rd.Read())
                        {
                            directories.Add(new Dir((string)rd[DIRECTORY_PATH], (string)rd[DIRECTORY_USERNAME]));
                        }
                        return directories;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public static TFile SelectFileWithPath(string path)
        {
            using var connection = new SQLiteConnection(connectionString);
            string sql = "SELECT * FROM " + TABLE_FILE + " WHERE " + FILE_PATH + " = @Path;";

            connection.Open();
            using (var selectSQL = new SQLiteCommand(sql, connection))
            {
                selectSQL.Parameters.AddWithValue("@Path", path);

                using (var rd = selectSQL.ExecuteReader())
                {
                    try
                    {
                        return rd.Read() ? new TFile((string)rd[FILE_PATH], (string)rd[FILE_SHA256], (string)rd[FILE_DIR]) : null;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public static int InsertFile(TFile file)
        {
            using var connection = new SQLiteConnection(connectionString);
            string sql = "INSERT INTO " + TABLE_FILE + " ( " + FILE_PATH + " , " + FILE_SHA256 +  " , " + FILE_DIR + " ) VALUES (@Path, @Sha256, @Dir)";

            connection.Open();
            using (var insertSQL = new SQLiteCommand(sql, connection))
            {
                insertSQL.Parameters.AddWithValue("@Path", file.path);
                insertSQL.Parameters.AddWithValue("@Sha256", file.sha256);
                insertSQL.Parameters.AddWithValue("@Dir", file.dir);

                try
                {
                    return insertSQL.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        public static int InsertFileHMAC(FileHmac file)
        {
            using var connection = new SQLiteConnection(connectionString);
            string sql = "INSERT INTO " + TABLE_FILEHMAC + " ( " + FILE_PATHHMAC + " , " + FILE_HMAC + " , " + FILE_DIRH + " ) VALUES (@Path, @hamc, @Dir)";

            connection.Open();
            using (var insertSQL = new SQLiteCommand(sql, connection))
            {
                insertSQL.Parameters.AddWithValue("@Path", file.path);
                insertSQL.Parameters.AddWithValue("@hmac", file.hmac);
                insertSQL.Parameters.AddWithValue("@Dir", file.dir);

                try
                {
                    return insertSQL.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        public static int UpdateFile(TFile file)
        {
            using var connection = new SQLiteConnection(connectionString);

            string sql = "UPDATE " + TABLE_FILE + " SET " + FILE_SHA256 + " = @Sha256  WHERE " + FILE_PATH + " = @Path";

            connection.Open();
            using (var updateSQL = new SQLiteCommand(sql, connection))
            {
                updateSQL.Parameters.AddWithValue("@Path", file.path);
                updateSQL.Parameters.AddWithValue("@Sha256", file.sha256);

                try
                {
                    return updateSQL.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        public static Dir SelectDirWithPath(string path)
        {
            using var connection = new SQLiteConnection(connectionString);
            string sql = "SELECT * FROM " + TABLE_DIRECTORY + " WHERE " + DIRECTORY_PATH + " = @Path;";

            connection.Open();
            using (var selectSQL = new SQLiteCommand(sql, connection))
            {
                selectSQL.Parameters.AddWithValue("@Path", path);

                using (var rd = selectSQL.ExecuteReader())
                {
                    try
                    {
                        return rd.Read() ? new Dir((string)rd[DIRECTORY_PATH], (string)rd[DIRECTORY_USERNAME]) : null;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public static List<TFile> SelectFilesWithDir(string dir)
        {
            using var connection = new SQLiteConnection(connectionString);
            string sql = "SELECT * FROM " + TABLE_FILE + " WHERE " + FILE_DIR + " = @Dir;";

            connection.Open();
            using (var selectSQL = new SQLiteCommand(sql, connection))
            {
                selectSQL.Parameters.AddWithValue("@Dir", dir);

                using (var rd = selectSQL.ExecuteReader())
                {
                    try
                    {
                        var directories = new List<TFile>();
                        while (rd.Read())
                        {
                            directories.Add(new TFile((string)rd[FILE_PATH], (string)rd[FILE_SHA256], (string)rd[FILE_DIR]));
                        }
                        return directories;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public static int DeleteFile(TFile file)
        {
            using var connection = new SQLiteConnection(connectionString);

            string sql = "DELETE FROM " + TABLE_FILE + " WHERE " + FILE_PATH + " = @Path";

            connection.Open();
            using (var updateSQL = new SQLiteCommand(sql, connection))
            {
                updateSQL.Parameters.AddWithValue("@Path", file.path);

                try
                {
                    return updateSQL.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }
    }
}
