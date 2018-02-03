using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Base64Library;

namespace Base64GUI
{
    public partial class GUI : Form
    {
        private int modeIndex;
        private Base64FormattingOptions format;

        public GUI()
        {
            InitializeComponent();
            modeIndex = 0;
            comboBoxMode.SelectedIndex = modeIndex;
            openFileDialogSource.Filter = "All files (*.*)|*.*";
            comboBoxFormat.SelectedIndex = 0;
            format = Base64FormattingOptions.InsertLineBreaks;
        }

        private void buttonSource_Click(object sender, EventArgs e)
        {
            var result = openFileDialogSource.ShowDialog();

            if(result == DialogResult.OK)
            {
                textBoxSource.Text = openFileDialogSource.FileName;
                textBoxSource.SelectionStart = textBoxSource.TextLength;
            }
        }

        private void buttonOutput_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialogOutput.ShowDialog();

            if(result == DialogResult.OK)
            {
                textBoxOutput.Text = folderBrowserDialogOutput.SelectedPath;
                textBoxOutput.SelectionStart = textBoxOutput.TextLength;
            }
        }

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(modeIndex == comboBoxMode.SelectedIndex)
            {
                return;
            }

            switch(comboBoxMode.SelectedIndex)
            {
                case 0:
                    openFileDialogSource.Filter = "All files (*.*)|*.*";
                    break;
                case 1:
                    openFileDialogSource.Filter = "Base64 file (*.b64)|*.b64";
                    break;
            }

            openFileDialogSource.FileName = "";
            modeIndex = comboBoxMode.SelectedIndex;
            textBoxSource.Text = "";
            textBoxOutput.Text = "";
        }

        private void comboBoxFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBoxFormat.SelectedIndex)
            {
                case 0:
                    format = Base64FormattingOptions.InsertLineBreaks;
                    break;

                case 1:
                    format = Base64FormattingOptions.None;
                    break;
            }
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if(textBoxSource.TextLength == 0 || textBoxOutput.TextLength == 0)
            {
                MessageBox.Show("Incorrectly filled fields", "Error", MessageBoxButtons.OK , MessageBoxIcon.Error);
                return;
            }

            switch(modeIndex)
            {
                case 0:
                    lock(this)
                    {
                        byte[] fileBytes = File.ReadAllBytes(textBoxSource.Text);
                        string base64 = Base64Converter.Encode(fileBytes, format);

                        FileInfo fileInfo = new FileInfo(textBoxSource.Text);

                        string outputName = $"{fileInfo.Name}.b64";

                        File.WriteAllText($"{textBoxOutput.Text}{Path.DirectorySeparatorChar}{outputName}", base64);
                    }
                    MessageBox.Show("Completed");
                    break;

                case 1:
                    lock(this)
                    {
                        string base64 = File.ReadAllText(textBoxSource.Text);
                        byte[] fileBytes = Base64Converter.Decode(base64);
                        FileInfo fileInfo = new FileInfo(textBoxSource.Text);

                        string outputName = $"decode_{fileInfo.Name.Replace(".b64", "")}";

                        File.WriteAllBytes($"{textBoxOutput.Text}{Path.DirectorySeparatorChar}{outputName}", fileBytes);
                    }
                    MessageBox.Show("Completed");
                    break;
            }
        }
    }
}
