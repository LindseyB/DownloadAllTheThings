using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace DownloadAllTheThings
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            passwordBox.PasswordChar = '*';

            usernameBox.Text = Properties.Settings.Default.username;
            passwordBox.Text = Properties.Settings.Default.password;

            createContextMenu();
        }

        // create the context menu for the notify icon
        public void createContextMenu()
        {
            ContextMenu downloadMenu = new ContextMenu();
            MenuItem changeAccount = new MenuItem("&Change Account");
            MenuItem exitProgram = new MenuItem("&Exit");

            downloadMenu.MenuItems.Add(changeAccount);
            downloadMenu.MenuItems.Add(exitProgram);

            changeAccount.Click += new EventHandler(this.changeAccount_Click);
            exitProgram.Click += new EventHandler(this.exitProgram_Click);

            downloadIcon.ContextMenu = downloadMenu;
        }

        // show and restore the form
        private void changeAccount_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        // exit the program
        private void exitProgram_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // get the values from the text boxes
            Properties.Settings.Default.username = usernameBox.Text;
            Properties.Settings.Default.password = passwordBox.Text;

            // save them
            Properties.Settings.Default.Save();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                downloadIcon.Visible = true;
                this.Hide();
                
                // get the number once this is hidden
                getDataFromComcast();
                downloadIcon.ShowBalloonTip(5000);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void getDataFromComcast()
        {
            // call the phantomjs script
            Process proc = new Process();
            ProcessStartInfo procStartInfo = new ProcessStartInfo("phantomjs.exe");
            procStartInfo.Arguments = "GetDataFromComcast.js " + Properties.Settings.Default.username + " " + Properties.Settings.Default.password;
            procStartInfo.UseShellExecute = false;
            procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.RedirectStandardOutput = true;
            proc.StartInfo = procStartInfo;
            proc.Start();


            // grab the output from the script
            StreamReader sr = proc.StandardOutput;
            string usage = sr.ReadLine();
            downloadIcon.BalloonTipText = usage;
            proc.Close();
        }

        // When the tooltip is clicked show the usage
        private void downloadIcon_Click(object sender, MouseEventArgs e)
        {
            downloadIcon.ShowBalloonTip(5000);
        }
    }
}
