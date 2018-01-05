using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.SocketUtil.Model
{
    /// <summary>
    /// 表示与服务端之间的客户端会话
    /// </summary>
    public class SessionEventArgs : EventArgs
    {
        #region 公共属性
        /// <summary>
        /// 客户端的IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 客户端的端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 客户端的IP和端口
        /// </summary>
        public string IpPort { get; set; }

        /// <summary>
        /// 接收的数据
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// 接收的数据
        /// </summary>
        public StringBuilder Message { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 与客户端关联的Socket对象
        /// </summary>
        public Socket SocketClient { get; private set; }
        #endregion


        #region 构造方法
        /// <summary>
        /// 使用指定的socket对象实例化对象
        /// </summary>
        /// <param name="socketCliet"></param>
        public SessionEventArgs(Socket socketCliet)
        {
            this.SocketClient = socketCliet;
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 关闭与客户端关联的会话
        /// </summary>
        public void Close()
        {
            this.SocketClient.Shutdown(SocketShutdown.Both);

            this.SocketClient.Close();
        }
        #endregion
    }
}
