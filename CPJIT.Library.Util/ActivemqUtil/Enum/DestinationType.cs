using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.Util.ActivemqUtil.Enum
{
    /// <summary>
    /// 表示目标类型，指定消息模式
    /// </summary>
    public enum DestinationType
    {
        /// <summary>
        /// 点对点消息模式
        /// </summary>
        Queue,
        /// <summary>
        /// 发布订阅消息模式
        /// </summary>
        Topic
    }
}
