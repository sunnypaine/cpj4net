/*********************************************************
 * 文 件 名: DelegateUtil
 * 命名空间：CPJIT.Util.CommonUtil
 * 开发人员：SunnyPaine
 * 创建时间：2016/9/2 16:36:26
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPJIT.Library.CPJ4net.CommonUtil
{
    /// <summary>
    /// 委托工具类
    /// </summary>
    public class DelegateUtil
    {
        /// <summary>
        /// 跨线程操作控件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="func"></param>
        public static void UIHelper(Control control,MethodInvoker func)
        {
            if (control.InvokeRequired == true)
            {
                control.BeginInvoke(func);
            }
            else
            {
                func();
            }
        }
        

        /// <summary>
        /// 委托声明--设置控件文本
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="text">文本</param>
        /// <param name="append">是否追加</param>
        public delegate void SetControlTextHandler(Control control, string text, bool append);
        /// <summary>
        /// 委托定义--设置控件文本
        /// </summary>
        public SetControlTextHandler setControlTextHandler;
        /// <summary>
        /// 委托方法--设置控件文本
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        /// <param name="append"></param>
        public void SetControlTextMethod(Control control, string text, bool append)
        {
            if (append)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(control.Text);
                sb.Append(text);
                control.Text = sb.ToString();
            }
            else
            {
                control.Text = text;
            }
        }

        /// <summary>
        /// 委托声明--设置控件是否启用
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="enable">是否启用（true：启用；false：禁用）</param>
        public delegate void SetControlEnableHandler(Control control, bool enable);
        /// <summary>
        /// 委托定义--设置控件是否启用
        /// </summary>
        public SetControlEnableHandler setControlEnableHandler;
        /// <summary>
        /// 委托方法--设置控件是否启用
        /// </summary>
        /// <param name="control">空间</param>
        /// <param name="enable"></param>
        public void SetControlEnableMethod(Control control, bool enable)
        {
            control.Enabled = enable;
        }

        /// <summary>
        /// 委托声明--设置控件是否显示
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="visibility">是否显示（true：显示；false：隐藏）</param>
        public delegate void SetControlVisibilityHandler(Control control, bool visibility);
        /// <summary>
        /// 委托定义--设置控件是否显示
        /// </summary>
        public SetControlVisibilityHandler setControlVisibilityHandler;
        /// <summary>
        /// 委托方法--设置空间是否显示
        /// </summary>
        /// <param name="control"></param>
        /// <param name="visibility"></param>
        public void SetControlVisibilityMethod(Control control, bool visibility)
        {
            control.Visible = visibility;
        }

        /// <summary>
        /// 委托声明--设置DataGridView数据
        /// </summary>
        /// <param name="grid">DataGridView控件</param>
        /// <param name="dt">DataTable数据</param>
        public delegate void SetDataGridViewDataByDataTableHandler(DataGridView grid, System.Data.DataTable dt);
        /// <summary>
        /// 委托定义--设置DataGridView数据
        /// </summary>
        public SetDataGridViewDataByDataTableHandler setDataGridViewDataByDataTableHandler;
        /// <summary>
        /// 委托方法--设置DataGridView数据
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="dt"></param>
        public void SetDataGridViewDataByDataTableMethod(DataGridView grid, System.Data.DataTable dt)
        {
            grid.DataSource = dt;
        }

        /// <summary>
        /// 委托声明--设置DataGridView数据
        /// </summary>
        /// <param name="grid">DataGridView控件</param>
        /// <param name="values">数据数组</param>
        public delegate void SetDataGridViewDataByObjectsHandler(DataGridView grid, object[] values);
        /// <summary>
        /// 委托定义--设置DataGridView数据
        /// </summary>
        public SetDataGridViewDataByObjectsHandler setDataGridViewDataByObjectsHandler;
        /// <summary>
        /// 委托方法--设置DataGridView数据
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="values"></param>
        /// <param name="count">表格中数据最大显示量。如果为0，则表示全都显示。</param>
        public void SetDataGridViewDataByObjectsMethod(DataGridView grid, object[] values, int count)
        {
            if (count == 0)
            {
                System.Windows.Forms.DataGridViewRow dgvr = new System.Windows.Forms.DataGridViewRow();
                dgvr.CreateCells(grid, values);
            }
        }
    }
}
