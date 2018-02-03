using FtpLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (textBoxFtpAddress.TextLength == 0)
            {
                MessageBox.Show("Field \"Ftp address\" cannot be empty.");
            }
            else if (textBoxUsername.TextLength == 0)
            {
                MessageBox.Show("Field \"Username\" cannot be empty.");
            }
            else if (textBoxPassword.TextLength == 0)
            {
                MessageBox.Show("Field \"Password\" cannot be empty.");
            }
            else
            {
                ResultWithFtpObject result = textBoxStartDir.TextLength != 0 ? 
                    FTP.ObjectsInPath(textBoxFtpAddress.Text, textBoxUsername.Text, textBoxPassword.Text, textBoxStartDir.Text) :
                    FTP.ObjectsInPath(textBoxFtpAddress.Text, textBoxUsername.Text, textBoxPassword.Text);

                if(result.Success)
                {
                    textBoxCurrentPath.Enabled = true;
                    listBoxObjects.Enabled = true;
                    buttonCDUP.Enabled = true;
                    buttonCWD.Enabled = true;

                    if(textBoxStartDir.TextLength != 0)
                    {
                        textBoxCurrentPath.Text = textBoxStartDir.Text;
                        if(textBoxStartDir.Text[textBoxStartDir.TextLength-1] != '/')
                        {
                            textBoxCurrentPath.Text += "/";
                        }
                    }
                    else
                    {
                        textBoxCurrentPath.Text = "/";
                    }

                    FillListBox(result.FtpObjectArray);
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            }
        }

        private void FillListBox(FtpObject[] structure)
        {
            List<string> directories = new List<string>();
            List<string> files = new List<string>();

            if(structure == null)
            {
                MessageBox.Show("Files structure is empty.");
                return;
            }

            for(int i = 0; i < structure.Length; i++)
            {
                if(structure[i].Type == "D")
                {
                    directories.Add($"{structure[i].Name} [D]");
                }
                else if(structure[i].Type == "F")
                {
                    files.Add($"{structure[i].Name} [F]");
                }
                else
                {
                    continue;
                }
            }

            directories.Sort();
            files.Sort();

            listBoxObjects.Items.Clear();

            listBoxObjects.Items.AddRange(directories.ToArray());
            listBoxObjects.Items.AddRange(files.ToArray());
        }

        private void buttonCWD_Click(object sender, EventArgs e)
        {
            if (listBoxObjects.SelectedItem != null)
            {
                lock (this)
                {
                    string selectedItem = listBoxObjects.SelectedItem.ToString();

                    if (selectedItem.Substring(selectedItem.Length - 3, 3) == "[D]")
                    {
                        string subpath = selectedItem.Remove(selectedItem.Length - 4);

                        var result = FTP.ObjectsInPath(textBoxFtpAddress.Text, textBoxUsername.Text, textBoxPassword.Text, $"{textBoxCurrentPath.Text}{subpath}");

                        if (result.Success)
                        {
                            FillListBox(result.FtpObjectArray);

                            textBoxCurrentPath.Text += $"{subpath}/";
                        }
                        else
                        {
                            MessageBox.Show(result.Message);
                        }
                    }
                }
            }
        }

        private void buttonCDUP_Click(object sender, EventArgs e)
        {
            lock(this)
            {
                string returnPath = Path.GetDirectoryName(Path.GetDirectoryName(textBoxCurrentPath.Text));

                if(returnPath == null)
                {
                    return;
                }

                returnPath = returnPath.Replace(@"\", "/");

                var result = FTP.ObjectsInPath(textBoxFtpAddress.Text, textBoxUsername.Text, textBoxPassword.Text, $"{returnPath}");

                if (result.Success)
                {
                    FillListBox(result.FtpObjectArray);

                    textBoxCurrentPath.Text = $"{returnPath}/";
                    textBoxCurrentPath.Text = textBoxCurrentPath.Text.Replace("//", "/");
                }
                else
                {
                    MessageBox.Show(result.Message);
                }
            }
        }
    }
}
