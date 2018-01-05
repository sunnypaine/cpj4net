using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CPJIT.Library.CPJ4net.DataBaseUtil
{
    public interface IDBAccess
    {
        /// <summary>
        /// 测试连接
        /// </summary>
        /// <returns></returns>
        bool TestConnect();

        /// <summary>
        /// 执行增、删、改的SQL语句或者存储过程，返回受影响的行数
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <returns>受影响的行数</returns>
        int ExecuteNonQuery(string cmdText, CommandType cmdType);

        /// <summary>
        /// 执行带参数的增、删、改的SQL语句或者存储过程，返回受影响的行数
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <param name="paras">表示参数的键值对（键：参数名称；值：参数值）</param>
        /// <returns>受影响的行数</returns>
        int ExecuteNonQuery(string cmdText, CommandType cmdType, Hashtable paras);

        /// <summary>
        /// 执行SQL查询语句或者存储过程，返回DataSet
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <returns>返回DataSet</returns>
        DataSet ExecuteDateSet(string cmdText, CommandType cmdType);

        /// <summary>
        /// 执行带参数的SQL查询语句或者存储过程，返回DataSet
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <param name="paras">表示参数的键值对（键：参数名称；值：参数值）</param>
        /// <returns>返回DataSet</returns>
        DataSet ExecuteDateSet(string cmdText, CommandType cmdType, Hashtable paras);

        /// <summary>
        /// 执行SQL查询语句或者存储过程，返回SqlDatareader对象
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <returns>返回SqlDatareader对象</returns>
        IDataReader ExecuteDataReader(string cmdText, CommandType cmdType);

        /// <summary>
        /// 执行带参数的SQL查询语句或者存储过程，返回SqlDatareader对象
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <param name="paras">表示参数的键值对（键：参数名称；值：参数值）</param>
        /// <returns>返回SqlDatareader对象</returns>
        IDataReader ExecuteDataReader(string cmdText, CommandType cmdType, Hashtable paras);

        /// <summary>
        /// 执行查找字段的SQL查询语句或者存储过程，返回object类型的字段变量
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <returns>返回object类型的字段变量</returns>
        object ExecuteScalar(string cmdText, CommandType cmdType);

        /// <summary>
        /// 执行查找字段的SQL查询语句或者存储过程，返回object类型的字段变量
        /// </summary>
        /// <param name="cmdText">执行的命令或者存储过程的名称</param>
        /// <param name="cmdType">执行的命令的类型（SQL语句或者存储过程）</param>
        /// <param name="paras">表示参数的键值对（键：参数名称；值：参数值）</param>
        /// <returns>返回object类型的字段变量</returns>
        object ExecuteScalar(string cmdText, CommandType cmdType, Hashtable paras);
    }
}
