using CPJIT.Library.CPJ4net.SocketUtil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CPJIT.Library.CPJ4net.SocketUtil
{
    /// <summary>
    /// 表示Udp通讯工具的接口
    /// </summary>
    public interface IUdpClient
    {
        #region 公共属性
        /// <summary>
        /// 收到消息后是否自动回复（true表示自动回复，false表示不自动回复，默认不自动回复）
        /// </summary>
        bool IsAutoReply { get; set; }

        /// <summary>
        /// 自动回复内容（默认值为“success”）
        /// </summary>
        string ReplyContent { get; set; }
        #endregion


        #region 事件委托
        /// <summary>
        /// 当收到消息时发生
        /// </summary>
        event EventHandler<DataEventArgs> OnReceiver;
        #endregion


        #region 公共方法
        /// <summary>
        /// 启动UDP通讯
        /// </summary>
        void Start();

        /// <summary>
        /// 停止并释放UDP通讯
        /// </summary>
        void Stop();

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="epRemote"></param>
        void Send(string message, EndPoint epRemote);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="bytes">数据</param>
        /// <param name="epRemote"></param>
        void Send(byte[] bytes, EndPoint epRemote);

        /// <summary>
        /// 释放系统资源
        /// </summary>
        void Dispose();
        #endregion
    }
}
