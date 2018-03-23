using CPJIT.Library.CPJ4net.PropertiesUtil.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPJ.Test.Model.ConfigModel
{
    [Configuration]
    public class AppConfig
    {
        [Value(Name = "elk.wcf.bindingtype")]
        public string BindingType
        { get; set; }

        [Value(Name = "elk.wcf.tcpport")]
        public string TcpPort
        { get; set; }

        [Value(Name = "elk.wcf.httpport")]
        public string HttpPort
        { get; set; }

        [Value(Name = "elk.wcf.soapport")]
        public string SoapPort
        { get; set; }
    }
}
