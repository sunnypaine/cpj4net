using CPJIT.Library.Util.SocketUtil;
using CPJIT.Library.Util.WebServiceUtil;
using System;
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
        private TcpClientUtil client;
        private UdpUtil udp;

        public Form1()
        {
            InitializeComponent();
        }

        #region 本地事件
        private void Form1_Load(object sender, EventArgs e)
        {
        }

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
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.client.Close();
            this.udp.Stop();
        }
        #endregion        
    }
}
