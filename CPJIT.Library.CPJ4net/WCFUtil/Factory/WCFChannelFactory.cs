using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace CPJIT.Library.CPJ4net.WCFUtil.Factory
{
    /// <summary>
    /// WCF信道工厂
    /// </summary>
    internal class WCFChannelFactory
    {
        #region 私有变量
        private static readonly Dictionary<string, ChannelFactory> channelFactories = new Dictionary<string, ChannelFactory>();

        private static readonly object syncRoot = new object();
        #endregion


        #region 私有方法
        /// <summary>
        /// 设置对象图中的最大项
        /// </summary>
        /// <param name="ep"></param>
        private void SetMaxItemsInObjectGraph(ServiceEndpoint ep)
        {
            IEndpointBehavior behavior = null;
            foreach (IEndpointBehavior b in ep.Behaviors)
            {
                if (b.GetType().Name == "DataContractSerializerServiceBehavior")
                {
                    behavior = b;
                    break;
                }
            }
            if (behavior == null)
            {
                object obj = typeof(IEndpointBehavior).Assembly.CreateInstance("System.ServiceModel.Dispatcher.DataContractSerializerServiceBehavior",
                    true,
                    BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new object[] { false, Int32.MaxValue },
                    null,
                    null);
                behavior = obj as IEndpointBehavior;
                ep.Behaviors.Add(behavior);
            }
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 创建信道
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <param name="ep">客户端与服务端的通信终结点</param>
        /// <returns></returns>
        public ChannelFactory<TContract> CreateChannel<TContract>(ServiceEndpoint ep)
        {
            string key = ep.ListenUri.AbsoluteUri;

            ChannelFactory<TContract> channel = null;
            if (channelFactories.ContainsKey(key))
            {
                channel = channelFactories[key] as ChannelFactory<TContract>;
            }
            if (null == channel)
            {
                channel = new ChannelFactory<TContract>(ep);
                SetMaxItemsInObjectGraph(channel.Endpoint);
                //channelFactory.Endpoint.Behaviors.Add(new MessageInspectorEndpointBehavior());

                lock (syncRoot)
                {
                    //将channel添加到channel工厂对象中
                    channelFactories[key] = channel;
                }
            }
            return channel;
        }

        /// <summary>
        /// 创建信道
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <param name="endpointConfigurationName">表示客户端与服务端的通信终结点的配置信息名称</param>
        /// <returns></returns>
        public ChannelFactory<TContract> CreateChannel<TContract>(string endpointConfigurationName)
        {
            ChannelFactory<TContract> channel = null;
            if (channelFactories.ContainsKey(endpointConfigurationName))
            {
                channel = channelFactories[endpointConfigurationName] as ChannelFactory<TContract>;
            }
            if (null == channel)
            {
                channel = new ChannelFactory<TContract>(endpointConfigurationName);
                SetMaxItemsInObjectGraph(channel.Endpoint);
                //channelFactory.Endpoint.Behaviors.Add(new MessageInspectorEndpointBehavior());

                lock (syncRoot)
                {
                    //将channel添加到channel工厂对象中
                    channelFactories[endpointConfigurationName] = channel;
                }
            }
            return channel;
        }
        #endregion
    }
}
