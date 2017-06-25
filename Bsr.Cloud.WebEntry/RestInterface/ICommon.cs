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
    #region  CheckUpdate参数
    [Serializable]
    public class CheckUpdateRequestDto : RequestBaseDto
    {
        public int AgentType;       // 终端平台，见 enum LoginTypeEnum
        public string SoftwareId;   // 该软件的唯一标识，用来区分是哪个软件
        public string MajorVersion; // 软件主版本号，例如 B01
        public string MinorVersion; // 软件子版本号，例如 D12
        public string BuildVersion; // 软件特殊版本后缀，例如T01

    }

    [Serializable]
    public class CheckUpdateResponseDto : ResponseBaseDto
    {
        public int Expired; // 1 表示已过期， 0表示可继续使用
        public string SoftwareId;   // 该软件的唯一标识，用来区分是哪个软件
        public string LatestMajorVersion; // 最近的软件主版本号，例如 B02
        public string LatestMinorVersion; // 最近的软件子版本号，例如 D12
        public string LatestBuildVersion; // 最近的软件特殊版本后缀，例如T01
        public string WhereToUpdate;      // 从哪去更新新版本，Expired为1时有意义。
    }
    #endregion  CheckUpdate参数


    /// <summary>
    /// 该接口提供一些通用的服务，例如客户端版本检测等。
    /// </summary>
    [ServiceContract]
    public interface ICommon
    {
        // 此接口提供客户端检测终端版本是否可以继续使用
        [OperationContract]
        [WebInvoke(UriTemplate = "/CheckUpdate", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CheckUpdateResponseDto CheckUpdate(CheckUpdateRequestDto req);  // ~/ServiceCommon.svc/CheckUpdate
    }
}
