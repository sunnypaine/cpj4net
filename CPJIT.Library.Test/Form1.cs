﻿using CPJIT.Library.Util.ActivemqUtil;
using CPJIT.Library.Util.ActivemqUtil.Impl;
using CPJIT.Library.Util.DataBaseUtil;
using CPJIT.Library.Util.DataBaseUtil.Impl;
using CPJIT.Library.Util.WebServiceUtil;
using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace CPJIT.Library.Test
{
    public partial class Form1 : Form
    {
        #region 私有变量
        IActivemqClient activemqClient;
        #endregion



        public Form1()
        {
            InitializeComponent();
            this.FormClosing += this.Form1_FormClosing;
        }

        #region 本地事件

        private void Button1_Click(object sender, EventArgs e)
        {
        }

        private void Button2_Click(object sender, EventArgs e)
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

        private void Button3_Click(object sender, EventArgs e)
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
        private void Button4_Click(object sender, EventArgs e)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button5_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 测试Oracle数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// ActiveMQ测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button7_Click(object sender, EventArgs e)
        {
            activemqClient = new ActivemqClient("failover:tcp://192.168.0.1:61616", "admin", "admin");
            IMessageManager messageManager = new CPJ.Test.TestMessageProcess(activemqClient);
            activemqClient.Connect();
            messageManager.SubscribeDestination();
        }

        /// <summary>
        /// 窗体关闭前发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.activemqClient != null)
            {
                this.activemqClient.Close();
                this.activemqClient = null;
            }
        }
        #endregion
    }
}
