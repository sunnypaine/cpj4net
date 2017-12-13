/*********************************************************
 * 文 件 名: TcpClientUtil
 * 命名空间：CPJIT.Util.SocketUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/21 22:10:03
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
    /// TCP客户端通讯工具
    /// </summary>
    public class TcpClientUtil : IDisposable
    {
        #region 私有变量
        /// <summary>
        /// TCP客户端
        /// </summary>
        private Socket socketClient;
        #endregion


        #region 公共属性
        /// <summary>
        /// 服务端IP
        /// </summary>
        public IPAddress ServerIpAddress { get; set; }

        /// <summary>
        /// 服务端端口
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// 是否连接到服务端
        /// </summary>
        public bool IsConnect { get; set; }

        /// <summary>
        /// 编码格式（默认Default）
        /// </summary>
        public Encoding Encoding { get; set; }
        #endregion


        #region 事件委托
        public delegate void ConnectedHandler();
        /// <summary>
        /// 当与服务端成功连接时发生
        /// </summary>
        public event ConnectedHandler OnConnected;

        public delegate void DisconnectedHandler();
        /// <summary>
        /// 当与服务端断开连接时发生
        /// </summary>
        public event DisconnectedHandler OnDisconnected;

        public delegate void ReceivedHandler(DataArgs args);
        /// <summary>
        /// 接收到来自服务端的消息时发生
        /// </summary>
        public event ReceivedHandler OnReceived;

        public delegate void ExceptionHandler(Exception ex);
        /// <summary>
        /// 当客户出现异常时发生
        /// </summary>
        public event ExceptionHandler OnException;
        #endregion


        #region 构造方法
        /// <summary>
        /// 使用指定的参数信息实例化TCP客户端对象
        /// </summary>
        /// <param name="serverIP">服务端IP地址</param>
        /// <param name="serverPort">服务端端口</param>
        public TcpClientUtil(string serverIP, int serverPort)
        {
            try
            {
                this.ServerIpAddress = IPAddress.Parse(serverIP);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("IP地址不合法。", ex);
            }
            catch (FormatException ex)
            {
                throw new FormatException("IP地址格式化出错。", ex);
            }

            if (serverPort < 0 || serverPort > 65535)
            {
                throw new ArgumentException("端口不合法，端口号范围应为0~65535");
            }

            this.ServerPort = serverPort;
            this.Encoding = Encoding.Default;
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 连接服务端的回调方法
        /// </summary>
        /// <param name="iar"></param>
        private void ConnectCallback(IAsyncResult iar)
        {
            try
            {
                this.IsConnect = true;
                Socket socket = iar.AsyncState as Socket;
                socket.EndConnect(iar);

                //触发与服务端连接成功的委托
                if (this.OnConnected != null)
                {
                    this.OnConnected();
                }

                //开始接收数据
                byte[] buffer = new byte[this.socketClient.ReceiveBufferSize];
                this.socketClient.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReceiveDataCallback), buffer);
            }
            catch (Exception ex)
            {
                //触发连接服务端出错的委托
                if (this.OnException != null)
                {
                    this.OnException(new Exception("连接服务端出错", ex));
                }
            }
        }

        /// <summary>
        /// 接收数据的回调方法
        /// </summary>
        /// <param name="iar"></param>
        private void ReceiveDataCallback(IAsyncResult iar)
        {
            try
            {
                int receiveCount = this.socketClient.EndReceive(iar);
                if (receiveCount == 0)
                {
                    this.socketClient = null;
                    if (this.OnDisconnected != null)
                    {
                        this.OnDisconnected();
                    }
                }
                else
                {
                    byte[] buffer = iar.AsyncState as byte[];
                    byte[] receiveData = new byte[receiveCount];
                    Buffer.BlockCopy(buffer, 0, receiveData, 0, receiveCount);

                    DataArgs args = new DataArgs()
                    {
                        Bytes = receiveData,
                        Data = this.Encoding.GetString(receiveData),
                        RemoteIpEndPoint = new IPEndPoint(this.ServerIpAddress, this.ServerPort)
                    };
                    if (this.OnReceived != null)
                    {
                        this.OnReceived(args);
                    }

                    if (this.socketClient != null && this.socketClient.Connected == true)
                    {
                        //继续接收数据
                        buffer = new byte[this.socketClient.ReceiveBufferSize];
                        this.socketClient.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None,
                            new AsyncCallback(ReceiveDataCallback), buffer);
                    }
                }
            }
            catch (Exception ex)
            {
                if (this.OnException == null)
                {
                    throw new Exception("接收数据出错", ex);
                }
                else
                {
                    this.OnException(new Exception("接收数据出错", ex));
                }
            }
        }

        /// <summary>
        /// 发送消息的回调方法
        /// </summary>
        /// <param name="iar"></param>
        private void SendCallback(IAsyncResult iar)
        {
            Socket socket = iar.AsyncState as Socket;
            socket.EndSend(iar);
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <returns></returns>
        public TcpClientUtil Connect()
        {
            if (this.IsConnect == false)
            {
                if (this.ServerIpAddress == null)
                {
                    if (this.OnException == null)
                    {
                        throw new ArgumentNullException("服务端IP不合法。");
                    }
                    else
                    {
                        this.OnException(new ArgumentNullException("服务端IP不合法。"));
                        return null;
                    }
                }

                try
                {
                    this.socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.socketClient.BeginConnect(this.ServerIpAddress, this.ServerPort,
                        new AsyncCallback(ConnectCallback), this.socketClient);
                }
                catch (Exception ex)
                {
                    if (this.OnException == null)
                    {
                        throw new Exception("初始化客户端出错", ex);
                    }
                    else
                    {
                        this.OnException(new Exception("初始化客户端出错", ex));
                        return null;
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// 断开与服务端的连接
        /// </summary>
        /// <returns></returns>
        public TcpClientUtil Close()
        {
            if (this.IsConnect == true)
            {
                if (this.socketClient != null)
                {
                    this.socketClient.Close();
                    this.IsConnect = false;
                    //触发与服务端断开的委托
                    if (this.OnDisconnected != null)
                    {
                        this.OnDisconnected();
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            this.Send(this.Encoding.GetBytes(message));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data)
        {
            if (data == null)
            {
                if (this.OnException == null)
                {
                    throw new ArgumentNullException("发送的内容不能为null");
                }
                else
                {
                    this.OnException(new ArgumentNullException("发送的内容不能为null"));
                }
            }
            this.socketClient.BeginSend(data, 0, data.Length, SocketFlags.None,
                new AsyncCallback(SendCallback), this.socketClient);
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.Close();

                if (this.socketClient != null)
                {
                    this.socketClient = null;
                }
            }
            catch (SocketException ex)
            {
                throw new Exception("释放对象出错", ex);
            }
        }
        #endregion
    }
}
