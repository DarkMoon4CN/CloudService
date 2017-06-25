#pragma once

#include <AtlBase.h>
#ifndef EXPORT
#define EXPORT
#endif

enum
{
  TYPE_BSRPLAYER = 0,
  TYPE_ICEPLAYER = 1,
  TYPE_RESTPLAYER = 2,
};

enum
{
  TYPE_REALPLAY = 0,
  TYPE_RECORDPLAY_BYFILE = 1,
  TYPE_RECORDPLAY_BYTIME = 2,
};


//窗口相关
/*
设置多窗格显示模式，支持：1， 4， 6， 9
*/
extern "C" void PASCAL EXPORT SetWndMode( int mode );

/*
 设置播放窗口的父窗口，通常是浏览器的window
*/
extern "C" void PASCAL EXPORT SetParentWnd( HWND hParent );

/*
 获取当前选中窗口号
*/
extern "C" int PASCAL EXPORT GetCurSelectWndIndex();

/*
 设置全屏模式，ESC退出全屏模式
*/
extern "C" void PASCAL EXPORT SetFullScreen( int index );

/*
 获取当前多窗格显示模式
*/
extern "C" int PASCAL EXPORT GetWndMode();

/*
 获取一个空闲窗口
*/
extern "C" int PASCAL EXPORT GetFirstFreeWndIndex();


/*
 现场预览播放相关：
 url为不同播放模式下的json串
*/
extern "C" int PASCAL EXPORT PlayOpen( char *url , int index ); //在指定窗口播放， url格式说明见备注1。
extern "C" int PASCAL EXPORT PlayClose( int index ); //关闭

/*
 伴音开关设置
*/
extern "C" int PASCAL EXPORT PlaySoundOpen( int index ); //开启音频
extern "C" int PASCAL EXPORT PlaySoundClose( int index ); //关闭音频

extern "C" int PASCAL EXPORT PlaySetRate( int index, int nRate, int nScale ); //设置播放速度

extern "C" int PASCAL EXPORT PlaySaveBegin( int index, char *pathname);//另存
extern "C" int PASCAL EXPORT PlaySaveEnd( int index );//另存结束

/*
 抓图1:给定抓图路径，存成jpe或bmp文件
 type：0：bmp 1：jpg
 抓图2：截取制定播放窗口的图，以json串的形式返回，返回的bsase64的图需要解码为二进制
*/
extern "C" int PASCAL EXPORT PlayCapturePic(int index, char *pathname, int type ); //抓图
extern "C" char* PASCAL EXPORT PlayCapturePic2(int index); //抓图2


//备注1： PlayOpen( char *url , int index )接口中url格式说明
//url按json格式组织，具体字段及含义说明如下
/***************************************************************************
//[part 1]
Json::Value root;
root["playerType"] 播放器类型，0-蓝星SDK方式，1-按ICE方式，2-rest服务方式
//[part 1 end]

//[part 2]
//***以下字段为按蓝星SDK方式需要
root["devIp"] 设备IP;
root["cmdPort"] 命令端口;
root["dataPort"] 数据端口;
root["userName"] 用户名;
root["userPsw"] 密码;
root["playType"] 视频类型，0-现场流， 1-回放
//[part 2 end]

//[part 2_1]
//打开现场流时的参数
root["real"]["channel"] 通道;
root["real"]["subStream"] 码流类型，0-表示主码流，1-表示子码流。
root["real"]["transProc"] 传输协议: 1-TCP，4-RTP
root["real"]["transMode"] 传输模式：1-被动，2-主动
root["real"]["ip"]  IP地址,被动时为零

//[part 2_2]
//回放参数
root["record"]["channel"] 通道;
root["record"]["protocol"] 传输协议: 1-TCP，4-RTP
root["record"]["ip"] IP地址,被动时为零
root["record"]["idxType"]     1：file, 仅DVR6使用 2：event，DVR7中事件的描述信息 3：clip，起止时间的描述
root["record"]["beginTime"]  录像开始时间
root["record"]["peroid"] 期望播放的最大持续时间(秒)
root["record"]["byteRate"]  使用的带宽(字节每秒，0为不限) 
//***以上字段为按蓝星SDK方式需要


//***以下字段为按rest服务方式需要
//[part 3]
//REST请求 Json参数
root["DestinationIP"] = "";  //设备目标ID
root["RestServIp"] = "192.168.8.239"; //REST服务端ip
root["RestServPort"] = 8006;                //REST服务端口
root["LoginModel"]["Address"] = "192.168.15.3"; //登陆设备ip
root["LoginModel"]["AddressType"] = 0;          //设备地址类型
root["LoginModel"]["CmdPort"] = "3721";          //登陆设备控制端口
root["LoginModel"]["DataPort"] = "3720";         //设备数据端口
root["LoginModel"]["DeviceType"] = "Bsr.LimitDevice"; //设备类型
root["LoginModel"]["Password"] = "123456";             //登陆设备密码
root["LoginModel"]["UserName"] = "admin";                //登陆设备用户名


//[part 3_1]
//现场视频json参数
root["RealStreamModel"]["Channel"] = 1;       //现场视频请求设备通道号
root["RealStreamModel"]["SubStream"] = 0;    //主码流，字码流
root["RealStreamModel"]["TransProc"] = 0;   //
root["RealStreamModel"]["ChannelId"] = 1;   //
//***以上字段为按rest服务方式需要


//***以下字段为按ICE方式需要
//[part 4]
root[iceServAddr"]     //ICE server 地址
root["iceServPort"]    //ICE server 端口
root["iceUser"]        //登陆ICE server用户名
root["icePsw"]           //登陆ICE server密码
root["stun_primaryhost"]   //STUN穿透方式 server主ip
root["stun_secodaryhost"]   //STUN穿透方式 server辅IP
root["stun_user"]           //STUN穿透方式 用户名
root["stun_psw"]            //STUN穿透方式 密码
root["stun_priport"]         //STUN穿透方式 主port
root["stun_secport"]       //STUN穿透方式  辅port
root["natProtocolType"]     //穿透协议类型
root["deviceCode"]           //设备唯一码标识
root["devDataPort"]       //设备数据端口
root["devCmdPort"]        //设备控制端口

//***以上字段为按ICE方式需要


***************************************************************************/