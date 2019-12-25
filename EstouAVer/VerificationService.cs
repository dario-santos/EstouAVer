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


        public VerificationService()
        {
            InitializeComponent();
            timer = new Timer(1000) { AutoReset = true };
            timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            string[] lines = new string[] {
                    "Big brother is watching you, " + username + "," + password 
                };

            File.AppendAllLines(Directories.servicePath, lines);
        }

        protected override void OnStart(string[] args)
        {
            // User, Pass, Dir
            if (args.Length < 2)
                this.OnStop();

            username = args[0];
            password = args[1];

            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
        }
    }
}
