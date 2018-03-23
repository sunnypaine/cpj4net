using CPJIT.Library.CPJ4net.PropertiesUtil.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.PropertiesUtil
{
    /// <summary>
    /// 提供读取properties配置文件。
    /// </summary>
    public class ReadProperties : Hashtable
    {
        #region 私有变量
        #endregion


        #region 公共属性
        /// <summary>
        /// 获取 System.Collections.ICollection 包含中的键 System.Collections.Hashtable。
        /// </summary>
        public override ICollection Keys
        {
            get { return base.Keys; }
        }
        #endregion


        #region 构造方法
        /// <summary>
        /// 使用默认的参数创建实例。默认读取程序根路径下的application.properties
        /// </summary>
        /// <exception cref="FileNotFoundException">指定的文件application.properties找不到。</exception>
        public ReadProperties()
        {
            if (!File.Exists(@"application.properties"))
            {
                throw new FileNotFoundException("未找到指定的文件application.properties");
            }
            LoadFile("application.properties");
        }

        /// <summary>
        /// 使用指定的参数创建实例。
        /// </summary>
        /// <param name="propertiesUri">包含*.properties配置文件的路径。</param>
        /// <exception cref="ArgumentException">参数路径不合法。</exception>
        /// <exception cref="FileNotFoundException">指定的文件找不到。</exception>
        public ReadProperties(string propertiesUri)
        {
            if (!".properties".Equals(Path.GetExtension(propertiesUri)))
            {
                throw new ArgumentException("指定的参数propertiesUri不合法。路径中应以*.properties文件结尾。");
            }
            if (!File.Exists(propertiesUri))
            {
                throw new FileNotFoundException("未找到指定的文件" + propertiesUri + "。");
            }
            LoadFile(propertiesUri);
        }
        #endregion


        #region 私有方法
        private void LoadFile(string proertiesUri)
        {
            using (StreamReader reader = new StreamReader(proertiesUri))
            {
                string line;
                int lineIndex = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    //如果是空白行，跳过。
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    //如果是注释，跳过。
                    string firstChart = line.Trim().Substring(0, 1);
                    if ("#".Equals(firstChart))
                    {
                        continue;
                    }

                    if (!line.Contains("=") || Regex.Matches(line, @"=").Count > 1)
                    {
                        throw new PropertiesParseException("properties的一条配置信息必须包含有且仅有一个等号。");
                    }

                    string[] kv = line.Split('=');
                    string key = kv[0].Trim();
                    string value = kv[1].Trim();
                    this.Add(key, value);

                    lineIndex++;
                }
            }
        }
        #endregion


        #region 公共方法
        /// <summary>
        /// 将带有指定键和值的元素添加到 System.Collections.Hashtable 中。
        /// </summary>
        /// <param name="key">要添加的元素的键。</param>
        /// <param name="value">要添加的元素的值。该值不能为null。</param>
        public override void Add(object key, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("指定的参数value为null。");
            }
            base.Add(key, value);
        }
        #endregion
    }
}
