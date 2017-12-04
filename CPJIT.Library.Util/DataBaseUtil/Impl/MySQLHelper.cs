/*********************************************************
 * 文 件 名: MySQLHelper
 * 命名空间：CPJIT.Util.DataBaseUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/2 15:53:07
 * 描述说明： 
 * *******************************************************/

using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.Util.DataBaseUtil.Impl
{
    /// <summary>
    /// MYSQL数据库访问工具
    /// </summary>
    public class MySQLHelper : IDBAccess
    {
        /// <summary>
        /// 连接数据库的字符串
        /// </summary>
        private readonly string conString = string.Empty;

        /// <summary>
        /// 与数据库交互（增删改查）
        /// </summary>
        /// <param name="datasource">数据源（IP地址）</param>
        /// <param name="port">端口</param>
        /// <param name="database">数据库名称</param>
        /// <param name="userId">用户名</param>
        /// <param name="password">密码</param>
        public MySQLHelper(string datasource, int port, string database, string userId, string password)
        {
            conString = string.Format("Data Source={0};port={1};Initial Catalog={2};user id={3};password={4};",
                datasource, port, database, userId, password);
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <returns></returns>
        public bool TestConnect()
        {
            using (MySqlConnection cn = new MySqlConnection(conString))
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
            using (MySqlConnection cn = new MySqlConnection(conString))
            {
                try
                {
                    cn.Open();
                    MySqlCommand cmd = new MySqlCommand();
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
            using (MySqlConnection cn = new MySqlConnection(conString))
            {
                try
                {
                    MySqlParameter[] sqlParameter = new MySqlParameter[paras.Count];
                    int i = 0;
                    foreach (DictionaryEntry de in paras)
                    {
                        sqlParameter[i] = new MySqlParameter(de.Key.ToString(), de.Value);
                        i++;
                    }

                    cn.Open();
                    MySqlCommand cmd = new MySqlCommand(cmdText, cn);
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
            using (MySqlConnection cn = new MySqlConnection(conString))
            {
                try
                {
                    cn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmdText, cn);
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
            using (MySqlConnection cn = new MySqlConnection(conString))
            {
                try
                {
                    MySqlParameter[] sqlParameter = new MySqlParameter[paras.Count];
                    int i = 0;
                    foreach (DictionaryEntry de in paras)
                    {
                        sqlParameter[i] = new MySqlParameter(de.Key.ToString(), de.Value);
                        i++;
                    }

                    cn.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmdText, cn);
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
        /// 执行SQL查询语句或者存储过程，返回SqlDatareader对象
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <returns>返回SqlDatareader对象</returns>
        public IDataReader ExecuteDataReader(string cmdText, CommandType cmdType)
        {
            if (string.IsNullOrWhiteSpace(cmdText) == true)
            {
                throw new ArgumentNullException("指定的参数cmdText不合法或无效。");
            }

            MySqlDataReader dr = null;
            MySqlConnection cn = new MySqlConnection(conString);
            try
            {
                cn.Open();
                MySqlCommand cmd = new MySqlCommand(cmdText, cn);
                cmd.CommandType = cmdType;
                //CommandBehavior.CloseConnection暂时让SqlConnection对象不关闭，当SqlDataReader对象调用完毕也关闭的后才关闭连接
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dr;
        }

        /// <summary>
        /// 执行带参数的SQL查询语句或者存储过程，返回SqlDatareader对象
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <param name="paras">表示参数的键值对（键：参数名称；值：参数值）</param>
        /// <returns>返回SqlDatareader对象</returns>
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

            MySqlDataReader dr = null;
            MySqlConnection cn = new MySqlConnection(conString);
            try
            {
                MySqlParameter[] sqlParameter = new MySqlParameter[paras.Count];
                int i = 0;
                foreach (DictionaryEntry de in paras)
                {
                    sqlParameter[i] = new MySqlParameter(de.Key.ToString(), de.Value);
                    i++;
                }

                cn.Open();
                MySqlCommand cmd = new MySqlCommand(cmdText, cn);
                cmd.CommandType = cmdType;
                cmd.Parameters.AddRange(sqlParameter);
                //CommandBehavior.CloseConnection暂时让SqlConnection对象不关闭，当SqlDataReader对象调用完毕也关闭的后才关闭连接
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
            using (MySqlConnection cn = new MySqlConnection(conString))
            {
                try
                {
                    cn.Open();
                    MySqlCommand cmd = new MySqlCommand(cmdText, cn);
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
            using (MySqlConnection cn = new MySqlConnection(conString))
            {
                try
                {
                    MySqlParameter[] sqlParameter = new MySqlParameter[paras.Count];
                    int i = 0;
                    foreach (DictionaryEntry de in paras)
                    {
                        sqlParameter[i] = new MySqlParameter(de.Key.ToString(), de.Value);
                        i++;
                    }

                    cn.Open();
                    MySqlCommand cmd = new MySqlCommand(cmdText, cn);
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
