/*********************************************************
 * 文 件 名: XmlDocumentUtil
 * 命名空间：CPJ.Util.XmlDocUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/11/2 22:04:52
 * 描述说明： 
 * *******************************************************/

using CPJIT.Library.Util.SerializeUtil;
using CPJIT.Library.Util.XmlUtil.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CPJIT.Library.Util.XmlUtil
{
    /// <summary>
    /// XML工具
    /// </summary>
    public class XmlDocumentUtil
    {
        private XmlDocument xmlDoc;

        /// <summary>
        /// 从数据流中加载xml
        /// </summary>
        /// <param name="stream">数据流</param>
        public void Load(Stream stream)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(stream);
        }

        /// <summary>
        /// 从文件或者url中加载xml
        /// </summary>
        /// <param name="fileName">文件名称或者url</param>
        public void Load(string fileName)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
        }

        /// <summary>
        /// 从表示xml的字符串中加载xml
        /// </summary>
        /// <param name="xml">xml字符串</param>
        public void LoadXml(string xml)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
        }
        
        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public NodeInfo FindNode(string xpath)
        {
            NodeInfo info = null;
            try
            {
                XmlNode node = xmlDoc.SelectSingleNode(xpath);
                info = new NodeInfo();
                info.NodeName = node.Name;
                info.InnerText = node.InnerText;
                info.ChildNodes = node.ChildNodes;
                XmlAttributeCollection attributes = node.Attributes;
                if (attributes != null && attributes.Count > 0)
                {
                    info.HtAttribute = new Hashtable();
                    foreach (XmlAttribute attribute in attributes)
                    {
                        info.HtAttribute.Add(attribute.Name, attribute.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return info;
        }

        /// <summary>
        /// 获取节点列表
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public List<NodeInfo> FindNodes(string xpath)
        {
            List<NodeInfo> infos = null;
            try
            {
                XmlNodeList nodes = xmlDoc.SelectNodes(xpath);
                foreach (XmlNode node in nodes)
                {
                    NodeInfo info = new NodeInfo();
                    info.NodeName = node.Name;
                    info.InnerText = node.InnerText;
                    info.ChildNodes = node.ChildNodes;
                    XmlAttributeCollection attributes = node.Attributes;
                    if (attributes != null && attributes.Count > 0)
                    {
                        info.HtAttribute = new Hashtable();
                        foreach (XmlAttribute attribute in attributes)
                        {
                            info.HtAttribute.Add(attribute.Name, attribute.Value);
                        }
                    }
                    infos.Add(info);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return infos;
        }

        /// <summary>
        /// 获取指定节点的属性
        /// </summary>
        /// <param name="xpath">表示指定属性的xml结构路径</param>
        /// <returns></returns>
        public string FindAttribute(string xpath)
        {
            string attribute = null;
            try
            {
                attribute = xmlDoc.SelectSingleNode(xpath).InnerText;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return attribute;
        }

        /// <summary>
        /// 获取指定节点的实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public T FindNode2Object<T>(string xpath) where T : new()
        {
            T t = new T();
            try
            {
                XmlElement element = xmlDoc.SelectSingleNode(xpath) as XmlElement;
                if ((element != null) && !string.IsNullOrEmpty(element.InnerText))
                {
                    Type conversionType = typeof(T);
                    if (conversionType.IsValueType || (conversionType == typeof(string)))
                    {
                        return (T)Convert.ChangeType(element.InnerText, conversionType, null);
                    }
                    if (conversionType.IsClass)
                    {
                        t = XmlSerializeUtil.XmlStringToObject<T>(element.OuterXml);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return t;
        }

        /// <summary>
        /// 获取指定节点的实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public IList<T> FindNodes2ListObject<T>(string xpath) where T : new()
        {
            IList<T> list = null;
            try
            {
                XmlNodeList nodes = xmlDoc.SelectNodes(xpath);
                if ((nodes == null) || (nodes.Count <= 0))
                {
                    return list;
                }

                list = new List<T>();
                Type conversionType = typeof(T);
                if (conversionType.IsValueType || (conversionType == typeof(string)))
                {
                    T item = new T();
                    foreach (XmlElement element in nodes)
                    {
                        item = (T)Convert.ChangeType(element.InnerText, conversionType, null);
                        list.Add(item);
                    }
                    return list;
                }
                if (!conversionType.IsClass)
                {
                    return list;
                }
                StringBuilder builder = new StringBuilder();
                foreach (XmlNode node in nodes)
                {
                    builder.Append(node.OuterXml);
                }
                return XmlSerializeUtil.XmlStringToObject<List<T>>(builder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
