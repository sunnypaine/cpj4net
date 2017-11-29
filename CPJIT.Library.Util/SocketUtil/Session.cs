using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.Util.SocketUtil
{
    /// <summary>
    /// 表示与服务端之间的客户端会话
    /// </summary>
    public class Session
    {
        #region 公共属性
        /// <summary>
        /// 客户端的IP和端口
        /// </summary>
        public string IpPort { get; set; }

        /// <summary>
        /// 接收的数据
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 接收的数据
        /// </summary>
        public StringBuilder Message { get; set; }

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
        public Session(Socket socketCliet)
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
