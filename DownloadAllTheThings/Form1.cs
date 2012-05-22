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
using System.Threading;

namespace DownloadAllTheThings {
	public partial class Form1 : Form {
		public DateTime time;
		public bool firstCheck;

		enum Errors { pageLoad = 1, enterInfo, login, unknown, findValue };

		public Form1() {
			InitializeComponent();
			passwordBox.PasswordChar = '*';
			firstCheck = true;

			usernameBox.Text = Properties.Settings.Default.username;
			passwordBox.Text = Properties.Settings.Default.password;

			createContextMenu();
		}

		// create the context menu for the notify icon
		public void createContextMenu() {
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
		private void changeAccount_Click(object sender, EventArgs e) {
			this.Show();
			WindowState = FormWindowState.Normal;
			downloadIcon.Visible = false;
		}

		// exit the program
		private void exitProgram_Click(object sender, EventArgs e) {
			Application.Exit();
		}

		private void saveButton_Click(object sender, EventArgs e) {
			// get the values from the text boxes
			Properties.Settings.Default.username = usernameBox.Text;
			Properties.Settings.Default.password = passwordBox.Text;

			// save them
			Properties.Settings.Default.Save();
		}

		private void Form1_Resize(object sender, EventArgs e) {
			if (FormWindowState.Minimized == this.WindowState) {

				// If no username and password entered make the user enter it
				if (Properties.Settings.Default.username.Equals("") ||
				   Properties.Settings.Default.password.Equals("")) {
					MessageBox.Show("Please enter and save your username and password.");
					this.Show();
					WindowState = FormWindowState.Normal;
					downloadIcon.Visible = false;
				} else {
					downloadIcon.Visible = true;
					this.Hide();

					// get the number once this is hidden
					Thread thread = new Thread(this.getDataFromComcast);
					thread.Start();
					time = DateTime.Now;
				}
			}
		}

		private void Form1_Load(object sender, EventArgs e) {

		}

		public void getDataFromComcast() {
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

			// we got an error
			if (usage.Length == 1) {
				firstCheck = false; // there won't be anything to display
				int errorNum = -1;
				try {
					errorNum = Convert.ToInt32(usage);
				} catch (Exception e) {
					Console.WriteLine("Exception: " + e.Message);
				}

				switch (errorNum) {
					case (int)Errors.pageLoad:
						MessageBox.Show("Unable to load comcast.com","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
					case (int)Errors.enterInfo:
						MessageBox.Show("Unable to enter credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
					case (int)Errors.login:
						MessageBox.Show("Unable to log you in. Please update your username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
					case (int)Errors.unknown:
						goto default;
					case (int)Errors.findValue:
						MessageBox.Show("Unable to determine your usage.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
					default:
						MessageBox.Show("An unknown error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						break;
				}
			} else {
				downloadIcon.BalloonTipText = usage;
			}

			proc.Close();

			if (firstCheck) {
				downloadIcon.ShowBalloonTip(5000);
				firstCheck = false;
			}
		}

		// When the tooltip is clicked show the usage
		private void downloadIcon_Click(object sender, MouseEventArgs e) {
			// Get the elasped time in hours to make sure we don't hit comcast too often
			TimeSpan elasped = DateTime.Now - time;
			time = DateTime.Now;
			if (elasped.Hours >= 1) {
				// get the updated data
				Thread thread = new Thread(this.getDataFromComcast);
				thread.Start();
			}

			downloadIcon.ShowBalloonTip(5000);
		}
	}
}
