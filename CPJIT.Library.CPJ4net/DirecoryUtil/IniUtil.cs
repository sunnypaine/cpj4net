/*********************************************************
 * 文 件 名: IniOperate
 * 命名空间：CPJIT.Util.FileUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/2 16:32:07
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.DirectoryUtil
{
    /// <summary>
    /// ini文件操作工具
    /// </summary>
    public class IniUtil
    {
        /// <summary>
        /// 写入ini配置
        /// </summary>
        /// <param name="section">小节名称</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回写入的字符串的长度</returns>
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /// <summary>
        /// 读取ini配置
        /// </summary>
        /// <param name="section">小节名称</param>
        /// <param name="key">键</param>
        /// <param name="def">指定的条目没有找到时返回的默认值</param>
        /// <param name="retVal">指定一个字串缓冲区</param>
        /// <param name="size">指定装载到BuildString缓冲区的最大字符数量</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回读取到的键对应的值</returns>
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 写入ini配置
        /// </summary>
        /// <param name="section">小节名称</param>
        /// <param name="key">键</param>
        /// <param name="value">The value.</param>
        /// <param name="path">文件路径(包含文件名称的完整路径)</param>
        public static void WriteINI(string section, string key, string value, string path)
        {
            if(System.IO.File.Exists(path)==false)
            {
                System.IO.File.Create(path).Close();
            }
            WritePrivateProfileString(section, key, value, path);
        }

        /// <summary>
        /// 读取ini配置
        /// </summary>
        /// <param name="section">小节名称</param>
        /// <param name="key">小节的键</param>
        /// <param name="def">指定的条目没有找到时返回的默认值</param>
        /// <param name="retVal">指定一个字串缓冲区，该字符串参数用于临时存储返回读取到的值</param>
        /// <param name="size">指定装载到BuildString缓冲区retVal的最大字符数量</param>
        /// <param name="path">文件路径(包含文件名称的完整路径)</param>
        /// <returns>返回读取到的小节的键对应的值</returns>
        public static string ReadINI(string section, string key, string def, StringBuilder retVal, int size, string path)
        {
            if (System.IO.File.Exists(path) == false)
            {
                System.IO.File.Create(path).Close();
            }
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, def, temp, 255, path);
            return temp.ToString();
        }
    }
}
