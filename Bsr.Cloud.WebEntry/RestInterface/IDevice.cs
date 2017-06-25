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


    #region 添加设备  AddDevice
    [DataContract]
    public class AddDeviceRequestDto : RequestBaseDto
    {
        [DataMember]
        public string DeviceName;
        [DataMember]
        public string SerialNumber;
    }
    [DataContract]
    public class AddDeviceResponseDto : ResponseBaseDto
    {

    }
    #endregion  AddDevice

    #region 删除设备  DeleteDevice
    [DataContract]
    public class DeleteDeviceRequestDto : RequestBaseDto
    {
        [DataMember]
        public int Deviceid;
    }
    [DataContract]
    public class DeleteDeviceResponseDto : ResponseBaseDto
    {

    }
    #endregion  DeleteDevice

    #region 更新设备名称  UpdateDeviceName
    [DataContract]
    public class UpdateDeviceNameRequestDto : RequestBaseDto
    {
        [DataMember]
        public int DeviceId;
        [DataMember]
        public string NewDeviceName;
    }
    [DataContract]
    public class UpdateDeviceNameResponseDto : ResponseBaseDto
    {

    }
    #endregion  UpdateDeviceName

    #region 搜索模块Device SearchDevice
    [DataContract]
    public class SearchDeviceRequestDto : RequestBaseDto 
    {
        [DataMember]
        public string KeyWord;
    }
    [DataContract]
    public class SearchDeviceResponseDto : ResponseBaseDto 
    {
        [DataMember]
        public  List<Bsr.Cloud.Model.Entities.Device> deviceList;
    }
    #endregion

    #region 查询当前用户的所有设备信息  GetDeviceBySelf
    [DataContract]
    public class GetDeviceBySelfRequestDto : RequestBaseDto
    {

    }
    [DataContract]
    public class GetDeviceBySelfResponseDto : ResponseBaseDto
    {
        [DataMember]
        public List<Model.DeviceResponse> deviceResponseList;
    }
    #endregion  

    #region 以设备ID查询通道 GetChannelByDeviceId
    [DataContract]
    public class GetChannelByDeviceIdRequestDto : RequestBaseDto
    {
        [DataMember]
        public int DeviceId;
    }
    [DataContract]
    public class GetChannelByDeviceIdResponseDto : ResponseBaseDto
    {
        [DataMember]
        public List<Bsr.Cloud.Model.Entities.Channel> channelList;
    }
    #endregion  GetChannelByDeviceId

    #region 流媒体播放时需要的参数 GetStreamerParameter
    [DataContract]
    public class GetStreamerParameterRequestDto : RequestBaseDto
    {
        [DataMember]
        public int ChannelId;//请求流媒体的通道Id
    }
    [DataContract]
    public class GetStreamerParameterResponseDto : ResponseBaseDto
    {
        [DataMember]
        public Bsr.Cloud.Model.BP4StreamerParameter streamerParameter;
    }
    #endregion  GetStreamerParameter

    #region 查询设备状态 GetServerDeviceState
    [DataContract]
    public class GetServerDeviceStateRequestDto : RequestBaseDto
    {
        [DataMember]
        public int[] DeviceIdList;
    }
    [DataContract]
    public class GetServerDeviceStateResponseDto : ResponseBaseDto
    {
        [DataMember]
         public List<DeviceState> deviceState;
    }
    public class DeviceState
    {
        public int DeviceId;
        public int State;
    }
    #endregion  

    #region 查询设备SN是否已存在 IsExistSN
    [DataContract]
    public class IsExistSNRequestDto : RequestBaseDto
    {
        [DataMember]
        public string SerialNumber;
    }
    [DataContract]
    public class IsExistSNResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int IsExist; // 1已存在 0不存在
    }
    #endregion  

    #region 验证SN码 返回是否可以被添加设备硬件类型 CheckSN
    [DataContract]
    public class CheckSNRequestDto : RequestBaseDto
    {
        [DataMember]
        public string SerialNumber;
    }
    [DataContract]
    public class CheckSNResponseDto : ResponseBaseDto
    {
        [DataMember]
        public int IsEnable;     // 1可用 0不可用
        [DataMember]
        public int IsOnline;     // 1在线 0不在线
        [DataMember]
        public int HardwareType; // 3表示ipc, 4,5,6为dvr,nvr
    }
    #endregion  


    /// <summary>
    /// 设备服务接口
    /// </summary>
    [ServiceContract]
    public interface IDevice
    {
        //添加设备
        [OperationContract]
        [WebInvoke(UriTemplate = "/AddDevice", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AddDeviceResponseDto AddDevice(AddDeviceRequestDto req);

        //删除设备
        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteDevice", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        DeleteDeviceResponseDto DeleteDevice(DeleteDeviceRequestDto req);

        //修改设备名称
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateDeviceName", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UpdateDeviceNameResponseDto UpdateDeviceName(UpdateDeviceNameRequestDto req);

        //根据当前用户获取设备
        [OperationContract]
        [ServiceKnownType(typeof(Bsr.Cloud.Model.Entities.Device))]
        [WebInvoke(UriTemplate = "/GetDeviceBySelf", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetDeviceBySelfResponseDto GetDeviceBySelf(GetDeviceBySelfRequestDto req);

        //根据设备Id获取通道
        [OperationContract]
        [ServiceKnownType(typeof(Bsr.Cloud.Model.Entities.Channel))]
        [WebInvoke(UriTemplate = "/GetChannelByDeviceId", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetChannelByDeviceIdResponseDto GetChannelByDeviceId(GetChannelByDeviceIdRequestDto req);

        //查询设备状态
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetServerDeviceState", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetServerDeviceStateResponseDto GetServerDeviceState(GetServerDeviceStateRequestDto req);

        //流媒体播放时需要的参数
        [OperationContract]
        [ServiceKnownType(typeof(Bsr.Cloud.Model.BP4StreamerParameter))]
        [WebInvoke(UriTemplate = "/GetStreamerParameter", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        GetStreamerParameterResponseDto GetStreamerParameter(GetStreamerParameterRequestDto req);

        //检索设备
        [OperationContract]
        [ServiceKnownType(typeof(Bsr.Cloud.Model.BP4StreamerParameter))]
        [WebInvoke(UriTemplate = "/SearchDevice", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SearchDeviceResponseDto SearchDevice(SearchDeviceRequestDto req);


        //查询设备SN是否已存在
        [OperationContract]
        [WebInvoke(UriTemplate = "/IsExistSN", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IsExistSNResponseDto IsExistSN(IsExistSNRequestDto req);

        //验证SN码 返回是否可以被添加设备硬件类型 CheckSN
        [OperationContract]
        [WebInvoke(UriTemplate = "/CheckSN", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
                    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CheckSNResponseDto CheckSN(CheckSNRequestDto req);
    }
}
