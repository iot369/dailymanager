using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;

namespace daily.Global
{
    public class DBManager
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string strConnectionString = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + Application.StartupPath + "\\DataBase\\daily.mdb;";
        private static OleDbConnection oleDbConn = new OleDbConnection(strConnectionString);
        private static OleDbConnection GetOledbConn()
        {
            if (oleDbConn == null)
            {
                oleDbConn = new OleDbConnection(strConnectionString);
            }
            return oleDbConn;
        }
        /// <summary>
        /// 以sql语句填充DataSet的第一张表（Table1）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet Query(string sql)
        {
            //锁上oleDbConn，保证每时刻只有一个线程使用oleDbConn。
            lock (oleDbConn)
            {
                OleDbDataAdapter myAdapter = new OleDbDataAdapter(sql, oleDbConn);
                DataSet ds = new DataSet();
                try
                {
                    myAdapter.Fill(ds, "Table1");
                }
                catch (OleDbException ex)
                {
                    MessageHandle.MessageError(ex.ToString(), "错误信息");
                }
                return ds;
            }
        }
        public static bool UpdateDataSet(string sql, DataSet ds)
        {
            lock (oleDbConn)
            {
                oleDbConn.Open();
                OleDbTransaction trans = oleDbConn.BeginTransaction();
                try
                {
                    OleDbDataAdapter myAdapter = new OleDbDataAdapter(sql, trans.Connection);
                    myAdapter.ContinueUpdateOnError = false;
                    myAdapter.SelectCommand = new OleDbCommand();
                    myAdapter.SelectCommand.Connection = trans.Connection;
                    myAdapter.SelectCommand.CommandText = sql;
                    OleDbCommandBuilder builder = new OleDbCommandBuilder();
                    builder.DataAdapter = myAdapter;
                    builder.ConflictOption = ConflictOption.OverwriteChanges;
                    myAdapter.SelectCommand.Transaction = trans;
                    myAdapter.InsertCommand = builder.GetInsertCommand();
                    myAdapter.InsertCommand.Transaction = trans;
                    myAdapter.DeleteCommand = builder.GetDeleteCommand();
                    myAdapter.DeleteCommand.Transaction = trans;
                    myAdapter.UpdateCommand = builder.GetUpdateCommand();
                    myAdapter.UpdateCommand.Transaction = trans;
                    string srcTable = ds.Tables[0].TableName;
                    myAdapter.Update(ds, srcTable);
                    trans.Commit();
                }
                catch (OleDbException ex)
                {
                    trans.Rollback();
                    MessageHandle.MessageError(ex.ToString(), "错误信息");
                    return false;
                }
                finally
                {
                    oleDbConn.Close();
                }
                return true;
            }
        }
        /// <summary>
        /// 执行一个sql语句，返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql)
        {
            int count = 0;
            oleDbConn.Open();
            OleDbTransaction trans = oleDbConn.BeginTransaction();
            try
            {
                OleDbCommand myCmd = new OleDbCommand(sql, oleDbConn, trans);
                count = myCmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (OleDbException ex)
            {
                trans.Rollback();
                MessageHandle.MessageError(ex.ToString(), "错误信息");
            }
            finally
            {
                oleDbConn.Close();
            }
            return count;
        }
    }
}
