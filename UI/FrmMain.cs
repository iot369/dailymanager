using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using daily.Global;
using daily.BusinessFacade;
using daily.Entity;
using System.Xml;
using Microsoft.Win32;

namespace daily.UI
{
    public partial class FrmMain : Form, IWin32Window
    {
        #region 成员变量
        /// <summary>
        /// 当日已处理事务数据源
        /// </summary>
        DataSet _dsDidToday = null;
        /// <summary>
        /// 当日未处理事务数据源
        /// </summary>
        DataSet _dsToday = null;
        /// <summary>
        /// 所有事务数据源
        /// </summary>
        DataSet _dsAll = null;
        /// <summary>
        /// 日程事务实体
        /// </summary>
        DailyEntity _dailyEntity = null;
        /// <summary>
        /// 将要后台运行
        /// </summary>
        bool _bWillRunBackground = false;
        /// <summary>
        /// 是否正在清空表单，清空表单不更新等级和阴阳历判断
        /// </summary>
        bool _bClearForm = false;
        /// <summary>
        /// 操作类型0代表增加，1代表修改
        /// </summary>
        int operType = 0;
        /// <summary>
        /// 修改来自哪里0代表所有日程事务，1代表今日日程事务
        /// </summary>
        int mFromWhere = 0;
        public delegate void RunBackgroundEeventHandler(object sender, RunBackgroundEventArgs e);
        /// <summary>
        /// 后台运行或者退出应用
        /// </summary>
        public event RunBackgroundEeventHandler RunBackground;
        #endregion

        private static FrmMain INSTANCE;
        public static bool ISRUN = false;

