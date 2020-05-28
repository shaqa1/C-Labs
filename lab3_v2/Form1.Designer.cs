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
            this.setstatusbutton = new System.Windows.Forms.Button();
            this.setstatus = new System.Windows.Forms.TextBox();
            this.oldpass = new System.Windows.Forms.TextBox();
            this.newpass = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.bdate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // setstatusbutton
            // 
            this.setstatusbutton.Location = new System.Drawing.Point(174, 10);
            this.setstatusbutton.Name = "setstatusbutton";
            this.setstatusbutton.Size = new System.Drawing.Size(75, 23);
            this.setstatusbutton.TabIndex = 0;
            this.setstatusbutton.Text = "Set Status";
            this.setstatusbutton.UseVisualStyleBackColor = true;
            this.setstatusbutton.Click += new System.EventHandler(this.setstatus_Click);
            // 
            // setstatus
            // 
            this.setstatus.Location = new System.Drawing.Point(68, 12);
            this.setstatus.Name = "setstatus";
            this.setstatus.Size = new System.Drawing.Size(100, 20);
            this.setstatus.TabIndex = 1;
            // 
            // oldpass
            // 
            this.oldpass.Location = new System.Drawing.Point(68, 71);
            this.oldpass.Name = "oldpass";
            this.oldpass.Size = new System.Drawing.Size(100, 20);
            this.oldpass.TabIndex = 2;
            // 
            // newpass
            // 
            this.newpass.Location = new System.Drawing.Point(68, 97);
            this.newpass.Name = "newpass";
            this.newpass.Size = new System.Drawing.Size(100, 20);
            this.newpass.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(174, 95);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Change!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.passchangeclick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Old Pass";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "New Pass";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "New Status";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(174, 158);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Change!";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.bdaychangeclick);
            // 
            // bdate
            // 
            this.bdate.Location = new System.Drawing.Point(68, 158);
            this.bdate.Name = "bdate";
            this.bdate.Size = new System.Drawing.Size(100, 20);
            this.bdate.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(77, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "DD.MM.YYYY";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "BDate";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(3, 215);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "Authenticate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.authclick);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 241);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bdate);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.newpass);
            this.Controls.Add(this.oldpass);
            this.Controls.Add(this.setstatus);
            this.Controls.Add(this.setstatusbutton);
            this.Name = "main";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button setstatusbutton;
        private System.Windows.Forms.TextBox setstatus;
        private System.Windows.Forms.TextBox oldpass;
        private System.Windows.Forms.TextBox newpass;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox bdate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button3;
    }
}

