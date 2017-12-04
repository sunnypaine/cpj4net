/*********************************************************
 * 文 件 名: MSSqlHelper
 * 命名空间：CPJIT.Util.DataBaseUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/2 15:47:50
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace CPJIT.Library.Util.DataBaseUtil.Impl
{
    /// <summary>
    /// MSSQL数据库访问工具
    /// </summary>
    public class MsSQLHelper : IDBAccess
    {
        /// <summary>
        /// 连接数据库的字符串
        /// </summary>
        private readonly string conString = string.Empty;

        /// <summary>
        /// 与数据库交互（增删改查）
        /// </summary>
        /// <param name="datasource">数据库源</param>
        /// <param name="database">数据库名称</param>
        /// <param name="userId">用户名</param>
        /// <param name="password">密码</param>
        public MsSQLHelper(string datasource, string database, string userId, string password)
        {
            conString = string.Format("data source={0};Initial Catelog={1};User Id={2};Password={3};", datasource, database, userId, password);
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <returns></returns>
        public bool TestConnect()
        {
            using (SqlConnection cn = new SqlConnection(conString))
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
            using (SqlConnection cn = new SqlConnection(conString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand();
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
            using (SqlConnection cn = new SqlConnection(conString))
            {
                try
                {
                    SqlParameter[] sqlParameter = new SqlParameter[paras.Count];
                    int i = 0;
                    foreach (DictionaryEntry de in paras)
                    {
                        sqlParameter[i] = new SqlParameter(de.Key.ToString(), de.Value);
                        i++;
                    }

                    cn.Open();
                    SqlCommand cmd = new SqlCommand(cmdText, cn);
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
            using (SqlConnection cn = new SqlConnection(conString))
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmdText, cn);
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
            using (SqlConnection cn = new SqlConnection(conString))
            {
                try
                {
                    SqlParameter[] sqlParameter = new SqlParameter[paras.Count];
                    int i = 0;
                    foreach (DictionaryEntry de in paras)
                    {
                        sqlParameter[i] = new SqlParameter(de.Key.ToString(), de.Value);
                        i++;
                    }

                    cn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmdText, cn);
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

            SqlDataReader dr = null;
            SqlConnection cn = new SqlConnection(conString);
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand(cmdText, cn);
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

            SqlDataReader dr = null;
            SqlConnection cn = new SqlConnection(conString);
            try
            {
                SqlParameter[] sqlParameter = new SqlParameter[paras.Count];
                int i = 0;
                foreach (DictionaryEntry de in paras)
                {
                    sqlParameter[i] = new SqlParameter(de.Key.ToString(), de.Value);
                    i++;
                }

                cn.Open();
                SqlCommand cmd = new SqlCommand(cmdText, cn);
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
            using (SqlConnection cn = new SqlConnection(conString))
            {
                try
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(cmdText, cn);
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
            using (SqlConnection cn = new SqlConnection(conString))
            {
                try
                {
                    SqlParameter[] sqlParameter = new SqlParameter[paras.Count];
                    int i = 0;
                    foreach (DictionaryEntry de in paras)
                    {
                        sqlParameter[i] = new SqlParameter(de.Key.ToString(), de.Value);
                        i++;
                    }

                    cn.Open();
                    SqlCommand cmd = new SqlCommand(cmdText, cn);
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
