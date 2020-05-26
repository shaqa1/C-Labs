namespace lab3_v4
{
    partial class fox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fox));
            this.Tray_Icon = new System.Windows.Forms.NotifyIcon(this.components);
            this.apirequestparam = new MetroFramework.Controls.MetroTextBox();
            this.authbutton = new MetroFramework.Controls.MetroButton();
            this.apirequestbutton = new MetroFramework.Controls.MetroButton();
            this.apirequest = new MetroFramework.Controls.MetroTextBox();
            this.browserView1 = new DotNetBrowser.WinForms.BrowserView();
            this.apirequestvalue = new MetroFramework.Controls.MetroTextBox();
            this.addparams = new MetroFramework.Controls.MetroButton();
            this.apilink = new MetroFramework.Controls.MetroLabel();
            this.setrequest = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // Tray_Icon
            // 
            this.Tray_Icon.Icon = ((System.Drawing.Icon)(resources.GetObject("Tray_Icon.Icon")));
            this.Tray_Icon.Text = "fox";
            this.Tray_Icon.Visible = true;
            this.Tray_Icon.BalloonTipClicked += new System.EventHandler(this.Tray_Icon_BalloonTipClicked);
            this.Tray_Icon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Tray_Icon_MouseDoubleClick);
            // 
            // apirequestparam
            // 
            // 
            // 
            // 
            this.apirequestparam.CustomButton.Image = null;
            this.apirequestparam.CustomButton.Location = new System.Drawing.Point(53, 1);
            this.apirequestparam.CustomButton.Name = "";
            this.apirequestparam.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.apirequestparam.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.apirequestparam.CustomButton.TabIndex = 1;
            this.apirequestparam.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.apirequestparam.CustomButton.UseSelectable = true;
            this.apirequestparam.CustomButton.Visible = false;
            this.apirequestparam.Lines = new string[0];
            this.apirequestparam.Location = new System.Drawing.Point(24, 130);
            this.apirequestparam.MaxLength = 32767;
            this.apirequestparam.Name = "apirequestparam";
            this.apirequestparam.PasswordChar = '\0';
            this.apirequestparam.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.apirequestparam.SelectedText = "";
            this.apirequestparam.SelectionLength = 0;
            this.apirequestparam.SelectionStart = 0;
            this.apirequestparam.ShortcutsEnabled = true;
            this.apirequestparam.Size = new System.Drawing.Size(75, 23);
            this.apirequestparam.Style = MetroFramework.MetroColorStyle.Black;
            this.apirequestparam.TabIndex = 0;
            this.apirequestparam.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.apirequestparam.UseSelectable = true;
            this.apirequestparam.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.apirequestparam.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // authbutton
            // 
            this.authbutton.Location = new System.Drawing.Point(23, 72);
            this.authbutton.Name = "authbutton";
            this.authbutton.Size = new System.Drawing.Size(75, 23);
            this.authbutton.TabIndex = 1;
            this.authbutton.Text = "Log In";
            this.authbutton.UseSelectable = true;
            this.authbutton.Click += new System.EventHandler(this.Authbutton_Click);
            // 
            // apirequestbutton
            // 
            this.apirequestbutton.Location = new System.Drawing.Point(24, 188);
            this.apirequestbutton.Name = "apirequestbutton";
            this.apirequestbutton.Size = new System.Drawing.Size(75, 23);
            this.apirequestbutton.TabIndex = 2;
            this.apirequestbutton.Text = "Reqest!";
            this.apirequestbutton.UseSelectable = true;
            this.apirequestbutton.Click += new System.EventHandler(this.Apirequestbutton_Click);
            // 
            // apirequest
            // 
            // 
            // 
            // 
            this.apirequest.CustomButton.Image = null;
            this.apirequest.CustomButton.Location = new System.Drawing.Point(53, 1);
            this.apirequest.CustomButton.Name = "";
            this.apirequest.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.apirequest.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.apirequest.CustomButton.TabIndex = 1;
            this.apirequest.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.apirequest.CustomButton.UseSelectable = true;
            this.apirequest.CustomButton.Visible = false;
            this.apirequest.Lines = new string[0];
            this.apirequest.Location = new System.Drawing.Point(24, 101);
            this.apirequest.MaxLength = 32767;
            this.apirequest.Name = "apirequest";
            this.apirequest.PasswordChar = '\0';
            this.apirequest.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.apirequest.SelectedText = "";
            this.apirequest.SelectionLength = 0;
            this.apirequest.SelectionStart = 0;
            this.apirequest.ShortcutsEnabled = true;
            this.apirequest.Size = new System.Drawing.Size(75, 23);
            this.apirequest.Style = MetroFramework.MetroColorStyle.Black;
            this.apirequest.TabIndex = 3;
            this.apirequest.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.apirequest.UseSelectable = true;
            this.apirequest.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.apirequest.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // browserView1
            // 
            this.browserView1.Location = new System.Drawing.Point(239, 72);
            this.browserView1.Name = "browserView1";
            this.browserView1.Size = new System.Drawing.Size(538, 352);
            this.browserView1.TabIndex = 4;
            // 
            // apirequestvalue
            // 
            // 
            // 
            // 
            this.apirequestvalue.CustomButton.Image = null;
            this.apirequestvalue.CustomButton.Location = new System.Drawing.Point(53, 1);
            this.apirequestvalue.CustomButton.Name = "";
            this.apirequestvalue.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.apirequestvalue.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.apirequestvalue.CustomButton.TabIndex = 1;
            this.apirequestvalue.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.apirequestvalue.CustomButton.UseSelectable = true;
            this.apirequestvalue.CustomButton.Visible = false;
            this.apirequestvalue.Lines = new string[0];
            this.apirequestvalue.Location = new System.Drawing.Point(24, 159);
            this.apirequestvalue.MaxLength = 32767;
            this.apirequestvalue.Name = "apirequestvalue";
            this.apirequestvalue.PasswordChar = '\0';
            this.apirequestvalue.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.apirequestvalue.SelectedText = "";
            this.apirequestvalue.SelectionLength = 0;
            this.apirequestvalue.SelectionStart = 0;
            this.apirequestvalue.ShortcutsEnabled = true;
            this.apirequestvalue.Size = new System.Drawing.Size(75, 23);
            this.apirequestvalue.Style = MetroFramework.MetroColorStyle.Black;
            this.apirequestvalue.TabIndex = 6;
            this.apirequestvalue.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.apirequestvalue.UseSelectable = true;
            this.apirequestvalue.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.apirequestvalue.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // addparams
            // 
            this.addparams.Location = new System.Drawing.Point(105, 130);
            this.addparams.Name = "addparams";
            this.addparams.Size = new System.Drawing.Size(75, 52);
            this.addparams.TabIndex = 7;
            this.addparams.Text = "Add";
            this.addparams.UseSelectable = true;
            this.addparams.Click += new System.EventHandler(this.Addparams_Click);
            // 
            // apilink
            // 
            this.apilink.AutoSize = true;
            this.apilink.Location = new System.Drawing.Point(24, 405);
            this.apilink.Name = "apilink";
            this.apilink.Size = new System.Drawing.Size(0, 0);
            this.apilink.TabIndex = 8;
            // 
            // setrequest
            // 
            this.setrequest.Location = new System.Drawing.Point(105, 101);
            this.setrequest.Name = "setrequest";
            this.setrequest.Size = new System.Drawing.Size(75, 23);
            this.setrequest.TabIndex = 9;
            this.setrequest.Text = "Set Request";
            this.setrequest.UseSelectable = true;
            this.setrequest.Click += new System.EventHandler(this.Setrequest_Click);
            // 
            // fox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.setrequest);
            this.Controls.Add(this.apilink);
            this.Controls.Add(this.addparams);
            this.Controls.Add(this.apirequestvalue);
            this.Controls.Add(this.browserView1);
            this.Controls.Add(this.apirequest);
            this.Controls.Add(this.apirequestbutton);
            this.Controls.Add(this.authbutton);
            this.Controls.Add(this.apirequestparam);
            this.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fox";
            this.Padding = new System.Windows.Forms.Padding(20, 69, 20, 23);
            this.Style = MetroFramework.MetroColorStyle.Black;
            this.Text = "fox";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon Tray_Icon;
        private MetroFramework.Controls.MetroTextBox apirequestparam;
        private MetroFramework.Controls.MetroButton authbutton;
        private MetroFramework.Controls.MetroButton apirequestbutton;
        private MetroFramework.Controls.MetroTextBox apirequest;
        private DotNetBrowser.WinForms.BrowserView browserView1;
        private MetroFramework.Controls.MetroTextBox apirequestvalue;
        private MetroFramework.Controls.MetroButton addparams;
        private MetroFramework.Controls.MetroLabel apilink;
        private MetroFramework.Controls.MetroButton setrequest;
    }
}

