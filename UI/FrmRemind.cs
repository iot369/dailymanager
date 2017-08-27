using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AxWMPLib;//Windows Media Player 控件
using daily.Global;
using System.Collections;
using daily.Entity;
using daily.BusinessFacade;

namespace daily.UI
{
    public partial class FrmRemind : Form
    {
        #region 成员变量
        /// <summary>
        /// 音乐路径
        /// </summary>
        string _strMusicPath = @"D:\视听中心\我的音乐\沧海一声笑.mp3";
        /// <summary>
        /// 阳历
        /// </summary>
        string _strSolar = null;
        /// <summary>
        /// 阴历
        /// </summary>
        string _strLunar = null;
        /// <summary>
        /// 等级
        /// </summary>
        string _strLevel = null;
        /// <summary>
        /// 日程事务
        /// </summary>
        string _strContent = null;
        /// <summary>
        /// 日程编号
        /// </summary>
        string _strID;

        #region 播放器相关控制变量
        /// <summary>
        /// 循环次数
        /// </summary>
        bool _bIsLoop = true;
        /// <summary>
        /// 开始播放状态
        /// </summary>
        bool _bIsAlreadyPlay = false;
        /// <summary>
        /// 媒体总时间
        /// </summary>
        string _strTotalTime = "00:00";
        #endregion

        #endregion

        #region 属性
        /// <summary>
        /// 音乐路径
        /// </summary>
        public string MusicPath
        {
            set { _strMusicPath = value; }
            get { return _strMusicPath; }
        }
        /// <summary>
        /// 阳历
        /// </summary>
        public string Solar
        {
            set { _strSolar = value; }
            get { return _strSolar; }
        }
        /// <summary>
        /// 阴历
        /// </summary>
        public string Lunar
        {
            set { _strLunar = value; }
            get { return _strLunar; }
        }
        /// <summary>
        /// 等级
        /// </summary>
        public string Level
        {
            set { _strLevel = value; }
            get { return _strLevel; }
        }
        /// <summary>
        /// 日程事务内容
        /// </summary>
        public string Content
        {
            set { _strContent = value; }
            get { return _strContent; }
        }
        #endregion

