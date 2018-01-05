using CPJIT.Library.CPJ4net.SocketUtil.Impl;
using CPJIT.Library.CPJ4net.SocketUtil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CPJIT.Library.CPJ4net.SocketUtil
{
    /// <summary>
    /// 表示TCP服务端通讯工具接口
    /// </summary>
    public interface ITcpServer
    {
        #region 公共属性
        /// <summary>
        /// 客户端键值表（键：IP:Port；值：Session对象）
        /// </summary>
        IDictionary<string, SessionEventArgs> TcpClients { get; set; }

        /// <summary>
        /// 服务端监听的IP地址
        /// </summary>
        IPAddress IPAddress { get; set; }

        /// <summary>
        /// 服务端监听的端口
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// 收到数据后是否自动回复（true：回复，false：不回复，默认为false）
        /// </summary>
        bool IsAutoReply { get; set; }

        /// <summary>
        /// 自动回复的内容（默认回复“success”）
        /// </summary>
        string ReplyContent { get; set; }

        /// <summary>
        /// 通讯使用的编码（默认使用Default）
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// 表示消息的终止符。避免消息粘连。
        /// </summary>
        string Terminator { get; set; }
        #endregion


        #region 事件委托
        /// <summary>
        /// 接收到来自客户端的消息时发生
        /// </summary>
        event EventHandler<SessionEventArgs> OnReceived;

        /// <summary>
        /// 当有客户端连上服务端时发生
        /// </summary>
        event EventHandler<SessionEventArgs> OnConnected;

        /// <summary>
        /// 当有客户端与服务端断开连接时发生
        /// </summary>
        event EventHandler<SessionEventArgs> OnDisconnected;

        /// <summary>
        /// 当服务端出现异常时发生
        /// </summary>
        event EventHandler<SessionEventArgs> OnServerException;
        #endregion


        #region 公共方法
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns></returns>
        TcpServer Start();

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <returns></returns>
        TcpServer Stop();

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socketClient">客户端socket对象</param>
        /// <param name="message">文本消息内容</param>
        void Send(Socket socketClient, string message);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socketClient">客户端socket对象</param>
        /// <param name="bytes">字节数组消息内容</param>
        void Send(Socket socketClient, byte[] bytes);

        /// <summary>
        /// 断开所有客户端会话
        /// </summary>
        void CloseAllSession();

        /// <summary>
        /// 实现释放分配的资源的接口
        /// </summary>
        void Dispose();
        #endregion
    }
}
