/*********************************************************
 * 文 件 名: TcpUtil
 * 命名空间：CPJIT.Util.SocketUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/21 21:42:25
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections;
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
    /// TCP服务端通讯工具
    /// </summary>
    public class TcpServerUtil : IDisposable
    {
        #region 私有变量
        /// <summary>
        /// tcp服务端是否继续工作
        /// </summary>
        private bool isRun = false;

        /// <summary>
        /// 服务器允许的最大客户端连接数
        /// </summary>
        private int maxClient;

        /// <summary>
        /// 当前客户端连接数
        /// </summary>
        private int currentClient;

        /// <summary>
        /// 服务端的Socket对象
        /// </summary>
        private Socket socetServer;

        /// <summary>
        /// 消息缓冲区大小
        /// </summary>
        private readonly int bufferSize = 1024;
        #endregion


        #region 公共属性
        /// <summary>
        /// 客户端键值表（键：IP:Port；值：Session对象）
        /// </summary>
        public Hashtable TcpClients { get; set; }

        /// <summary>
        /// 服务端监听的IP地址
        /// </summary>
        public IPAddress IPAddress { get; set; }

        /// <summary>
        /// 服务端监听的端口
        /// </summary>
        private int Port { get; set; }

        /// <summary>
        /// 收到数据后是否自动回复（true：回复，false：不回复，默认为false）
        /// </summary>
        public bool IsAutoReply { get; set; }

        /// <summary>
        /// 自动回复的内容（默认回复“success”）
        /// </summary>
        public string ReplyContent { get; set; }

        /// <summary>
        /// 通讯使用的编码（默认使用Default）
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 表示消息的终止符。避免消息粘连。
        /// </summary>
        public string Terminator { get; set; }
        #endregion


        #region 事件委托
        /// <summary>
        /// 接收到来自客户端的消息时发生
        /// </summary>
        public event EventHandler<SessionEventArgs> OnReceived;

        /// <summary>
        /// 当有客户端连上服务端时发生
        /// </summary>
        public event EventHandler<SessionEventArgs> OnConnected;

        /// <summary>
        /// 当有客户端与服务端断开连接时发生
        /// </summary>
        public event EventHandler<SessionEventArgs> OnDisconnected;

        /// <summary>
        /// 表示当服务端发生异常时将处理该事件的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="session"></param>
        /// <param name="ex"></param>
        public delegate void ServerExceptionHandler(object sender, SessionEventArgs session, Exception ex);
        /// <summary>
        /// 当服务端出现异常时发生
        /// </summary>
        public event ServerExceptionHandler OnServerException;
        #endregion


        #region 构造方法
        /// <summary>
        /// 使用指定的ip和端口实例化服务端通讯工具
        /// </summary>
        /// <param name="ip">服务端ip</param>
        /// <param name="port">服务端端口</param>
        public TcpServerUtil(string ip, int port) : this(ip, port, 1024)
        { }

        /// <summary>
        /// 使用指定的ip和端口实例化服务端通讯工具
        /// </summary>
        /// <param name="ip">服务端ip</param>
        /// <param name="port">服务端端口</param>
        /// <param name="maxClient">服务端允许的最大客户端连接数</param>
        public TcpServerUtil(string ip, int port, int maxClient)
        {
            try
            {
                this.IPAddress = IPAddress.Parse(ip);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("IP地址不合法。", ex);
            }
            catch (FormatException ex)
            {
                throw new FormatException("IP地址格式化出错。", ex);
            }

            if (port < 0 || port > 65535)
            {
                throw new ArgumentException("端口不合法，端口号范围应为0~65535");
            }

            this.Port = port;
            this.maxClient = maxClient;

            this.IsAutoReply = false;
            this.ReplyContent = "success";
            this.Encoding = Encoding.Default;

            this.TcpClients = new Hashtable();
            this.socetServer = new Socket(this.IPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 异步接收客户端连接
        /// </summary>
        /// <param name="iar"></param>
        private void AcceptTcpClientCallback(IAsyncResult iar)
        {
            if (this.isRun == true)
            {
                Socket server = iar.AsyncState as Socket;
                Socket client = server.EndAccept(iar);
                SessionEventArgs session = new SessionEventArgs(client);

                if (this.TcpClients.Count >= this.maxClient)
                {
                    this.OnServerException(this, session, new IndexOutOfRangeException("服务端超出最大客户端连接数"));
                }
                else
                {
                    lock (this.TcpClients)
                    {
                        IPEndPoint iep = (IPEndPoint)session.SocketClient.RemoteEndPoint;
                        string ipport = iep.Address.ToString() + ":" + iep.Port;
                        session.IP = iep.Address.ToString();
                        session.Port = iep.Port;
                        session.IpPort = ipport;
                        session.Message = new StringBuilder();
                        if (this.TcpClients.ContainsKey(ipport) == false)
                        {
                            this.TcpClients.Add(ipport, session);
                            this.currentClient = this.TcpClients.Count;

                            if (this.OnConnected != null)
                            {
                                this.OnConnected(this, session);
                            }
                        }
                    }
                    //开始接收来自客户端的数据
                    session.Bytes = new byte[this.bufferSize];
                    client.BeginReceive(session.Bytes, 0, session.Bytes.Length, SocketFlags.None,
                        new AsyncCallback(ReceiveMessageCallback), session);
                }
                //继续接收下一个客户端请求
                this.socetServer.BeginAccept(new AsyncCallback(AcceptTcpClientCallback), this.socetServer);
            }
        }

        /// <summary>
        /// 异步接受消息
        /// </summary>
        /// <param name="iar"></param>
        private void ReceiveMessageCallback(IAsyncResult iar)
        {
            if (this.isRun == true)
            {
                SessionEventArgs session = iar.AsyncState as SessionEventArgs;
                Socket client = session.SocketClient;

                try
                {
                    int receiveCount = client.EndReceive(iar);
                    if (receiveCount == 0)//表示客户端已经断开连接
                    {
                        this.CloseSession(session);
                        if (this.OnDisconnected != null)
                        {
                            this.OnDisconnected(this, session);
                        }
                    }
                    else if (receiveCount > 0)//表示客户端连接正常
                    {
                        string tmpMsg = this.Encoding.GetString(session.Bytes, 0, receiveCount);
                        if (client.Available > 0)//表示消息还没有接收完。没有接受完的消息可能是正常消息余下的，也有可能是第二条粘连的消息余下的。
                        {
                            session.Message.Append(tmpMsg);
                            if (client != null && client.Connected == true)
                            {
                                session.Bytes = new byte[this.bufferSize];
                                //继续接收来自客户端的消息
                                client.BeginReceive(session.Bytes, 0, session.Bytes.Length, SocketFlags.None,
                                    new AsyncCallback(ReceiveMessageCallback), session);
                            }
                        }
                        else//表示消息已经接受完成。接收完成的消息可能包含粘连消息
                        {
                            session.Message.Append(tmpMsg);
                            string message = session.Message.ToString();

                            if (string.IsNullOrWhiteSpace(this.Terminator))//如果没有设置消息内容终止符
                            {
                                byte[] bytes = this.Encoding.GetBytes(message);
                                if (this.OnReceived != null)
                                {
                                    this.OnReceived(this, new SessionEventArgs(session.SocketClient)
                                    {
                                        IP = session.IP,
                                        Port = session.Port,
                                        IpPort = session.IpPort,
                                        Bytes = bytes,
                                        Message = session.Message
                                    });
                                }
                            }
                            else
                            {
                                //判断消息是否粘连
                                if (tmpMsg.Contains(this.Terminator))
                                {
                                    string[] splitTmp = tmpMsg.Split(new string[] { this.Terminator }, StringSplitOptions.None);
                                    foreach (string item in splitTmp)
                                    {
                                        if (string.IsNullOrEmpty(item) == true)
                                        {
                                            continue;
                                        }

                                        if (this.OnReceived != null)
                                        {
                                            byte[] bytes = this.Encoding.GetBytes(item);
                                            this.OnReceived(this, new SessionEventArgs(session.SocketClient)
                                            {
                                                IP = session.IP,
                                                Port = session.Port,
                                                IpPort = session.IpPort,
                                                Bytes = bytes,
                                                Message = new StringBuilder(message + this.Terminator)
                                            });
                                        }
                                    }
                                }
                            }


                            session.Bytes = new byte[this.bufferSize];
                            session.Message = new StringBuilder();
                            //继续接收来自客户端的消息
                            client.BeginReceive(session.Bytes, 0, session.Bytes.Length, SocketFlags.None,
                                new AsyncCallback(ReceiveMessageCallback), session);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (this.OnServerException != null)
                    {
                        this.OnServerException(this, session, new Exception("处理接收的消息出错", ex));
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
        /// 释放客户端会话
        /// </summary>
        /// <param name="session"></param>
        private void CloseSession(SessionEventArgs session)
        {
            if (session != null)
            {
                session.Message = null;
                session.Bytes = null;

                this.TcpClients.Remove(session.IpPort);
                this.currentClient = this.TcpClients.Count;

                session.Close();
            }
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        private void DisposeObject()
        {
            this.Stop();
            this.CloseAllSession();
        }

        /// <summary>
        /// 析构函数，释放资源
        /// </summary>
        ~TcpServerUtil()
        {
            DisposeObject();
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns></returns>
        public TcpServerUtil Start()
        {
            if (this.isRun == false)
            {
                this.isRun = true;
                this.socetServer.Bind(new IPEndPoint(this.IPAddress, this.Port));
                this.socetServer.Listen(this.maxClient);
                this.socetServer.BeginAccept(new AsyncCallback(AcceptTcpClientCallback), this.socetServer);
            }
            return this;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <returns></returns>
        public TcpServerUtil Stop()
        {
            if (this.isRun == true)
            {
                this.isRun = false;
                this.socetServer.Close();
            }
            return this;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(Socket socket, string message)
        {
            try
            {
                byte[] bytes = null;
                if (string.IsNullOrWhiteSpace(this.Terminator) == false)//如果设置了消息终止符
                {
                    if (message.Contains(this.Terminator) == true)//如果消息里包含消息终止符
                    {
                        bytes = this.Encoding.GetBytes(message);
                    }
                    else//如果消息里不包含消息终止符
                    {
                        bytes = this.Encoding.GetBytes(message + this.Terminator);
                    }
                }
                else
                {
                    bytes = this.Encoding.GetBytes(message);
                }
                Send(socket, bytes);
            }
            catch (Exception ex)
            {
                if (this.OnServerException != null)
                {
                    this.OnServerException(this, new SessionEventArgs(socket), new Exception("处理接收的消息出错", ex));
                }
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(Socket socket, byte[] bytes)
        {
            if (this.isRun == false)
            {
                throw new InvalidProgramException("无效的TCP通讯服务实例，当前TCP服务已经停止工作。");
            }
            if (socket == null)
            {
                throw new ArgumentNullException("参数socket为null，指定客户端对象不能为空。");
            }

            socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None,
                new AsyncCallback(SendCallback), socket);
        }

        /// <summary>
        /// 断开所有客户端会话
        /// </summary>
        public void CloseAllSession()
        {
            foreach (DictionaryEntry de in this.TcpClients)
            {
                SessionEventArgs session = de.Value as SessionEventArgs;
                this.CloseSession(session);
            }
            this.currentClient = 0;
            this.TcpClients.Clear();
        }

        /// <summary>
        /// 实现释放分配的资源的接口
        /// </summary>
        public void Dispose()
        {
            DisposeObject();
        }
        #endregion
    }
}
