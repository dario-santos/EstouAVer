using EstouAVer.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace EstouAVer
{
    partial class VerificationService : ServiceBase
    {
        private readonly Timer timer;
        private string username;
        private string password;
        private Dir dir;

        public VerificationService()
        {
            InitializeComponent();

            timer = new Timer(1000) { AutoReset = true };
            timer.Elapsed += VerifyDirectory;
        }

        private void VerifyDirectory(object sender, ElapsedEventArgs e)
        {
            // O ficheiro ------------- foi alterado em ------------
            string[] lines = VerificarIntegridade();
            File.AppendAllLines(Directories.servicePath, lines);
        }

        public string[] VerificarIntegridade()
        {
            var currentfiles = SHA256Code.GenerateFromDir(dir.path);
            var databaseFiles = AjudanteParaBD.SelectFilesWithDir(dir.path);

            var lines = new List<string>();

            foreach (TFile f in databaseFiles)
            {
                //Se foi eliminado
                if (!currentfiles.Keys.Contains(f.path))
                {
                    lines.Add(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " - Removido o ficheiro \'" + f.path + "\' da base de dados.");
                    AjudanteParaBD.DeleteFile(f);

                    continue;
                }

                // Se existir vamos ver se foi alterado
                if (!currentfiles[f.path].Equals(f.sha256))
                {
                    lines.Add(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")+ " - O ficheiro \'" + f.path + "\' sofreu alteracoes.");
                    AjudanteParaBD.UpdateFile(new TFile(f.path, currentfiles[f.path], dir.path));
                }

                currentfiles.Remove(f.path);
            }

            // Se ainda houver ficheiros, são os que foram adicionados
            foreach (string file in currentfiles.Keys)
            {
                lines.Add(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " - Adicionado o ficheiro \'" + file + "\' a base de dados.");
                AjudanteParaBD.InsertFile(new TFile(file, currentfiles[file], dir.path));
            }

            return lines.ToArray();
        }

        private bool Login(string username, string password)
        {
            User user = AjudanteParaBD.SelectUserWithUsername(username);
            string insertedRep = SHA256Code.GenerateFromText(SHA256Code.GenerateFromText(password) + user.salt);

            return user.rep.Equals(insertedRep);
        }

        protected override void OnStart(string[] args)
        {
            string directory = string.Empty;
            username = string.Empty;
            password = string.Empty;

            if (args.Length != 3)
            {
                OnStop();
                return;
            }
            username  = args[0];
            password  = args[1];
            directory = args[2];

            if (!Login(username, password))
            {
                OnStop();
                return;
            }
                
            var dirs = AjudanteParaBD.SelectDirsWithUsername(username);

            foreach(Dir d in dirs)
                if(d.path.Equals(directory))
                    dir = d;
            
            if(dir == null)
            {
                OnStop();
                return;
            }
            
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
        }
    }
}
