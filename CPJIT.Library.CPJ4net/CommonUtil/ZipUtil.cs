/*********************************************************
 * 文 件 名: ZipUtil
 * 命名空间：CPJIT.Util.CommonUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/2 16:16:12
 * 描述说明： 
 * *******************************************************/

using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.CommonUtil
{
    /// <summary>
    /// 解压缩工具类
    /// </summary>
    public class ZipUtil
    {
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="dirSource">文件源路径</param>
        /// <param name="zipFile">压缩文件名称</param>
        /// <param name="emptyFolder">是否压缩空文件夹</param>
        public static void ZipFile(string dirSource, string zipFile, bool emptyFolder)
        {
            FastZip fastzip = new FastZip();
            fastzip.CreateEmptyDirectories = emptyFolder;
            fastzip.CreateZip(zipFile, dirSource, true, string.Empty);
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="dirSource">文件源路径</param>
        /// <param name="zipFile">压缩文件名称</param>
        /// <param name="fileFilter">文件过滤条件</param>
        /// <param name="emptyFolder">是否压缩空文件夹</param>
        public static void ZipFile(string dirSource, string zipFile, string fileFilter, bool emptyFolder)
        {
            FastZip fastzip = new FastZip();
            fastzip.CreateEmptyDirectories = emptyFolder;
            fastzip.CreateZip(zipFile, dirSource, true, string.Empty);
        }


        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="zipFile">被解压的压缩文件</param>
        /// <param name="dirAim">解压到目标路径</param>
        /// <param name="emptyFolder">是否解压空文件夹</param>
        public static void UnZipFile(string zipFile, string dirAim, bool emptyFolder)
        {
            FastZip fastzip = new FastZip();
            //// Create Empty Directory
            fastzip.CreateEmptyDirectories = emptyFolder;
            fastzip.ExtractZip(zipFile, dirAim, string.Empty);
        }
    }
}
