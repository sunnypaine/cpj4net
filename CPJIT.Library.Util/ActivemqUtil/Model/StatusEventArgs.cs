using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.Util.ActivemqUtil.Model
{
    /// <summary>
    /// 包含与AcitveMQ连接状态的信息和事件数据。
    /// </summary>
    public class StatusEventArgs : EventArgs
    {
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected
        { get; set; }
    }
}
