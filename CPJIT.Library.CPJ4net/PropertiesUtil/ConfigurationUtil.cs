using CPJIT.Library.CPJ4net.PropertiesUtil.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.PropertiesUtil
{
    public static class ConfigurationUtil
    {
        /// <summary>
        /// 扫描类型的Configuration特性。
        /// </summary>
        /// <param name="t"></param>
        public static T ScanType<T>()
        {
            Type type = typeof(T);
            Attribute attributeConfigurationAttribute = type.GetCustomAttribute(typeof(ConfigurationAttribute));
            if (attributeConfigurationAttribute == null)//如果该类没有Configuration特性
            {
                return default(T);
            }
            ConfigurationAttribute configurationAttribute = (ConfigurationAttribute)attributeConfigurationAttribute;
            ReadProperties readProperties = null;
            if (!string.IsNullOrWhiteSpace(configurationAttribute.Uri))
            {
                readProperties = new ReadProperties(configurationAttribute.Uri);
            }
            else
            {
                readProperties = new ReadProperties();
            }

            T clazz = Activator.CreateInstance<T>();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (PropertyInfo pi in propertyInfos)//扫描类中具有ValueAttribute特性的属性。
            {
                Attribute attributeValueAttribute = pi.GetCustomAttribute(typeof(ValueAttribute));
                if (attributeValueAttribute == null)//如果该属性没有Value特性，则扫描下一个
                {
                    continue;
                }

                ValueAttribute valueAttribute = (ValueAttribute)attributeValueAttribute;
                string valueAttribute_name = pi.Name;
                if (!string.IsNullOrWhiteSpace(valueAttribute.Name))//如果属性的Value特性没有设置Name值
                {
                    valueAttribute_name = valueAttribute.Name;
                }
                //给属性赋值

                pi.SetValue(clazz, readProperties[valueAttribute_name], null);
            }
            return clazz;
        }
    }
}
