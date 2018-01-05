using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.CPJ4net.SocketUtil.Model
{
    /// <summary>
    /// 表示包含事件异常信息。
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 异常对象
        /// </summary>
        public Exception Exception
        { get; set; }
    }
}
