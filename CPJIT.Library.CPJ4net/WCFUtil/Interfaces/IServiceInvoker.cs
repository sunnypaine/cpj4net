using CPJIT.Library.CPJ4net.WCFUtil.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.CPJ4net.WCFUtil.Interfaces
{
    /// <summary>
    /// 服务调用
    /// </summary>
    internal interface IServiceInvoker
    {
        /// <summary>
        /// IP:Port形式的地址
        /// </summary>
        string IpAddress { get; set; }

        /// <summary>
        /// 传输协议类型
        /// </summary>
        TransferProtocol TransferProtocl { get; set; }

        /// <summary>
        /// 调用接口
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <param name="invokeHandler">调用委托</param>
        void InvokeService<TContract>(Action<TContract> invokeHandler);

        /// <summary>
        /// 调用带返回值的接口
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <typeparam name="TResult">返回值</typeparam>
        /// <param name="invokeHandler">调用委托</param>
        /// <returns></returns>
        TResult InvokeService<TContract, TResult>(Func<TContract, TResult> invokeHandler);

        /// <summary>
        /// 异步调用接口
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <param name="invokeHandler">调用委托</param>
        /// <param name="isSuccessHandler">返回委托，是否调用成功</param>
        void AsyncInvokeService<TContract>(Action<TContract> invokeHandler, Action<bool> isSuccess);

        /// <summary>
        /// 异步调用带返回值的接口
        /// </summary>
        /// <typeparam name="TContract">接口协议</typeparam>
        /// <typeparam name="TResult">返回值</typeparam>
        /// <param name="invokeHandler">调用委托</param>
        /// <param name="resultHandler">返回委托，处理响应的数据</param>
        void AsyncInvokeService<TContract, TResult>(Func<TContract, TResult> invokeHandler, Action<TResult> resultHandler);
    }
}