        public FrmRemind()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 日程实例
        /// </summary>
        /// <param name="dailyEntity"></param>
        public FrmRemind(DailyEntity dailyEntity)
        {
            InitializeComponent();
            _strMusicPath = dailyEntity.D_REMIND_MUSIC_PATH;
            _strSolar = dailyEntity.D_SOLAR_CALENDAR;
            _strLunar = dailyEntity.D_LUNAR_CALENDAR;
            _strContent = dailyEntity.D_CONTENT;
            _strID = dailyEntity.ID;
            _strLevel = dailyEntity.D_LEVEL;
        }
        private void FrmRemind_Load(object sender, EventArgs e)
        {
            GlobalVariable.InitForm(this);
            lbSolar.Text = _strSolar;
            lbNow.Text = DateTime.Now.ToString();

            lbLevel.Text = _strLevel;
            if (_strLevel == "特急")
            {
                lbLevel.ForeColor = Color.Red;
            }
            else if (_strLevel == "紧急")
            {
                lbLevel.ForeColor = Color.FromArgb(204, 102, 00);
            }
            else if (_strLevel == "过期")
            {
                lbLevel.ForeColor = Color.Green;
            }
            if (_strMusicPath.Length <= 16)
            {
                lbMusicPath.Text = _strMusicPath;
            }
            else if (16 < _strMusicPath.Length && _strMusicPath.Length <= 32)
            {
                lbMusicPath.Text = _strMusicPath.Substring(0, 16) + "\n";
                lbMusicPath.Text += _strMusicPath.Substring(16);
            }
            else
            {
                lbMusicPath.Text = _strMusicPath.Substring(0, 16) + "\n";
                lbMusicPath.Text += _strMusicPath.Substring(16, 32) + "…";
            }

            if (_strLunar != null && _strLunar != "")
            {
                lbLunar.Visible = true;
                lbLunar.Text += _strLunar;
            }
            rtbContent.Text = _strContent;
            try
            {
                musicPlayer.URL = _strMusicPath;
                musicPlayer.settings.autoStart = false;
                musicPlayer.Ctlcontrols.play();
                musicPlayer.settings.volume = 100;
                musicPlayer.settings.enableErrorDialogs = false;
            }
            catch
            {
            }
        }
        private void FrmRemind_FormClosed(object sender, FormClosedEventArgs e)
        {
            musicPlayer.close();
        }
        private void musicPlayer_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (_bIsLoop && e.newState == 1)
            {
                musicPlayer.Ctlcontrols.play();
            }
            else if (!_bIsAlreadyPlay && e.newState == 3)
            {
                _strTotalTime = musicPlayer.currentMedia.durationString;
                pgbPlay.Maximum = (int)musicPlayer.currentMedia.duration;
                _bIsAlreadyPlay = true;
                lbPlayPgb.Text = "00:00|" + _strTotalTime;
            }
            else if (e.newState == 6)
            {
                lbPlayPgb.Text = "正在缓冲…";
            }
            else if (e.newState == 9)
            {
                lbPlayPgb.Text = "正在连接…";
            }
            else if (e.newState == 10)
            {
                lbPlayPgb.Text = "已经停止";
                btnPlay.Text = "4";
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _bIsLoop = false;
            _bIsAlreadyPlay = false;
            pgbPlay.Value = 0;
            musicPlayer.Ctlcontrols.stop();
            btnPlay.Text = "4";
            if (_strTotalTime != "00:00")
            {
                lbPlayPgb.Text = "00:00|" + _strTotalTime;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (btnPlay.Text == ";")
            {
                musicPlayer.Ctlcontrols.pause();
                btnPlay.Text = "4";
            }
            else
            {
                musicPlayer.Ctlcontrols.play();
                _bIsLoop = true;
                btnPlay.Text = ";";
            }
        }

        private void btnKnow_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.HASH_BEGIN_DOING_DAILY.ContainsKey(_strID))
            {
                GlobalVariable.HASH_BEGIN_DOING_DAILY[_strID]--;
                if (GlobalVariable.HASH_BEGIN_DOING_DAILY[_strID] > 0)
                {
                    if (!GlobalVariable.HASH_DOING_DAILY_LAST_TIME.ContainsKey(_strID))
                    {
                        GlobalVariable.HASH_DOING_DAILY_LAST_TIME.Add(_strID, DateTime.Now);
                    }
                    else
                    {
                        GlobalVariable.HASH_DOING_DAILY_LAST_TIME[_strID] = DateTime.Now;
                    }
                }
                else
                {
                    btnComplete_Click(null, null);
                }
            }
            this.Close();
            DialogResult = DialogResult.OK;
        }
        private void btnComplete_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.HASH_BEGIN_DOING_DAILY.ContainsKey(_strID))
            {
                GlobalVariable.HASH_BEGIN_DOING_DAILY.Remove(_strID);
                if (GlobalVariable.HASH_DOING_DAILY_LAST_TIME.ContainsKey(_strID))
                {
                    GlobalVariable.HASH_DOING_DAILY_LAST_TIME.Remove(_strID);
                }
            }
            BusAdminDaily.UpdateDaliyState(_strID, "0");
            this.Close();
            DialogResult = DialogResult.OK;
        }

        private void timerChange_Tick(object sender, EventArgs e)
        {
            if (_bIsLoop && _bIsAlreadyPlay)
            {
                pgbPlay.Value = (int)musicPlayer.Ctlcontrols.currentPosition;
                lbPlayPgb.Text = musicPlayer.Ctlcontrols.currentPositionString + "|" + _strTotalTime;
            }
            Color color = GetRndColor();
            try
            {
                rtbContent.ForeColor = color;
            }
            catch
            { }
        }
        /// <summary>
        /// 得到随机颜色
        /// </summary>
        /// <returns></returns>
        private Color GetRndColor()
        {
            /*使用所有有名称的颜色
             ArrayList lstColors = new ArrayList();
             foreach (KnownColor kColor in Enum.GetValues(typeof(KnownColor)))
             {
                 if (kColor != KnownColor.White)
                 {
                     lstColors.Add(kColor);
                 }
             }
             if (lstColors.Count != 0)
             {
                 Random rnd = new Random((int)DateTime.Now.Ticks);
                 int indexKColor = rnd.Next(0, lstColors.Count);
                 return Color.FromKnownColor((KnownColor)lstColors[indexKColor]);
             }
             else
             {
                 return Color.Red;
             }
             */
            Color[] colors = new Color[] { Color.Red, Color.Purple, Color.Blue, Color.BlueViolet, Color.RoyalBlue, Color.SeaGreen, Color.SkyBlue, Color.Black, Color.YellowGreen };

            Random rnd = new Random((int)DateTime.Now.Ticks);
            int index = rnd.Next(0, colors.Length);
            return colors[index];
        }
    }
}