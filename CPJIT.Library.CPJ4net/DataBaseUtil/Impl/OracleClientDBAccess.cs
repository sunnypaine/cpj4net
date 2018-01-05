/*********************************************************
 * 文 件 名: OracleClientDBAccess
 * 命名空间：CPJIT.Library.CPJ4net.DataBaseUtil.Impl
 * 开发人员：SunnyPaine
 * 创建时间：2018/01/04 20:43:53
 * 描述说明： 
 * *******************************************************/

using Oracle.DataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CPJIT.Library.CPJ4net.DataBaseUtil.Impl
{
    /// <summary>
    /// Oracle数据库访问工具（依赖Oracle客户端应用）
    /// </summary>
    public class OracleClientDBAccess : IDBAccess
    {
        /// <summary>
        /// 连接数据库的字符串
        /// </summary>
        private readonly string conString = string.Empty;

        /// <summary>
        /// 与数据库交互（增删改查）
        /// </summary>
        /// <param name="serverName">服务名</param>
        /// <param name="userId">用户名</param>
        /// <param name="password">密码</param>
        public OracleClientDBAccess(string serverName, string userId, string password)
        {
            conString = string.Format("User Id={0};Password={1};Data Source={2}", userId, password, serverName);
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <returns></returns>
        public bool TestConnect()
        {
            using (OracleConnection cn = new OracleConnection(conString))
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
            using (OracleConnection cn = new OracleConnection(conString))
            {
                try
                {
                    cn.Open();
                    OracleCommand cmd = new OracleCommand();
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
            using (OracleConnection cn = new OracleConnection(conString))
            {
                try
                {
                    OracleParameter[] oracleParameter = new OracleParameter[paras.Count];
                    int i = 0;
                    foreach (DictionaryEntry de in paras)
                    {
                        oracleParameter[i] = new OracleParameter(de.Key.ToString(), de.Value);
                        i++;
                    }

                    cn.Open();
                    OracleCommand cmd = new OracleCommand(cmdText, cn);
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(oracleParameter);
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
            using (OracleConnection cn = new OracleConnection(conString))
            {
                try
                {
                    cn.Open();
                    OracleDataAdapter da = new OracleDataAdapter(cmdText, cn);
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
            using (OracleConnection cn = new OracleConnection(conString))
            {
                try
                {
                    OracleParameter[] oracleParameter = new OracleParameter[paras.Count];
                    int i = 0;
                    foreach (DictionaryEntry de in paras)
                    {
                        oracleParameter[i] = new OracleParameter(de.Key.ToString(), de.Value);
                        i++;
                    }

                    cn.Open();
                    OracleDataAdapter da = new OracleDataAdapter(cmdText, cn);
                    da.SelectCommand.CommandType = cmdType;
                    da.SelectCommand.Parameters.AddRange(oracleParameter);
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

            OracleDataReader dr = null;
            OracleConnection cn = new OracleConnection(conString);
            try
            {
                cn.Open();
                OracleCommand cmd = new OracleCommand(cmdText, cn);
                cmd.CommandType = cmdType;
                //CommandBehavior.CloseConnection暂时让OracleConnection对象不关闭，当OracleDataReader对象调用完毕也关闭的后才关闭连接
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

            OracleDataReader dr = null;
            OracleConnection cn = new OracleConnection(conString);
            try
            {
                OracleParameter[] oracleParameter = new OracleParameter[paras.Count];
                int i = 0;
                foreach (DictionaryEntry de in paras)
                {
                    oracleParameter[i] = new OracleParameter(de.Key.ToString(), de.Value);
                    i++;
                }

                cn.Open();
                OracleCommand cmd = new OracleCommand(cmdText, cn);
                cmd.CommandType = cmdType;
                cmd.Parameters.AddRange(oracleParameter);
                //CommandBehavior.CloseConnection暂时让OracleConnection对象不关闭，当OracleDataReader对象调用完毕也关闭的后才关闭连接
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
            using (OracleConnection cn = new OracleConnection(conString))
            {
                try
                {
                    cn.Open();
                    OracleCommand cmd = new OracleCommand(cmdText, cn);
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
            using (OracleConnection cn = new OracleConnection(conString))
            {
                try
                {
                    OracleParameter[] oracleParameter = new OracleParameter[paras.Count];
                    int i = 0;
                    foreach (DictionaryEntry de in paras)
                    {
                        oracleParameter[i] = new OracleParameter(de.Key.ToString(), de.Value);
                        i++;
                    }

                    cn.Open();
                    OracleCommand cmd = new OracleCommand(cmdText, cn);
                    cmd.CommandType = cmdType;
                    cmd.Parameters.AddRange(oracleParameter);
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
