using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CPJIT.Library.Util.CommonUtil
{
    public class StringUtil
    {
        /// <summary>
        /// 将字符串转换成半角。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }

        /// <summary>
        /// 将字符串转换成全角。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String ToSBC(String input)
        {
            // 半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new String(c);
        }

        /// <summary>
        /// 获取字符串中的数字。
        /// </summary>
        /// <param name="str">被判断的字符串。</param>
        /// <returns>只包含数字的字符串。</returns>
        public static string GetNumber(string str)
        {
            string result = "";
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            str = Regex.Replace(str, @"[^\d.\d]", "");
            if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
            {
                result = str;
            }
            return result;
        }

        /// <summary>
        /// 判断字符串中是否含有英文字母。
        /// </summary>
        /// <param name="str">被判断的字符串。</param>
        /// <returns>true，含有；false，不含有。</returns>
        public static bool IsExistLetter(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            bool flag = false;
            char[] chars = str.ToCharArray();
            foreach (char c in chars)
            {
                if (Char.IsLetter(c))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
    }
}
