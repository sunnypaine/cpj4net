using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.Util.SocketUtil
{
    /// <summary>
    /// 表示远程端UDP信息
    /// </summary>
    public class DataArgs
    {
        /// <summary>
        /// 接收到的数据
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// 接收到的数据
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 远程端IP与端口
        /// </summary>
        public EndPoint RemoteIpEndPoint { get; set; }
    }
}
