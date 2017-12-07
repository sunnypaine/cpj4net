/*********************************************************
 * 文 件 名: DateTimeUtil
 * 命名空间：CPJIT.Util.CommonUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/2 16:19:39
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.Util.CommonUtil
{
    /// <summary>
    /// 日期工具
    /// </summary>
    public class DateTimeUtil
    {
        /// <summary>
        /// 获取上月的第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastMonthFirstDay()
        {
            DateTime now = DateTime.Now;
            DateTime thisMonthFirstDay = new DateTime(now.Year, now.Month, 1);  //这月的第一天
            DateTime lastMonthFirstDay = thisMonthFirstDay.AddMonths(-1);  //上月的第一天

            return lastMonthFirstDay;
        }

        /// <summary>
        /// 获取上月的最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastMonthLastDay()
        {
            DateTime now = DateTime.Now;
            DateTime thisMonthFirstDay = new DateTime(now.Year, now.Month, 1);  //这月的第一天
            DateTime lastMonthLastDay = thisMonthFirstDay.AddDays(-1);  //上月的最后一天

            return lastMonthLastDay;
        }

        /// <summary>
        /// 获取本月的第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetThisMonthFirstDay()
        {
            DateTime now = DateTime.Now;
            DateTime thisMonthFirstDay = new DateTime(now.Year, now.Month, 1);

            return thisMonthFirstDay;
        }

        /// <summary>
        /// 获取本月的最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetThisMonthLastDay()
        {
            DateTime now = DateTime.Now;
            DateTime thisMonthFirstDay = new DateTime(now.Year, now.Month, 1);  // 这月第一天
            DateTime thisMonthLastDay = thisMonthFirstDay.AddMonths(1).AddDays(-1);  //这月最后一天

            return thisMonthLastDay;
        }

        /// <summary>
        /// 获取本月的第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNextMonthFirstDay()
        {
            DateTime now = DateTime.Now;
            DateTime thisMonthFirstDay = new DateTime(now.Year, now.Month, 1);  // 这月第一天
            DateTime nextMonthFirstDay = thisMonthFirstDay.AddMonths(1);  //下月的第一天

            return nextMonthFirstDay;
        }

        /// <summary>
        /// 获取下月的最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNextMonthLastDay()
        {
            DateTime now = DateTime.Now;
            DateTime thisMonthFirstDay = new DateTime(now.Year, now.Month, 1);  // 这月第一天
            DateTime nextMonthLastDay = thisMonthFirstDay.AddMonths(2).AddDays(-1);  //下月的最后一天

            return nextMonthLastDay;
        }

        /// <summary>
        /// 格式化日期为指定样式的字符串
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="dateTimeFormatStyle">日期字符串样式</param>
        /// <returns>日期字符串</returns>
        public static string DateTimeString(DateTime dateTime, DateTimeFormatStyle dateTimeFormatStyle)
        {
            string result = "";
            if (dateTimeFormatStyle == DateTimeFormatStyle.YYYY)
            {
                result = dateTime.Year.ToString();
            }
            else if (dateTimeFormatStyle == DateTimeFormatStyle.YYYYMM)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(dateTime.Year.ToString());
                sb.Append(dateTime.Month.ToString().PadLeft(2, '0'));

                result = sb.ToString();
                sb = null;
            }
            else if (dateTimeFormatStyle == DateTimeFormatStyle.YYYYMM1)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}/{1}", dateTime.Year.ToString().PadLeft(2, '0'),
                    dateTime.Month.ToString().PadLeft(2, '0'));

                result = sb.ToString();
                sb = null;
            }
            else if (dateTimeFormatStyle == DateTimeFormatStyle.YYYYMM2)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}-{1}", dateTime.Year.ToString().PadLeft(2, '0'),
                    dateTime.Month.ToString().PadLeft(2, '0'));

                result = sb.ToString();
                sb = null;
            }
            else if (dateTimeFormatStyle == DateTimeFormatStyle.YYYYMMDD)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(dateTime.Year.ToString());
                sb.Append(dateTime.Month.ToString().PadLeft(2, '0'));
                sb.Append(dateTime.Day.ToString().PadLeft(2, '0'));

                result = sb.ToString();
                sb = null;
            }
            else if (dateTimeFormatStyle == DateTimeFormatStyle.YYYYMMDD1)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}/{1}/{2}", dateTime.Year.ToString(),
                    dateTime.Month.ToString().PadLeft(2, '0'),
                    dateTime.Day.ToString().PadLeft(2, '0'));

                result = sb.ToString();
                sb = null;
            }
            else if (dateTimeFormatStyle == DateTimeFormatStyle.YYYYMMDD2)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}-{1}-{2}", dateTime.Year.ToString(),
                    dateTime.Month.ToString().PadLeft(2, '0'),
                    dateTime.Day.ToString().PadLeft(2, '0'));

                result = sb.ToString();
                sb = null;
            }
            else if (dateTimeFormatStyle == DateTimeFormatStyle.YYYYMMDDHHMMSS)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(dateTime.Year.ToString());
                sb.Append(dateTime.Month.ToString().PadLeft(2, '0'));
                sb.Append(dateTime.Day.ToString().PadLeft(2, '0'));
                sb.Append(dateTime.Hour.ToString().PadLeft(2, '0'));
                sb.Append(dateTime.Minute.ToString().PadLeft(2, '0'));
                sb.Append(dateTime.Second.ToString().PadLeft(2, '0'));

                result = sb.ToString();
                sb = null;
            }
            else if (dateTimeFormatStyle == DateTimeFormatStyle.YYYYMMDDHHMMSS1)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}/{1}/{2} {3}:{4}:{5}", dateTime.Year.ToString(),
                    dateTime.Month.ToString().PadLeft(2, '0'),
                    dateTime.Day.ToString().PadLeft(2, '0'),
                    dateTime.Hour.ToString().PadLeft(2, '0'),
                    dateTime.Minute.ToString().PadLeft(2, '0'),
                    dateTime.Second.ToString().PadLeft(2, '0'));

                result = sb.ToString();
                sb = null;
            }
            else if (dateTimeFormatStyle == DateTimeFormatStyle.YYYYMMDDHHMMSS2)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}-{1}-{2} {3}:{4}:{5}", dateTime.Year.ToString(),
                    dateTime.Month.ToString().PadLeft(2, '0'),
                    dateTime.Day.ToString().PadLeft(2, '0'),
                    dateTime.Hour.ToString().PadLeft(2, '0'),
                    dateTime.Minute.ToString().PadLeft(2, '0'),
                    dateTime.Second.ToString().PadLeft(2, '0'));

                result = sb.ToString();
                sb = null;
            }
            else if (dateTimeFormatStyle == DateTimeFormatStyle.YYYYMMDDHHMMSSFFF)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(dateTime.Year.ToString());
                sb.Append(dateTime.Month.ToString().PadLeft(2, '0'));
                sb.Append(dateTime.Day.ToString().PadLeft(2, '0'));
                sb.Append(dateTime.Hour.ToString().PadLeft(2, '0'));
                sb.Append(dateTime.Minute.ToString().PadLeft(2, '0'));
                sb.Append(dateTime.Second.ToString().PadLeft(2, '0'));
                sb.Append(dateTime.Millisecond.ToString().PadLeft(3, '0'));

                result = sb.ToString();
                sb = null;
            }
            else if (dateTimeFormatStyle == DateTimeFormatStyle.YYYYMMDDHHMMSSFFF1)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}/{1}/{2} {3}:{4}:{5}.{6}", dateTime.Year.ToString(),
                    dateTime.Month.ToString().PadLeft(2, '0'),
                    dateTime.Day.ToString().PadLeft(2, '0'),
                    dateTime.Hour.ToString().PadLeft(2, '0'),
                    dateTime.Minute.ToString().PadLeft(2, '0'),
                    dateTime.Second.ToString().PadLeft(2, '0'),
                    dateTime.Millisecond.ToString().PadLeft(3, '0'));

                result = sb.ToString();
                sb = null;
            }
            else if (dateTimeFormatStyle == DateTimeFormatStyle.YYYYMMDDHHMMSSFFF2)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}-{1}-{2} {3}:{4}:{5}.{6}", dateTime.Year.ToString(),
                    dateTime.Month.ToString().PadLeft(2, '0'),
                    dateTime.Day.ToString().PadLeft(2, '0'),
                    dateTime.Hour.ToString().PadLeft(2, '0'),
                    dateTime.Minute.ToString().PadLeft(2, '0'),
                    dateTime.Second.ToString().PadLeft(2, '0'),
                    dateTime.Millisecond.ToString().PadLeft(3, '0'));

                result = sb.ToString();
                sb = null;
            }

            return result;
        }
    }

    /// <summary>
    /// 时间格式化的样式
    /// </summary>
    public enum DateTimeFormatStyle
    {
        /// <summary>
        /// 仅显示年（例如：2015）
        /// </summary>
        YYYY,

        /// <summary>
        /// 显示年和月（例如：201506）
        /// </summary>
        YYYYMM,
        /// <summary>
        /// 显示年月日（例如：20150626）
        /// </summary>
        YYYYMMDD,
        /// <summary>
        /// 显示年月日时分秒（例如：20150626232856）
        /// </summary>
        YYYYMMDDHHMMSS,
        /// <summary>
        /// 显示年月日时分秒毫秒（例如：20150626232856165）
        /// </summary>
        YYYYMMDDHHMMSSFFF,

        /// <summary>
        /// 显示年和月（例如：2015/06）
        /// </summary>
        YYYYMM1,
        /// <summary>
        /// 显示年月日（例如：2015/06/26）
        /// </summary>
        YYYYMMDD1,
        /// <summary>
        /// 显示年月日时分秒（例如：2015/06/26 23:33:45）
        /// </summary>
        YYYYMMDDHHMMSS1,
        /// <summary>
        /// 显示年月日时分秒毫秒（例如：2015/06/26 23:33:45.165）
        /// </summary>
        YYYYMMDDHHMMSSFFF1,

        /// <summary>
        /// 显示年和月（例如：2015-06）
        /// </summary>
        YYYYMM2,
        /// <summary>
        /// 显示年月日（例如：2015-06-26）
        /// </summary>
        YYYYMMDD2,
        /// <summary>
        /// 显示年月日时分秒（例如：2015-06-26 23:33:45）
        /// </summary>
        YYYYMMDDHHMMSS2,
        /// <summary>
        /// 显示年月日时分秒毫秒（例如：2015-06-26 23:33:45.165）
        /// </summary>
        YYYYMMDDHHMMSSFFF2
    }
}
