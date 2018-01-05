using CPJIT.Library.CPJ4net.ActivemqUtil.Impl;
using CPJIT.Library.CPJ4net.ActivemqUtil.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPJIT.Library.CPJ4net.ActivemqUtil;
using CPJIT.Library.CPJ4net.ActivemqUtil.Model;

namespace CPJ.Test
{
    public class TestMessageProcess : AbstractMessageManager
    {
        public TestMessageProcess(IActivemqClient mqclient) : base(mqclient)
        {
            base.DestinationName = "Topic.Test";
            base.DestinationType = DestinationType.Topic;
            base.IsSubscibe = true;
        }

        protected override void ReciverMessage(object sender, DataEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            Console.WriteLine("接收到消息：" + e.Text);
        }
    }
}
