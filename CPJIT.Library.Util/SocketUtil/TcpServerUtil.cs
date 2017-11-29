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
        #endregion


        #region 事件委托
        public delegate void ReceivedHandler(Session session);
        /// <summary>
        /// 接收到来自客户端的消息时发生
        /// </summary>
        public event ReceivedHandler OnReceived;

        public delegate void ConnectedHandler(Session session);
        /// <summary>
        /// 当有客户端连上服务端时发生
        /// </summary>
        public event ConnectedHandler OnConnected;

        public delegate void DisconnectedHandler(Session session);
        /// <summary>
        /// 当有客户端与服务端断开连接时发生
        /// </summary>
        public event DisconnectedHandler OnDisconnected;

        public delegate void ServerExceptionHandler(Session session, Exception ex);
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
        /// <param name="listner"></param>
        private void AcceptTcpClientCallback(IAsyncResult iar)
        {
            if (this.isRun == true)
            {
                Socket server = iar.AsyncState as Socket;
                Socket client = server.EndAccept(iar);
                Session session = new Session(client);

                if (this.TcpClients.Count >= this.maxClient)
                {
                    this.OnServerException(session, new IndexOutOfRangeException("服务端超出最大客户端连接数"));
                }
                else
                {
                    lock (this.TcpClients)
                    {
                        IPEndPoint iep = (IPEndPoint)session.SocketClient.RemoteEndPoint;
                        string ipport = iep.Address.ToString() + ":" + iep.Port;
                        session.IpPort = ipport;
                        session.Message = new StringBuilder();
                        if (this.TcpClients.ContainsKey(ipport) == false)
                        {
                            this.TcpClients.Add(ipport, session);
                            this.currentClient = this.TcpClients.Count;

                            this.OnConnected?.Invoke(session);
                        }
                    }
                    //开始接收来自客户端的数据
                    session.Data = new byte[client.ReceiveBufferSize];
                    client.BeginReceive(session.Data, 0, session.Data.Length, SocketFlags.None,
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
                Session session = iar.AsyncState as Session;
                Socket client = session.SocketClient;

                try
                {
                    int receiveCount = client.EndReceive(iar);
                    if (receiveCount == 0)
                    {
                        this.CloseSession(session);
                        this.OnDisconnected?.Invoke(session);
                    }
                    else if (receiveCount > 0)
                    {
                        string tmp = this.Encoding.GetString(session.Data, 0, receiveCount);
                        session.Message.Append(tmp);
                        if (client.Available > 0)
                        {
                            if (client != null && client.Connected == true)
                            {
                                session.Data = new byte[client.ReceiveBufferSize];
                                //继续接收来自客户端的消息
                                client.BeginReceive(session.Data, 0, session.Data.Length, SocketFlags.None,
                                    new AsyncCallback(ReceiveMessageCallback), session);
                            }
                        }
                        else
                        {
                            this.OnReceived?.Invoke(session);
                            session.Data = new byte[client.ReceiveBufferSize];
                            session.Message = new StringBuilder();
                            //继续接收来自客户端的消息
                            client.BeginReceive(session.Data, 0, session.Data.Length, SocketFlags.None,
                                new AsyncCallback(ReceiveMessageCallback), session);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.OnServerException?.Invoke(session, new Exception("处理接收的消息出错", ex));
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
        private void CloseSession(Session session)
        {
            if (session != null)
            {
                session.Message = null;
                session.Data = null;

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
            byte[] bytes = this.Encoding.GetBytes(message);
            Send(socket, bytes);
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
                Session session = de.Value as Session;
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
