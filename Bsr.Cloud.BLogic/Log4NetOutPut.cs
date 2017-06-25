using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.BLogic
{
    public class Log4NetOutPut
    {
        public static void OutPutInit() 
        {

            try
            {
                // 指定log4net的配置文件地址
                var path = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
                // 初始化log4net库
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(path));
            }
            catch (BPCloudException ex)
            {
                Console.Write("1.输出位置被保护，2写入异常{0}",ex.Message);
            }
        }
    }
}
