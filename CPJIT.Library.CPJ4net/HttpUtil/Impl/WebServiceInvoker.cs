/*********************************************************
 * 文 件 名: WebServiceUtil
 * 命名空间：CPJIT.Util.WebServiceUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/2 16:19:39
 * 描述说明： 
 * *******************************************************/

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml.Serialization;

namespace CPJIT.Library.CPJ4net.HttpUtil.Impl
{
    /// <summary>
    /// 提供webservice动态调用。
    /// </summary>
    public class WebServiceInvoker : IWebServiceInvoker
    {
        #region 私有变量
        /// <summary>
        /// WebService代理类实例
        /// </summary>
        private object proxyClassInstance;

        /// <summary>
        /// 接口方法字典。键：目标方法名称；值：方法信息
        /// </summary>
        private Dictionary<string, MethodInfo> dictMethod = new Dictionary<string, MethodInfo>();
        #endregion


        #region 公共属性
        /// <summary>
        /// WebService服务地址
        /// </summary>
        public string WebServiceUrl { get; set; }

        /// <summary>
        /// 输出编译后的dll的文件名。例如：cpjit.test
        /// </summary>
        public string OutputDllFilename { get; set; }

        /// <summary>
        /// 代理类的名称
        /// </summary>
        public string ProxyClassName { get; set; }
        #endregion


        #region 私有方法

        #endregion


        #region 构造方法
        /// <summary>
        /// 使用默认信息初始化WebServiceInvoker的实例
        /// </summary>
        public WebServiceInvoker()
        { }

        /// <summary>
        /// 使用指定信息初始化WebServiceInvoker的实例
        /// </summary>
        /// <param name="webserviceUrl">WebService服务地址</param>
        /// <param name="outputDllFilename">输出编译后的dll的文件名。例如：cpjit.test</param>
        /// <param name="proxyClassName">代理类的名称</param>
        public WebServiceInvoker(string webserviceUrl, string outputDllFilename, string proxyClassName)
        {
            this.WebServiceUrl = webserviceUrl;
            this.OutputDllFilename = outputDllFilename;
            this.ProxyClassName = proxyClassName;
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 反射构建Methods
        /// </summary>
        private void BuildMethods()
        {
            Assembly asm = Assembly.LoadFrom(this.OutputDllFilename);
            Type asmType = asm.GetType(this.ProxyClassName);
            this.proxyClassInstance = Activator.CreateInstance(asmType);

            var methods = asmType.GetMethods();
            foreach (var item in methods)
            {
                if (item != null && this.dictMethod.ContainsKey(item.Name) == false)
                {
                    this.dictMethod.Add(item.Name, item);
                }
            }
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 创建WebService，生成客户端代理程序集文件
        /// </summary>
        /// <returns>返回：true或false</returns>
        public bool CreateWebService()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.WebServiceUrl))
                {
                    throw new MissingMemberException("必须属性对WebServiceUrl赋值。");
                }
                if (string.IsNullOrWhiteSpace(this.OutputDllFilename) == true)
                {
                    throw new MissingMemberException("必须属性对OutputDllFilename赋值。");
                }
                if (string.IsNullOrWhiteSpace(this.ProxyClassName) == true)
                {
                    throw new MissingMemberException("必须属性对ProxyClassName赋值。");
                }
                this.WebServiceUrl += "?WSDL";

                // 如果程序集已存在，直接使用
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, this.OutputDllFilename)))
                {
                    BuildMethods();
                    return true;
                }

                //使用 WebClient 下载 WSDL 信息。
                WebClient web = new WebClient();
                Stream stream = web.OpenRead(this.WebServiceUrl);

                //创建和格式化 WSDL 文档。
                if (stream != null)
                {
                    // 格式化WSDL
                    ServiceDescription description = ServiceDescription.Read(stream);

                    // 创建客户端代理类。
                    ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
                    importer.ProtocolName = "Soap";
                    importer.Style = ServiceDescriptionImportStyle.Client;
                    importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;

                    // 添加 WSDL 文档。
                    importer.AddServiceDescription(description, null, null);

                    //使用 CodeDom 编译客户端代理类。
                    CodeNamespace nmspace = new CodeNamespace();
                    CodeCompileUnit unit = new CodeCompileUnit();
                    unit.Namespaces.Add(nmspace);

                    //ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
                    importer.Import(nmspace, unit);
                    CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");

                    CompilerParameters parameter = new CompilerParameters();
                    parameter.GenerateExecutable = false;
                    parameter.OutputAssembly = this.OutputDllFilename;

                    parameter.ReferencedAssemblies.Add("System.dll");
                    parameter.ReferencedAssemblies.Add("System.XML.dll");
                    parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
                    parameter.ReferencedAssemblies.Add("System.Data.dll");

                    // 编译输出程序集
                    CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);

                    // 使用 Reflection 调用 WebService。
                    if (!result.Errors.HasErrors)
                    {
                        BuildMethods();
                        stream.Close();
                        stream.Dispose();
                        return true;
                    }
                    else
                    {
                        stream.Close();
                        stream.Dispose();
                        throw new AmbiguousMatchException("反射生成dll文件时异常。");
                    }
                }
                else
                {
                    throw new WebException("打开webservice失败。");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("创建WebService，生成客户端代理程序集文件出错。", ex);
            }
        }

        /// <summary>
        /// 获取请求响应
        /// </summary>
        /// <param name="methodName">调用的目标方法名称</param>
        /// <param name="paras">参数</param>
        /// <returns>返回Json字符串</returns>
        public T GetResponseString<T>(string methodName, params object[] paras)
        {
            T t;

            //string result = null;
            if (this.dictMethod.ContainsKey(methodName))
            {
                t = (T)this.dictMethod[methodName].Invoke(this.proxyClassInstance, paras);
                if (t != null)
                {
                    return t;
                }
            }
            return default(T);
        }
        #endregion
    }
}
