using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.SocketUtil.Model
{
    /// <summary>
    /// 表示远程端UDP信息
    /// </summary>
    public class DataEventArgs : EventArgs
    {
        /// <summary>
        /// 接收到的数据
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// 接收到的数据
        /// </summary>
        public StringBuilder Message { get; set; }

        /// <summary>
        /// 远端IP地址
        /// </summary>
        public string RemoteIP { get; set; }

        /// <summary>
        /// 远端端口
        /// </summary>
        public int RemotePort { get; set; }

        /// <summary>
        /// 远程端IP与端口
        /// </summary>
        public EndPoint RemoteIpEndPoint { get; set; }
    }
}
