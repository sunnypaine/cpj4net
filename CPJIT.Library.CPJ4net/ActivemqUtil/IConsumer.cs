using CPJIT.Library.CPJ4net.ActivemqUtil.Enum;
using CPJIT.Library.CPJ4net.ActivemqUtil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.ActivemqUtil
{
    /// <summary>
    /// 表示一个消费者。
    /// </summary>
    public interface IConsumer
    {
        /// <summary>
        /// 消息目标名称。
        /// </summary>
        string DestinationName { get; set; }

        /// <summary>
        /// 消息目标类型。
        /// </summary>
        DestinationType DestinationType { get; set; }

        /// <summary>
        /// 是否订阅消息。true：订阅；false：不订阅。
        /// <para>如果为false，即便是实现了CPJIT.Library.CPJ4net.ActivemqUtil.IConsumer接口也不会订阅消息。</para>
        /// </summary>
        bool IsSubscibe { get; set; }

        /// <summary>
        /// 接收消息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Receive(object sender, DataEventArgs args);
    }
}
