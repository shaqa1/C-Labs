namespace foxclient
{
    partial class OAuthWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OAuthWindow));
            this.authwindow = new DotNetBrowser.WinForms.BrowserView();
            this.SuspendLayout();
            // 
            // authwindow
            // 
            this.authwindow.Location = new System.Drawing.Point(-1, 60);
            this.authwindow.Name = "authwindow";
            this.authwindow.Size = new System.Drawing.Size(562, 341);
            this.authwindow.TabIndex = 5;
            // 
            // OAuthWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 365);
            this.Controls.Add(this.authwindow);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OAuthWindow";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.Style = MetroFramework.MetroColorStyle.Black;
            this.Text = "Log In";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private DotNetBrowser.WinForms.BrowserView authwindow;
    }
}