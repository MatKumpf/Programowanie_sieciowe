namespace GUI
{
    partial class GUI
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxFtpAddress = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxStartDir = new System.Windows.Forms.TextBox();
            this.listBoxObjects = new System.Windows.Forms.ListBox();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.textBoxCurrentPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCDUP = new System.Windows.Forms.Button();
            this.buttonCWD = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "FTP address:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Password:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Start dir:";
            // 
            // textBoxFtpAddress
            // 
            this.textBoxFtpAddress.Location = new System.Drawing.Point(118, 13);
            this.textBoxFtpAddress.Name = "textBoxFtpAddress";
            this.textBoxFtpAddress.Size = new System.Drawing.Size(190, 20);
            this.textBoxFtpAddress.TabIndex = 1;
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(118, 42);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(190, 20);
            this.textBoxUsername.TabIndex = 2;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(118, 74);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(190, 20);
            this.textBoxPassword.TabIndex = 3;
            // 
            // textBoxStartDir
            // 
            this.textBoxStartDir.Location = new System.Drawing.Point(118, 106);
            this.textBoxStartDir.Name = "textBoxStartDir";
            this.textBoxStartDir.Size = new System.Drawing.Size(190, 20);
            this.textBoxStartDir.TabIndex = 4;
            // 
            // listBoxObjects
            // 
            this.listBoxObjects.Enabled = false;
            this.listBoxObjects.FormattingEnabled = true;
            this.listBoxObjects.HorizontalScrollbar = true;
            this.listBoxObjects.Location = new System.Drawing.Point(16, 219);
            this.listBoxObjects.Name = "listBoxObjects";
            this.listBoxObjects.Size = new System.Drawing.Size(222, 121);
            this.listBoxObjects.TabIndex = 4;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(126, 143);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(132, 23);
            this.buttonLoad.TabIndex = 5;
            this.buttonLoad.Text = "Load directory structure";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // textBoxCurrentPath
            // 
            this.textBoxCurrentPath.Location = new System.Drawing.Point(84, 193);
            this.textBoxCurrentPath.Name = "textBoxCurrentPath";
            this.textBoxCurrentPath.ReadOnly = true;
            this.textBoxCurrentPath.Size = new System.Drawing.Size(224, 20);
            this.textBoxCurrentPath.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 196);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Current path";
            // 
            // buttonCDUP
            // 
            this.buttonCDUP.Enabled = false;
            this.buttonCDUP.Location = new System.Drawing.Point(244, 243);
            this.buttonCDUP.Name = "buttonCDUP";
            this.buttonCDUP.Size = new System.Drawing.Size(75, 23);
            this.buttonCDUP.TabIndex = 7;
            this.buttonCDUP.Text = "Return up";
            this.buttonCDUP.UseVisualStyleBackColor = true;
            this.buttonCDUP.Click += new System.EventHandler(this.buttonCDUP_Click);
            // 
            // buttonCWD
            // 
            this.buttonCWD.Enabled = false;
            this.buttonCWD.Location = new System.Drawing.Point(244, 292);
            this.buttonCWD.Name = "buttonCWD";
            this.buttonCWD.Size = new System.Drawing.Size(75, 23);
            this.buttonCWD.TabIndex = 8;
            this.buttonCWD.Text = "Enter";
            this.buttonCWD.UseVisualStyleBackColor = true;
            this.buttonCWD.Click += new System.EventHandler(this.buttonCWD_Click);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 421);
            this.Controls.Add(this.buttonCWD);
            this.Controls.Add(this.buttonCDUP);
            this.Controls.Add(this.textBoxCurrentPath);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.listBoxObjects);
            this.Controls.Add(this.textBoxStartDir);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.textBoxFtpAddress);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "GUI";
            this.Text = "Ftp GUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxFtpAddress;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxStartDir;
        private System.Windows.Forms.ListBox listBoxObjects;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.TextBox textBoxCurrentPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCDUP;
        private System.Windows.Forms.Button buttonCWD;
    }
}

