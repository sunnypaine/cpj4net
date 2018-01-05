/*********************************************************
 * 文 件 名: JsonSeializeUtil
 * 命名空间：CPJIT.Util.SerializeUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/2 16:06:59
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.JsonUtil
{
    /// <summary>
    /// Json序列化工具
    /// </summary>
    public class JsonSeializeUtil
    {
        /// <summary>
        /// 将类序列化成json规范的字符串
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        public static string ObjectToJsonString(object jsonObject)
        {
            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(jsonObject.GetType()).WriteObject(ms, jsonObject);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// 将json字符串反序列化为对应的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T JsonStringToObject<T>(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }
    }
}
