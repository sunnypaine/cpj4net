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

        /// <summary>
        /// 消息缓冲区大小
        /// </summary>
        private readonly int bufferSize = 1024;
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

        /// <summary>
        /// 表示消息的终止符。避免消息粘连。
        /// </summary>
        public string Terminator { get; set; }
        #endregion


        #region 事件委托
        /// <summary>
        /// 当与服务端成功连接时发生
        /// </summary>
        public event EventHandler OnConnected;

        /// <summary>
        /// 当与服务端断开连接时发生
        /// </summary>
        public event EventHandler OnDisconnected;

        /// <summary>
        /// 接收到来自服务端的消息时发生
        /// </summary>
        public event EventHandler<DataEventArgs> OnReceived;

        /// <summary>
        /// 表示当tcp客户端出现异常时处理该事件的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        public delegate void ExceptionHandler(object sender, Exception ex);
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
                    this.OnConnected(this, new EventArgs());
                }

                //开始接收数据
                DataEventArgs args = new DataEventArgs();
                args.RemoteIP = this.ServerIpAddress.ToString();
                args.RemotePort = this.ServerPort;
                args.RemoteIpEndPoint = new IPEndPoint(this.ServerIpAddress, this.ServerPort);
                args.Bytes = new byte[this.bufferSize];
                args.Message = new StringBuilder();
                this.socketClient.BeginReceive(args.Bytes, 0, args.Bytes.Length, SocketFlags.None,
                    new AsyncCallback(ReceiveDataCallback), args);
            }
            catch (Exception ex)
            {
                //触发连接服务端出错的委托
                if (this.OnException != null)
                {
                    this.OnException(this, new Exception("连接服务端出错", ex));
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
                DataEventArgs dataEventArgs = iar.AsyncState as DataEventArgs;

                int receiveCount = this.socketClient.EndReceive(iar);
                if (receiveCount == 0)
                {
                    this.socketClient = null;
                    if (this.OnDisconnected != null)
                    {
                        this.OnDisconnected(this, new EventArgs());
                    }
                }
                else
                {
                    byte[] receiveData = new byte[receiveCount];
                    string tmpMsg = this.Encoding.GetString(dataEventArgs.Bytes, 0, receiveCount);
                    if (this.socketClient.Available > 0)//表示消息还没有接收完
                    {
                        dataEventArgs.Message.Append(tmpMsg);
                        if (this.socketClient != null && this.socketClient.Connected == true)
                        {
                            dataEventArgs.Bytes = new byte[this.bufferSize];
                            //继续接收来自客户端的消息
                            this.socketClient.BeginReceive(dataEventArgs.Bytes, 0, dataEventArgs.Bytes.Length, SocketFlags.None,
                                new AsyncCallback(ReceiveDataCallback), dataEventArgs);
                        }
                    }
                    else//表示消息已经接收完成。可能包含粘连的消息。
                    {
                        dataEventArgs.Message.Append(tmpMsg);
                        string message = dataEventArgs.Message.ToString();

                        if (string.IsNullOrWhiteSpace(this.Terminator))//如果没有设置消息内容终止符
                        {
                            if (this.OnReceived != null)
                            {
                                byte[] bytes = this.Encoding.GetBytes(message);
                                this.OnReceived(this, new DataEventArgs()
                                {
                                    RemoteIP = dataEventArgs.RemoteIP,
                                    RemotePort = dataEventArgs.RemotePort,
                                    RemoteIpEndPoint = dataEventArgs.RemoteIpEndPoint,
                                    Bytes = bytes,
                                    Message = dataEventArgs.Message
                                });
                            }
                        }
                        else
                        {
                            //判断消息是否粘连
                            if (message.Contains(this.Terminator))
                            {
                                string[] splitTmp = message.Split(new string[] { this.Terminator }, StringSplitOptions.None);
                                foreach (string item in splitTmp)
                                {
                                    if (string.IsNullOrEmpty(item) == true)
                                    {
                                        continue;
                                    }

                                    if (this.OnReceived != null)
                                    {
                                        byte[] bytes = this.Encoding.GetBytes(item);
                                        this.OnReceived(this, new DataEventArgs()
                                        {
                                            RemoteIP = dataEventArgs.RemoteIP,
                                            RemotePort = dataEventArgs.RemotePort,
                                            RemoteIpEndPoint = dataEventArgs.RemoteIpEndPoint,
                                            Bytes = dataEventArgs.Bytes,
                                            Message = new StringBuilder(item + this.Terminator)
                                        });
                                    }
                                }
                            }
                        }


                        dataEventArgs.Bytes = new byte[this.bufferSize];
                        dataEventArgs.Message = new StringBuilder();
                        //继续接收来自客户端的消息
                        this.socketClient.BeginReceive(dataEventArgs.Bytes, 0, dataEventArgs.Bytes.Length, SocketFlags.None,
                            new AsyncCallback(ReceiveDataCallback), dataEventArgs);
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
                    this.OnException(this, new Exception("接收数据出错", ex));
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
                        this.OnException(this, new ArgumentNullException("服务端IP不合法。"));
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
                        this.OnException(this, new Exception("初始化客户端出错", ex));
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
                        this.OnDisconnected(this, new EventArgs());
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息文本</param>
        public void Send(string message)
        {
            try
            {
                byte[] bytes = null;
                if (string.IsNullOrWhiteSpace(this.Terminator) == false)//如果设置了消息终止符
                {
                    if (message.Contains(this.Terminator))//如果消息里已经包含了终止符
                    {
                        bytes = this.Encoding.GetBytes(message);
                    }
                    else//如果消息不包含终止符
                    {
                        bytes = this.Encoding.GetBytes(message + this.Terminator);
                    }
                }
                else//如果没有设置消息终止符，就直接发送消息
                {
                    bytes = this.Encoding.GetBytes(message);
                }

                this.Send(bytes);
            }
            catch (Exception ex)
            {
                if (this.OnException != null)
                {
                    this.OnException(this, new Exception("发送消息出错", ex));
                }
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data">字节数组形式的消息</param>
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
                    this.OnException(this, new ArgumentNullException("发送的内容不能为null"));
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
