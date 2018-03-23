using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.PropertiesUtil.Attributes
{
    /// <summary>
    /// 表示一个类可自动注入配置。无法继承此类。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConfigurationAttribute : Attribute
    {
        private string uri;
        public string Uri
        {
            get { return this.uri; }
            set { this.uri = value; }
        }


        public ConfigurationAttribute()
        {
            this.uri = "application.properties";
        }

        /// <summary>
        /// 使用指定的参数创建CPJIT.Library.CPJ4net.ConfigUtil.Attributes.ConfigurationAttribute的实例。
        /// </summary>
        /// <param name="uri">*.properties配置文件的路径</param>
        public ConfigurationAttribute(string uri)
        {
            this.uri = uri;
        }
    }
}
