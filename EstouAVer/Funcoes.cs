using System;

namespace EstouAVer
{
    public class Funcoes
    {
        public static byte[] GenerateSalt()
        {
            Random number = new Random();
            byte[] salt = new byte[32];

            number.NextBytes(salt);
            return salt;
        }
    }
}