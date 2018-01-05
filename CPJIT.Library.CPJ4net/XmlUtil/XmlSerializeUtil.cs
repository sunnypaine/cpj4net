/*********************************************************
 * 文 件 名: XmlSerializeUtil
 * 命名空间：CPJIT.Util.SerializeUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/2 16:06:12
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPJIT.Library.CPJ4net.XmlUtil
{
    /// <summary>
    /// Xml序列化工具
    /// </summary>
    public class XmlSerializeUtil
    {
        /// <summary>
        /// 将类序列化成xml规范的字符串
        /// </summary>
        /// <param name="xmlObject"></param>
        /// <returns></returns>
        public static string ObjectToXmlString(object xmlObject)
        {
            using (var ms = new MemoryStream())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add(string.Empty, string.Empty);

                XmlSerializer xmlSerializer = new XmlSerializer(xmlObject.GetType());
                xmlSerializer.Serialize(ms, xmlObject, ns);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// 将xml字符串反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T XmlStringToObject<T>(string xmlString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(ms);
            }
        }
    }
}
