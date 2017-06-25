using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.ServiceModel;
using Bsr.Cloud.Model;
using System.Runtime.Serialization;


namespace Bsr.Cloud.WebEntry
{
    public static class RestHelper
    {
        /// <summary>
        /// 基本的安全检测。某接口的权限需要用户传入token时调用此方法。
        /// </summary>
        /// <param name="CustomerToken">输出用户的token值</param>
        /// <returns>false表示token不存在</returns>
        internal static bool SecurityCheck(ref string customerToken)
        {
            customerToken = WebOperationContext.Current.IncomingRequest.Headers["BstarCloud-User-Token"];
            if (customerToken == null || customerToken == "")
            {
                // 如果没有token,置状态码为403
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Forbidden;
                return false;
            }
            else
            {
                return true;
            }
        }
        public static string SecNoTokenMessage = "此服务需Token支持";
    }
}