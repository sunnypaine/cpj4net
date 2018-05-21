using CPJIT.Library.CPJ4net.PropertiesUtil.Exceptions;
using System;
using System.Collections;
using System.Collections.Concurrent;
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
    public class PropertiesUtil
    {
        #region 私有变量
        /// <summary>
        /// 配置信息字典。
        /// </summary>
        IDictionary<string, string> dict = new ConcurrentDictionary<string, string>();
        #endregion


        #region 构造方法
        /// <summary>
        /// 使用指定的参数创建实例。
        /// </summary>
        /// <param name="propertiesUri">包含*.properties配置文件的路径。</param>
        /// <exception cref="ArgumentException">参数路径不合法。</exception>
        /// <exception cref="FileNotFoundException">指定的文件找不到。</exception>
        public PropertiesUtil(string propertiesUri)
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


        #region 公共属性
        /// <summary>
        /// 获取指定键的项。
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (dict.ContainsKey(key))
                {
                    return dict[key];
                }
                return null;
            }
        }

        /// <summary>
        /// 获取配置信息的所有键
        /// </summary>
        public ICollection<string> Keys
        {
            get { return dict.Keys; }
        }

        /// <summary>
        /// 获取配置信息的所有值。
        /// </summary>
        public ICollection<string> Values
        {
            get { return dict.Values; }
        }

        /// <summary>
        /// 获取配置信息的数量。
        /// </summary>
        public int Count
        {
            get { return dict.Count; }
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 加载并解析文件。
        /// </summary>
        /// <param name="proertiesUri"></param>
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
                    string value = string.IsNullOrWhiteSpace(kv[1]) ? string.Empty : kv[1].Trim();
                    this.Add(key, value);

                    lineIndex++;
                }
            }
        }

        private void Add(string key, string value)
        {
            if (dict.ContainsKey(key))
            {
                throw new ArgumentException("已添加了具有相同键的项。");
            }

            dict.Add(key, value);
        }
        #endregion
    }
}
