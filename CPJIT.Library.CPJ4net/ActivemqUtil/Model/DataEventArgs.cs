using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.CPJ4net.ActivemqUtil.Model
{
    /// <summary>
    /// 包含与AcitveMQ收发消息的事件关联的信息和事件数据。
    /// </summary>
    public class DataEventArgs : EventArgs
    {
        public string Text
        { get; set; }

        public byte[] Bytes
        { get; set; }

        public IMessage Message
        { get; set; }


        /// <summary>
        /// 使用默认的参数创建实例。
        /// </summary>
        public DataEventArgs()
        { }

        /// <summary>
        /// 使用指定的参数创建实例。
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="bytes">消息数据</param>
        public DataEventArgs(string content, byte[] bytes)
        {
            this.Text = content;
            this.Bytes = bytes;
        }
    }
}
