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

    #region GetSelfLoginInfoRequest 当前用户的登陆分页信息
    [DataContract]
    public class GetSelfLoginInfoRequestBaseDto : RequestBaseDto
    {
        [DataMember]
        public int RequestCount;//请求条数
        [DataMember]
        public int StartCount;//请求起始条数
    
    }
    [DataContract]
    public class GetSelfLoginInfoResponseBaseDto : ResponseBaseDto
    {
        [DataMember]
        public int Total;//总条数
        [DataMember]
        public List<OperaterLogResponse> operaterLogList;
    }
    public class OperaterLogResponse 
    {
        public int OperaterId;
        public string AgentType; // 登陆类型
        public string AgentVersion; //登陆版本
        public string OperaterTime;//时间
    }
    #endregion

    #region GetPrimaryCustomerLoginInfo 前台管理员查看主用户的登陆分页信息
    [DataContract]
    public class GetPrimaryCustomerLoginInfoRequestBaseDto : RequestBaseDto
    {
        [DataMember]
        public int PrimaryCustomerId;//主用户Id
        [DataMember]
        public int RequestCount;//请求条数
        [DataMember]
        public int StartCount;//请求起始条数

    }
    [DataContract]
    public class GetPrimaryCustomerLoginInfoResponseBaseDto : ResponseBaseDto
    {
        [DataMember]
        public int Total;//总条数
        [DataMember]
        public List<OperaterLogResponse> operaterLogList;
    }
    #endregion


    #region GetSubCustomerLoginInfo 主用户查看子用户的登陆分页信息
    [DataContract]
    public class GetSubCustomerLoginInfoRequestBaseDto : RequestBaseDto
    {
        [DataMember]
        public int SubCustomerId;//子用户Id
        [DataMember]
        public int RequestCount;//请求条数
        [DataMember]
        public int StartCount;//请求起始条数

    }
    [DataContract]
    public class GetSubCustomerLoginInfoResponseBaseDto : ResponseBaseDto
    {
        [DataMember]
        public int Total;//总条数
        [DataMember]
        public List<OperaterLogResponse> operaterLogList;
    }
    #endregion


    /// <summary>
    /// 用户日志服务接口
    /// </summary>
    [ServiceContract]
    public interface IOperaterLog
    {
        //获取当前用户的登陆信息
        [OperationContract]
        [ServiceKnownType(typeof(OperaterLogResponse))]
        [WebInvoke(UriTemplate = "/GetSelfLoginInfo", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetSelfLoginInfoResponseBaseDto GetSelfLoginInfo(GetSelfLoginInfoRequestBaseDto req);

        //前台管理员查看主用户的登陆分页信息
        [OperationContract]
        [ServiceKnownType(typeof(OperaterLogResponse))]
        [WebInvoke(UriTemplate = "/GetPrimaryCustomerLoginInfo", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetPrimaryCustomerLoginInfoResponseBaseDto GetPrimaryCustomerLoginInfo(GetPrimaryCustomerLoginInfoRequestBaseDto req);

        //主用户查看子用户的登陆分页信息
        [OperationContract]
        [ServiceKnownType(typeof(OperaterLogResponse))]
        [WebInvoke(UriTemplate = "/GetSubCustomerLoginInfo", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetSubCustomerLoginInfoResponseBaseDto GetSubCustomerLoginInfo(GetSubCustomerLoginInfoRequestBaseDto req);
    }
}
