using CPJIT.Library.Util.DataBaseUtil;
using CPJIT.Library.Util.DataBaseUtil.Impl;
using CPJIT.Library.Util.SocketUtil;
using CPJIT.Library.Util.WebServiceUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPJIT.Library.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 本地事件

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WebServiceInvoker invoker = new WebServiceInvoker();
            invoker.WebServiceUrl = "http://www.webxml.com.cn/WebServices/WeatherWebService.asmx";
            invoker.ProxyClassName = "WeatherWebService";
            invoker.OutputDllFilename = "WeatherWebService.dll";
            invoker.CreateWebService();
            DataSet json = invoker.GetResponseString<DataSet>("getSupportDataSet");
            //object aa = invoker.GetResponseString("getWeatherbyCityName", new object[] { "成都" });
            DataTable dt = json.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                this.richTextBox1.AppendText(dr["ID"].ToString() + ":" + dr["Zone"].ToString() + Environment.NewLine);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IDBAccess dBAccess = new SQLiteHelper(@"test.db3");
            string cmdText = "select * from t_user where user_name=@UserName";
            Hashtable paras = new Hashtable();
            paras.Add("@UserName", this.tbUserName.Text);
            paras.Add("@Password", this.tbPassword.Text);
            DataSet ds = dBAccess.ExecuteDateSet(cmdText, CommandType.Text, paras);
            DataTable dt = ds.Tables[0];
            MessageBox.Show(dt.Rows[0]["user_name"].ToString() + "," + dt.Rows[0]["user_password"].ToString());
        }

        /// <summary>
        /// MySQL测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            IDBAccess dBAccess = new MySQLHelper("192.168.0.1", 3306, "blog", "root", "root");
            string cmdText = "select * from t_user where user_name=@UserName and user_password=@Password";
            Hashtable paras = new Hashtable();
            paras.Add("@UserName", this.tbUserName.Text);
            paras.Add("@Password", this.tbPassword.Text);
            DataSet ds = dBAccess.ExecuteDateSet(cmdText, CommandType.Text, paras);
            DataTable dt = ds.Tables[0];
            MessageBox.Show(dt.Rows[0]["user_name"].ToString() + ","
                + dt.Rows[0]["user_password"].ToString());
        }
        #endregion
    }
}
