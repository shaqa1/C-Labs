namespace lab3_v2
{
    partial class main
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
            this.profileinfo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.friends = new MetroFramework.Controls.MetroComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.screenname = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.screennametb = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // profileinfo
            // 
            this.profileinfo.Location = new System.Drawing.Point(12, 25);
            this.profileinfo.Multiline = true;
            this.profileinfo.Name = "profileinfo";
            this.profileinfo.Size = new System.Drawing.Size(229, 76);
            this.profileinfo.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Profile Info:";
            // 
            // friends
            // 
            this.friends.FormattingEnabled = true;
            this.friends.ItemHeight = 23;
            this.friends.Location = new System.Drawing.Point(12, 120);
            this.friends.Name = "friends";
            this.friends.Size = new System.Drawing.Size(229, 29);
            this.friends.TabIndex = 12;
            this.friends.UseSelectable = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "List of Friends:";
            // 
            // screenname
            // 
            this.screenname.Location = new System.Drawing.Point(12, 168);
            this.screenname.Name = "screenname";
            this.screenname.Size = new System.Drawing.Size(188, 20);
            this.screenname.TabIndex = 14;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(206, 166);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Set";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SetScreenName);
            // 
            // screennametb
            // 
            this.screennametb.AutoSize = true;
            this.screennametb.Location = new System.Drawing.Point(12, 152);
            this.screennametb.Name = "screennametb";
            this.screennametb.Size = new System.Drawing.Size(100, 13);
            this.screennametb.TabIndex = 16;
            this.screennametb.Text = "Screen Name (link):";
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 200);
            this.Controls.Add(this.screennametb);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.screenname);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.friends);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.profileinfo);
            this.Name = "main";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox profileinfo;
        private System.Windows.Forms.Label label3;
        private MetroFramework.Controls.MetroComboBox friends;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox screenname;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label screennametb;
    }
}

