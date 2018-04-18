namespace MTFramework.WinForm.Controls
{
    partial class GlobalExceptionForm
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
            this.btn_Ignore = new System.Windows.Forms.Button();
            this.btn_Abort = new System.Windows.Forms.Button();
            this.btn_Feedback = new System.Windows.Forms.Button();
            this.lbl_UserName = new System.Windows.Forms.Label();
            this.lbl_OSVersion = new System.Windows.Forms.Label();
            this.lbl_MachineName = new System.Windows.Forms.Label();
            this.lbl_CurrentDirectory = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_TargeSite = new System.Windows.Forms.TextBox();
            this.txt_StackTrace = new System.Windows.Forms.TextBox();
            this.txt_Source = new System.Windows.Forms.TextBox();
            this.txt_Info = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Info = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Ignore
            // 
            this.btn_Ignore.Location = new System.Drawing.Point(489, 301);
            this.btn_Ignore.Name = "btn_Ignore";
            this.btn_Ignore.Size = new System.Drawing.Size(120, 21);
            this.btn_Ignore.TabIndex = 100004;
            this.btn_Ignore.Text = "忽略当前错误(&I)";
            this.btn_Ignore.UseVisualStyleBackColor = true;
            this.btn_Ignore.Click += new System.EventHandler(this.btn_Ignore_Click);
            // 
            // btn_Abort
            // 
            this.btn_Abort.Location = new System.Drawing.Point(364, 301);
            this.btn_Abort.Name = "btn_Abort";
            this.btn_Abort.Size = new System.Drawing.Size(120, 21);
            this.btn_Abort.TabIndex = 100002;
            this.btn_Abort.Text = "中止程序运行(&A)";
            this.btn_Abort.UseVisualStyleBackColor = true;
            this.btn_Abort.Click += new System.EventHandler(this.btn_Abort_Click);
            // 
            // btn_Feedback
            // 
            this.btn_Feedback.Location = new System.Drawing.Point(13, 301);
            this.btn_Feedback.Name = "btn_Feedback";
            this.btn_Feedback.Size = new System.Drawing.Size(120, 21);
            this.btn_Feedback.TabIndex = 100001;
            this.btn_Feedback.Text = "发送反馈信息(&F)";
            this.btn_Feedback.UseVisualStyleBackColor = true;
            this.btn_Feedback.Click += new System.EventHandler(this.btn_Feedback_Click);
            // 
            // lbl_UserName
            // 
            this.lbl_UserName.AutoSize = true;
            this.lbl_UserName.Location = new System.Drawing.Point(147, 276);
            this.lbl_UserName.Name = "lbl_UserName";
            this.lbl_UserName.Size = new System.Drawing.Size(0, 12);
            this.lbl_UserName.TabIndex = 100021;
            // 
            // lbl_OSVersion
            // 
            this.lbl_OSVersion.AutoSize = true;
            this.lbl_OSVersion.Location = new System.Drawing.Point(149, 251);
            this.lbl_OSVersion.Name = "lbl_OSVersion";
            this.lbl_OSVersion.Size = new System.Drawing.Size(0, 12);
            this.lbl_OSVersion.TabIndex = 100019;
            // 
            // lbl_MachineName
            // 
            this.lbl_MachineName.AutoSize = true;
            this.lbl_MachineName.Location = new System.Drawing.Point(147, 226);
            this.lbl_MachineName.Name = "lbl_MachineName";
            this.lbl_MachineName.Size = new System.Drawing.Size(0, 12);
            this.lbl_MachineName.TabIndex = 100018;
            // 
            // lbl_CurrentDirectory
            // 
            this.lbl_CurrentDirectory.AutoSize = true;
            this.lbl_CurrentDirectory.Location = new System.Drawing.Point(147, 201);
            this.lbl_CurrentDirectory.Name = "lbl_CurrentDirectory";
            this.lbl_CurrentDirectory.Size = new System.Drawing.Size(0, 12);
            this.lbl_CurrentDirectory.TabIndex = 100017;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(93, 276);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 100016;
            this.label8.Text = "用户名:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(84, 251);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 100015;
            this.label7.Text = "操作系统:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(96, 226);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 100014;
            this.label6.Text = "机器名:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(84, 201);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 100013;
            this.label5.Text = "当前路径:";
            // 
            // txt_TargeSite
            // 
            this.txt_TargeSite.BackColor = System.Drawing.SystemColors.Window;
            this.txt_TargeSite.Location = new System.Drawing.Point(149, 173);
            this.txt_TargeSite.Name = "txt_TargeSite";
            this.txt_TargeSite.ReadOnly = true;
            this.txt_TargeSite.Size = new System.Drawing.Size(460, 21);
            this.txt_TargeSite.TabIndex = 100020;
            // 
            // txt_StackTrace
            // 
            this.txt_StackTrace.BackColor = System.Drawing.SystemColors.Window;
            this.txt_StackTrace.Location = new System.Drawing.Point(149, 83);
            this.txt_StackTrace.Multiline = true;
            this.txt_StackTrace.Name = "txt_StackTrace";
            this.txt_StackTrace.ReadOnly = true;
            this.txt_StackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_StackTrace.Size = new System.Drawing.Size(460, 85);
            this.txt_StackTrace.TabIndex = 100012;
            // 
            // txt_Source
            // 
            this.txt_Source.BackColor = System.Drawing.SystemColors.Window;
            this.txt_Source.Location = new System.Drawing.Point(149, 58);
            this.txt_Source.Name = "txt_Source";
            this.txt_Source.ReadOnly = true;
            this.txt_Source.Size = new System.Drawing.Size(460, 21);
            this.txt_Source.TabIndex = 100011;
            // 
            // txt_Info
            // 
            this.txt_Info.BackColor = System.Drawing.SystemColors.Window;
            this.txt_Info.Location = new System.Drawing.Point(149, 33);
            this.txt_Info.Name = "txt_Info";
            this.txt_Info.ReadOnly = true;
            this.txt_Info.Size = new System.Drawing.Size(460, 21);
            this.txt_Info.TabIndex = 100010;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(108, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 100009;
            this.label4.Text = "方法:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(108, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 100008;
            this.label3.Text = "堆栈:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 100007;
            this.label2.Text = "对象:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 100006;
            this.label1.Text = "信息:";
            // 
            // lb_Info
            // 
            this.lb_Info.Location = new System.Drawing.Point(1, 1);
            this.lb_Info.Margin = new System.Windows.Forms.Padding(0);
            this.lb_Info.Name = "lb_Info";
            this.lb_Info.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lb_Info.Size = new System.Drawing.Size(608, 25);
            this.lb_Info.TabIndex = 100022;
            this.lb_Info.Text = "{0} 遇到问题需要关闭。我们对此引起的不便表示抱歉。请将此问题报告给 {1}。";
            this.lb_Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.ErrorImage = global::MTFramework.WinForm.Controls.Properties.Resources.Computer;
            this.pictureBox2.Image = global::MTFramework.WinForm.Controls.Properties.Resources.Computer;
            this.pictureBox2.InitialImage = global::MTFramework.WinForm.Controls.Properties.Resources.Computer;
            this.pictureBox2.Location = new System.Drawing.Point(13, 197);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(60, 60);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 100005;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = global::MTFramework.WinForm.Controls.Properties.Resources.Info_Box_Blue;
            this.pictureBox1.Image = global::MTFramework.WinForm.Controls.Properties.Resources.Info_Box_Blue;
            this.pictureBox1.InitialImage = global::MTFramework.WinForm.Controls.Properties.Resources.Info_Box_Blue;
            this.pictureBox1.Location = new System.Drawing.Point(13, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 100003;
            this.pictureBox1.TabStop = false;
            // 
            // GlobalExceptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 323);
            this.Controls.Add(this.btn_Ignore);
            this.Controls.Add(this.btn_Abort);
            this.Controls.Add(this.btn_Feedback);
            this.Controls.Add(this.lbl_UserName);
            this.Controls.Add(this.lbl_OSVersion);
            this.Controls.Add(this.lbl_MachineName);
            this.Controls.Add(this.lbl_CurrentDirectory);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.txt_TargeSite);
            this.Controls.Add(this.txt_StackTrace);
            this.Controls.Add(this.txt_Source);
            this.Controls.Add(this.txt_Info);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lb_Info);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GlobalExceptionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "错误提示";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Ignore;
        private System.Windows.Forms.Button btn_Abort;
        private System.Windows.Forms.Button btn_Feedback;
        private System.Windows.Forms.Label lbl_UserName;
        private System.Windows.Forms.Label lbl_OSVersion;
        private System.Windows.Forms.Label lbl_MachineName;
        private System.Windows.Forms.Label lbl_CurrentDirectory;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox txt_TargeSite;
        private System.Windows.Forms.TextBox txt_StackTrace;
        private System.Windows.Forms.TextBox txt_Source;
        private System.Windows.Forms.TextBox txt_Info;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lb_Info;

    }
}