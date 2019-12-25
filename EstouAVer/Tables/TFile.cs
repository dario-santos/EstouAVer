namespace EstouAVer.Tables
{
    public class TFile
    {   
        public string path { get; }

        public string sha256 { get; }

        public TFile(string path, string sha256)
        {
            this.path = path;
            this.sha256 = sha256;
        }
    }
}
