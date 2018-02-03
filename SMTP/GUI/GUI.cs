using SMTPLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private void buttonPort_Click(object sender, EventArgs e)
        {
            numericUpDownPort.Enabled = true;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if(textBoxSmtpUri.TextLength == 0)
            {
                Message("Field \"Smtp address\" cannot be empty.");
            }
            else if(textBoxUsername.TextLength == 0)
            {
                Message("Field \"Username\" cannot be empty.");
            }
            else if(textBoxPassword.TextLength == 0)
            {
                Message("Field \"Password\" cannot be empty.");
            }
            else if(textBoxFrom.TextLength == 0)
            {
                Message("Field \"From\" cannot be empty.");
            }
            else if(textBoxTo.TextLength == 0)
            {
                Message("Field \"To\" cannot be empty.");
            }
            else
            {
                var result = SMTP.SendMail(textBoxSmtpUri.Text, (int)numericUpDownPort.Value,
                    textBoxUsername.Text, textBoxPassword.Text, textBoxFrom.Text,
                    textBoxTo.Text, textBoxSubject.Text, richTextBoxMessage.Text);

                if(result.Success)
                {
                    Message($"Send status: Success");
                }
                else
                {
                    Message($"Send status: Failure\nMessage: {result.Message}");
                }
            }
        }

        private void Message(string message)
        {
            MessageBox.Show(message);
        }
    }
}
