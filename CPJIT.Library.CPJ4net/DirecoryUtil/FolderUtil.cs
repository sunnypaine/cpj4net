/*********************************************************
 * 文 件 名: FolderOperateUtil
 * 命名空间：CPJIT.Util.FolderUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/2 16:34:32
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.DirectoryUtil
{
    /// <summary>
    /// 文件夹操作工具
    /// </summary>
    public class FolderUtil
    {
        #region 私有变量
        /// <summary>
        /// 文件可读写标识符。
        /// </summary>
        private static readonly int OF_READWIRTE = 2;

        /// <summary>
        /// 文件共享标识符。
        /// </summary>
        private static readonly int OF_SHARE_DENY_NONE = 0x40;

        /// <summary>
        /// 文件被占用时的标识符。
        /// </summary>
        private static readonly IntPtr HFILE_ERROR = new IntPtr(-1);
        #endregion


        #region 私有方法
        /// <summary>
        /// 获取文件占用状态。
        /// </summary>
        /// <param name="lpPathName">文件路径。</param>
        /// <param name="iReadWrite">文件读写权限标识。</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        /// <summary>
        /// 解除文件或文件夹占用。
        /// </summary>
        /// <param name="hObject">文件占用状态。</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);
        #endregion


        #region 公共方法
        /// <summary>
        /// 文件是否被占用。
        /// </summary>
        /// <param name="filePath">文件路径。</param>
        /// <returns>true，被占用；false，未被占用。</returns>
        public static bool IsOccupied(string filePath)
        {
            IntPtr vHandle = _lopen(filePath, OF_READWIRTE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 解除文件占用。
        /// </summary>
        /// <param name="filePath">文件路径。</param>
        public static void RelieveOccupied(string filePath)
        {
            IntPtr vHandle = _lopen(filePath, OF_READWIRTE | OF_SHARE_DENY_NONE);
            CloseHandle(vHandle);
        }

        /// <summary>
        /// 通过堆栈的方式获取指定文件夹下的所有文件。不包含子文件夹。
        /// </summary>
        /// <param name="dirPath">指定文件夹的路径。</param>
        /// <returns>子文件路径。</returns>
        public static List<string> GetAllFilesByStack(string dirPath)
        {
            List<string> list = null;
            try
            {
                list = new List<string>();
                Stack<string> skTmpDir = new Stack<string>();
                skTmpDir.Push(dirPath);
                while (skTmpDir.Count > 0)
                {
                    string tmpDirPath = skTmpDir.Pop();
                    string[] subDirs = Directory.GetDirectories(tmpDirPath);
                    string[] subFiles = Directory.GetFiles(tmpDirPath);
                    if (subDirs != null && subDirs.Length > 0)
                    {
                        foreach (string subDir in subDirs)//将文件夹推入堆栈
                        {
                            skTmpDir.Push(subDir);
                        }
                    }
                    if (subFiles != null && subFiles.Length > 0)
                    {
                        foreach (string subFile in subFiles)
                        {
                            list.Add(subFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CPJIT.Util.DirectoryUtil.GetAllFilesByStack(string dirPath)出错,", ex);
            }
            return list;
        }

        /// <summary>
        /// 通过堆栈的方式获取指定文件夹下的所有文件夹及文件。
        /// </summary>
        /// <param name="dirPath">指定文件夹的路径。</param>
        /// <returns>子文件和子文件夹的路径。</returns>
        public static List<string> GetAllFilesAndFoldersByStack(string dirPath)
        {
            List<string> list = null;
            try
            {
                list = new List<string>();
                Stack<string> skTmpDir = new Stack<string>();
                skTmpDir.Push(dirPath);
                while (skTmpDir.Count > 0)
                {
                    string tmpDirPath = skTmpDir.Pop();
                    string[] subDirs = Directory.GetDirectories(tmpDirPath);
                    string[] subFiles = Directory.GetFiles(tmpDirPath);
                    if (subDirs != null && subDirs.Length > 0)
                    {
                        foreach (string subDir in subDirs)//将文件夹推入堆栈
                        {
                            skTmpDir.Push(subDir);
                        }
                    }
                    if (subFiles != null && subFiles.Length > 0)
                    {
                        foreach (string subFile in subFiles)
                        {
                            list.Add(subFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CPJIT.Util.DirectoryUtil.GetAllFilesByStack(string dirPath)出错,", ex);
            }
            return list;
        }













        /// <summary>
        /// 获取指定文件夹下的所有文件（包括子文件夹的文件）
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="searchPattern">搜索的条件。只获取该指定后缀的文件。如“*.txt”</param>
        /// <returns></returns>
        public static List<string> GetAllFiles(string dirPath, string searchPattern = "")
        {
            List<string> list = null;
            try
            {
                list = new List<string>();
                Stack<string> skDir = new Stack<string>();
                skDir.Push(dirPath);
                while (skDir.Count > 0)
                {
                    dirPath = skDir.Pop();
                    string[] subDirs = System.IO.Directory.GetDirectories(dirPath);
                    string[] subFiles = null;
                    if (string.IsNullOrWhiteSpace(searchPattern))
                    {
                        subFiles = System.IO.Directory.GetFiles(dirPath);
                    }
                    else
                    {
                        subFiles = System.IO.Directory.GetFiles(dirPath, searchPattern);
                    }
                    if (subDirs != null)
                    {
                        for (int i = 0; i < subDirs.Length; i++)
                        {
                            skDir.Push(subDirs[i]);
                        }
                    }

                    if (subFiles != null)
                    {
                        list.AddRange(subFiles);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// 将整个文件夹复制到目标文件夹中。
        /// </summary>
        /// <param name="srcPath">源文件夹</param>
        /// <param name="aimPath">目标文件夹</param>
        public static void CopyDirectory(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    aimPath += Path.DirectorySeparatorChar;
                }
                // 判断目标目录是否存在如果不存在则新建之
                if (!System.IO.Directory.Exists(aimPath))
                {
                    System.IO.Directory.CreateDirectory(aimPath);
                }
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (System.IO.Directory.Exists(file))
                    {
                        CopyDirectory(file, aimPath + Path.GetFileName(file));
                    }
                    // 否则直接Copy文件
                    else
                    {
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将整个文件夹删除
        /// </summary>
        /// <param name="aimPath">目标文件夹</param>
        public static void DeleteDirectory(string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    aimPath += Path.DirectorySeparatorChar;
                }
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向Delete目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles(aimPath);
                string[] fileList = System.IO.Directory.GetFileSystemEntries(aimPath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Delete该目录下面的文件
                    if (System.IO.Directory.Exists(file))
                    {
                        DeleteDirectory(aimPath + Path.GetFileName(file));
                    }
                    // 否则直接Delete文件
                    else
                    {
                        File.Delete(aimPath + Path.GetFileName(file));
                    }
                }
                //删除文件夹
                System.IO.Directory.Delete(aimPath, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除指定文件夹下的文件和子文件夹
        /// </summary>
        /// <param name="dirPath">目标路径</param>
        /// <param name="deleteFolder">是否删除子文件夹</param>
        public static void DeleteDirectory(string dirPath, bool deleteFolder)
        {
            //删除这个目录下的所有子目录
            if (deleteFolder == true)
            {
                if (System.IO.Directory.GetDirectories(dirPath).Length > 0)
                {
                    foreach (string var in System.IO.Directory.GetDirectories(dirPath))
                    {
                        System.IO.Directory.Delete(var, true);
                    }
                }
            }
            //删除这个目录下的所有文件
            if (System.IO.Directory.GetFiles(dirPath).Length > 0)
            {
                foreach (string var in System.IO.Directory.GetFiles(dirPath))
                {
                    File.Delete(var);
                }
            }
        }

        /// <summary>
        /// 返回指定路径上层目录
        /// </summary>
        /// <param name="sourceDir">指定路径</param>
        /// <param name="level">返回指定路径几层</param>
        public static string ReturnUpDirectory(string sourceDir, int level)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < level; i++)
            {
                sb.Append(@"..\");
            }
            DirectoryInfo di = new DirectoryInfo(string.Format(@"{0}" + sb.ToString(), sourceDir));
            sb = null;
            return di.FullName;
        }
        #endregion
    }
}
