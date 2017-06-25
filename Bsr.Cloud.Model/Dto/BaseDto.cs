using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Bsr.Cloud.Model
{
    /// <summary>
    /// 所有服务接口的入参，都从这个类派生
    /// </summary>
    [DataContract]
    public class RequestBaseDto
    {

    }

    
    /// <summary>
    /// 所有服务返回类型，都从这个类派生。
    /// </summary>
    [DataContract]
    public class ResponseBaseDto
    {

        public ResponseBaseDto()
        {
            Code = (int)CodeEnum.Success;
            Message = "";
        }

        /// <summary>
        /// 错误码, 0表示成功
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }

    /// <summary>
    ///  查询授权时返回类型（主用户）
    /// </summary>
    [DataContract]
    public class AuthorizePrimaryResponse
    {
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public int DeviceCount{get;set;}
        [DataMember]
        public List<AuthorizeDeviceResponse> authorizeDeviceResponseList{get;set;}
    }
    /// <summary>
    ///  查询授权时返回类型（子用户）
    /// </summary>
    [DataContract]
    public class AuthorizeSubResponse
    {
        [DataMember]
        public int CustomerId { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public int DeviceCount { get; set; }
        [DataMember]
        public List<AuthorizeDeviceResponse> authorizeDeviceResponseList { get; set; }
        [DataMember]
        public List<AuthorizePermissionResponse> subCustomerPermissionList{ get; set; }
    }
    /// <summary>
    /// 查询授权时返回类型(设备)
    /// </summary>
    public class AuthorizeDeviceResponse
    {
        [DataMember]
        public int DeviceId { get; set; }
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public int IsEnable { get; set; }//是否是授权 1 是授权设备 0是否  <注：与其他IsEnable 含义不同>
        [DataMember]
        public List<AuthorizeChannelResponse> authorizeChannelResponse { get; set; }

    }
    /// <summary>
    /// 查询授权时返回类型(通道)
    /// </summary>
    public class AuthorizeChannelResponse 
    {
        [DataMember]
        public int ChannelId { get; set; }
        [DataMember]
        public string ChannelName { get; set; }
        [DataMember]
        public int IsEnable { get; set; } //通道是否被主用户禁用或者启用 1.启用，0禁用
        [DataMember]
        public List<AuthorizePermissionResponse> authorizePermissionResponse { get; set; }
    }
    /// <summary>
    ///  查询授权时返回类型(权限)
    /// </summary>
    public class AuthorizePermissionResponse 
    {
        [DataMember]
        public string PermissionName { get; set; }
        [DataMember]
        public int IsEnable { get; set; } //是否启用权限  1启用 0禁用
    }


    /// <summary>
    ///  设备返回时参数
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeviceResponse
    {
        [DataMember]
        public int DeviceId { get; set; } //设备Id
        [DataMember]
        public string DeviceName { get; set; } //设备名
        [DataMember]
        public int DeviceType { get; set; } //设备类型
        [DataMember]
        public string SerialNumber { get; set; } //SN码
        [DataMember]
        public int IsAuthorize { get; set; } // 1表示授权设备 0当前用户设备
        [DataMember]
        public int BPServerDeviceState { get; set; } //设备状态
        [DataMember]
        public int HardwareType { get; set; }//设备硬件类型
    }
    /// <summary>
    /// 用户信息返回时参数
    /// </summary>
    [DataContract]
    public class CustomerResponse
    {
        [DataMember]
        public int CustomerId { get; set; } //用户Id
        [DataMember]
        public string CustomerName { get; set; } //用户名
        [DataMember]
        public int SignInType { get; set; } //用户类型 （1前台管理员，2主账号，3子用户）
        [DataMember]
        public string ReceiverName { get; set; }//用户真实姓名
        [DataMember]
        public string ReceiverEmail { get; set; }//mail
        [DataMember]
        public string ReceiverCellPhone { get; set; }//移动电话
        [DataMember]
        public string AccountIDNumber { get; set; }//身份证件
        [DataMember]
        public string AccountTelephone { get; set; }//固定电话
        [DataMember]
        public string AccountCompany { get; set; }//公司
        [DataMember]
        public string AccountCompanyAddress { get; set; }//公司地址
        [DataMember]
        public int IsEnable { get; set; } // 冻结状态
        [DataMember]
        public string AccountHomeAddress { get; set; }//家庭住址 
        [DataMember]
        public string ImagePath { get; set; }//图片路径
        [DataMember]
        public string ForbiddenTime { get; set; }//冻结时间
        [DataMember]
        public string ValidTime { get; set; }    //有效时间
        [DataMember]
        public string LoginTypes { get; set; }  //可以使用的客户端多登陆类型 LoginTypeEnum
    }


    /// <summary>
    /// 通道详细数据返回时需要的信息
    /// </summary>
    public class GroupChannelResponse
    {
        public int ChannelId  { get; set; }
        public string ChannelName  { get; set; }
        public int ChannelNumber  { get; set; }
        public int DeviceId { get; set; }
        public int IsAuthorize { get; set; } //当前通道所属设备是否是授权的  1表示授权设备 0当前用户设备
        public int IsEnable { get; set; }    //通道是否可用 1可用 0不可用
        public int IsFavorite { get; set; }  //该通道是否被收藏 1收藏 0未收藏
        public int ResourceGroupId { get; set; }
        public string ImagePath { get; set; }
    }


    /// <summary>
    ///  分组时返回是需要的信息
    /// </summary>
    public class ResourceGroupResponse
    {
        public  int ResourceGroupId { get; set; }
        public  string ResourceGroupName { get; set; }
        public  int ParentResourceGroupId { get; set; }
        public  int CustomerId { get; set; }
        public int IsNext { get; set; }   //是否有子分组 1有 0否
    }

    /// <summary>
    /// 收藏返回时需要的参数
    /// </summary>
    public class UserFavoriteResponse
    {
        public int UserFavoriteId { get; set; }
        public int CustomerId { get; set; }
        public int UserFavoriteType { get; set; }
        public int UserFavoriteTypeId { get; set; }
        public string FavoriteTime { get; set; }
        public string AliasName { get; set; }
        public string ImagePath { get; set; }
    }
}
