namespace daily.UI
{
    partial class FrmRemind
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRemind));
            this.musicPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.timerChange = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pgbPlay = new System.Windows.Forms.ProgressBar();
            this.lbPlayPgb = new System.Windows.Forms.Label();
            this.lbLunar = new System.Windows.Forms.Label();
            this.lbMusicPath = new System.Windows.Forms.Label();
            this.lbSolar = new System.Windows.Forms.Label();
            this.lbNow = new System.Windows.Forms.Label();
            this.lbLevel = new System.Windows.Forms.Label();
            this.btnComplete = new System.Windows.Forms.Button();
            this.btnKnow = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.rtbContent = new System.Windows.Forms.RichTextBox();
            this.btnPlay = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.musicPlayer)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // musicPlayer
            // 
            this.musicPlayer.Enabled = true;
            this.musicPlayer.Location = new System.Drawing.Point(214, 202);
            this.musicPlayer.Name = "musicPlayer";
            this.musicPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("musicPlayer.OcxState")));
            this.musicPlayer.Size = new System.Drawing.Size(40, 36);
            this.musicPlayer.TabIndex = 9;
            this.musicPlayer.Visible = false;
            this.musicPlayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.musicPlayer_PlayStateChange);
            // 
            // timerChange
            // 
            this.timerChange.Enabled = true;
            this.timerChange.Interval = 1000;
            this.timerChange.Tick += new System.EventHandler(this.timerChange_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pgbPlay);
            this.groupBox1.Controls.Add(this.lbPlayPgb);
            this.groupBox1.Controls.Add(this.lbLunar);
            this.groupBox1.Controls.Add(this.lbMusicPath);
            this.groupBox1.Controls.Add(this.lbSolar);
            this.groupBox1.Controls.Add(this.lbNow);
            this.groupBox1.Controls.Add(this.lbLevel);
            this.groupBox1.Controls.Add(this.musicPlayer);
            this.groupBox1.Controls.Add(this.btnComplete);
            this.groupBox1.Controls.Add(this.btnKnow);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.rtbContent);
            this.groupBox1.Controls.Add(this.btnPlay);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 320);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 205);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 40;
            this.label4.Text = "提示音乐：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 39;
            this.label3.Text = "现在时间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 38;
            this.label2.Text = "事务等级：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 37;
            this.label1.Text = "事务内容：";
            // 
            // pgbPlay
            // 
            this.pgbPlay.Location = new System.Drawing.Point(93, 232);
            this.pgbPlay.Name = "pgbPlay";
            this.pgbPlay.Size = new System.Drawing.Size(94, 12);
            this.pgbPlay.TabIndex = 36;
            // 
            // lbPlayPgb
            // 
            this.lbPlayPgb.AutoSize = true;
            this.lbPlayPgb.BackColor = System.Drawing.Color.Transparent;
            this.lbPlayPgb.Location = new System.Drawing.Point(190, 232);
            this.lbPlayPgb.Name = "lbPlayPgb";
            this.lbPlayPgb.Size = new System.Drawing.Size(71, 12);
            this.lbPlayPgb.TabIndex = 35;
            this.lbPlayPgb.Text = "00:00/00:00";
            this.lbPlayPgb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbLunar
            // 
            this.lbLunar.AutoSize = true;
            this.lbLunar.Location = new System.Drawing.Point(92, 184);
            this.lbLunar.Name = "lbLunar";
            this.lbLunar.Size = new System.Drawing.Size(35, 12);
            this.lbLunar.TabIndex = 33;
            this.lbLunar.Text = "阴历 ";
            this.lbLunar.Visible = false;
            // 
            // lbMusicPath
            // 
            this.lbMusicPath.AutoSize = true;
            this.lbMusicPath.Location = new System.Drawing.Point(92, 205);
            this.lbMusicPath.Name = "lbMusicPath";
            this.lbMusicPath.Size = new System.Drawing.Size(23, 12);
            this.lbMusicPath.TabIndex = 32;
            this.lbMusicPath.Text = "D:\\";
            // 
            // lbSolar
            // 
            this.lbSolar.AutoSize = true;
            this.lbSolar.Location = new System.Drawing.Point(92, 164);
            this.lbSolar.Name = "lbSolar";
            this.lbSolar.Size = new System.Drawing.Size(119, 12);
            this.lbSolar.TabIndex = 30;
            this.lbSolar.Text = "2007-11-16 00:00:00";
            // 
            // lbNow
            // 
            this.lbNow.AutoSize = true;
            this.lbNow.ForeColor = System.Drawing.Color.Red;
            this.lbNow.Location = new System.Drawing.Point(91, 141);
            this.lbNow.Name = "lbNow";
            this.lbNow.Size = new System.Drawing.Size(119, 12);
            this.lbNow.TabIndex = 29;
            this.lbNow.Text = "2007-11-16 00:00:00";
            // 
            // lbLevel
            // 
            this.lbLevel.AutoSize = true;
            this.lbLevel.Location = new System.Drawing.Point(92, 119);
            this.lbLevel.Name = "lbLevel";
            this.lbLevel.Size = new System.Drawing.Size(29, 12);
            this.lbLevel.TabIndex = 28;
            this.lbLevel.Text = "中级";
            // 
            // btnComplete
            // 
            this.btnComplete.Location = new System.Drawing.Point(149, 288);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(75, 23);
            this.btnComplete.TabIndex = 26;
            this.btnComplete.Text = "做完了";
            this.btnComplete.UseVisualStyleBackColor = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // btnKnow
            // 
            this.btnKnow.Location = new System.Drawing.Point(56, 288);
            this.btnKnow.Name = "btnKnow";
            this.btnKnow.Size = new System.Drawing.Size(75, 23);
            this.btnKnow.TabIndex = 27;
            this.btnKnow.Text = "知道了";
            this.btnKnow.UseVisualStyleBackColor = true;
            this.btnKnow.Click += new System.EventHandler(this.btnKnow_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 26;
            this.label5.Text = "事务时间：";
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Webdings", 12F);
            this.btnStop.Location = new System.Drawing.Point(133, 250);
            this.btnStop.Name = "btnStop";
            this.btnStop.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnStop.Size = new System.Drawing.Size(34, 27);
            this.btnStop.TabIndex = 19;
            this.btnStop.Text = "<";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // rtbContent
            // 
            this.rtbContent.BackColor = System.Drawing.Color.White;
            this.rtbContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbContent.Location = new System.Drawing.Point(94, 20);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.ReadOnly = true;
            this.rtbContent.Size = new System.Drawing.Size(165, 89);
            this.rtbContent.TabIndex = 25;
            this.rtbContent.Text = "";
            // 
            // btnPlay
            // 
            this.btnPlay.Font = new System.Drawing.Font("Webdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPlay.Location = new System.Drawing.Point(93, 250);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(34, 27);
            this.btnPlay.TabIndex = 18;
            this.btnPlay.Text = ";";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // FrmRemind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 320);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(555, 555);
            this.MinimizeBox = false;
            this.Name = "FrmRemind";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "日程事务管理系统（阿景）";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmRemind_FormClosed);
            this.Load += new System.EventHandler(this.FrmRemind_Load);
            ((System.ComponentModel.ISupportInitialize)(this.musicPlayer)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer musicPlayer;
        private System.Windows.Forms.Timer timerChange;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnKnow;
        private System.Windows.Forms.Label lbLunar;
        private System.Windows.Forms.Label lbMusicPath;
        private System.Windows.Forms.Label lbSolar;
        private System.Windows.Forms.Label lbNow;
        private System.Windows.Forms.Label lbLevel;
        private System.Windows.Forms.RichTextBox rtbContent;
        private System.Windows.Forms.Label lbPlayPgb;
        private System.Windows.Forms.ProgressBar pgbPlay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
    }
}