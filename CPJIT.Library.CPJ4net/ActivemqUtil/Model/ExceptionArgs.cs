using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.CPJ4net.ActivemqUtil.Model
{
    /// <summary>
    /// 包含与AcitveMQ异常的事件关联的信息和事件数据。
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception
        { get; set; }
    }
}
