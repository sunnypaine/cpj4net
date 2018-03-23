using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPJIT.Library.CPJ4net.PropertiesUtil.Attributes
{
    /// <summary>
    /// 以声明方式实例化配置属性。无法继承此类。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValueAttribute : Attribute
    {
        public string Name
        { get; set; }

        public ValueAttribute()
        { }

        public ValueAttribute(string name)
        {
            this.Name = name;
        }
    }
}
