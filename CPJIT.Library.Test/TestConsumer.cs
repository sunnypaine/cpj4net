using CPJIT.Library.CPJ4net.ActivemqUtil;
using CPJIT.Library.CPJ4net.ActivemqUtil.Enum;
using CPJIT.Library.CPJ4net.ActivemqUtil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPJ.Test
{
    public class TestConsumer : IConsumer
    {
        #region 公共属性，IConsumer成员
        public string DestinationName { get; set; }

        public DestinationType DestinationType { get; set; }

        public bool IsSubscibe { get; set; }
        #endregion


        #region 构造方法
        public TestConsumer()
        {
            this.DestinationName = "Topic.TestConsumer";
            this.DestinationType = DestinationType.Topic;
            this.IsSubscibe = true;
        }
        #endregion


        #region 公共方法，IConsumer成员
        public void Receive(object sender, DataEventArgs args)
        {
            if (args == null)
            {
                return;
            }

            Console.WriteLine("接收到消息：" + args.Text);
        }
        #endregion
    }
}
