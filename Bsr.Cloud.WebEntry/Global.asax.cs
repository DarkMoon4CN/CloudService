using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using log4net;
using System.Threading;

namespace Bsr.Cloud.WebEntry
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // 自启动的任务
            AutoTask.Run();
        }

        void Application_End(object sender, EventArgs e)
        {
            // 在这访问本地的一个aspx程序，以解决应用池回收问题
            //System.Threading.Thread.Sleep(2000);
            //string strUrl = "http://127.0.0.1/Login.aspx";
            //System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strUrl);
            //System.Net.HttpWebResponse _HttpWebResponse = (System.Net.HttpWebResponse)_HttpWebRequest.GetResponse();
            //System.IO.Stream _Stream = _HttpWebResponse.GetResponseStream(); //得到回写的字节流 
            //System.Threading.Thread.Sleep(1000);

        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码

        }

        void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码

        }

        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。

        }
        void Application_ResolveRequestCache()
        {
            //当ASP.NET完成授权事件以使缓存模块从缓存中为请求提供服务时发生，从而跳过处理程序（页面或者是WebService）的执行。这样做可以改善网站的性能，这个事件还可以用来判断正文是不是从Cache中得到的。 
            
        }
        void Application_AcquireRequestState()
        {
            //当ASP.NET获取当前请求所关联的当前状态（如Session）时执行。 
        }
        void Application_PreRequestHandlerExecute()
        {
            //当ASP.Net即将把请求发送到处理程序对象（页面或者是WebService）之前执行。这个时候，Session就可以用了。
        }

    }
}
