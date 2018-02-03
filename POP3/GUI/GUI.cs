using POP3Library;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace GUI
{
    public partial class GUI : Form
    {
        private static Thread thread;
        private static string serverName;
        private static int serverPort;
        private static string username;
        private static string password;
        private static int updateTime;

        private static List<ReceivedMail> lastReceivedMail;

        public GUI()
        {
            InitializeComponent();
            lastReceivedMail = new List<ReceivedMail>();
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                thread.Abort();
            }
            catch { }
        }

        private void buttonFileDialog_Click(object sender, EventArgs e)
        {
            ResetParameters();
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                XmlDocument xmlDoc = new XmlDocument();

                try
                {
                    xmlDoc.Load(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                var xmlDocElement = xmlDoc.DocumentElement;
                var xmlAllChild = xmlDocElement.ChildNodes;

                if (xmlAllChild.Count != 5)
                {
                    MessageBox.Show("Wrong file structure.");
                    buttonRun.Enabled = false;
                    buttonStop.Enabled = false;
                    return;
                }

                for (int i = 0; i < xmlAllChild.Count; i++)
                {
                    switch (xmlAllChild[i].Name)
                    {
                        case "Pop3Server":
                            serverName = xmlAllChild[i].InnerText;
                            break;
                        case "PortServer":
                            int.TryParse(xmlAllChild[i].InnerText, out serverPort);
                            break;
                        case "Username":
                            username = xmlAllChild[i].InnerText;
                            break;
                        case "Password":
                            password = xmlAllChild[i].InnerText;
                            break;
                        case "UpdateTimeMilliseconds":
                            int.TryParse(xmlAllChild[i].InnerText, out updateTime);
                            break;
                    }
                }

                if (serverName == default(string) && serverPort == default(int) &&
                    username == default(string) && password == default(string) &&
                    updateTime == default(int))
                {
                    MessageBox.Show("Wrong file structure.");
                    buttonRun.Enabled = false;
                    buttonStop.Enabled = false;
                }
                else
                {
                    buttonRun.Enabled = true;

                    textBoxPop3Server.Text = serverName;
                    textBoxPort.Text = serverPort.ToString();
                    textBoxUsername.Text = username;
                    textBoxPassword.Text = password;
                    textBoxUpdateTime.Text = updateTime.ToString();
                    textBoxConfigFile.Text = openFileDialog.FileName;
                    textBoxConfigFile.Select(textBoxConfigFile.TextLength, 0);
                }
            }
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            listBoxMessages.Items.Clear();
            textBoxStart.Clear();
            textBoxCurrent.Clear();
            lastReceivedMail.Clear();

            thread = new Thread(new ThreadStart(UpdateMailList));
            thread.Start();

            buttonStop.Enabled = true;
            buttonRun.Enabled = false;
        }

        private void UpdateMailList()
        {
            ResultDownloadMessages result;

            while (true)
            {
                if (lastReceivedMail.Count == 0)
                {
                    result = POP3.DownloadMessages(serverName, serverPort, username, password);

                    if (result.Success == true)
                    {
                        textBoxStart.Invoke(new Action(() => textBoxStart.Text = result.ReceivedMails.Length.ToString()));
                    }
                }
                else
                {
                    result = POP3.DownloadMessages(serverName, serverPort, username, password, lastReceivedMail.ToArray());
                }

                if(result.Success)
                {
                    lastReceivedMail.AddRange(result.ReceivedMails);

                    foreach(var mail in result.ReceivedMails)
                    {
                        listBoxMessages.Invoke((new Action(() => listBoxMessages.Items.Add(mail.ToString()))));
                    }

                    textBoxCurrent.Invoke(new Action(() => textBoxCurrent.Text = listBoxMessages.Items.Count.ToString()));

                    if (result.ReceivedMails.Length != 0)
                    {
                        MessageBox.Show("You have new message/messages.");
                    }
                }
                else
                {
                    MessageBox.Show(result.Message);
                }

                Thread.Sleep(updateTime);

                
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            thread.Abort();

            buttonStop.Enabled = false;
            buttonRun.Enabled = true;
        }

        private void ResetParameters()
        {
            serverName = default(string);
            serverPort = default(int);
            username = default(string);
            password = default(string);
            updateTime = default(int);

            textBoxPop3Server.Clear();
            textBoxPort.Clear();
            textBoxUsername.Clear();
            textBoxPassword.Clear();
            textBoxUpdateTime.Clear();
            textBoxConfigFile.Clear();
        }
    }
}