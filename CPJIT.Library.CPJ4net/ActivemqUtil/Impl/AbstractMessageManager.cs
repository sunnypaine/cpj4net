using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPJIT.Library.CPJ4net.ActivemqUtil.Enum;

namespace CPJIT.Library.CPJ4net.ActivemqUtil.Impl
{
    public abstract class AbstractMessageManager : IMessageManager
    {
        #region 私有变量
        /// <summary>
        /// 消息管理器
        /// </summary>
        private readonly IActivemqClient activemqClient;
        #endregion


        #region 构造方法
        /// <summary>
        /// 使用指定参数创建实例。
        /// </summary>
        /// <param name="manager"></param>
        protected AbstractMessageManager(IActivemqClient mqclient)
        {
            if (mqclient == null)
            {
                throw new ArgumentNullException("指定参数mqclient为null。");
            }

            this.activemqClient = mqclient;
        }
        #endregion


        #region 私有属性
        /// <summary>
        /// 消息模式
        /// </summary>
        protected virtual DestinationType DestinationType { get; set; }

        /// <summary>
        /// 消息目标
        /// </summary>
        protected virtual string DestinationName { get; set; }

        /// <summary>
        /// 是否订阅消息
        /// </summary>
        protected virtual bool IsSubscibe { get; set; }
        #endregion


        #region 私有方法
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected abstract void ReciverMessage(object sender, Model.DataEventArgs args);
        #endregion


        #region 公共方法，IMessageManager成员
        /// <summary>
        /// 订阅目标
        /// </summary>
        public void SubscribeDestination()
        {
            this.SubscribeDestination(this.DestinationType, this.DestinationName);
        }

        /// <summary>
        /// 订阅目标
        /// </summary>
        /// <param name="destinationType">目标类型</param>
        /// <param name="destinationName">目标名称</param>
        public void SubscribeDestination(DestinationType destinationType, string destinationName)
        {
            if (IsSubscibe)
            {
                this.activemqClient.SubscribeDestination(destinationType, destinationName, this.ReciverMessage);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            this.Send(message, this.DestinationType, this.DestinationName);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        public void Send(string message, DestinationType destinationType, string destinationName)
        {
            this.activemqClient.Send(message, destinationType, destinationName);
        }

        /// <summary>
        /// 取消订阅目标
        /// </summary>
        public void CancelSubscribeDestination()
        {
            this.activemqClient.CancelSubscribeDestination(this.DestinationType, this.DestinationName);
        }
        #endregion
    }
}
