using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace daily.UI
{
    public partial class FrmMainThread : Form
    {
        #region 成员变量
        /// <summary>
        /// 运行状态
        /// </summary>
        bool _runState = true;
        /// <summary>
        /// 代理，创建提醒窗体
        /// </summary>
        /// <param name="dailyEntity"></param>
        delegate void RunRemindFormInvoke(Entity.DailyEntity dailyEntity);
        #endregion

        public FrmMainThread()
        {
            InitializeComponent();
        }
        private void FrmMainThread_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            //设定窗体皮肤
            Global.GlobalVariable.InitForm(this, dailySkinUI);
            Thread.CurrentThread.Name = "MainThread";
            ThreadStart startFunc = new ThreadStart(StartListen);
            Thread threadListen = new Thread(startFunc);
            threadListen.Name = "ListenThread";
            threadListen.Start();
            ThreadStart startUpGrade = new ThreadStart(StartUpGrade);
            Thread threadUpGrade = new Thread(startUpGrade);
            threadUpGrade.Name = "UpGradeThread";
            threadUpGrade.IsBackground = true;
            threadUpGrade.Start();
            if (!Global.GlobalVariable.RUN_BACKGROUND)
            {
                FrmMain frmMain = InitMainForm();
                ShowMainForm(frmMain);
            }
            else
            {
                ntiMain.ShowBalloonTip(5000);
            }
        }

        /// <summary>
        /// 监听线程启动函数
        /// </summary>
        private void StartListen()
        {
            while (_runState)
            {
                foreach (Entity.DailyEntity dailyEntity in Global.GlobalVariable.LST_TODAY_UNDO_DAILY)
                {
                    if (DateTime.Now.CompareTo(Convert.ToDateTime(dailyEntity.D_SOLAR_CALENDAR)) >= 0)
                    {
                        RunRemindFormInvoke runInvoke = new RunRemindFormInvoke(RunRemindForm);
                        if (!Global.GlobalVariable.HASH_BEGIN_DOING_DAILY.ContainsKey(dailyEntity.ID))
                        {
                            Global.GlobalVariable.HASH_BEGIN_DOING_DAILY.Add(dailyEntity.ID, Global.GlobalVariable.TIMES);
                            this.Invoke(runInvoke, new Entity.DailyEntity[] { dailyEntity });
                            //runInvoke(dailyEntity);
                        }
                        else if (Global.GlobalVariable.HASH_DOING_DAILY_LAST_TIME.ContainsKey(dailyEntity.ID))
                        {
                            DateTime lastTime = Global.GlobalVariable.HASH_DOING_DAILY_LAST_TIME[dailyEntity.ID];
                            DateTime nowTime = DateTime.Now;
                            TimeSpan timeSpan = nowTime.Subtract(lastTime);
                            if (timeSpan.TotalMinutes >= Global.GlobalVariable.MINUTES)
                            {
                                this.Invoke(runInvoke, new Entity.DailyEntity[] { dailyEntity });
                                //runInvoke(dailyEntity);
                            }
                        }

                    }
                    break;
                }
                Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// 自动调整等级
        /// </summary>
        private void StartUpGrade()
        {
            while (_runState)
            {
                if (!FrmMain.ISRUN)
                {
                    BusinessFacade.BusAdminDaily.UpGradeBySystem();
                }
                Thread.Sleep(1800000);//1800000
            }
        }

        /// <summary>
        /// RunRemindFormInvoke的实际函数
        /// </summary>
        /// <param name="dailyEntity"></param>
        private void RunRemindForm(Entity.DailyEntity dailyEntity)
        {
            FrmRemind frmRemind = new FrmRemind(dailyEntity);
            while (true)
            {
                if (frmRemind.ShowDialog() == DialogResult.OK)
                {
                    if (FrmMain.ISRUN)
                    {
                        FrmMain.GetInstance().RefreshData();
                    }
                    break;
                }
            }
        }

        private void cmnusMain_Opening(object sender, CancelEventArgs e)
        {
            if (FrmMain.ISRUN)
            {
                miShowMainForm.Checked = true;
            }
            else
            {
                miShowMainForm.Checked = false;
            }
        }

        /// <summary>
        /// 构造主界面
        /// </summary>
        /// <returns></returns>
        private FrmMain InitMainForm()
        {
            FrmMain frmMain = FrmMain.GetInstance();
            frmMain.RunBackground += new FrmMain.RunBackgroundEeventHandler(frmMain_RunBackground);
            return frmMain;
        }

        /// <summary>
        /// 托管事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_RunBackground(object sender, Global.RunBackgroundEventArgs e)
        {
            if (e.RunState == Global.RunBackgroundState.Exit)
            {
                _runState = false;
                this.Close();
            }
            else if (e.RunState == Global.RunBackgroundState.GoOn)
            {
                FrmMain frmMain = ((FrmMain)sender);
                frmMain.Close();
                ntiMain.ShowBalloonTip(5000);
            }
        }

        /// <summary>
        /// 显示主界面
        /// </summary>
        /// <param name="frmMain"></param>
        private void ShowMainForm(FrmMain frmMain)
        {
            if (frmMain.WindowState == FormWindowState.Minimized || frmMain.WindowState == FormWindowState.Maximized)
            {
                frmMain.WindowState = FormWindowState.Normal;
            }
            frmMain.Show();
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            if (Global.MessageHandle.MessageAsk("确定要退出系统？", "确定退出") == DialogResult.OK)
            {
                _runState = false;
                this.Close();
            }
        }


        private void miShowMainForm_Click(object sender, EventArgs e)
        {
            FrmMain frmMain = InitMainForm();
            ShowMainForm(frmMain);
        }

        private void miShowToday_Click(object sender, EventArgs e)
        {
            FrmMain frmMain = InitMainForm();
            frmMain.tbcContainer.SelectedTab = frmMain.tabPageToday;
            ShowMainForm(frmMain);
        }

        private void miShowAll_Click(object sender, EventArgs e)
        {
            FrmMain frmMain = InitMainForm();
            frmMain.tbcContainer.SelectedTab = frmMain.tabPageAdmin;
            ShowMainForm(frmMain);
        }

        private void miConfig_Click(object sender, EventArgs e)
        {
            FrmMain frmMain = InitMainForm();
            frmMain.tbcContainer.SelectedTab = frmMain.tabPageConfig;
            ShowMainForm(frmMain);
        }

        private void ntiMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            miShowMainForm_Click(null, null);
        }
    }
}