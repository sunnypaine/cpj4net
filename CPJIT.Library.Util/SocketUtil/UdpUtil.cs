/*********************************************************
 * 文 件 名: UdpUtil
 * 命名空间：CPJIT.Util.SocketUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/21 21:43:04
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPJIT.Library.Util.SocketUtil
{
    /// <summary>
    /// 提供UDP通讯
    /// </summary>
    public class UdpUtil : IDisposable
    {
        #region 私有变量
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private const int bufferSize = 1024;

        private byte[] buffer = new byte[bufferSize];

        /// <summary>
        /// 本地ip与端口信息
        /// </summary>
        private EndPoint epLocal;

        /// <summary>
        /// UDP通讯对象
        /// </summary>
        private Socket udp;

        /// <summary>
        /// 程序是否在工作
        /// </summary>
        private bool isRun = false;

        /// <summary>
        /// 事件通知对象
        /// </summary>
        //private AutoResetEvent are = new AutoResetEvent(false);
        #endregion


        #region 公共属性
        /// <summary>
        /// 收到消息后是否自动回复（true表示自动回复，false表示不自动回复，默认不自动回复）
        /// </summary>
        public bool IsAutoReply { get; set; }

        /// <summary>
        /// 自动回复内容（默认值为“success”）
        /// </summary>
        public string ReplyContent { get; set; }
        #endregion


        #region 事件委托
        /// <summary>
        /// 当收到消息时发生
        /// </summary>
        public event EventHandler<DataEventArgs> OnReceiver;
        #endregion


        #region 构造方法
        /// <summary>
        /// 创建一个UDP通讯的实例
        /// </summary>
        /// <param name="localIP">本地IP</param>
        /// <param name="localPort">本地端口</param>
        public UdpUtil(string localIP, int localPort)
        {
            this.epLocal = new IPEndPoint(IPAddress.Parse(localIP), localPort);

            this.IsAutoReply = false;
            this.ReplyContent = "success";
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 接收消息
        /// </summary>
        private void Receive(Socket socket)
        {
            EndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            socket.BeginReceiveFrom(this.buffer, 0, bufferSize, SocketFlags.None, ref remoteIPEndPoint,
                new AsyncCallback(ReceiveCallback), socket);
            //this.are.WaitOne();
        }

        /// <summary>
        /// 接收消息的回调方法
        /// </summary>
        /// <param name="iar"></param>
        private void ReceiveCallback(IAsyncResult iar)
        {
            if (iar.IsCompleted)
            {
                Socket socket = iar.AsyncState as Socket;
                //this.are.Set();

                //获取发送端的终节点
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 0);
                EndPoint tmpEPRemote = (EndPoint)ipep;
                int bytesRead = -1;

                try
                {
                    bytesRead = socket.EndReceiveFrom(iar, ref tmpEPRemote);
                    if (this.IsAutoReply == true)
                    {
                        this.Send(this.ReplyContent, tmpEPRemote);
                    }
                    DataEventArgs args = new DataEventArgs();
                    args.Bytes = this.buffer;
                    args.Message = new StringBuilder(Encoding.Default.GetString(this.buffer, 0, bytesRead));
                    args.RemoteIP = ipep.Address.ToString();
                    args.RemotePort = ipep.Port;
                    args.RemoteIpEndPoint = tmpEPRemote;
                    if (this.OnReceiver != null)
                    {
                        this.OnReceiver(this, args);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (this.isRun == true && socket != null)
                    {
                        socket.BeginReceiveFrom(this.buffer, 0, bufferSize, SocketFlags.None, ref tmpEPRemote, new AsyncCallback(ReceiveCallback), socket);
                    }
                }
            }

        }

        /// <summary>
        /// 发送消息的回调方法
        /// </summary>
        /// <param name="iar"></param>
        private void SendCallback(IAsyncResult iar)
        {
            if (iar.IsCompleted)
            {
                Socket socket = iar.AsyncState as Socket;
                socket.EndSendTo(iar);
            }
        }

        /// <summary>
        /// 释放系统资源
        /// </summary>
        private void DisposeObject()
        {
            //this.are.Set();
            this.isRun = false;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~UdpUtil()
        {
            DisposeObject();
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 启动UDP通讯
        /// </summary>
        public void Start()
        {
            this.isRun = true;
            this.udp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udp.Bind(this.epLocal);
            Receive(udp);
        }

        /// <summary>
        /// 停止并释放UDP通讯
        /// </summary>
        public void Stop()
        {
            this.isRun = false;
            this.udp.Close();
            this.udp.Dispose();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="epRemote"></param>
        public void Send(string message, EndPoint epRemote)
        {
            byte[] buffer = Encoding.Default.GetBytes(message);
            Send(buffer, epRemote);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="bytes">数据</param>
        /// <param name="epRemote"></param>
        public void Send(byte[] bytes, EndPoint epRemote)
        {
            this.udp.BeginSendTo(bytes, 0, bytes.Length, SocketFlags.None, epRemote, new AsyncCallback(SendCallback), this.udp);
        }

        /// <summary>
        /// 释放系统资源
        /// </summary>
        public void Dispose()
        {
            DisposeObject();
        }
        #endregion
    }
}
