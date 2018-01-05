using CPJIT.Library.CPJ4net.WCFUtil.Enums;
using CPJIT.Library.CPJ4net.WCFUtil.Factory;
using CPJIT.Library.CPJ4net.WCFUtil.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CPJIT.Library.CPJ4net.WCFUtil.Services
{
    internal class ServiceInvoker : IServiceInvoker
    {
        #region 私有变量
        private static readonly WCFChannelFactory wcfChannelFactory = new WCFChannelFactory();

        private string protocolHeader = "net.tcp";
        #endregion


        #region 公共属性，IServiceInvoker成员
        public string IpAddress
        { get; set; }

        public TransferProtocol TransferProtocl
        { get; set; }
        #endregion


        #region 私有方法
        /// <summary>
        /// 获取接口代理对象
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <param name="ep">客户端与服务端通信的终结点</param>
        /// <returns></returns>
        protected TContract GetProxy<TContract>(ServiceEndpoint ep)
        {
            ChannelFactory<TContract> channelFactory = wcfChannelFactory.CreateChannel<TContract>(ep);
            return channelFactory.CreateChannel();
        }

        /// <summary>
        /// 获取客户端与服务端的通信终结点
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <returns></returns>
        protected ServiceEndpoint GetServiceEndpoint<TContract>()
        {
            if (this.TransferProtocl == TransferProtocol.TCP)
            {
                protocolHeader = "net.tcp";
            }
            else if (this.TransferProtocl == TransferProtocol.HTTP)
            {
                protocolHeader = "http";
            }
            else if (this.TransferProtocl == TransferProtocol.PEER)
            {
                protocolHeader = "net.peer";
            }
            else if (this.TransferProtocl == TransferProtocol.PIPE)
            {
                protocolHeader = "net.pipe";
            }
            else if (this.TransferProtocl == TransferProtocol.MSMQ)
            {
                protocolHeader = " net.msmq";
            }

            ServiceEndpoint serviceEndpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(TContract)),
                new NetTcpBinding(SecurityMode.None)
                {
                    TransferMode = TransferMode.StreamedResponse,
                    MaxBufferSize = 2147483647,
                    MaxReceivedMessageSize = 2147483647,
                    ReaderQuotas = XmlDictionaryReaderQuotas.Max
                },
                new EndpointAddress(string.Format("{0}://" + this.IpAddress + "/services/"
                    + typeof(TContract).Name.Substring(1).ToLower(), protocolHeader)));
            return serviceEndpoint;
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 调用接口
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <param name="invokeHandler">调用委托</param>
        public void InvokeService<TContract>(Action<TContract> invokeHandler)
        {
            ServiceEndpoint serviceEndpoint = GetServiceEndpoint<TContract>();
            TContract proxy = GetProxy<TContract>(serviceEndpoint);
            try
            {
                invokeHandler(proxy);
                (proxy as ICommunicationObject).Close();
            }
            catch (CommunicationException ce)
            {
                throw new CommunicationException("通信错误。", ce);
            }
            catch (TimeoutException te)
            {
                throw new TimeoutException("通信超时。", te);
            }
            catch (Exception ex)
            {
                throw new TimeoutException("调用目标接口出错。", ex);
            }
        }

        /// <summary>
        /// 调用带返回值的接口
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <typeparam name="TResult">返回值</typeparam>
        /// <param name="invokeHandler">调用委托</param>
        /// <returns></returns>
        public TResult InvokeService<TContract, TResult>(Func<TContract, TResult> invokeHandler)
        {
            ServiceEndpoint serviceEndpoint = GetServiceEndpoint<TContract>();
            TContract proxy = GetProxy<TContract>(serviceEndpoint);
            TResult returnValue = default(TResult);
            try
            {
                returnValue = invokeHandler(proxy);
                (proxy as ICommunicationObject).Close();
            }
            catch (CommunicationException ex)
            {
                throw new CommunicationException("通信错误。", ex);
            }
            catch (TimeoutException te)
            {
                throw new TimeoutException("通信超时。", te);
            }
            catch (Exception ex)
            {
                throw new TimeoutException("调用目标接口出错。", ex);
            }
            return returnValue;
        }

        /// <summary>
        /// 异步调用接口
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <param name="invokeHandler">调用委托</param>
        /// <param name="isSuccessHandler">返回委托，是否调用成功</param>
        public void AsyncInvokeService<TContract>(Action<TContract> invokeHandler, Action<bool> isSuccessHandler)
        {
            Task.Factory.StartNew(() =>
            {
                ServiceEndpoint serviceEndpoint = GetServiceEndpoint<TContract>();
                TContract proxy = GetProxy<TContract>(serviceEndpoint);
                invokeHandler(proxy);
            }).ContinueWith((t) =>
            {
                if (isSuccessHandler != null)
                {
                    isSuccessHandler(!t.IsFaulted);
                }
            });
        }

        /// <summary>
        /// 异步调用带返回值的接口
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <typeparam name="TResult">返回值</typeparam>
        /// <param name="invokeHandler">调用委托</param>
        /// <param name="resultHandler">返回委托，处理响应的数据</param>
        public void AsyncInvokeService<TContract, TResult>(Func<TContract, TResult> invokeHandler, Action<TResult> resultHandler)
        {
            Task.Factory.StartNew(() =>
            {
                ServiceEndpoint serviceEndpoint = GetServiceEndpoint<TContract>();
                TContract proxy = GetProxy<TContract>(serviceEndpoint);
                return invokeHandler(proxy);
            }).ContinueWith((t) =>
            {
                if (resultHandler != null)
                {
                    resultHandler(t.Result);
                }
            });
        }
        #endregion
    }
}
