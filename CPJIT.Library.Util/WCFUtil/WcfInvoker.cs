using CPJIT.Library.Util.WCFUtil.Enums;
using CPJIT.Library.Util.WCFUtil.Interfaces;
using CPJIT.Library.Util.WCFUtil.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.Util.WCFUtil
{
    public class WcfInvoker
    {
        #region 私有变量
        /// <summary>
        /// 调用服务的接口对象
        /// </summary>
        private IServiceInvoker serviceInvoker;
        #endregion


        #region 构造方法
        /// <summary>
        /// 使用指定的参数实例化对象。
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="transferProtocl"></param>
        public WcfInvoker(string ip, int port, TransferProtocol transferProtocl)
        {
            this.serviceInvoker = new ServiceInvoker();
            this.serviceInvoker.IpAddress = ip + ":" + port;
            this.serviceInvoker.TransferProtocl = transferProtocl;
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 调用接口
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <param name="invokeHandler">调用委托</param>
        public void Invoke<TContract>(Action<TContract> invokeHandler)
        {
            this.serviceInvoker.InvokeService<TContract>(invokeHandler);
        }

        /// <summary>
        /// 调用带返回值的接口
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <typeparam name="TResult">返回值</typeparam>
        /// <param name="invokeHandler">调用委托</param>
        /// <returns></returns>
        public TResult Invoke<TContract, TResult>(Func<TContract, TResult> invokeHandler)
        {
            return this.serviceInvoker.InvokeService<TContract, TResult>(invokeHandler);
        }

        /// <summary>
        /// 异步调用接口
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <param name="invokeHandler">调用委托</param>
        /// <param name="isSuccessHandler">返回委托，是否调用成功</param>
        public void AsyncInvoke<TContract>(Action<TContract> invokeHandler, Action<bool> isSuccess)
        {
            this.serviceInvoker.AsyncInvokeService<TContract>(invokeHandler, isSuccess);
        }

        /// <summary>
        /// 异步调用带返回值的接口
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <typeparam name="TResult">返回值</typeparam>
        /// <param name="invokeHandler">调用委托</param>
        /// <param name="resultHandler">返回委托，处理响应的数据</param>
        public void AsyncInvoke<TContract, TResult>(Func<TContract, TResult> invokeHandler, Action<TResult> isSuccess)
        {
            this.serviceInvoker.AsyncInvokeService<TContract, TResult>(invokeHandler, isSuccess);
        }
        #endregion
    }
}
