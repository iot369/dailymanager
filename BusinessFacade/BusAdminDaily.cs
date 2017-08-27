using System;
using System.Collections.Generic;
using System.Text;
using daily.Entity;
using daily.Global;
using System.Data;
using System.Data.OleDb;

namespace daily.BusinessFacade
{
    public class BusAdminDaily
    {
        /// <summary>
        /// 添加一个日程事务
        /// </summary>
        /// <param name="dailyEntity">日程事务实体</param>
        /// <returns></returns>
        public static bool AddDaily(DailyEntity dailyEntity)
        {
            string sql = "";
            if (dailyEntity.D_LUNAR_CALENDAR == null || dailyEntity.D_LUNAR_CALENDAR == "")
            {
                sql = "insert into daily(D_SOLAR_CALENDAR,D_CONTENT,D_REMIND_MUSIC_PATH,D_LEVEL) values(#{0}#,'{1}','{2}',{3})";
                sql = string.Format(sql, dailyEntity.D_SOLAR_CALENDAR, dailyEntity.D_CONTENT, dailyEntity.D_REMIND_MUSIC_PATH, dailyEntity.D_LEVEL);
            }
            else
            {
                sql = "insert into daily(D_SOLAR_CALENDAR,D_LUNAR_CALENDAR,D_CONTENT,D_REMIND_MUSIC_PATH,D_LEVEL) values(#{0}#,#{1}#,'{2}','{3}',{4})";
                sql = string.Format(sql, dailyEntity.D_SOLAR_CALENDAR, dailyEntity.D_LUNAR_CALENDAR, dailyEntity.D_CONTENT, dailyEntity.D_REMIND_MUSIC_PATH, dailyEntity.D_LEVEL);
            }
            if (DBManager.ExecuteNonQuery(sql) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 修改日程事务
        /// </summary>
        /// <param name="dailyEntity">日程事务实体</param>
        /// <returns></returns>
        public static bool ModifyDaily(DailyEntity dailyEntity)
        {
            string sql = "";
            if (dailyEntity.D_LUNAR_CALENDAR == null || dailyEntity.D_LUNAR_CALENDAR == "")
            {
                sql = "update daily set D_SOLAR_CALENDAR=#{0}#,D_LUNAR_CALENDAR=null,D_CONTENT='{1}',D_REMIND_MUSIC_PATH='{2}',D_LEVEL={3} where ID={4}";
                sql = string.Format(sql, dailyEntity.D_SOLAR_CALENDAR, dailyEntity.D_CONTENT, dailyEntity.D_REMIND_MUSIC_PATH, dailyEntity.D_LEVEL, dailyEntity.ID);
            }
            else
            {
                sql = "update daily set D_SOLAR_CALENDAR=#{0}#,D_LUNAR_CALENDAR=#{1}#,D_CONTENT='{2}',D_REMIND_MUSIC_PATH='{3}',D_LEVEL={4} where ID={5}";
                sql = string.Format(sql, dailyEntity.D_SOLAR_CALENDAR, dailyEntity.D_LUNAR_CALENDAR, dailyEntity.D_CONTENT, dailyEntity.D_REMIND_MUSIC_PATH, dailyEntity.D_LEVEL, dailyEntity.ID);
            }
            if (DBManager.ExecuteNonQuery(sql) == 1)
            {
                InitTodayUnDoDaily();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 根据编号删除日程
        /// </summary>
        /// <param name="id">日程编号</param>
        /// <returns></returns>
        public static bool DeleteDailyById(string id)
        {
            string sql = "delete from daily where id=" + id;
            if (DBManager.ExecuteNonQuery(sql) > -1)
            {
                InitTodayUnDoDaily();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除当天日程事务
        /// </summary>
        public static bool DeleteDailyOfToday()
        {
            string sql = "delete from daily where #{0}#<=D_SOLAR_CALENDAR and D_SOLAR_CALENDAR<#{1}# and D_REMIND_FLAG=0";
            sql = string.Format(sql, DateTime.Today.ToString(), DateTime.Today.AddDays(1).ToString());
            if (DBManager.ExecuteNonQuery(sql) > -1)
            {
                InitTodayUnDoDaily();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除所有已处理日程事务
        /// </summary>
        public static bool DeleteDidDailyOfAll()
        {
            string sql = "delete from daily where D_REMIND_FLAG=0";
            sql = string.Format(sql, DateTime.Today.ToString(), DateTime.Today.AddDays(1).ToString());
            if (DBManager.ExecuteNonQuery(sql) > -1)
            {
                InitTodayUnDoDaily();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除所有事务
        /// </summary>
        /// <returns></returns>
        public static bool DeleteDailyOfAll()
        {
            string sql = "delete from daily";
            if (DBManager.ExecuteNonQuery(sql) > -1)
            {
                InitTodayUnDoDaily();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 查询当天日程事务
        /// </summary>
        /// <param name="isDid">是否处理过</param>
        /// <returns></returns>
        public static DataSet QueryDailyOfToday(bool isDid)
        {
            string sql = "";
            if (isDid)
            {
                sql = "select ID,D_SOLAR_CALENDAR,D_LUNAR_CALENDAR,D_CONTENT,D_REMIND_MUSIC_PATH,"
                   + "iif(D_LEVEL=0,'特急',iif(D_LEVEL=1,'紧急',iif(D_LEVEL=2,'中等',iif(D_LEVEL=3,'缓慢',iif(D_LEVEL=4,'过期','未知'))))) as D_LEVEL,"
                   + "iif(D_REMIND_FLAG=0,'已处理','未处理') as D_REMIND_FLAG "
                   + "from daily where #{0}#<=D_SOLAR_CALENDAR and D_SOLAR_CALENDAR<#{1}# and D_REMIND_FLAG=0 order by D_LEVEL asc,D_SOLAR_CALENDAR asc";
            }
            else
            {
                sql = "select ID,D_SOLAR_CALENDAR,D_LUNAR_CALENDAR,D_CONTENT,D_REMIND_MUSIC_PATH,"
                  + "iif(D_LEVEL=0,'特急',iif(D_LEVEL=1,'紧急',iif(D_LEVEL=2,'中等',iif(D_LEVEL=3,'缓慢',iif(D_LEVEL=4,'过期','未知'))))) as D_LEVEL,"
                  + "iif(D_REMIND_FLAG=0,'已处理','未处理') as D_REMIND_FLAG "
                  + "from daily where  #{0}#<=D_SOLAR_CALENDAR and D_SOLAR_CALENDAR<#{1}# and D_REMIND_FLAG=1 order by D_LEVEL asc,D_SOLAR_CALENDAR asc";
            }
            sql = string.Format(sql, DateTime.Today.ToString(), DateTime.Today.AddDays(1).ToString());
            return DBManager.Query(sql);
        }
        /// <summary>
        /// 查询返回所有日程的DataSet
        /// </summary>
        /// <returns></returns>
        public static DataSet QueryDailyOfAll()
        {
            string sql = "select ID,D_SOLAR_CALENDAR,D_LUNAR_CALENDAR,D_CONTENT,D_REMIND_MUSIC_PATH,"
                    + "iif(D_LEVEL=0,'特急',iif(D_LEVEL=1,'紧急',iif(D_LEVEL=2,'中等',iif(D_LEVEL=3,'缓慢',iif(D_LEVEL=4,'过期','未知'))))) as D_LEVEL,"
                    + "iif(D_REMIND_FLAG=0,'已处理','未处理') as D_REMIND_FLAG "
                    + "from daily order by D_REMIND_FLAG  desc,D_LEVEL asc,D_SOLAR_CALENDAR asc";
            return DBManager.Query(sql);
        }
        /// <summary>
        /// 更新处理状态
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="strRemindFlag">状态标志</param>
        /// <returns></returns>
        public static bool UpdateDaliyState(string id, string strRemindFlag)
        {
            string sql = "";
            if (strRemindFlag == "0")
            {
                sql = "Update daily Set D_REMIND_FLAG=" + strRemindFlag + ",D_LEVEL=4 where ID=" + id;
            }
            else
            {
                sql = "Update daily Set D_REMIND_FLAG=" + strRemindFlag + " where ID=" + id;
            }
            if (DBManager.ExecuteNonQuery(sql) == 1)
            {
                InitTodayUnDoDaily();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 自动更新日程状态
        /// </summary>
        public static void UpdateDailyStateBySystem()
        {
            string sql = "select * from daily where D_SOLAR_CALENDAR<=#{0}# and D_REMIND_FLAG=1";
            sql = string.Format(sql, DateTime.Now.ToString());
            DataSet ds = DBManager.Query(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (!GlobalVariable.HASH_BEGIN_DOING_DAILY.ContainsKey(dr["ID"].ToString()))
                {
                    dr["D_REMIND_FLAG"] = "0";
                    dr["D_LEVEL"] = "4";
                }
            }
            if (ds.HasChanges())
            {
                DBManager.UpdateDataSet(sql, ds.GetChanges());
            }
            ds.AcceptChanges();
            InitTodayUnDoDaily();
        }
        /// <summary>
        /// 自动更新日程等级
        /// </summary>
        public static void UpGradeBySystem()
        {
            string sql = "select * from daily where D_REMIND_FLAG=1";
            string connString = DBManager.strConnectionString;
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connString);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            DateTime nowTime = DateTime.Now;
            TimeSpan timeSpan = new TimeSpan();
            int minutes = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["D_LEVEL"].ToString() != "0")
                {
                    timeSpan = Convert.ToDateTime(dr["D_SOLAR_CALENDAR"]).Subtract(nowTime);
                    minutes = (int)timeSpan.TotalMinutes;
                    if (minutes <= 30)
                    {
                        dr["D_LEVEL"] = "0";
                    }
                    else if (30 < minutes && minutes <= 60)
                    {
                        dr["D_LEVEL"] = "1";
                    }
                    else if (60 < minutes && minutes <= 90)
                    {
                        dr["D_LEVEL"] = "2";
                    }
                    else if (90 < minutes)
                    {
                        dr["D_LEVEL"] = "3";
                    }
                }
            }
            if (ds.HasChanges())
            {
                OleDbCommandBuilder builder = new OleDbCommandBuilder();
                builder.DataAdapter = dataAdapter;
                dataAdapter.UpdateCommand = builder.GetUpdateCommand();
                dataAdapter.Update(ds.GetChanges());
            }
            ds.AcceptChanges();
            sql = "select ID,D_SOLAR_CALENDAR,D_LUNAR_CALENDAR,D_CONTENT,D_REMIND_MUSIC_PATH,"
                  + "iif(D_LEVEL=0,'特急',iif(D_LEVEL=1,'紧急',iif(D_LEVEL=2,'中等',iif(D_LEVEL=3,'缓慢',iif(D_LEVEL=4,'过期','未知'))))) as D_LEVEL,"
                  + "iif(D_REMIND_FLAG=0,'已处理','未处理') as D_REMIND_FLAG "
                  + "from daily where  #{0}#<=D_SOLAR_CALENDAR and D_SOLAR_CALENDAR<#{1}# and D_REMIND_FLAG=1 order by D_LEVEL asc,D_SOLAR_CALENDAR asc";
            ds = new DataSet();
            dataAdapter.Fill(ds);
            if (ds != null)
            {
                //锁定LST_TODAY_UNDO_DAILY，防止主线程调用
                lock (GlobalVariable.LST_TODAY_UNDO_DAILY)
                {
                    GlobalVariable.LST_TODAY_UNDO_DAILY.Clear();
                    DailyEntity dailyEntity;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dailyEntity = new DailyEntity(dr);
                        GlobalVariable.LST_TODAY_UNDO_DAILY.Add(dailyEntity);
                    }
                }
            }
        }
        /// <summary>
        /// 计算日程等级
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static string CalculateGrade(DateTime time)
        {
            DateTime nowTime = DateTime.Now;
            TimeSpan timeSpan = time.Subtract(nowTime);
            int minutes = (int)timeSpan.TotalMinutes;
            string grade = null;
            if (minutes <= 30)
            {
                grade = "0";
            }
            else if (30 < minutes && minutes <= 60)
            {
                grade = "1";
            }
            else if (60 < minutes && minutes <= 90)
            {
                grade = "2";
            }
            else if (90 < minutes)
            {
                grade = "3";
            }
            return grade;
        }
        /// <summary>
        /// 初始化等级下拉列表
        /// </summary>
        /// <param name="cmbLevel"></param>
        public static void InitLevelComboBox(System.Windows.Forms.ComboBox cmbLevel)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("display");
            dt.Columns.Add("value");
            DataRow dr = null;
            dr = dt.NewRow();
            dr[0] = GetLevelName("0");
            dr[1] = "0";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = GetLevelName("1");
            dr[1] = "1";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = GetLevelName("2");
            dr[1] = "2";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = GetLevelName("3");
            dr[1] = "3";
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr[0] = GetLevelName("4");
            dr[1] = "4";
            dt.Rows.Add(dr);
            cmbLevel.DataSource = dt;
            cmbLevel.DisplayMember = "display";
            cmbLevel.ValueMember = "value";
            cmbLevel.SelectedValue = "2";
        }
        /// <summary>
        /// 根据等级返回等级名称
        /// </summary>
        /// <param name="strLevel">等级编号</param>
        /// <returns></returns>
        public static string GetLevelName(string strLevel)
        {
            string strLevelName = null;
            switch (strLevel)
            {
                case "0":
                    strLevelName = "特急";
                    break;
                case "1":
                    strLevelName = "紧急";
                    break;
                case "2":
                    strLevelName = "中等";
                    break;
                case "3":
                    strLevelName = "缓慢";
                    break;
                case "4":
                    strLevelName = "过期";
                    break;
                default:
                    strLevelName = "未知";
                    break;
            }
            return strLevelName;
        }
        /// <summary>
        /// 构造当日未完成事务
        /// </summary>
        public static void InitTodayUnDoDaily()
        {
            DataSet ds = QueryDailyOfToday(false);
            if (ds != null)
            {
                //锁定LST_TODAY_UNDO_DAILY,防止自动提升线程调用
                lock (GlobalVariable.LST_TODAY_UNDO_DAILY)
                {
                    GlobalVariable.LST_TODAY_UNDO_DAILY.Clear();
                    DailyEntity dailyEntity;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        dailyEntity = new DailyEntity(dr);
                        GlobalVariable.LST_TODAY_UNDO_DAILY.Add(dailyEntity);
                    }
                }
            }
        }
    }
}
