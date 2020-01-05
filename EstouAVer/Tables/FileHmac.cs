using System;
using System.Collections.Generic;
using System.Text;

namespace EstouAVer.Tables
{
    public class FileHmac
    {

        public string path { get; set; }
        public string hmac { get; set; }
        public string UserName { get; set; }

        public FileHmac()
        {

        }

        public FileHmac(string path, string hmac, string UserNAme)
        {
            this.path = path;
            this.hmac = hmac;
            this.UserName = UserNAme;
        }
    }
}
