using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PornHub
{
    public partial class LoadingPage : Form
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        private void LoadingPage_Load(object sender, EventArgs e)
        {
            //Security.WebAuth.ApplyLoadingBranding(pictureBox1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            var delay = 60;
            Process[] Game = Process.GetProcessesByName("BlackOpsColdWar");
            if (Game.Length != 0)
            {
#if DEBUG
                delay = 0;
#endif
                if (Game[0].StartTime.AddSeconds(delay) < DateTime.Now)
                {
                    Form1 MainForm = new Form1();
                    MainForm.Show();
                    this.Close();
                }
            }
        }
    }
}