namespace daily.UI
{
    partial class FrmMainThread
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMainThread));
            this.ntiMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmnusMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miShowToday = new System.Windows.Forms.ToolStripMenuItem();
            this.miShowAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miShowMainForm = new System.Windows.Forms.ToolStripMenuItem();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.dailySkinUI = new DotNetSkin.SkinUI();
            this.cmnusMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // ntiMain
            // 
            this.ntiMain.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ntiMain.BalloonTipText = "日程事务管理系统正在后台运行";
            this.ntiMain.BalloonTipTitle = "日程事务管理系统";
            this.ntiMain.ContextMenuStrip = this.cmnusMain;
            this.ntiMain.Icon = ((System.Drawing.Icon)(resources.GetObject("ntiMain.Icon")));
            this.ntiMain.Text = "日程事务管理系统";
            this.ntiMain.Visible = true;
            this.ntiMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ntiMain_MouseDoubleClick);
            // 
            // cmnusMain
            // 
            this.cmnusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miConfig,
            this.toolStripSeparator2,
            this.miShowToday,
            this.miShowAll,
            this.toolStripSeparator1,
            this.miShowMainForm,
            this.miExit});
            this.cmnusMain.Name = "cmnusMain";
            this.cmnusMain.Size = new System.Drawing.Size(147, 126);
            this.cmnusMain.Opening += new System.ComponentModel.CancelEventHandler(this.cmnusMain_Opening);
            // 
            // miConfig
            // 
            this.miConfig.Image = global::daily.Properties.Resources.c;
            this.miConfig.Name = "miConfig";
            this.miConfig.Size = new System.Drawing.Size(146, 22);
            this.miConfig.Text = "系统设置";
            this.miConfig.ToolTipText = "设置系统运行参数";
            this.miConfig.Click += new System.EventHandler(this.miConfig_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // miShowToday
            // 
            this.miShowToday.Image = global::daily.Properties.Resources.b;
            this.miShowToday.Name = "miShowToday";
            this.miShowToday.Size = new System.Drawing.Size(146, 22);
            this.miShowToday.Text = "查看当日日程";
            this.miShowToday.ToolTipText = "查看当日所有日程事务";
            this.miShowToday.Click += new System.EventHandler(this.miShowToday_Click);
            // 
            // miShowAll
            // 
            this.miShowAll.Image = global::daily.Properties.Resources.a;
            this.miShowAll.Name = "miShowAll";
            this.miShowAll.Size = new System.Drawing.Size(146, 22);
            this.miShowAll.Text = "查看所有日程";
            this.miShowAll.ToolTipText = "查看所有日程事务";
            this.miShowAll.Click += new System.EventHandler(this.miShowAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // miShowMainForm
            // 
            this.miShowMainForm.Checked = true;
            this.miShowMainForm.CheckState = System.Windows.Forms.CheckState.Checked;
            this.miShowMainForm.Name = "miShowMainForm";
            this.miShowMainForm.Size = new System.Drawing.Size(146, 22);
            this.miShowMainForm.Text = "显示主界面";
            this.miShowMainForm.ToolTipText = "显示日程事务管理系统主界面";
            this.miShowMainForm.Click += new System.EventHandler(this.miShowMainForm_Click);
            // 
            // miExit
            // 
            this.miExit.Image = ((System.Drawing.Image)(resources.GetObject("miExit.Image")));
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(146, 22);
            this.miExit.Text = "退出系统";
            this.miExit.ToolTipText = "退出日程事务管理系统";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // dailySkinUI
            // 
            this.dailySkinUI.Active = true;
            this.dailySkinUI.Button = true;
            this.dailySkinUI.Caption = true;
            this.dailySkinUI.CheckBox = true;
            this.dailySkinUI.ComboBox = true;
            this.dailySkinUI.ContextMenu = true;
            this.dailySkinUI.DisableTag = 999;
            this.dailySkinUI.Edit = true;
            this.dailySkinUI.GroupBox = true;
            this.dailySkinUI.ImageList = null;
            this.dailySkinUI.MaiMenu = true;
            this.dailySkinUI.Panel = true;
            this.dailySkinUI.Progress = false;
            this.dailySkinUI.RadioButton = true;
            this.dailySkinUI.ScrollBar = true;
            this.dailySkinUI.SkinFile = null;
            this.dailySkinUI.SkinSteam = null;
            this.dailySkinUI.Spin = true;
            this.dailySkinUI.StatusBar = true;
            this.dailySkinUI.SystemMenu = true;
            this.dailySkinUI.TabControl = true;
            this.dailySkinUI.Text = "Mycontrol1=edit\r\nMycontrol2=edit\r\n";
            this.dailySkinUI.ToolBar = true;
            this.dailySkinUI.TrackBar = true;
            // 
            // FrmMainThread
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(124, 81);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMainThread";
            this.Opacity = 0;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "主线程窗体";
            this.Load += new System.EventHandler(this.FrmMainThread_Load);
            this.cmnusMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon ntiMain;
        private System.Windows.Forms.ContextMenuStrip cmnusMain;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private DotNetSkin.SkinUI dailySkinUI;
        private System.Windows.Forms.ToolStripMenuItem miShowMainForm;
        private System.Windows.Forms.ToolStripMenuItem miShowToday;
        private System.Windows.Forms.ToolStripMenuItem miShowAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miConfig;
    }
}