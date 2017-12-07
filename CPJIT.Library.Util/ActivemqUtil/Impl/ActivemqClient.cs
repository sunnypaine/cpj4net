using Apache.NMS;
using CPJIT.Library.Util.ActivemqUtil.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.Util.ActivemqUtil.Impl
{
    /// <summary>
    /// 提供ActiveMQ客户端工具
    /// </summary>
    public class ActivemqClient : IActivemqClient
    {
        #region 私有变量
        /// <summary>
        /// 连接对象
        /// </summary>
        private IConnection connection;

        /// <summary>
        /// 创建连接的工厂
        /// </summary>
        private IConnectionFactory connectionFactory;

        /// <summary>
        /// 会话对象
        /// </summary>
        private ISession session;

        /// <summary>
        /// 表示可由多个线程同时访问的Queue消息模式生的产者键值对线程安全集合（键：消息目标名称；值：消息生产者）
        /// </summary>
        private ConcurrentDictionary<string, IMessageProducer> producerQueueDict;

        /// <summary>
        /// 表示可由多个线程同时访问的Topic消息模式的生产者键值对线程安全集合（键：消息目标名称；值：消息生产者）
        /// </summary>
        private ConcurrentDictionary<string, IMessageProducer> producerTopicDict;

        /// <summary>
        /// 表示可由多个线程同时访问的Queue消息模式的消费者键值对线程安全集合（键：消息目标名称；值：消息消费者）
        /// </summary>
        private ConcurrentDictionary<string, IMessageConsumer> consumerQueueDict;

        /// <summary>
        /// 表示可由多个线程同时访问的Topic消息模式的消费者键值对线程安全集合（键：消息目标名称；值：消息消费者）
        /// </summary>
        private ConcurrentDictionary<string, IMessageConsumer> consumerTopicDict;

        /// <summary>
        /// 同步控制标识
        /// </summary>
        private static readonly object SyncLockObject = new object();
        #endregion


        #region 委托，IActivemqClient成员
        /// <summary>
        /// 当与ActiveMQ连接时发生
        /// </summary>
        public event EventHandler<StatusEventArgs> ConnectionStatusChanged;

        /// <summary>
        /// 当与ActiveMQ通讯过程中出现错误时发生
        /// </summary>
        public event EventHandler<ExceptionEventArgs> Error;
        #endregion


        #region 公共属性，IActivemqClient成员
        /// <summary>
        /// ActiveMQ代理服务的地址（必须赋值）
        /// </summary>
        public string BrokerUri { get; set; }

        /// <summary>
        /// 连接ActiveMQ的用户名（非必须赋值）
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 连接ActiveMQ的密码（非必须赋值）
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 客户端Id名称（非必须赋值）
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 发送的消息持久化的有效时长
        /// </summary>
        public TimeSpan LiveTime { get; set; }
        #endregion


        #region 构造方法
        /// <summary>
        /// 使用默认参数创建实例
        /// </summary>
        public ActivemqClient()
        {
            this.producerQueueDict = new ConcurrentDictionary<string, IMessageProducer>();
            this.producerTopicDict = new ConcurrentDictionary<string, IMessageProducer>();
            this.consumerQueueDict = new ConcurrentDictionary<string, IMessageConsumer>();
            this.consumerTopicDict = new ConcurrentDictionary<string, IMessageConsumer>();
        }

        /// <summary>
        /// 使用指定的参数创建实例
        /// </summary>
        /// <param name="brokerUri">ActiveMQ服务代理地址</param>
        public ActivemqClient(string brokerUri)
            : this()
        {
            this.BrokerUri = brokerUri;
        }

        /// <summary>
        /// 使用指定的参数创建实例。
        /// </summary>
        /// <param name="brokerUri">ActiveMQ服务代理地址</param>
        /// <param name="userName">连接ActiveMQ所需的用户名</param>
        /// <param name="password">连接Activ所需的密码</param>
        public ActivemqClient(string brokerUri, string userName, string password)
            : this(brokerUri)
        {
            this.BrokerUri = brokerUri;
            this.UserName = userName;
            this.Password = password;
        }

        /// <summary>
        /// 使用指定的参数创建实例。
        /// </summary>
        /// <param name="brokerUri">ActiveMQ服务代理地址</param>
        /// <param name="userName">连接ActiveMQ所需的用户名</param>
        /// <param name="password">连接Activ所需的密码</param>
        /// <param name="clientId">连接到ActiveMQ后，客户端在ActiveMQ上呈现的名称</param>
        public ActivemqClient(string brokerUri, string userName, string password, string clientId)
            : this(brokerUri, userName, password)
        {
            this.BrokerUri = brokerUri;
            this.UserName = userName;
            this.Password = password;
            this.ClientId = clientId;
        }

        /// <summary>
        /// 使用指定的参数创建实例。
        /// </summary>
        /// <param name="brokerUri">ActiveMQ服务代理地址</param>
        /// <param name="userName">连接ActiveMQ所需的用户名</param>
        /// <param name="password">连接Activ所需的密码</param>
        /// <param name="clientId">连接到ActiveMQ后，客户端在ActiveMQ上呈现的名称</param>
        /// <param name="liveTime">客户端发送的消息的持久化有效时长</param>
        public ActivemqClient(string brokerUri, string userName, string password, string clientId, TimeSpan liveTime)
            : this(brokerUri, userName, password, clientId)
        {
            this.BrokerUri = brokerUri;
            this.UserName = userName;
            this.Password = password;
            this.ClientId = clientId;
            this.LiveTime = liveTime;
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 创建生产者
        /// </summary>
        /// <param name="destinationType">目标类型</param>
        /// <param name="destinationName">目标名称</param>
        /// <returns>返回生产者</returns>
        private IMessageProducer CreateProducer(Enum.DestinationType destinationType, string destinationName)
        {
            if (string.IsNullOrWhiteSpace(destinationName))
            {
                throw new ArgumentNullException("指定的参数destinationName为null或不合法。");
            }

            IMessageProducer messageProducer = null;
            try
            {
                if (Enum.DestinationType.Queue.Equals(destinationType))//创建Queue的生产者
                {
                    if (this.producerQueueDict.ContainsKey(destinationName))//如果指定目标名称的生产者在Queue生产者键值表中存在
                    {
                        messageProducer = this.producerQueueDict[destinationName];
                    }
                    else//如果指定目标名称的生产者在Queue生产者键值表中不存在
                    {
                        IQueue queue = this.session.GetQueue(destinationName);
                        if (queue == null)
                        {
                            throw new NMSException("创建指定目标名称的IQueue出错。");
                        }
                        messageProducer = this.session.CreateProducer(queue);
                        this.producerQueueDict.TryAdd(destinationName, messageProducer);
                    }
                }
                else if (Enum.DestinationType.Topic.Equals(destinationType))//创建Topic的生产者
                {
                    if (this.producerTopicDict.ContainsKey(destinationName))//如果指定目标名称的生产者在Topic生产者键值表中存在
                    {
                        messageProducer = this.producerTopicDict[destinationName];
                    }
                    else//如果指定目标名称的生产者在Topic生产者键值表中不存在
                    {
                        ITopic topic = this.session.GetTopic(destinationName);
                        if (topic == null)
                        {
                            throw new NMSException("创建指定目标名称的IQueue出错。");
                        }
                        messageProducer = this.session.CreateProducer(topic);
                        this.producerTopicDict.TryAdd(destinationName, messageProducer);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new NMSException("创建生产者时发生错误。", ex);
            }
            return messageProducer;
        }

        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="destinationType">目标类型</param>
        /// <param name="destinationName">目标名称</param>
        /// <returns></returns>
        private IMessageConsumer CreateConsumer(Enum.DestinationType destinationType, string destinationName)
        {
            if (string.IsNullOrWhiteSpace(destinationName))
            {
                throw new ArgumentNullException("指定的参数destinationName为null或不合法。");
            }

            IMessageConsumer messageConsumer;
            try
            {
                messageConsumer = CreateConsumer(destinationType, destinationName, true);
            }
            catch (Exception ex)
            {
                throw new NMSException("创建消费者时发生错误。", ex);
            }
            return messageConsumer;
        }

        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="destinationType">目标类型</param>
        /// <param name="destinationName">目标名称</param>
        /// <param name="ignoreExpiration">是否忽略过期</param>
        /// <returns></returns>
        private IMessageConsumer CreateConsumer(Enum.DestinationType destinationType, string destinationName, bool ignoreExpiration)
        {
            if (string.IsNullOrWhiteSpace(destinationName))
            {
                throw new ArgumentNullException("指定的参数destinationName为null或不合法。");
            }

            IMessageConsumer messageConsumer;
            try
            {
                if (Enum.DestinationType.Queue.Equals(destinationType))//Queue类型的消费者
                {
                    if (this.consumerQueueDict.ContainsKey(destinationName) == false)//如果不存在，则新增
                    {
                        IQueue queue = this.session.GetQueue(destinationName);
                        messageConsumer = this.session.CreateConsumer(queue);
                        Apache.NMS.ActiveMQ.MessageConsumer mp = messageConsumer as Apache.NMS.ActiveMQ.MessageConsumer;
                        mp.IgnoreExpiration = true;//忽略过期
                        this.consumerQueueDict.TryAdd(destinationName, mp);
                    }
                    this.consumerQueueDict.TryGetValue(destinationName, out messageConsumer);
                }
                else//Topic类型的消费者
                {
                    if (this.consumerTopicDict.ContainsKey(destinationName) == false)//如果不存在，则新增
                    {
                        ITopic topic = this.session.GetTopic(destinationName);
                        messageConsumer = this.session.CreateConsumer(topic);
                        Apache.NMS.ActiveMQ.MessageConsumer mc = messageConsumer as Apache.NMS.ActiveMQ.MessageConsumer;
                        mc.IgnoreExpiration = true;
                        this.consumerTopicDict.TryAdd(destinationName, mc);
                    }
                    this.consumerTopicDict.TryGetValue(destinationName, out messageConsumer);
                }
            }
            catch (Exception ex)
            {
                throw new NMSException("创建消费者时发生错误。", ex);
            }
            return messageConsumer;
        }

        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="destinationType">目标类型</param>
        /// <param name="destinationName">目标名称</param>
        /// <param name="selector">选择器</param>
        /// <param name="ignoreExpiration">是否忽略消费者过期</param>
        /// <param name="isDurable">消费者是否持久化</param>
        /// <returns>返回消费者</returns>
        private IMessageConsumer CreateConsumer(Enum.DestinationType destinationType, string destinationName, string selector, bool ignoreExpiration, bool isDurable)
        {
            if (string.IsNullOrWhiteSpace(destinationName))
            {
                throw new ArgumentNullException("指定的参数destinationName为null或不合法。");
            }
            if (string.IsNullOrWhiteSpace(selector))
            {
                throw new ArgumentNullException("指定的参数selector为null或不合法。");
            }

            IMessageConsumer messageConsumer;
            try
            {
                if (Enum.DestinationType.Queue.Equals(destinationType))//Queue类型的消费者
                {
                    if (this.consumerQueueDict.ContainsKey(destinationName) == false)//如果不存在，则新增
                    {
                        IQueue queue = this.session.GetQueue(destinationName);
                        messageConsumer = this.session.CreateConsumer(queue, selector);
                        Apache.NMS.ActiveMQ.MessageConsumer mp = messageConsumer as Apache.NMS.ActiveMQ.MessageConsumer;
                        mp.IgnoreExpiration = true;//忽略过期
                        this.consumerQueueDict.TryAdd(destinationName, mp);
                    }
                    this.consumerQueueDict.TryGetValue(destinationName, out messageConsumer);
                }
                else//Topic类型的消费者
                {
                    if (this.consumerTopicDict.ContainsKey(destinationName) == false)//如果不存在，则新增
                    {
                        ITopic topic = this.session.GetTopic(destinationName);
                        //客户端是否持久化
                        if (isDurable == true)
                        {
                            messageConsumer = this.session.CreateDurableConsumer(topic, destinationName, selector, false);
                        }
                        else
                        {
                            messageConsumer = this.session.CreateConsumer(topic, selector);
                        }
                        Apache.NMS.ActiveMQ.MessageConsumer mc = messageConsumer as Apache.NMS.ActiveMQ.MessageConsumer;
                        mc.IgnoreExpiration = ignoreExpiration;
                        this.consumerTopicDict.TryAdd(destinationName, mc);
                    }
                    this.consumerTopicDict.TryGetValue(destinationName, out messageConsumer);
                }
            }
            catch (Exception ex)
            {
                throw new NMSException("创建消费者时发生错误。", ex);
            }
            return messageConsumer;
        }

        /// <summary>
        /// 使用指定的消费者订阅一个目标
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="handler"></param>
        private void SubscribeDestination(IMessageConsumer messageConsumer, EventHandler<DataEventArgs> handler)
        {
            try
            {
                if (messageConsumer == null)
                {
                    if (this.Error != null)
                    {
                        ExceptionEventArgs args = new ExceptionEventArgs();
                        args.Exception = new Exception("创建消费者失败。");
                        this.Error(this, args);
                    }
                }
                else
                {
                    EventHandler<DataEventArgs> receiver = handler;
                    messageConsumer.Listener += delegate (IMessage msg)
                    {
                        if (receiver == null)
                        {
                            return;
                        }
                        ITextMessage textMessage = (ITextMessage)msg;
                        DataEventArgs args = new DataEventArgs();
                        args.Text = textMessage.Text;
                        args.Message = msg;
                        receiver(this, args);
                    };
                }
            }
            catch (Exception ex)
            {
                if (this.Error != null)
                {
                    this.Error(this, new ExceptionEventArgs() { Exception = ex });
                }
            }
        }
        #endregion


        #region 委托方法
        /// <summary>
        /// 当与ActiveMQ连接上时发生
        /// </summary>
        private void Connection_ConnectionResumedListener()
        {
            if (this.ConnectionStatusChanged != null)
            {
                StatusEventArgs args = new StatusEventArgs();
                args.IsConnected = true;

                this.ConnectionStatusChanged(this, args);
            }
        }

        /// <summary>
        /// 当与ActiveMQ的连接断开时发生
        /// </summary>
        private void Connection_ConnectionInterruptedListener()
        {
            if (this.ConnectionStatusChanged != null)
            {
                StatusEventArgs args = new StatusEventArgs();
                args.IsConnected = false;

                this.ConnectionStatusChanged(this, args);
            }
        }

        /// <summary>
        /// 当与ActiveMQ通讯过程中发生变化
        /// </summary>
        /// <param name="exception"></param>
        private void Connection_ExceptionListener(Exception exception)
        {
            if (this.Error != null)
            {
                this.Error(this, new ExceptionEventArgs() { Exception = exception });
            }
        }
        #endregion


        #region 公共方法，IActivemqClient成员
        /// <summary>
        /// 连接ActiveMQ
        /// </summary>
        /// <param name="brokerUri">AcitveMQ服务代理的地址</param>
        public void Connect()
        {
            if (string.IsNullOrWhiteSpace(this.BrokerUri))
            {
                throw new ArgumentNullException("指定的参数url为null或不合法。");
            }

            try
            {
                this.connectionFactory = new Apache.NMS.ActiveMQ.ConnectionFactory(this.BrokerUri);

                if (string.IsNullOrWhiteSpace(this.UserName) || string.IsNullOrWhiteSpace(this.Password))
                {
                    this.connection = this.connectionFactory.CreateConnection();
                }
                else
                {
                    this.connection = this.connectionFactory.CreateConnection(this.UserName, this.Password);
                }

                if (string.IsNullOrWhiteSpace(this.ClientId) == false)
                {
                    this.connection.ClientId = this.ClientId;
                }

                this.connection.ConnectionInterruptedListener += this.Connection_ConnectionInterruptedListener;
                this.connection.ConnectionResumedListener += this.Connection_ConnectionResumedListener;
                this.connection.ExceptionListener += this.Connection_ExceptionListener;

                this.session = this.connection.CreateSession();
                this.connection.Start();
            }
            catch (Exception ex)
            {
                if (this.Error!=null)
                {
                    this.Error(this, new ExceptionEventArgs() { Exception = ex });
                }
            }
        }

        /// <summary>
        /// 断开与ActiveMQ的连接
        /// </summary>
        public void Close()
        {

            //释放Queue模式的生产者
            foreach (IMessageProducer producer in this.producerQueueDict.Values)
            {
                if (producer != null)
                {
                    producer.Close();
                    producer.Dispose();
                }
            }

            //释放Topic模式的生产者
            foreach (IMessageProducer producer in this.producerTopicDict.Values)
            {
                if (producer != null)
                {
                    producer.Close();
                    producer.Dispose();
                }
            }

            //释放Queue模式的消费者
            foreach (IMessageConsumer consumer in this.consumerQueueDict.Values)
            {
                if (consumer!=null)
                {
                    consumer.Close();
                    consumer.Dispose();
                }
            }

            //释放Topic模式的消费者
            foreach (IMessageConsumer consumer in this.consumerTopicDict.Values)
            {
                if (consumer != null)
                {
                    consumer.Close();
                    consumer.Dispose();
                }
            }

            //清理字典
            this.producerQueueDict.Clear();
            this.producerTopicDict.Clear();
            this.consumerQueueDict.Clear();
            this.consumerTopicDict.Clear();

            //释放session，connection，connectionFactory
            if (this.session != null)
            {
                this.session.Close();
                this.session.Dispose();
            }
            if (this.connection != null)
            {
                this.connection.Close();
                this.connection.Dispose();
            }
            if (this.connectionFactory != null)
            {
                this.connectionFactory = null;
            }

            if (this.ConnectionStatusChanged != null)
            {
                StatusEventArgs args = new StatusEventArgs();
                args.IsConnected = false;
                this.ConnectionStatusChanged(this, args);
            }
        }

        /// <summary>
        /// 订阅一个目标
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="handler"></param>
        public void SubscribeDestination(Enum.DestinationType destinationType, string destinationName, EventHandler<DataEventArgs> handler)
        {
            IMessageConsumer messageConsumer = this.CreateConsumer(destinationType, destinationName);
            this.SubscribeDestination(messageConsumer, handler);
        }

        /// <summary>
        /// 订阅一个目标
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="ignoreExpiration"></param>
        /// <param name="handler"></param>
        public void SubscribeDestination(Enum.DestinationType destinationType, string destinationName, bool ignoreExpiration, EventHandler<DataEventArgs> handler)
        {
            IMessageConsumer messageConsumer = this.CreateConsumer(destinationType, destinationName, ignoreExpiration);
            this.SubscribeDestination(messageConsumer, handler);
        }

        /// <summary>
        /// 订阅一个目标
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="selector"></param>
        /// <param name="ignoreExpiration"></param>
        /// <param name="isDurable"></param>
        /// <param name="handler"></param>
        public void SubscribeDestination(Enum.DestinationType destinationType, string destinationName, string selector, bool ignoreExpiration, bool isDurable, EventHandler<DataEventArgs> handler)
        {
            IMessageConsumer messageConsumer = this.CreateConsumer(destinationType, destinationName, selector, ignoreExpiration, isDurable);
            this.SubscribeDestination(messageConsumer, handler);
        }

        /// <summary>
        /// 取消订阅一个目标
        /// </summary>
        /// <param name="destinationType"></param>
        /// <param name="destination"></param>
        public void CancelSubscribeDestination(Enum.DestinationType destinationType, string destination)
        {
            IMessageConsumer messageConsumer = null;
            if (Enum.DestinationType.Queue.Equals(destinationType))
            {
                if (this.consumerQueueDict.ContainsKey(destination))
                {
                    messageConsumer = this.consumerQueueDict[destination];
                }
            }
            else
            {
                if (this.consumerTopicDict.ContainsKey(destination))
                {
                    messageConsumer = this.consumerTopicDict[destination];
                }
            }

            if (messageConsumer != null)
            {
                messageConsumer.Close();
                messageConsumer.Dispose();
            }
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        public void Send(string text, Enum.DestinationType destinationType, string destinationName)
        {
            this.Send(text, destinationType, destinationName, false);
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="isPersistent"></param>
        public void Send(string text, Enum.DestinationType destinationType, string destinationName, bool isPersistent)
        {
            this.Send(text, null, destinationType, destinationName, isPersistent);
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="messageProperties"></param>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="isPersistent"></param>
        public void Send(string text, IDictionary<string, string> messageProperties, Enum.DestinationType destinationType, string destinationName, bool isPersistent)
        {
            try
            {
                IMessageProducer messageProducer = this.CreateProducer(destinationType, destinationName);
                if (messageProducer != null)
                {
                    IMessage tempMessage = messageProducer.CreateTextMessage(text);
                    if (messageProperties != null)
                    {
                        foreach (KeyValuePair<string, string> current in messageProperties)
                        {
                            tempMessage.Properties.SetString(current.Key, current.Value);
                        }
                    }
                    MsgDeliveryMode msgDeliveryMode = isPersistent == true ? MsgDeliveryMode.Persistent : MsgDeliveryMode.NonPersistent;
                    messageProducer.Send(tempMessage, msgDeliveryMode, MsgPriority.Normal, this.LiveTime);
                }
            }
            catch (Exception ex)
            {
                if (this.Error != null)
                {
                    this.Error(this, new ExceptionEventArgs() { Exception = ex });
                }
            }
        }

        /// <summary>
        /// 发送字节数组消息
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        public void Send(byte[] bytes, Enum.DestinationType destinationType, string destinationName)
        {
            this.Send(bytes, null, destinationType, destinationName, false);
        }

        /// <summary>
        /// 发送字节数组消息
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="destinationType"></param>
        /// <param name="destinationName"></param>
        /// <param name="isPersistent"></param>
        public void Send(byte[] bytes, Enum.DestinationType destinationType, string destinationName, bool isPersistent)
        {
            this.Send(bytes, null, destinationType, destinationName, isPersistent);
        }

        /// <summary>
        /// 发送字节数组消息
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="messageProperties"></param>
        /// <param name="destinationType"></param>
        /// <param name="destination"></param>
        /// <param name="isPersistent"></param>
        public void Send(byte[] bytes, IDictionary<string, string> messageProperties, Enum.DestinationType destinationType, string destination, bool isPersistent)
        {
            try
            {
                IMessageProducer messageProducer = this.CreateProducer(destinationType, destination);
                if (messageProducer != null)
                {
                    IMessage tempMessage = messageProducer.CreateBytesMessage(bytes);
                    if (messageProperties != null)
                    {
                        foreach (KeyValuePair<string, string> current in messageProperties)
                        {
                            tempMessage.Properties.SetString(current.Key, current.Value);
                        }
                    }
                    MsgDeliveryMode msgDeliveryMode = isPersistent == true ? MsgDeliveryMode.Persistent : MsgDeliveryMode.NonPersistent;
                    messageProducer.Send(tempMessage, msgDeliveryMode, MsgPriority.Normal, this.LiveTime);
                }
            }
            catch (Exception ex)
            {
                if (this.Error != null)
                {
                    this.Error(this, new ExceptionEventArgs() { Exception = ex });
                }
            }
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// 析构函数，释放对象
        /// </summary>
        ~ActivemqClient()
        {
            this.Close();
        }
        #endregion
    }
}
