/*********************************************************
 * 文 件 名: SQLiteHelper
 * 命名空间：CPJIT.Util.DataBaseUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/2 16:17:08
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.Util.DataBaseUtil.Impl
{
    /// <summary>
    /// SQLite数据库访问工具
    /// </summary>
    public class SQLiteHelper
    {
        /// <summary>
        /// SQLIite数据库辅助工具类
        /// </summary>
        private readonly string conString = string.Empty;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dbPath">数据库文件</param>
        public SQLiteHelper(string dbPath)
        {
            conString = string.Format("Data Source={0};Version=3;", dbPath);
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <returns></returns>
        public bool TestConnect()
        {
            using (SQLiteConnection cn = new SQLiteConnection(conString))
            {
                try
                {
                    cn.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    cn.Close();
                }
            }
        }

        /// <summary>
        /// 执行增、删、改的SQL语句或者存储过程，返回受影响的行数
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string cmdText, CommandType cmdType)
        {
            int r;
            using (SQLiteConnection cn = new SQLiteConnection(conString))
            {
                try
                {
                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = cmdText;
                    cmd.CommandType = cmdType;

                    r = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return r;
        }

        /// <summary>
        /// 执行带参数的增、删、改的SQL语句或者存储过程，返回受影响的行数
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <param name="paras">数组参数类型(向该方法添加任意个参数)</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string cmdText, CommandType cmdType, SQLiteParameter[] paras)
        {
            int r;
            using (SQLiteConnection cn = new SQLiteConnection(conString))
            {
                try
                {
                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(cmdText, cn);
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(paras);
                    r = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return r;
        }

        /// <summary>
        /// 执行SQL查询语句或者存储过程，返回DataSet
        /// </summary>
        /// <param name="cmdTex">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <returns>返回DataSet</returns>
        public DataSet ExecuteDateSet(string cmdTex, CommandType cmdType)
        {
            DataSet ds = new DataSet();
            using (SQLiteConnection cn = new SQLiteConnection(conString))
            {
                try
                {
                    cn.Open();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmdTex, cn);
                    da.SelectCommand.CommandType = cmdType;
                    da.Fill(ds);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行带参数的SQL查询语句或者存储过程，返回DataSet
        /// </summary>
        /// <param name="cmdTex">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <param name="paras">数组参数类型(向该方法添加任意个参数)</param>
        /// <returns>返回DataSet</returns>
        public DataSet ExecuteDateSet(string cmdTex, CommandType cmdType, SQLiteParameter[] paras)
        {
            DataSet ds = new DataSet();
            using (SQLiteConnection cn = new SQLiteConnection(conString))
            {
                try
                {
                    cn.Open();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmdTex, cn);
                    da.SelectCommand.CommandType = cmdType;
                    da.SelectCommand.Parameters.AddRange(paras);
                    da.Fill(ds);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return ds;
        }

        /// <summary>
        /// 执行SQL查询语句或者存储过程，返回SQLiteDatareader对象
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <returns>返回SQLiteDatareader对象</returns>
        public SQLiteDataReader ExecuteDataReader(string cmdText, CommandType cmdType)
        {
            SQLiteDataReader dr = null;
            SQLiteConnection cn = new SQLiteConnection(conString);
            try
            {
                cn.Open();
                SQLiteCommand cmd = new SQLiteCommand(cmdText, cn);
                cmd.CommandType = cmdType;
                //CommandBehavior.CloseConnection暂时让SQLiteConnection对象不关闭，当SQLiteDataReader对象调用完毕也关闭的后才关闭连接
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dr;
        }

        /// <summary>
        /// 执行带参数的SQL查询语句或者存储过程，返回SQLiteDatareader对象
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <param name="paras">数组参数类型(向该方法添加任意个参数</param>
        /// <returns>返回SQLiteDatareader对象</returns>
        public SQLiteDataReader ExecuteDataReader(string cmdText, CommandType cmdType, SQLiteParameter[] paras)
        {
            SQLiteDataReader dr = null;
            SQLiteConnection cn = new SQLiteConnection(conString);
            try
            {
                cn.Open();
                SQLiteCommand cmd = new SQLiteCommand(cmdText, cn);
                cmd.CommandType = cmdType;
                cmd.Parameters.AddRange(paras);
                //CommandBehavior.CloseConnection暂时让SQLiteConnection对象不关闭，当SQLiteDataReader对象调用完毕也关闭的后才关闭连接
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dr;
        }

        /// <summary>
        /// 执行查找字段的SQL查询语句或者存储过程，返回object类型的字段变量
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <returns>返回object类型的字段变量</returns>
        public object ExecuteScalar(string cmdText, CommandType cmdType)
        {
            object o;
            using (SQLiteConnection cn = new SQLiteConnection(conString))
            {
                try
                {
                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(cmdText, cn);
                    cmd.CommandType = cmdType;
                    o = cmd.ExecuteScalar();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return o;
        }

        /// <summary>
        /// 执行查找字段的SQL查询语句或者存储过程，返回object类型的字段变量
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <param name="paras">数组参数类型(向该方法添加任意个参数</param>
        /// <returns>返回object类型的字段变量</returns>
        public object ExecuteScalar(string cmdText, CommandType cmdType, SQLiteParameter[] paras)
        {
            object o;
            using (SQLiteConnection cn = new SQLiteConnection(conString))
            {
                try
                {
                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(cmdText, cn);
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(paras);
                    o = cmd.ExecuteScalar();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return o;
        }
    }
}
