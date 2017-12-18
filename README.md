# cpj4net
cpj4net是cpjit团队的C#语言公共库，为开发者封装了一些常用、实用的工具。为项目源码架构的工作提供便利，增强源码的复用，极大的减少代码的重复工作量。    
因为处于某些原因，上次将本库从github删除，感谢之前支持并点赞的码友。从本次提交开始，后续将不停的对cpj4net进行更新。    


## 目录
[`一. 更新说明`](#一-更新说明)    

[`二. 引用组件`](#二-引用组件)    

[`三. 主要工具类说明`](#三-主要工具类说明)    

[`四. 其它`](#二-其它)

## 一. 更新说明
1. 新增了WCFInvoker。（2017.11.29）
2. 将数据库交互类接口化。（2017.12.04）
3. 新增了ActivemqUtil。（2017.12.07）
4. 去掉了某些小于vs2017版本无法编译的语法糖。    
包装win32 API，录制普通音质音频的工具。（2017.12.13）
5. 解决TCPClientUtil和TcpServerUtil接收数据粘包的问题。（2017.12.16）
6. 引入Newtonsoft.Json.dll，封装json格式化工具和字符串是否为json的校验工具。（2017.12.18）

## 引用组件
工程中有一个dll文件夹，因为cpj4net中的有一些简单的工具类并非是原生工具，而是对一些工具类进行了二次封装。    
需要引用的dll有：
- ICSharpCode.SharpZipLib.dll
- Ionic.Zip.dll
- Oracle.ManagedDataAccess.dll
- SQLite.Interop.dll
- System.Data.SQLite.dll
- Apache.NMS.ActiveMQ.dll
- Apache.NMS.dll


注意：cpj4net的Oracle数据库交互工具是依赖Oracle.ManagedDataAccess.dll，该dll不依赖Oracle客户端应用，但是执行效率相比依赖Oracle客户端的方式要低一点。使用该工具类的码友请根据实际运用场景的数据交互量决定。    

## 主要工具类说明
### 1. IniUtil.cs
#### 1.1 public static void WriteINI(string section, string key, string value, string path)
`说明`  写入ini配置信息。如果配置文件的section不存在将添加，如果存在，将修改或添加key&value。    

`参数`    
- section：表示一组字典的标识符。
- key：表示配置信息的键。
- value：表示配置信息的值。
- path：表示ini配置文件的路径。    

`使用示例`
```
IniUtil.WriteINI("Food", "Apple", "5", "d:/test.ini");
```
上面的代码执行后，d盘中test.ini文件里的内容为：
```
[Food]
Apple=5
```

#### 2.2 public static string ReadINI(string section, string key, string def, StringBuilder retVal, int size, string path)
`说明`  读取ini配置信息。    

`参数`    
- section：表示一组字典的标识符。
- key：表示配置信息的键。
- def：表示当没有找到section或者key时，返回的默认值。
- retVal：表示数据缓冲区。
- size：表示缓冲区大小。
- path：ini配置文件的路径。    

`使用示例`    
```
StringBuilder sb = new StringBuilder();
string count = IniOperate.ReadINI("Food", "Apple", "0", sb, 128, "d:/test.ini");
Console.WriteLine("Apple的数量为：" + count);
```
上面的代码执行后的结果为：    
Apple的数量为：5    

### 2. DelegateUtil.cs
#### 2.1 public static void UIHelper(Control control,MethodInvoker func)
`说明`  操作主线程中父类为Control的控件。    

`参数`    
- control：视图中的控件，例如Button，TextBox。不能是ToolStripMenuItem之类的控件，因为它们的父类不是Control。
- func：操作控件的委托方法。    

`使用示例`
```
DelegateUtil.UIHelper(this.textBox, ()=>
{
    this.textBox.Text = "Hello World！";
});
```

### 3. TcpServerUtil.cs
`说明` 提供TCP服务端的消息通讯工具。    

`使用示例`
```
class Program
{
    private TcpServerUtil tcpServer;

    public Form1()
    {
        InitializeComponent();
        
        this.tcpServer = new TcpServerUtil("192.168.0.1", "6666");
        //表示一条新消息的终止符为“:exit”，以此分包。该值可以自定义设置。如果不设置，则消息就不处理分包。
        this.tcpServer.Terminator = ":exit";
        this.tcpServer.OnReceived += TcpServer_OnReceived;
        this.tcpServer.OnConnected += TcpServer_OnConnected;
        this.tcpServer.OnDisconnected += TcpServer_OnDisconnected;
        this.tcpServer.OnServerException += TcpServer_OnServerException;
        this.tcpServer.Start();//开启监听
    }

    /// <summary>
    /// 当接收到消息时发生
    /// </summary>
    /// <param name="session"></param>
    private void TcpServer_OnReceived(Session session)
    {
        //TODO
    }

    /// <summary>
    /// 当客户端连接上来时发生
    /// </summary>
    /// <param name="session"></param>
    private void TcpServer_OnConnected(Session session)
    {
        //TODO
    }

    /// <summary>
    /// 当客户端断开时发生
    /// </summary>
    /// <param name="session"></param>
    private void TcpServer_OnDisconnected(Session session)
    {
        //TODO
    }
    /// <summary>
    /// 当TcpServerUtil出现异常时发生
    /// </summary>
    /// <param name="session"></param>
    /// <param name="ex"></param>
    private void TcpServer_OnServerException(Session session, Exception ex)
    {
        //TODO
    }

    ~Program()
    {
        this.tcpServer.Stop();//停止监听
    }
}
```

### 4. TcpClientUtil.cs
`说明` 提供TCP客户端的消息通讯工具。    

`使用示例`
```
class Program
{
    private TcpClientUtil tcpClient;

    public Form1()
    {
        InitializeComponent();
        
        this.tcpClient = new TcpClientUtil("192.168.0.1", "6666");
        this.tcpClient.OnReceived += TcpClient_OnReceived;
        this.tcpClient.OnConnected += TcpClient_OnConnected;
        this.tcpClient.OnDisconnected += TcpClient_OnDisconnected;
        this.tcpClient.OnServerException += TcpClient_OnServerException;
        this.tcpClient.Connect();//连接到服务端，如果要判断连接成功与否，请使用下面这一行代码。
        //bool flag = this.tcpClient.Connect().IsConnect;//连接到服务端，并返回连接成功与否的结果。
    }

    /// <summary>
    /// 当接收到消息时发生
    /// </summary>
    /// <param name="args"></param>
    private void TcpClient_OnReceived(DataArgs args)
    {
        //TODO
    }

    /// <summary>
    /// 当连上服务端时发生
    /// </summary>
    private void TcpClient_OnConnected()
    {
        //TODO
    }

    /// <summary>
    /// 当与服务端断开时发生
    /// </summary>
    private void TcpClient_OnDisconnected()
    {
        //TODO
    }
    /// <summary>
    /// 当TcpClientUtil出现异常时发生
    /// </summary>
    /// <param name="ex"></param>
    private void TcpClient_OnServerException(Exception ex)
    {
        //TODO
    }

    ~Program()
    {
        this.tcpClient.Close();//断开与服务端的连接
    }
}
```

### 5. UdpUtil.cs
`说明`  提供Udp通讯工具。

`使用示例`
```
class Program
{
    private UdpUtil udp;

    public Form1()
    {
        InitializeComponent();

        this.udp = new UdpUtil("192.168.0.1", "6666");
        this.udp.OnReceived += Udp_OnReceived;
        this.udp.Start();//开启监听
    }

    /// <summary>
    /// 当接收到消息时发生
    /// </summary>
    /// <param name="args"></param>
    private void Udp_OnReceived(DataArgs args)
    {
        //TODO
    }

    ~Program()
    {
        this.udp.Stop();//停止监听
    }
}
```

### 6. WcfInvoker
`说明` 提供WCF服务接口式动态调用工具。    

`使用示例`
```
static void Main()
{
    WCFInvoker invoker = new WCFInvoker("192.168.0.1", 6666, TransferProtocol.TCP);

    //添加一个用户（编号，用户名，密码，邮箱，生日，性别）
    //IUserContract是服务端的WCF服务暴露的接口，该接口会被客户端以dll的形式引用到工程中。
    //AddUser是IuserContract接口中的一个定义。
    bool result = invoker.Invoke<IUserContract, bool>(p =>
    {
        return p.AddUser("001", "admin", "1234", "test@163.com", "2017/11/30", "M");
    });
    
    if (result == true)
    {
        Console.WriteLine("添加成功！");
    }
}
```

### 7. WebServiceInvoker.cs
`说明`  提供WebService动态调用工具。而无需生成代理类的方式。相对要更灵活。本质上是通过代码动态的生成了代理类的dll，所以效率相对人工的先生成代理类慢一些。    

`使用示例`
```
void Main()
{
	WebServiceInvoker invoker = new WebServiceInvoker();

	invoker.WebServiceUrl = "http://www.webxml.com.cn/WebServices/WeatherWebService.asmx";//我们以网上提供的一些免费的天气预报ws服务为例

	invoker.ProxyClassName = "WeatherWebService";

	invoker.OutputDllFilename = "WeatherWebService.dll";

	invoker.CreateWebService();

	DataSet ds= invoker.GetResponseString<DataSet>("getSupportDataSet");

	//object aa = invoker.GetResponseString("getWeatherbyCityName", new object[] { "成都" });

	DataTable dt = ds.Tables[0];

	foreach (DataRow dr in dt.Rows)
	{
		Console.WriteLine(dr["ID"].ToString() + ":" + dr["Zone"].ToString() + Environment.NewLine);
	}
}
```

### 8. ActivemqUtil
`说明` 将NMS进行封装。提供程序作为ActiveMQ的消费者时实现消息路由机制；提供程序作为ActiveMQ的消息生产者时，更方面的发送消息。此工具可使用IOC框架完成消息路由，完美契合，非常方便。    

`使用示例-常规使用`
```
//该示例在解决方案中的Test工程中一斤更有示例了。详细的可以查看demo。
class Program
{
    void Main()
    {
        activemqClient = new ActivemqClient("failover:tcp://192.168.0.1:61616", "admin", "admin");

        IMessageManager messageManager1 = new CPJ.Test.TestMessageProcess1(activemqClient);
        IMessageManager messageManager2 = new CPJ.Test.TestMessageProcess2(activemqClient);

        activemqClient.Connect();

        messageManager1.SubscribeDestination();//TestMessageProcess1订阅并接收消息
        messageManager2.SubscribeDestination();//TestMessageProcess2订阅并接收消息
    }
}

class TestMessageProcess1 : AbstractMessageManager
{
    public TestMessageProcess1(IActivemqClient mqclient) : base(mqclient)
    {
        base.DestinationName = "Topic.Test1";
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

class TestMessageProcess2 : AbstractMessageManager
{
    public TestMessageProcess(IActivemqClient mqclient) : base(mqclient)
    {
        base.DestinationName = "Topic.Test2";
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
```
`使用示例-IOC容器使用`
```
///IOC容器举例以Microsoft.Practice.Unity为例
class Program
{
    private IUnityContainer container;//Unity容器的实例化自行完成，此demo不做演示。
    void Main()
    {
        //以下三行代码可以通过Unity.config配置文件配置
        this.container.RegisterType<IActivemqClient, ActivemqClient>(); 
        this.container.RegisterType<IMessageManager, TestMessageProcess1>("TestMessageProcess1");
        this.container.RegisterType<IMessageManager, TestMessageProcess2>("TestMessageProcess2");

        var activemqClient = this.container.Resolve<IActivemqClient>();
        activemqClient.BrokerUri = "failover:tcp://192.168.0.1:61616";
        activemqClient.UserName = "admin";
        activemqClient.Password = "admin";
        activemqClient.Connect();
        
        //初始化所有消息路由
        var listMessageManager = this.container.ResolveAll<IMessageManager>();
        foreach (var messageManager in listMessageManager)
        {
            messageManager.SubscribeDestination();
        }
    }
}


class TestMessageProcess1 : AbstractMessageManager
{
    public TestMessageProcess1(IActivemqClient mqclient) : base(mqclient)
    {
        base.DestinationName = "Topic.Test1";
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

class TestMessageProcess2 : AbstractMessageManager
{
    public TestMessageProcess(IActivemqClient mqclient) : base(mqclient)
    {
        base.DestinationName = "Topic.Test2";
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
```


## 四. 其它
1. 欢迎各位码友提出宝贵的意见。大恩不言谢。    
2. 我在码云上面也看到有很多同类型的工具封装，不同的是着重的方向不同，工具的源码实现方式不同等。其中有一个大神（我真心认为他是大神），封装了非常非常非常多好用的、强大的工具，这一方面我很佩服，但是他说“如果你发现了网上类似的项目，说明你遇到了个装逼的”。大家都有学习、自己研究的自由，而且有共享精神，我个人认为都是值得褒奖、值得鼓励、值得大家一起奋斗学习的，什么叫“装逼的”，别太自负，我们这些菜逼臭皮匠，组成起来也能干掉诸葛亮。（仅代表我个人观点，绝不包含任何攻击意思）