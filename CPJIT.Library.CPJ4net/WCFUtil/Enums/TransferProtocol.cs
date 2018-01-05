using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.CPJ4net.WCFUtil.Enums
{
    /// <summary>
    /// 传输协议
    /// </summary>
    public enum TransferProtocol
    {
        /// <summary>
        /// 使用TCP协议传输数据
        /// </summary>
        TCP,
        /// <summary>
        /// 使用HTTP协议传输数据
        /// </summary>
        HTTP,
        /// <summary>
        /// 使用对等网络协议传输数据
        /// </summary>
        PEER,
        /// <summary>
        /// 使用通信管道传输数据
        /// </summary>
        PIPE,
        /// <summary>
        /// 使用微软消息队列MicroSoft Message Queue传输数据
        /// </summary>
        MSMQ
    }
}
