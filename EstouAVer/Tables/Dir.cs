namespace EstouAVer.Tables
{
    public class Dir
    {
        public int id { get; set; } = -1;
        
        public string nameDirectory { get; }
        
        public string path { get; }

        public string username { get; }

        public Dir(string nameDirectory, string path, string username)
        {
            this.nameDirectory = nameDirectory;
            this.path = path;
            this.username = username;
        }
    }
}
