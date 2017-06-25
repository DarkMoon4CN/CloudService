using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Bsr.Cloud.Core;
using System.Threading;


namespace Bsr.Cloud.WebEntry
{
    public class AutoTask
    {
        static private ILogger myLog = new Logger<AutoTask>();
                
        // 自启动的任务，开始运行
        public static void Run()
        {
            myLog.InfoFormat("Initializing Application");

            try
            {
                // 指定log4net的配置文件地址
                var path = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
                // 初始化log4net库
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(path));
            }
            catch (Exception ex)
            {
                // Do nothing
                Console.Write(ex);
            }



            return;
        }

    }
}