using CPJIT.Library.CPJ4net.SocketUtil.Impl;
using CPJIT.Library.CPJ4net.SocketUtil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CPJIT.Library.CPJ4net.SocketUtil
{
    /// <summary>
    /// 表示TCP客户端工具的接口。
    /// </summary>
    public interface ITcpClient
    {
        #region 公共属性
        /// <summary>
        /// 服务端IP
        /// </summary>
        IPAddress ServerIpAddress { get; set; }

        /// <summary>
        /// 服务端端口
        /// </summary>
        int ServerPort { get; set; }

        /// <summary>
        /// 是否连接到服务端
        /// </summary>
        bool IsConnect { get; set; }

        /// <summary>
        /// 编码格式（默认Default）
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// 表示消息的终止符。避免消息粘连。
        /// </summary>
        string Terminator { get; set; }
        #endregion


        #region 事件委托
        /// <summary>
        /// 当与服务端成功连接时发生
        /// </summary>
        event EventHandler OnConnected;

        /// <summary>
        /// 当与服务端断开连接时发生
        /// </summary>
        event EventHandler OnDisconnected;

        /// <summary>
        /// 接收到来自服务端的消息时发生
        /// </summary>
        event EventHandler<DataEventArgs> OnReceived;

        /// <summary>
        /// 当客户出现异常时发生
        /// </summary>
        event EventHandler<ExceptionEventArgs> OnException;
        #endregion


        #region 公共方法
        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <returns></returns>
        TcpClient Connect();

        /// <summary>
        /// 断开与服务端的连接
        /// </summary>
        /// <returns></returns>
        TcpClient Close();

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息文本</param>
        void Send(string message);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data">字节数组形式的消息</param>
        void Send(byte[] data);

        /// <summary>
        /// 释放对象
        /// </summary>
        void Dispose();
        #endregion
    }
}
