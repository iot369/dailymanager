using System;
using System.Collections.Generic;
using System.Text;
using daily.Global;
using System.Collections;
using System.Data;

namespace daily.Entity
{
    public class DailyEntity
    {
        #region 成员变量
        private string _id;
        private string _content;
        private string _solar;
        private string _lunar;
        private string _level;
        private string _remind_flag;
        private string _remind_music_path;
        #endregion

        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 日程内容
        /// </summary>
        public string D_CONTENT
        {
            get { return _content; }
            set { _content = value; }
        }
        /// <summary>
        /// 提醒音乐路径
        /// </summary>
        public string D_REMIND_MUSIC_PATH
        {
            get { return _remind_music_path; }
            set { _remind_music_path = value; }
        }
        /// <summary>
        /// 太阳历
        /// </summary>
        public string D_SOLAR_CALENDAR
        {
            get { return _solar; }
            set { _solar = value; }
        }
        /// <summary>
        /// 太阴历
        /// </summary>
        public string D_LUNAR_CALENDAR
        {
            get { return _lunar; }
            set { _lunar = value; }
        }
        /// <summary>
        /// 日程等级
        /// </summary>
        public string D_LEVEL
        {
            get { return _level; }
            set { _level = value; }
        }
        /// <summary>
        /// 提醒标志
        /// </summary>
        public string D_REMIND_FLAG
        {
            get { return _remind_flag; }
            set { _remind_flag = value; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DailyEntity()
        {
        }
        public DailyEntity(DataRow dr)
        {
            ID = dr["ID"].ToString();
            D_SOLAR_CALENDAR = dr["D_SOLAR_CALENDAR"].ToString();
            D_LUNAR_CALENDAR = dr["D_LUNAR_CALENDAR"].ToString();
            D_CONTENT = dr["D_CONTENT"].ToString();
            D_REMIND_MUSIC_PATH = dr["D_REMIND_MUSIC_PATH"].ToString();
            D_LEVEL = dr["D_LEVEL"].ToString();
            D_REMIND_FLAG = dr["D_REMIND_FLAG"].ToString();
        }
        #endregion

        #region 静态函数
        /// <summary>
        /// 根据太阳历得到太阴历
        /// </summary>
        /// <param name="solarDateTime">阳历</param>
        /// <returns>阴历</returns>
        public static DateTime GetLunarBySolar(DateTime solarDateTime)
        {
            return DateTime.Today;
        }
        #endregion
    }
}
