using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace EstouAVer
{
    partial class VerificationService : ServiceBase
    {
        private readonly Timer timer;
        private string username;

        public VerificationService()
        {
            InitializeComponent();
            timer = new Timer(1000) { AutoReset = true };
            timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            string[] lines = new string[] {
                    "Big brother is watching you, " + username
                };

            File.AppendAllLines(Directories.servicePath, lines);
        }

        protected override void OnStart(string[] args)
        {
            username = args[0];
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
        }
    }
}
