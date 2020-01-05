using System;

namespace EstouAVer
{
    class Directories
    {
        public static readonly string database = AppDomain.CurrentDomain.BaseDirectory    + "\\EstouAVer2.sqlite";
        public static readonly string databaseAES = AppDomain.CurrentDomain.BaseDirectory + "\\EstouAVer2.aes";
        public static readonly string servicePath = AppDomain.CurrentDomain.BaseDirectory + "\\EstouAVerlog.txt";
    }
}
