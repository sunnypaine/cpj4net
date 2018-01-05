/*********************************************************
 * 文 件 名: MD5Util
 * 命名空间：CPJIT.Util.ScurityUtil
 * 开发人员：SunnyPaine
 * 创建时间：2017/1/16 20:54:22
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.SecurityUtil
{
    /// <summary>
    /// 提供不可逆的MD5加密
    /// </summary>
    public class MD5Util
    {
        /// <summary>
        /// 获取字符串的MD5值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string GetMd5(string str)
        {
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();
            byte[] unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
