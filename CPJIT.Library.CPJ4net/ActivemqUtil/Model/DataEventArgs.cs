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
        /// <summary>
        /// 字符串文本表示的消息。
        /// </summary>
        public string Text
        { get; set; }

        /// <summary>
        /// 字节数组表示的消息。
        /// </summary>
        public byte[] Bytes
        { get; set; }

        /// <summary>
        /// IMessage表示的消息。
        /// </summary>
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
