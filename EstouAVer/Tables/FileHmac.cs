using System;
using System.Collections.Generic;
using System.Text;

namespace EstouAVer.Tables
{
    public class FileHmac
    {

        public string path { get; }
        public string hmac { get; }
        public string dir { get; }

        public string UserName { get; }

        public FileHmac(string path, string hmac, string dir, string UserNAme)
        {
            this.path = path;
            this.hmac = hmac;
            this.dir = dir;
            this.UserName = UserNAme;
        }
    }
}
