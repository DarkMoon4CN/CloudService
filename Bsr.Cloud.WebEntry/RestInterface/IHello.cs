using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Bsr.Cloud.Model;
using Bsr.Cloud.Model.Entities;
using System.ServiceModel.Web;

namespace Bsr.Cloud.WebEntry.RestService
{
    /// <summary>
    /// 该接口提供一些与业务无关的REST接口，例如服务探测等。
    /// </summary>
    [ServiceContract]
    public interface IHello
    {

        // 此接口提供给外部来探测REST服务是否工作正常
        [OperationContract]
        [WebGet(UriTemplate = "/HelloNow", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string HelloNow();  // ~/Hello.svc/HelloNow
    }
}
