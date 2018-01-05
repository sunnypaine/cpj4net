using CPJIT.Library.CPJ4net.ActivemqUtil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.CPJ4net.ActivemqUtil
{
    /// <summary>
    /// 提供ActiveMQ客户端工具的接口
    /// </summary>
    public interface IActivemqClient : IDisposable
    {
        #region 属性定义
        /// <summary>
        /// ActiveMQ代理服务的地址（必须赋值）
        /// </summary>
        string BrokerUri { get; }

        /// <summary>
        /// 连接ActiveMQ的用户名（非必须赋值）
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 连接ActiveMQ的密码（非必须赋值）
        /// </summary>
        string Password { get; }

        /// <summary>
        /// 客户端Id名称（非必须赋值）
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// 发送的消息持久化的有效时长
        /// </summary>
        TimeSpan LiveTime { get; }
        #endregion


        #region 委托定义
        /// <summary>
        /// 当与服务端连接状态变化时发生
        /// </summary>
        event EventHandler<StatusEventArgs> ConnectionStatusChanged;

        /// <summary>
        /// 当出现错误时发生。
        /// </summary>
        event EventHandler<ExceptionEventArgs> Error;
        #endregion


        #region 方法定义
        /// <summary>
        /// 连接ActiveMQ
        /// </summary>
        void Connect();

        /// <summary>
        /// 断开连接ActiveMQ
        /// </summary>
        void Close();

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="handler">回调的目标方法</param>
        void SubscribeDestination(Enum.DestinationType destinationType, string destinationName, EventHandler<DataEventArgs> handler);

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="ignoreExpiration"></param>
        /// <param name="handler"></param>
        void SubscribeDestination(Enum.DestinationType destinationType, string destinationName, bool ignoreExpiration, EventHandler<DataEventArgs> handler);

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="selector"></param>
        /// <param name="ignoreExpiration"></param>
        /// <param name="isDurable"></param>
        /// <param name="handler"></param>
        void SubscribeDestination(Enum.DestinationType destinationType, string destinationName, string selector, bool ignoreExpiration, bool isDurable, EventHandler<DataEventArgs> handler);

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="destination"></param>
        void CancelSubscribeDestination(Enum.DestinationType destinationType, string destination);

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="destType"></param>
        /// <param name="destination"></param>
        void Send(string text, Enum.DestinationType destinationType, string destination);

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="isPersistent"></param>
        void Send(string text, Enum.DestinationType destinationType, string destinationName, bool isPersistent);

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="msgProperties"></param>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="isPersistent"></param>
        void Send(string text, IDictionary<string, string> msgProperties, Enum.DestinationType destinationType, string destinationName, bool isPersistent);

        /// <summary>
        /// 发送字节数组消息
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        void Send(byte[] bytes, Enum.DestinationType destinationType, string destinationName);

        /// <summary>
        /// 发送字节数组消息
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="isPersistent"></param>
        void Send(byte[] bytes, Enum.DestinationType destinationType, string destinationName, bool isPersistent);

        /// <summary>
        /// 发送字节数组消息
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="msgProperties"></param>
        /// <param name="destinationType"></param>
        /// <param name="destination"></param>
        /// <param name="isPersistent"></param>
        void Send(byte[] bytes, IDictionary<string, string> msgProperties, Enum.DestinationType destinationType, string destination, bool isPersistent);
        #endregion
    }
}