        public FrmMain()
        {
            InitializeComponent();
        }
        public static FrmMain GetInstance()
        {
            if (INSTANCE == null || INSTANCE.IsDisposed)
            {
                INSTANCE = new FrmMain();
            }
            ISRUN = true;
            return INSTANCE;
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        internal void RefreshData()
        {
            tbcContainer_SelectedIndexChanged(null, null);
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            InitUI();
            //判断是否第一次运行，如果是第一次运行转换到配置选项卡
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\景仁海\");
            if (regKey != null)
            {
                bool IsFirstRun = Convert.ToBoolean(regKey.GetValue("firstRun"));
                if (IsFirstRun)
                {
                    MessageHandle.MessageInfo("初次使用系统，请设置运行参数！", "系统提示");
                    tbcContainer.SelectedTab = tabPageConfig;
                }
                regKey.Close();
            }
        }
        private void chkLunar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLunar.Checked)
            {
                dtpTime.ShowUpDown = true;
                tooltipTime.SetToolTip(dtpTime, "正在使用太阴历输入时间…");
            }
            else
            {
                dtpTime.ShowUpDown = false;
                tooltipTime.RemoveAll();
            }
        }
        private void dtpTime_ValueChanged(object sender, EventArgs e)
        {
            if (!_bClearForm)
            {
                string grade = "4";
                if (chkLunar.Checked)
                {
                    try
                    {
                        ChineseCalendarInfo cCalendar = ChineseCalendarInfo.FromLunarDate(dtpTime.Value, false);
                        tooltipTime.SetToolTip(dtpTime, "对应太阳历：" + cCalendar.SolarDate.ToShortDateString());
                        grade = BusAdminDaily.CalculateGrade(cCalendar.SolarDate.Add(new TimeSpan(dtpTime.Value.Hour, dtpTime.Value.Minute, dtpTime.Value.Second)));
                    }
                    catch (Exception ex)
                    {
                        MessageHandle.MessageError("错误：" + ex.Message + "！", "错误信息");
                        dtpTime.Focus();
                    }
                }
                else
                {
                    grade = BusAdminDaily.CalculateGrade(dtpTime.Value);
                }
                cboLevel.SelectedValue = grade;
                if (grade == "4")
                {
                    MessageHandle.MessageWarnning("事务时间已过期！", "警告信息");
                    dtpTime.Focus();
                }
            }
        }
        private void btnDelDidToday_Click(object sender, EventArgs e)
        {
            if (dgvDidToday.SelectedRows.Count > 0 && MessageHandle.MessageAsk("确定要删除该日程事务？", "确定删除") == DialogResult.OK)
            {
                DataGridViewSelectedRowCollection selectedRows = dgvDidToday.SelectedRows;
                if (BusAdminDaily.DeleteDailyById(selectedRows[0].Cells[0].Value.ToString()))
                {
                    if (_dailyEntity != null && _dailyEntity.ID == selectedRows[0].Cells[0].Value.ToString())
                    {
                        _dailyEntity = null;
                        ClearForm();
                    }
                    dgvDidToday.Rows.Remove(selectedRows[0]);
                    MessageHandle.MessageInfo("恭喜，删除成功！", "提示信息");
                }
            }
        }
        private void btnCancelToday_Click(object sender, EventArgs e)
        {
            if (dgvToday.SelectedRows.Count > 0 && MessageHandle.MessageAsk("确定要取消该日程事务？", "确定取消") == DialogResult.OK)
            {
                DataGridViewSelectedRowCollection selectedRows = dgvToday.SelectedRows;
                if (BusAdminDaily.UpdateDaliyState(selectedRows[0].Cells[0].Value.ToString(), "0"))
                {
                    selectedRows[0].Cells["colRemindFlag_Today"].Value = "已处理";
                    DataRow dr = ((DataRowView)selectedRows[0].DataBoundItem).Row;
                    _dsDidToday.Tables[0].Rows.Add(dr.ItemArray);
                    dgvDidToday.CurrentCell = dgvDidToday.Rows[dgvDidToday.Rows.Count - 1].Cells[1];
                    if (_dailyEntity != null && _dailyEntity.ID == selectedRows[0].Cells[0].Value.ToString())
                    {
                        _dailyEntity.D_REMIND_FLAG = "0";
                    }
                    dgvToday.Rows.Remove(selectedRows[0]);
                    MessageHandle.MessageInfo("恭喜，取消成功！", "提示信息");
                }
            }
        }
        private void btnDelToday_Click(object sender, EventArgs e)
        {
            if (dgvToday.SelectedRows.Count > 0 && MessageHandle.MessageAsk("注意：该日程还没执行！\n 确定要删除该日程事务？", "确定删除") == DialogResult.OK)
            {
                DataGridViewSelectedRowCollection selectedRows = dgvToday.SelectedRows;
                if (BusAdminDaily.DeleteDailyById(selectedRows[0].Cells[0].Value.ToString()))
                {
                    if (_dailyEntity != null && _dailyEntity.ID == selectedRows[0].Cells[0].Value.ToString())
                    {
                        _dailyEntity = null;
                        ClearForm();
                    }
                    dgvToday.Rows.Remove(selectedRows[0]);
                    MessageHandle.MessageInfo("恭喜，删除成功！", "提示信息");
                }
            }
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitUI()
        {
            //自动更新过期日程
            BusAdminDaily.UpdateDailyStateBySystem();
            //初始化等级下拉列表
            BusAdminDaily.InitLevelComboBox(cboLevel);
            BindToday();
            BindDidToday();
            BindAll();
            LoadDefault();
        }
        /// <summary>
        /// 绑定今日未处理事务
        /// </summary>
        private void BindToday()
        {
            //绑定今日日程
            dgvToday.AutoGenerateColumns = false;
            _dsToday = BusAdminDaily.QueryDailyOfToday(false);
            dgvToday.DataSource = _dsToday.Tables[0].DefaultView;
            foreach (DataGridViewRow dgvRow in dgvToday.SelectedRows)
            {
                dgvRow.Selected = false;
            }
        }
        /// <summary>
        /// 绑定今日已处理事务
        /// </summary>
        private void BindDidToday()
        {
            //绑定今日已处理日程
            dgvDidToday.AutoGenerateColumns = false;
            _dsDidToday = BusAdminDaily.QueryDailyOfToday(true);
            dgvDidToday.DataSource = _dsDidToday.Tables[0].DefaultView;
            foreach (DataGridViewRow dgvRow in dgvDidToday.SelectedRows)
            {
                dgvRow.Selected = false;
            }
        }
        /// <summary>
        /// 绑定所有事务
        /// </summary>
        private void BindAll()
        {
            //绑定所有日程
            dgvAll.AutoGenerateColumns = false;
            _dsAll = BusAdminDaily.QueryDailyOfAll();
            dgvAll.DataSource = _dsAll.Tables[0].DefaultView;
            foreach (DataGridViewRow dgvRow in dgvAll.SelectedRows)
            {
                dgvRow.Selected = false;
            }

        }
        private void LoadDefault()
        {
            nudTimes.Value = GlobalVariable.TIMES;
            nudMinute.Value = GlobalVariable.MINUTES;
            txtMusicPath.Text = GlobalVariable.DEFAULT_MUSIC_PATH;
            txtDefaultMusicPath.Text = GlobalVariable.DEFAULT_MUSIC_PATH;
            chkRunOnStart.Checked = GlobalVariable.RUN_ON_START;
            chkRunBackground.Checked = GlobalVariable.RUN_BACKGROUND;
        }
        private void btnClearDidToday_Click(object sender, EventArgs e)
        {
            if (dgvDidToday.SelectedRows.Count > 0 && MessageHandle.MessageAsk("确定要删除当日所有已处理日程事务？", "确定删除") == DialogResult.OK)
            {
                if (BusAdminDaily.DeleteDailyOfToday())
                {
                    if (_dailyEntity != null)
                    {
                        foreach (DataGridViewRow dataRow in dgvDidToday.Rows)
                        {
                            if (_dailyEntity.ID == dataRow.Cells[0].Value.ToString())
                            {
                                ClearForm();
                                _dailyEntity = null;
                                break;
                            }
                        }
                    }
                    _dsDidToday.Tables[0].Rows.Clear();
                    MessageHandle.MessageInfo("恭喜，清空成功！", "提示信息");
                }
            }
        }
        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            if (dgvAll.SelectedRows.Count > 0)
            {
                DataGridViewSelectedRowCollection selectedRows = dgvAll.SelectedRows;
                if (selectedRows[0].Cells["colRemindFlag_ALL"].Value.ToString() == "未处理" && MessageHandle.MessageAsk("确定要取消该日程事务？", "确定取消") == DialogResult.OK)
                {
                    if (BusAdminDaily.UpdateDaliyState(selectedRows[0].Cells[0].Value.ToString(), "0"))
                    {
                        selectedRows[0].Cells["colRemindFlag_ALL"].Value = "已处理";
                        MessageHandle.MessageInfo("恭喜，取消成功！", "提示信息");
                    }
                }
            }
        }
        private void btnDel_All_Click(object sender, EventArgs e)
        {
            if (dgvAll.SelectedRows.Count > 0)
            {
                DataGridViewSelectedRowCollection selectedRows = dgvAll.SelectedRows;
                if (selectedRows[0].Cells["colRemindFlag_ALL"].Value.ToString() == "未处理" && MessageHandle.MessageAsk("注意：该日程还没执行！\n确定要删除该日程事务？", "确定删除") == DialogResult.OK)
                {
                    BusAdminDaily.DeleteDailyById(selectedRows[0].Cells[0].Value.ToString());
                    if (_dailyEntity != null && _dailyEntity.ID == selectedRows[0].Cells[0].Value.ToString())
                    {
                        _dailyEntity = null;
                        ClearForm();
                    }
                    dgvAll.Rows.Remove(selectedRows[0]);
                    MessageHandle.MessageInfo("恭喜，删除成功！", "提示信息");
                }
                else if (selectedRows[0].Cells["colRemindFlag_ALL"].Value.ToString() == "已处理" && MessageHandle.MessageAsk("确定要删除该日程事务？", "确定删除") == DialogResult.OK)
                {
                    BusAdminDaily.DeleteDailyById(selectedRows[0].Cells[0].Value.ToString());
                    if (_dailyEntity != null && _dailyEntity.ID == selectedRows[0].Cells[0].Value.ToString())
                    {
                        _dailyEntity = null;
                        ClearForm();
                    }
                    dgvAll.Rows.Remove(selectedRows[0]);
                    MessageHandle.MessageInfo("恭喜，删除成功！", "提示信息");
                }
            }
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (dgvAll.Rows.Count > 0)
            {
                if (MessageHandle.MessageAsk("确定要删除所有已处理日程事务？", "确定删除") == DialogResult.OK)
                {
                    BusAdminDaily.DeleteDidDailyOfAll();
                    if (_dailyEntity != null)
                    {
                        foreach (DataGridViewRow dataRow in dgvAll.Rows)
                        {
                            if (_dailyEntity.ID == dataRow.Cells[0].Value.ToString())
                            {
                                ClearForm();
                                _dailyEntity = null;
                                break;
                            }
                        }
                    }
                    _dsAll.Tables[0].DefaultView.RowFilter = "D_REMIND_FLAG='未处理'";
                    MessageHandle.MessageInfo("恭喜，清空成功！", "提示信息");
                }
            }
        }
        private void btnDeleteAll_All_Click(object sender, EventArgs e)
        {
            if (dgvAll.Rows.Count > 0)
            {
                bool bHasToDo = false;
                if (dgvAll.Rows[0].Cells[5].Value.ToString() == "未处理")
                {
                    bHasToDo = true;
                }
                if (bHasToDo && MessageHandle.MessageAsk("注意：有日程事务还没执行！\n确定要删除所有日程事务？", "确定删除") == DialogResult.OK)
                {
                    BusAdminDaily.DeleteDailyOfAll();
                    if (_dailyEntity != null)
                    {
                        foreach (DataGridViewRow dataRow in dgvAll.Rows)
                        {
                            if (_dailyEntity.ID == dataRow.Cells[0].Value.ToString())
                            {
                                ClearForm();
                                _dailyEntity = null;
                                break;
                            }
                        }
                    }
                    _dsAll.Tables[0].Rows.Clear();
                    MessageHandle.MessageInfo("恭喜，删除成功！", "提示信息");
                }
                else if (!bHasToDo && MessageHandle.MessageAsk("确定要删除所有日程事务？", "确定删除") == DialogResult.OK)
                {
                    BusAdminDaily.DeleteDidDailyOfAll();
                    if (_dailyEntity != null)
                    {
                        foreach (DataGridViewRow dataRow in dgvAll.Rows)
                        {
                            if (_dailyEntity.ID == dataRow.Cells[0].Value.ToString())
                            {
                                ClearForm();
                                _dailyEntity = null;
                                break;
                            }
                        }
                    }
                    _dsAll.Tables[0].Rows.Clear();
                    MessageHandle.MessageInfo("恭喜，删除成功！", "提示信息");
                }
            }
        }
        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            //openfiledlgMuicePath.Filter = "常见音频文件(*.mp3;*.wav;*.wma;*.mid;*.asf)|*.mp3;*.wav;*.wma;*.mid;*.asf|所有文件(*.*)|*.*";
            openfiledlgMuicePath.Filter = "常见音频文件(*.mp3;*.wav;*.wma;*.mid;*.asf)|*.mp3;*.wav;*.wma;*.mid;*.asf";
            if (openfiledlgMuicePath.ShowDialog() == DialogResult.OK)
            {
                txtMusicPath.Text = openfiledlgMuicePath.FileName;
            }
        }
        private void btnBrowseDefaultFile_Click(object sender, EventArgs e)
        {
            openfiledlgMuicePath.Filter = "常见音频文件(*.mp3;*.wav;*.wma;*.mid;*.asf)|*.mp3;*.wav;*.wma;*.mid;*.asf";
            if (openfiledlgMuicePath.ShowDialog() == DialogResult.OK)
            {
                txtDefaultMusicPath.Text = openfiledlgMuicePath.FileName;
            }
        }
        private void btnModifyAll_Click(object sender, EventArgs e)
        {
            if (dgvAll.SelectedRows.Count > 0)
            {
                DataGridViewSelectedRowCollection selectedRows = dgvAll.SelectedRows;
                ModifyDaily(selectedRows);
            }
        }
        private void btnAddDaily_Click(object sender, EventArgs e)
        {
            if (operType == 1)
            {
                if (MessageHandle.MessageAsk("增加将会丢失修改数据，确定要执行增加操作？", "确定增加") == DialogResult.OK)
                {
                    ClearForm();
                    cboLevel.Select();
                }
            }
            else
            {
                if (IsValid())
                {
                    _dailyEntity = new DailyEntity();
                    _dailyEntity.D_CONTENT = rtbContent.Text;
                    _dailyEntity.D_REMIND_MUSIC_PATH = txtMusicPath.Text;
                    string grade;
                    if (chkLunar.Checked)
                    {
                        _dailyEntity.D_LUNAR_CALENDAR = dtpTime.Value.ToString();
                        ChineseCalendarInfo cCalendar = ChineseCalendarInfo.FromLunarDate(dtpTime.Value.Date, false);
                        DateTime solar = cCalendar.SolarDate.Add(new TimeSpan(dtpTime.Value.Hour, dtpTime.Value.Minute, dtpTime.Value.Second));
                        _dailyEntity.D_SOLAR_CALENDAR = solar.ToString();
                        grade = BusAdminDaily.CalculateGrade(solar);
                        _dailyEntity.D_LEVEL = grade;
                    }
                    else
                    {
                        _dailyEntity.D_SOLAR_CALENDAR = dtpTime.Value.ToString();
                        grade = BusAdminDaily.CalculateGrade(dtpTime.Value);
                        _dailyEntity.D_LEVEL = grade;
                    }
                    if (BusAdminDaily.AddDaily(_dailyEntity))
                    {
                        ClearForm();
                        _dailyEntity = null;
                        BindAll();
                        if (MessageHandle.MessageInfo("恭喜，增加成功！", "提示信息") == DialogResult.OK)
                        {
                            BusAdminDaily.InitTodayUnDoDaily();
                        }
                    }
                }
            }
        }
        private void btnModifyToday_Click(object sender, EventArgs e)
        {
            if (dgvToday.SelectedRows.Count > 0)
            {
                DataGridViewSelectedRowCollection selectedRows = dgvToday.SelectedRows;
                mFromWhere = 1;
                ModifyDaily(selectedRows);
            }
        }
        private void dgvToday_DoubleClick(object sender, EventArgs e)
        {
            if (dgvToday.SelectedRows.Count > 0)
            {
                DataGridViewSelectedRowCollection selectedRows = dgvToday.SelectedRows;
                mFromWhere = 1;
                ModifyDaily(selectedRows);
            }
        }
        private void dgvAll_DoubleClick(object sender, EventArgs e)
        {
            if (dgvAll.SelectedRows.Count > 0)
            {
                DataGridViewSelectedRowCollection selectedRows = dgvAll.SelectedRows;
                ModifyDaily(selectedRows);
            }
        }
        private void btnModifyDidToday_Click(object sender, EventArgs e)
        {
            if (dgvDidToday.SelectedRows.Count > 0)
            {
                DataGridViewSelectedRowCollection selectedRows = dgvDidToday.SelectedRows;
                mFromWhere = 1;
                ModifyDaily(selectedRows);
            }
        }
        private void dgvDidToday_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDidToday.SelectedRows.Count > 0)
            {
                DataGridViewSelectedRowCollection selectedRows = dgvDidToday.SelectedRows;
                mFromWhere = 1;
                ModifyDaily(selectedRows);
            }
        }
        /// <summary>
        /// 修改日程事务
        /// </summary>
        /// <param name="selectedRows"></param>
        private void ModifyDaily(DataGridViewSelectedRowCollection selectedRows)
        {
            ClearForm();
            tbcContainer.SelectedTab = tabPageAdmin;
            DataRow dr = ((DataRowView)selectedRows[0].DataBoundItem).Row;
            DataTable newDataTable = dr.Table.Clone();
            dr = newDataTable.Rows.Add(dr.ItemArray);
            if (dr["D_REMIND_FLAG"].ToString() == "未处理")
            {
                dr["D_REMIND_FLAG"] = "1";
            }
            else
            {
                dr["D_REMIND_FLAG"] = "0";
            }
            switch (dr["D_LEVEL"].ToString())
            {
                case "特急":
                    dr["D_LEVEL"] = "0";
                    break;
                case "紧急":
                    dr["D_LEVEL"] = "1";
                    break;
                case "中等":
                    dr["D_LEVEL"] = "2";
                    break;
                case "缓慢":
                    dr["D_LEVEL"] = "3";
                    break;
                case "过期":
                    dr["D_LEVEL"] = "4";
                    break;
            }
            _dailyEntity = new DailyEntity(dr);
            if (_dailyEntity != null)
            {
                gbAddDaily.Text = "修改日程事务";
                cboLevel.SelectedValue = _dailyEntity.D_LEVEL;
                rtbContent.Text = _dailyEntity.D_CONTENT;
                txtMusicPath.Text = _dailyEntity.D_REMIND_MUSIC_PATH;
                _bClearForm = true;
                if (_dailyEntity.D_LUNAR_CALENDAR != null && _dailyEntity.D_LUNAR_CALENDAR != "")
                {
                    chkLunar.Checked = true;
                    dtpTime.Value = Convert.ToDateTime(_dailyEntity.D_LUNAR_CALENDAR);
                }
                else
                {
                    dtpTime.Value = Convert.ToDateTime(_dailyEntity.D_SOLAR_CALENDAR);
                }
                operType = 1;
                _bClearForm = false;

                btnDeleteDaily.Enabled = true;
                if (_dailyEntity.D_REMIND_FLAG == "0")
                {
                    chkRemindFlag.Checked = true;
                    cboLevel.Enabled = false;
                    rtbContent.Enabled = false;
                    txtMusicPath.Enabled = false;
                    btnBrowseFile.Enabled = false;
                    dtpTime.Enabled = false;
                    chkLunar.Enabled = false;
                }
                else
                {
                    btnModifyDaily.Enabled = true;
                }
                if (mFromWhere == 1)
                {
                    foreach (DataGridViewRow dgvRow in dgvAll.SelectedRows)
                    {
                        dgvRow.Selected = false;
                    }
                    foreach (DataGridViewRow dgvRow in dgvAll.Rows)
                    {
                        if (dgvRow.Cells[0].Value.ToString() == _dailyEntity.ID)
                        {
                            dgvRow.Selected = true;
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 清除填写内容
        /// </summary>
        private void ClearForm()
        {
            rtbContent.Enabled = true;
            txtMusicPath.Enabled = true;
            btnBrowseFile.Enabled = true;
            dtpTime.Enabled = true;
            chkLunar.Enabled = true;

            btnDeleteDaily.Enabled = false;
            btnModifyDaily.Enabled = false;
            chkRemindFlag.Checked = false;
            chkLunar.Checked = false;

            operType = 0;
            _bClearForm = true;
            dtpTime.Value = DateTime.Now;
            _bClearForm = false;
            txtMusicPath.Text = "";
            rtbContent.Text = "";
            cboLevel.SelectedValue = "2";
            gbAddDaily.Text = "新增日程事务";
            txtMusicPath.Text = GlobalVariable.DEFAULT_MUSIC_PATH;
        }
        /// <summary>
        /// 检验数据输入
        /// </summary>
        private bool IsValid()
        {
            if (rtbContent.Text == "")
            {
                MessageHandle.MessageError("事务内容不能为空！", "错误信息");
                rtbContent.Focus();
                return false;
            }
            if (txtMusicPath.Text == "")
            {
                MessageHandle.MessageError("提示音乐不能为空！", "错误信息");
                txtMusicPath.Focus();
                return false;
            }
            if (cboLevel.SelectedValue.ToString() == "4")
            {
                if (MessageBox.Show("事务时间已过期，继续添加请点击确定！", "警告信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    dtpTime.Focus();
                    return false;
                }
            }
            if (chkLunar.Checked)
            {
                try
                {
                    ChineseCalendarInfo.FromLunarDate(dtpTime.Value, false);
                }
                catch (Exception ex)
                {
                    MessageHandle.MessageError("错误：" + ex.Message + "！", "错误信息");
                    dtpTime.Focus();
                    return false;
                }
            }
            return true;
        }
        private void tbcContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            BusAdminDaily.UpdateDailyStateBySystem();
            if (tbcContainer.SelectedTab == tabPageToday)
            {
                BindToday();
                BindDidToday();
            }
            else if (tbcContainer.SelectedTab == tabPageAdmin)
            {
                BindAll();
            }
        }
        private void btnModifyDaily_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                _dailyEntity.D_CONTENT = rtbContent.Text;
                _dailyEntity.D_LEVEL = cboLevel.SelectedValue.ToString();
                _dailyEntity.D_REMIND_MUSIC_PATH = txtMusicPath.Text;
                string grade;
                if (chkLunar.Checked)
                {
                    _dailyEntity.D_LUNAR_CALENDAR = dtpTime.Value.ToString();
                    ChineseCalendarInfo cCalendar = ChineseCalendarInfo.FromLunarDate(dtpTime.Value.Date, false);
                    DateTime solar = cCalendar.SolarDate.Add(new TimeSpan(dtpTime.Value.Hour, dtpTime.Value.Minute, dtpTime.Value.Second));
                    _dailyEntity.D_SOLAR_CALENDAR = solar.ToString();
                    grade = BusAdminDaily.CalculateGrade(solar);
                    _dailyEntity.D_LEVEL = grade;
                }
                else
                {
                    _dailyEntity.D_LUNAR_CALENDAR = "";
                    _dailyEntity.D_SOLAR_CALENDAR = dtpTime.Value.ToString();
                    grade = BusAdminDaily.CalculateGrade(dtpTime.Value);
                    _dailyEntity.D_LEVEL = grade;
                }
                if (BusAdminDaily.ModifyDaily(_dailyEntity))
                {
                    if (MessageHandle.MessageInfo("恭喜，修改成功！", "提示信息") == DialogResult.OK)
                    {
                        BusAdminDaily.UpdateDailyStateBySystem();
                    }
                    if (mFromWhere == 1)
                    {
                        tbcContainer.SelectedTab = tabPageToday;
                        mFromWhere = 0;
                        foreach (DataGridViewRow dgvRow in dgvToday.SelectedRows)
                        {
                            dgvRow.Selected = false;
                        }
                        foreach (DataGridViewRow dgvRow in dgvToday.Rows)
                        {
                            if (dgvRow.Cells[0].Value.ToString() == _dailyEntity.ID)
                            {
                                dgvRow.Selected = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        BindAll();
                        dgvAll.Select();
                        foreach (DataGridViewRow dgvRow in dgvAll.SelectedRows)
                        {
                            dgvRow.Selected = false;
                        }
                        foreach (DataGridViewRow dgvRow in dgvAll.Rows)
                        {
                            if (dgvRow.Cells[0].Value.ToString() == _dailyEntity.ID)
                            {
                                dgvRow.Selected = true;
                                break;
                            }
                        }
                    }
                    ClearForm();
                    _dailyEntity = null;
                }
            }
        }
        private void btnDeleteDaily_Click(object sender, EventArgs e)
        {
            if (_dailyEntity.D_REMIND_FLAG == "1" && MessageHandle.MessageAsk("注意：该日程事务还没处理！\n确定要删除该日程事务？", "确定删除") == DialogResult.OK)
            {
                BusAdminDaily.DeleteDailyById(_dailyEntity.ID);
                ClearForm();
                _dailyEntity = null;
                MessageHandle.MessageInfo("恭喜，删除成功！", "提示信息");
                if (mFromWhere == 1)
                {
                    tbcContainer.SelectedTab = tabPageToday;
                    mFromWhere = 0;
                }
                else
                {
                    BindAll();
                    dgvAll.Select();
                }
            }
            else if (_dailyEntity.D_REMIND_FLAG == "0" && MessageHandle.MessageAsk("你确定要删除该日程事务？", "确定删除") == DialogResult.OK)
            {
                BusAdminDaily.DeleteDailyById(_dailyEntity.ID);
                ClearForm();
                _dailyEntity = null;
                MessageHandle.MessageInfo("恭喜，删除成功！", "提示信息");
                if (mFromWhere == 1)
                {
                    tbcContainer.SelectedTab = tabPageToday;
                    mFromWhere = 0;
                }
                else
                {
                    BindAll();
                    dgvAll.Select();
                }
            }
        }
        private void dgvAll_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && (e.Value != null && e.Value.ToString() == "特急"))
            {
                DataGridViewCellStyle style = e.CellStyle;
                style.ForeColor = Color.Red;
                e.CellStyle = style;
            }
            else if (e.ColumnIndex == 2 && (e.Value != null && e.Value.ToString() == "紧急"))
            {
                DataGridViewCellStyle style = e.CellStyle;
                style.ForeColor = Color.FromArgb(204, 102, 00);
                e.CellStyle = style;
            }
            else if (e.ColumnIndex == 2 && (e.Value != null && e.Value.ToString() == "过期"))
            {
                DataGridViewCellStyle style = e.CellStyle;
                style.ForeColor = Color.Green;
                e.CellStyle = style;
            }
            if (e.ColumnIndex == 5 && (e.Value != null && e.Value.ToString() == "未处理"))
            {
                DataGridViewCellStyle style = e.CellStyle;
                style.ForeColor = Color.Red;
                e.CellStyle = style;
            }
            if (e.ColumnIndex == 5 && (e.Value != null && e.Value.ToString() == "已处理"))
            {
                DataGridViewCellStyle style = e.CellStyle;
                style.ForeColor = Color.Green;
                e.CellStyle = style;
            }
        }

        private void dgvToday_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && (e.Value != null && e.Value.ToString() == "特急"))
            {
                DataGridViewCellStyle style = e.CellStyle;
                style.ForeColor = Color.Red;
                e.CellStyle = style;
            }
            else if (e.ColumnIndex == 2 && (e.Value != null && e.Value.ToString() == "紧急"))
            {
                DataGridViewCellStyle style = e.CellStyle;
                style.ForeColor = Color.FromArgb(204, 102, 00);
                e.CellStyle = style;
            }
            if (e.ColumnIndex == 5)
            {
                DataGridViewCellStyle style = e.CellStyle;
                style.ForeColor = Color.Red;
                e.CellStyle = style;
            }
        }

        private void dgvDidToday_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                DataGridViewCellStyle style = e.CellStyle;
                style.ForeColor = Color.Green;
                e.CellStyle = style;
            }
            if (e.ColumnIndex == 5)
            {
                DataGridViewCellStyle style = e.CellStyle;
                style.ForeColor = Color.Green;
                e.CellStyle = style;
            }
        }

        private void btnModifyDefault_Click(object sender, EventArgs e)
        {
            string fileName = Application.StartupPath + "\\Config.xml";
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(fileName);
            }
            catch (XmlException ex)
            {
                MessageHandle.MessageError(ex.Message, "错误信息");
                return;
            }
            //修改到配置文件
            bool bModifyDefault = true;
            string errMessage = "";
            XmlNode node = xml.SelectSingleNode("Root/MusicPath");
            if (node != null)
            {
                node.InnerText = txtDefaultMusicPath.Text;
            }
            else
            {
                errMessage += "配置文件节点：Root/MusicPath不存在。\n";
                bModifyDefault = false;
            }
            node = xml.SelectSingleNode("Root/Times");
            if (node != null)
            {
                node.InnerText = nudTimes.Value.ToString();
            }
            else
            {
                errMessage += "配置文件节点：Root/Times不存在。\n";
                bModifyDefault = false;
            }
            node = xml.SelectSingleNode("Root/Minutes");
            if (node != null)
            {
                node.InnerText = nudMinute.Value.ToString();
            }
            else
            {
                errMessage += "配置文件节点：Root/Minutes不存在。\n";
                bModifyDefault = false;
            }
            node = xml.SelectSingleNode("Root/RunBackground");
            if (node != null)
            {
                node.InnerText = chkRunBackground.Checked.ToString();
            }
            else
            {
                errMessage += "配置文件节点：Root/RunBackground不存在。\n";
                bModifyDefault = false;
            }
            xml.Save(fileName);
            //修改到注册表
            if (bModifyDefault)
            {

                if (chkRunOnStart.Checked)
                {
                    RegistryKey dailyRun = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    dailyRun.SetValue("daily", Application.StartupPath + "\\daily.exe");
                    dailyRun.Close();
                }
                else
                {
                    RegistryKey dailyRun = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                    string[] subkeyNames = dailyRun.GetValueNames();
                    foreach (string key in subkeyNames)
                    {
                        if (key.ToLower() == "daily")
                        {
                            dailyRun.DeleteValue("daily");
                            break;
                        }
                    }
                    dailyRun.Close();
                }
                try
                {
                    RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\景仁海\", true);
                    regKey.SetValue("firstRun", false);
                    regKey.Close();
                }
                catch
                { }
                GlobalVariable.DEFAULT_MUSIC_PATH = txtDefaultMusicPath.Text;
                GlobalVariable.TIMES = (int)nudTimes.Value;
                GlobalVariable.MINUTES = (int)nudMinute.Value;
                GlobalVariable.RUN_BACKGROUND = chkRunBackground.Checked;
                GlobalVariable.RUN_ON_START = chkRunOnStart.Checked;
                lbMinute.Visible = false;
                lbTimes.Visible = false;
                MessageHandle.MessageInfo("修改默认设置成功！", "提示信息");
            }
            else
            {
                MessageHandle.MessageError(errMessage, "错误信息");
            }
            LoadDefault();
        }

        private void miConfig_Click(object sender, EventArgs e)
        {
            tbcContainer.SelectedTab = tabPageConfig;
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void miAdminToday_Click(object sender, EventArgs e)
        {
            tbcContainer.SelectedTab = tabPageToday;
        }

        private void miAdminAll_Click(object sender, EventArgs e)
        {
            tbcContainer.SelectedTab = tabPageAdmin;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && !_bWillRunBackground)
            {
                if (MessageHandle.MessageAsk("确定要退出系统？", "确定退出") == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else
                {
                    RunBackgroundEventArgs runEventArgs = new RunBackgroundEventArgs(RunBackgroundState.Exit);
                    RunBackground(this, runEventArgs);
                }
            }
        }

        private void miRunBackground_Click(object sender, EventArgs e)
        {
            RunBackgroundEventArgs runEventArgs = new RunBackgroundEventArgs(RunBackgroundState.GoOn);
            _bWillRunBackground = true;
            RunBackground(this, runEventArgs);
        }

        private void dgvAll_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvAll.Rows.Count; i++)
            {
                dgvAll.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void dgvToday_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvToday.Rows.Count; i++)
            {
                dgvToday.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void dgvDidToday_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dgvDidToday.Rows.Count; i++)
            {
                dgvDidToday.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void btnAbandonModifyDefault_Click(object sender, EventArgs e)
        {
            if (MessageHandle.MessageAsk("确定要放弃该操作？", "确定放弃") == DialogResult.OK)
            {
                LoadDefault();
            }
        }

        private void btnAbandon_Click(object sender, EventArgs e)
        {
            if (MessageHandle.MessageAsk("确定要放弃该操作？", "确定放弃") == DialogResult.OK)
            {
                ClearForm();
                if (mFromWhere == 1)
                {
                    tbcContainer.SelectedTab = tabPageToday;
                    mFromWhere = 0;
                    foreach (DataGridViewRow dgvRow in dgvToday.SelectedRows)
                    {
                        dgvRow.Selected = false;
                    }
                    foreach (DataGridViewRow dgvRow in dgvToday.Rows)
                    {
                        if (dgvRow.Cells[0].Value.ToString() == _dailyEntity.ID)
                        {
                            dgvRow.Selected = true;
                            break;
                        }
                    }
                    foreach (DataGridViewRow dgvRow in dgvDidToday.SelectedRows)
                    {
                        dgvRow.Selected = false;
                    }
                    foreach (DataGridViewRow dgvRow in dgvDidToday.Rows)
                    {
                        if (dgvRow.Cells[0].Value.ToString() == _dailyEntity.ID)
                        {
                            dgvRow.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                miRunBackground_Click(null, null);
            }
        }

        private void nudTimes_Enter(object sender, EventArgs e)
        {
            lbTimes.Visible = true;
        }

        private void nudTimes_Leave(object sender, EventArgs e)
        {
            lbTimes.Visible = false;
        }

        private void nudMinute_Enter(object sender, EventArgs e)
        {
            lbMinute.Visible = true;
        }

        private void nudMinute_Leave(object sender, EventArgs e)
        {
            lbMinute.Visible = true;
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            ISRUN = false;
        }
    }
}