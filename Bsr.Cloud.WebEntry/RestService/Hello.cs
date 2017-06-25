using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bsr.Cloud.BLogic;

namespace Bsr.Cloud.WebEntry.RestService
{
    public class Hello : IHello
    {

        public string HelloNow()
        {
            // 返回系统时间
            return "BstarCloud REST Service is running, current time: " + DateTime.Now.ToString();
        }
    }
}