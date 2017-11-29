/*********************************************************
 * 文 件 名: NodeInfo
 * 命名空间：CPJ.Util.XmlUtil.Model
 * 开发人员：SunnyPaine
 * 创建时间：2016/11/3 9:20:20
 * 描述说明： 
 * *******************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CPJIT.Library.Util.XmlUtil.Model
{
    public class NodeInfo
    {
        private string nodeName;

        public string NodeName
        {
            get { return nodeName; }
            set { nodeName = value; }
        }

        private Hashtable htAttribute;

        public Hashtable HtAttribute
        {
            get { return htAttribute; }
            set { htAttribute = value; }
        }

        private XmlNodeList childNodes;

        public XmlNodeList ChildNodes
        {
            get { return childNodes; }
            set { childNodes = value; }
        }

        private string innerText;

        public string InnerText
        {
            get { return innerText; }
            set { innerText = value; }
        }
    }
}
