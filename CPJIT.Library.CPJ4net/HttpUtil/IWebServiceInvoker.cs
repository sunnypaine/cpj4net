using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPJIT.Library.CPJ4net.HttpUtil
{
    /// <summary>
    /// web服务调用接口。
    /// </summary>
    public interface IWebServiceInvoker
    {
        #region 公共属性
        /// <summary>
        /// WebService服务地址
        /// </summary>
        string WebServiceUrl { get; set; }

        /// <summary>
        /// 输出编译后的dll的文件名。例如：cpjit.test
        /// </summary>
        string OutputDllFilename { get; set; }

        /// <summary>
        /// 代理类的名称
        /// </summary>
        string ProxyClassName { get; set; }
        #endregion


        #region 公共方法
        /// <summary>
        /// 创建WebService，生成客户端代理程序集文件
        /// </summary>
        /// <returns>返回：true或false</returns>
        bool CreateWebService();

        /// <summary>
        /// 获取请求响应
        /// </summary>
        /// <param name="methodName">调用的目标方法名称</param>
        /// <param name="paras">参数</param>
        /// <returns>返回Json字符串</returns>
        T GetResponseString<T>(string methodName, params object[] paras);
        #endregion
    }
}
