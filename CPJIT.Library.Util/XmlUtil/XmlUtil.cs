/*********************************************************
 * 文 件 名: XmlUtil
 * 命名空间：CPJIT.Library.Util.XmlUtil
 * 开发人员：SunnyPaine
 * 创建时间：2017/12/21 22:04:52
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CPJIT.Library.Util.XmlUtil
{
    /// <summary>
    /// 基于XPath的XML工具。
    /// </summary>
    public sealed class XmlUtil
    {
        /// <summary>
        /// 获取表示目标xml节点内容的实体。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <param name="filePath">xml文件完整路径。</param>
        /// <param name="expression">xpath表达式。</param>
        /// <returns>返回实体。</returns>
        public static T GetObject<T>(string filePath, string expression)
        {
            if (System.IO.File.Exists(filePath) == false)
            {
                throw new ArgumentException("指定的参数filePath不合法，找不到指定的文件。");
            }
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException("指定参数expression不合法或为null。");
            }

            var model = default(T);
            XDocument xmlDoc = XDocument.Load(filePath);
            XElement xroot = xmlDoc.Root;//获取根节点
            XElement element = xroot.XPathSelectElement(expression);
            var type = typeof(T);
            if (type.IsValueType == true || type == typeof(string))
            {
                model = (T)Convert.ChangeType(element.Value, type, null);
            }
            else if (type.IsClass == true)
            {
                model = XmlSerializeUtil.XmlStringToObject<T>(element.ToString());
            }
            return model;
        }

        /// <summary>
        /// 获取表示目标xml节点内容的实体列表。
        /// </summary>
        /// <typeparam name="T">实体类型。</typeparam>
        /// <param name="filePath">xml文件完整路径。</param>
        /// <param name="expression">xpath表达式。</param>
        /// <returns>返回实体列表。</returns>
        public static IList<T> GetObjects<T>(string filePath, string expression)
        {
            if (System.IO.File.Exists(filePath) == false)
            {
                throw new ArgumentException("指定的参数filePath不合法，找不到指定的文件。");
            }
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException("指定参数expression不合法或为null。");
            }

            IList<T> list = new List<T>();
            XDocument xmlDoc = XDocument.Load(filePath);
            XElement xroot = xmlDoc.Root;//获取根节点
            IEnumerable<XElement> elements = xroot.XPathSelectElements(expression);
            var type = typeof(T);
            if (type.IsValueType == true || type == typeof(string))
            {
                foreach (XElement item in elements)
                {
                    var model = (T)Convert.ChangeType(item.Value, type, null);
                    list.Add(model);
                }
            }
            else if (type.IsClass == true)
            {
                foreach (XElement item in elements)
                {
                    var model = XmlSerializeUtil.XmlStringToObject<T>(item.ToString());
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 保存或更新节点信息。如果不存在该节点则新增，如果存在则修改。
        /// </summary>
        /// <param name="filePath">xml文件完整路径。</param>
        /// <param name="expression">xpath表达式。</param>
        /// <param name="elementName">被修改的节点。</param>
        /// <param name="elementVlaue">被修改的节点的值。</param>
        public static void SaveOrUpdateElement(string filePath, string expression, string elementName, string elementVlaue)
        {
            if (System.IO.File.Exists(filePath) == false)
            {
                throw new ArgumentException("指定的参数filePath不合法。未找到filePath指向的文件。");
            }
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException("指定的参数expression不合法或为null。");
            }
            if (string.IsNullOrWhiteSpace(elementName))
            {
                throw new ArgumentNullException("指定的参数elementName不合法或为null。");
            }
            if (string.IsNullOrWhiteSpace(elementVlaue))
            {
                throw new ArgumentNullException("指定的参数elementValue不合法或为null。");
            }

            try
            {
                XDocument xmlDoc = XDocument.Load(filePath);
                XElement xroot = xmlDoc.Root;//获取根节点
                IEnumerable<XElement> elements = xroot.XPathSelectElements(expression);
                foreach (XElement item in elements)
                {
                    item.Element(elementName).Value = elementVlaue;
                }
                xmlDoc.Save(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("保存或更新节点信息出错。", filePath, expression), ex);
            }
        }

        /// <summary>
        /// 保存或更新节点信息。如果不存在该节点则新增，如果存在则修改。
        /// </summary>
        /// <param name="filePath">xml文件完整路径。</param>
        /// <param name="expression">xpath表达式</param>
        /// <param name="param">要修改的节点信息。（键：节点名称；值：节点值）</param>
        public static void SaveOrUpdateElement(string filePath, string expression, IDictionary<string, object> param)
        {
            if (System.IO.File.Exists(filePath) == false)
            {
                throw new ArgumentException("指定的参数filePath不合法，找不到指定的文件。");
            }
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException("指定参数expression不合法或为null。");
            }
            if (param == null)
            {
                throw new ArgumentNullException("指定的参数param不合法或为null。");
            }
            if (param.Count <= 0)
            {
                throw new ArgumentException("指定的参数param至少应包含一组键值数据。");
            }

            try
            {
                XDocument xmlDoc = XDocument.Load(filePath);
                XElement xroot = xmlDoc.Root;//获取根节点
                XElement element = xroot.XPathSelectElement(expression);
                if (element == null)
                {
                    throw new ArgumentException("指定的参数expression不合法。为找到指定xpath表达式表示的xml节点及内容。");
                }
                else
                {
                    foreach (KeyValuePair<string, object> kv in param)
                    {
                        element.SetElementValue(kv.Key, kv.Value);
                    }
                }
                xmlDoc.Save(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("在xml文件{0}中使用表达式{1}更新值出错。", filePath, expression), ex);
            }
        }

        /// <summary>
        /// 保存或更新节点信息。如果不存在该节点则新增，如果存在则修改。
        /// </summary>
        /// <typeparam name="T">实体类型，限定为类。</typeparam>
        /// <param name="filePath">xml文件完整路径。</param>
        /// <param name="expression">xpath表达式。</param>
        /// <param name="model">实体对象。</param>
        public static void SaveOrUpdateElement<T>(string filePath, string expression, T model) where T : class
        {
            if (System.IO.File.Exists(filePath) == false)
            {
                throw new ArgumentException("指定的参数filePath不合法，找不到指定的文件。");
            }
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException("指定参数expression不合法或为null。");
            }
            if (model == null)
            {
                throw new ArgumentNullException("指定参数model不合法或为null。");
            }

            try
            {
                XDocument xmlDoc = XDocument.Load(filePath);
                XElement xroot = xmlDoc.Root;//获取根节点
                XElement element = xroot.XPathSelectElement(expression);
                if (element == null)
                {
                    throw new ArgumentException("指定的参数expression不合法。为找到指定xpath表达式表示的xml节点及内容。");
                }
                else
                {
                    Type type = model.GetType();
                    System.Reflection.PropertyInfo[] pis = type.GetProperties();
                    foreach (System.Reflection.PropertyInfo pi in pis)
                    {
                        if (pi == null)
                        {
                            continue;
                        }
                        if (pi.PropertyType.Name == "Boolean")
                        {
                            element.SetElementValue(pi.Name, pi.GetValue(model, null).ToString().ToLower());
                        }
                        else
                        {
                            element.SetElementValue(pi.Name, pi.GetValue(model, null).ToString());
                        }
                    }
                }
                xmlDoc.Save(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("在xml文件{0}中使用表达式{1}更新值出错。", filePath, expression), ex);
            }
        }

        /// <summary>
        /// 保存或更新节点的属性信息。如果不存在该节点的属性则新增，如果存在则修改。
        /// </summary>
        /// <param name="filePath">xml文件完整路径。</param>
        /// <param name="expression">xpath表达式。</param>
        /// <param name="param">要修改的节点信息。（键：节点名称；值：节点值）</param>
        public static void SaveOrUpdateAttribute(string filePath, string expression, IDictionary<string, object> param)
        {
            if (System.IO.File.Exists(filePath) == false)
            {
                throw new ArgumentException("指定的参数filePath不合法，找不到指定的文件。");
            }
            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new ArgumentNullException("指定参数expression不合法或为null。");
            }
            if (param == null)
            {
                throw new ArgumentNullException("指定的参数param不合法或为null。");
            }
            if (param.Count <= 0)
            {
                throw new ArgumentException("指定的参数param至少应包含一组键值数据。");
            }

            try
            {
                XDocument xmlDoc = XDocument.Load(filePath);
                XElement xroot = xmlDoc.Root;//获取根节点
                XElement element = xroot.XPathSelectElement(expression);
                if (element == null)
                {
                    throw new ArgumentException("指定的参数expression不合法。为找到指定xpath表达式表示的xml节点及内容。");
                }
                else
                {
                    foreach (KeyValuePair<string, object> kv in param)
                    {
                        element.SetAttributeValue(kv.Key, kv.Value);
                    }
                }
                xmlDoc.Save(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("在xml文件{0}中使用表达式{1}更新属性出错。", filePath, expression), ex);
            }
        }
    }
}
