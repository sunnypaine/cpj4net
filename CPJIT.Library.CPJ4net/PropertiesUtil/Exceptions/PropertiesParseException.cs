using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.PropertiesUtil.Exceptions
{
    /// <summary>
    /// 当Properties配制文件的配制信息不合法时引发的异常。
    /// </summary>
    [Serializable]
    public class PropertiesParseException : Exception
    {
        public PropertiesParseException()
            : base()
        { }

        public PropertiesParseException(string message)
            : base(message)
        { }

        public PropertiesParseException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
