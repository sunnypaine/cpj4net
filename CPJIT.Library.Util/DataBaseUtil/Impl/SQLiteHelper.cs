/*********************************************************
 * 文 件 名: SQLiteHelper
 * 命名空间：CPJIT.Util.DataBaseUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/2 16:17:08
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections;
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
    public class SQLiteHelper : IDBAccess
    {
        /// <summary>
        /// SQLIite数据库辅助工具类
        /// </summary>
        private readonly string conString = string.Empty;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dbPath">数据库文件（可读取的相对路径或者完整的路径）</param>
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
            if (string.IsNullOrWhiteSpace(cmdText) == true)
            {
                throw new ArgumentNullException("指定的参数cmdText不合法或无效。");
            }

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
        /// <param name="paras">表示参数的键值对（键：参数名称；值：参数值）</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteNonQuery(string cmdText, CommandType cmdType, Hashtable paras)
        {
            if (string.IsNullOrWhiteSpace(cmdText) == true)
            {
                throw new ArgumentNullException("指定的参数cmdText不合法或无效。");
            }
            if (paras == null || paras.Count <= 0)
            {
                throw new ArgumentNullException("指定的参数paras为null或者没有有效的值。");
            }

            int r;
            using (SQLiteConnection cn = new SQLiteConnection(conString))
            {
                try
                {
                    SQLiteParameter[] sqlParameter = new SQLiteParameter[paras.Count];
                    int i = 0;
                    foreach (DictionaryEntry de in paras)
                    {
                        sqlParameter[i] = new SQLiteParameter(de.Key.ToString(), de.Value);
                        i++;
                    }

                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(cmdText, cn);
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(sqlParameter);
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
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <returns>返回DataSet</returns>
        public DataSet ExecuteDateSet(string cmdText, CommandType cmdType)
        {
            if (string.IsNullOrWhiteSpace(cmdText) == true)
            {
                throw new ArgumentNullException("指定的参数cmdText不合法或无效。");
            }

            DataSet ds = new DataSet();
            using (SQLiteConnection cn = new SQLiteConnection(conString))
            {
                try
                {
                    cn.Open();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmdText, cn);
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
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <param name="paras">表示参数的键值对（键：参数名称；值：参数值）</param>
        /// <returns>返回DataSet</returns>
        public DataSet ExecuteDateSet(string cmdText, CommandType cmdType, Hashtable paras)
        {
            if (string.IsNullOrWhiteSpace(cmdText) == true)
            {
                throw new ArgumentNullException("指定的参数cmdText不合法或无效。");
            }
            if (paras == null || paras.Count <= 0)
            {
                throw new ArgumentNullException("指定的参数paras为null或者没有有效的值。");
            }

            DataSet ds = new DataSet();
            using (SQLiteConnection cn = new SQLiteConnection(conString))
            {
                try
                {
                    SQLiteParameter[] sqlParameter = new SQLiteParameter[paras.Count];
                    int i = 0;
                    foreach (DictionaryEntry de in paras)
                    {
                        sqlParameter[i] = new SQLiteParameter(de.Key.ToString(), de.Value);
                        i++;
                    }

                    cn.Open();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmdText, cn);
                    da.SelectCommand.CommandType = cmdType;
                    da.SelectCommand.Parameters.AddRange(sqlParameter);
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
        public IDataReader ExecuteDataReader(string cmdText, CommandType cmdType)
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
        /// <param name="paras">表示参数的键值对（键：参数名称；值：参数值）</param>
        /// <returns>返回SQLiteDatareader对象</returns>
        public IDataReader ExecuteDataReader(string cmdText, CommandType cmdType, Hashtable paras)
        {
            if (string.IsNullOrWhiteSpace(cmdText) == true)
            {
                throw new ArgumentNullException("指定的参数cmdText不合法或无效。");
            }
            if (paras == null || paras.Count <= 0)
            {
                throw new ArgumentNullException("指定的参数paras为null或者没有有效的值。");
            }

            SQLiteDataReader dr = null;
            SQLiteConnection cn = new SQLiteConnection(conString);
            try
            {
                SQLiteParameter[] sqlParameter = new SQLiteParameter[paras.Count];
                int i = 0;
                foreach (DictionaryEntry de in paras)
                {
                    sqlParameter[i] = new SQLiteParameter(de.Key.ToString(), de.Value);
                    i++;
                }

                cn.Open();
                SQLiteCommand cmd = new SQLiteCommand(cmdText, cn);
                cmd.CommandType = cmdType;
                cmd.Parameters.AddRange(sqlParameter);
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
            if (string.IsNullOrWhiteSpace(cmdText) == true)
            {
                throw new ArgumentNullException("指定的参数cmdText不合法或无效。");
            }

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
        /// <param name="paras">表示参数的键值对（键：参数名称；值：参数值）</param>
        /// <returns>返回object类型的字段变量</returns>
        public object ExecuteScalar(string cmdText, CommandType cmdType, Hashtable paras)
        {
            if (string.IsNullOrWhiteSpace(cmdText) == true)
            {
                throw new ArgumentNullException("指定的参数cmdText不合法或无效。");
            }
            if (paras == null || paras.Count <= 0)
            {
                throw new ArgumentNullException("指定的参数paras为null或者没有有效的值。");
            }

            object o;
            using (SQLiteConnection cn = new SQLiteConnection(conString))
            {
                try
                {
                    SQLiteParameter[] sqlParameter = new SQLiteParameter[paras.Count];
                    int i = 0;
                    foreach (DictionaryEntry de in paras)
                    {
                        sqlParameter[i] = new SQLiteParameter(de.Key.ToString(), de.Value);
                        i++;
                    }

                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(cmdText, cn);
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(sqlParameter);
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
