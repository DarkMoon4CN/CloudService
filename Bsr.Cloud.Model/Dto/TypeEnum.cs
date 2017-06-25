using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model
{
    // 用户访问服务时，使用的客户端类型
    public enum LoginTypeEnum
    {
        web = 1, // web
        window,  // 桌面客户端
        android, // 移动端 android
        ios,     // 移动端 ios
    }

    // 用户注册时，使用的客户端类型
    public enum RegisterTypeEnum
    {
        web = 1, // web
        window,  // 桌面客户端
        android, // 移动端 android
        ios,     // 移动端 ios
    }

    //设备的硬件类型
    public enum HardwareTypeEnum
    {
        IPC = 3, 
        DVR = 4,
        DVS = 5,
        NVR = 6,
    }

    // 用户的身份，对应db中的表：Customer, 对应字段：SignInType
    public enum CustomerSignInTypeEnum
    {
        ManagerAccount  = 1,// 前台管理员账号
        PrimaryCustomer,    // 主账号
        SubCustomer         // 子账号
    }

    // 权限的'目标对象'类型 
    public enum PermissionNodeTypeEnum
    {
        Device = 1,    // 设备
        Channel,       // 通道       
    }

    // 日志的'目标对象'类型
    public enum OperaterLogEnum 
    {
        Customer=1,     //用户
        Device,         //设备
        Channel,        //通道
        ResourceGroup,  //分组
        Favorite,       //收藏
    }

    //权限'目标对象功能'类型
    public enum PermissionNameTypeEnum 
    {
        Video=1,         //现场视频
        Playback,        //回放
        Authorize,       //授权设备
        MySpace,         //我的空间 
        EventMessage,    //消息事件
        CloudVideo,      //云视频
    }
    //通道权限
    public enum PermissionChannelNameEnum 
    {
        Video =100,      //现场视频
        Playback,        //回放
    }
    //设备权限
    public enum PermissionDeviceNameEnum
    {
        Authorize=200,   //授权设备
    }
    //用户权限
    public enum PermissionCustomerEnum
    {
        MySpace=300,      //我的空间 
        EventMessage,     //消息事件
        CloudVideo,       //云视频
    }
    public enum UserFavoriteTypeEnum
    {
        Channel=1,       //通道
        Event,           //事件
        Video,           //录像
    }
}
