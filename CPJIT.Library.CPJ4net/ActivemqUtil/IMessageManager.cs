using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.CPJ4net.ActivemqUtil
{
    /// <summary>
    /// 消息管理器
    /// </summary>
    public interface IMessageManager
    {
        /// <summary>
        /// 订阅消息
        /// </summary>
        void SubscribeDestination();

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        void SubscribeDestination(Enum.DestinationType destinationType, string destinationName);

        /// <summary>
        ///     发送消息
        /// </summary>
        /// <param name="message"></param>
        void Send(string message);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="destinationType">消息的目标模式</param>
        /// <param name="destinationName">消息地址</param>
        void Send(string message, Enum.DestinationType destinationType, string destinationName);

        /// <summary>
        /// 取消订阅
        /// </summary>
        void CancelSubscribeDestination();
    }
}
