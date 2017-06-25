using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Core;

namespace Bsr.Cloud.BLogic
{
    //SystemException为系统异常,程序无法解决
    //ApplicationException为程序异常,可捕获
    public class BPCloudException : ApplicationException
    {
        public BPCloudException() 
　　    { 
　　  　
　　    }

        public BPCloudException(string message, Exception inner, ILogger myLog)
            : base(message, inner)
        {
            //异常：提示异常+程序异常
            myLog.ErrorFormat(message,inner);
        }

    }
}
