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

            getDataFromComcast();
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
                //downloadIcon.Text = "test tooltip text";
                //downloadIcon.BalloonTipText = "teeeeeeeeeeest";
                //downloadIcon.ShowBalloonTip(5000);
                this.Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void getDataFromComcast()
        {
            //<span id="ctl00_ctl00_ContentArea_PrimaryColumnContent_UsedWrapper" style="color:red;">251GB</span>
        }

    }
}
