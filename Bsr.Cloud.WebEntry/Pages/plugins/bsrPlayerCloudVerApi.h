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


//�������
/*
���öര����ʾģʽ��֧�֣�1�� 4�� 6�� 9
*/
extern "C" void PASCAL EXPORT SetWndMode( int mode );

/*
 ���ò��Ŵ��ڵĸ����ڣ�ͨ�����������window
*/
extern "C" void PASCAL EXPORT SetParentWnd( HWND hParent );

/*
 ��ȡ��ǰѡ�д��ں�
*/
extern "C" int PASCAL EXPORT GetCurSelectWndIndex();

/*
 ����ȫ��ģʽ��ESC�˳�ȫ��ģʽ
*/
extern "C" void PASCAL EXPORT SetFullScreen( int index );

/*
 ��ȡ��ǰ�ര����ʾģʽ
*/
extern "C" int PASCAL EXPORT GetWndMode();

/*
 ��ȡһ�����д���
*/
extern "C" int PASCAL EXPORT GetFirstFreeWndIndex();


/*
 �ֳ�Ԥ��������أ�
 urlΪ��ͬ����ģʽ�µ�json��
*/
extern "C" int PASCAL EXPORT PlayOpen( char *url , int index ); //��ָ�����ڲ��ţ� url��ʽ˵������ע1��
extern "C" int PASCAL EXPORT PlayClose( int index ); //�ر�

/*
 ������������
*/
extern "C" int PASCAL EXPORT PlaySoundOpen( int index ); //������Ƶ
extern "C" int PASCAL EXPORT PlaySoundClose( int index ); //�ر���Ƶ

extern "C" int PASCAL EXPORT PlaySetRate( int index, int nRate, int nScale ); //���ò����ٶ�

extern "C" int PASCAL EXPORT PlaySaveBegin( int index, char *pathname);//���
extern "C" int PASCAL EXPORT PlaySaveEnd( int index );//������

/*
 ץͼ1:����ץͼ·�������jpe��bmp�ļ�
 type��0��bmp 1��jpg
 ץͼ2����ȡ�ƶ����Ŵ��ڵ�ͼ����json������ʽ���أ����ص�bsase64��ͼ��Ҫ����Ϊ������
*/
extern "C" int PASCAL EXPORT PlayCapturePic(int index, char *pathname, int type ); //ץͼ
extern "C" char* PASCAL EXPORT PlayCapturePic2(int index); //ץͼ2


//��ע1�� PlayOpen( char *url , int index )�ӿ���url��ʽ˵��
//url��json��ʽ��֯�������ֶμ�����˵������
/***************************************************************************
//[part 1]
Json::Value root;
root["playerType"] ���������ͣ�0-����SDK��ʽ��1-��ICE��ʽ��2-rest����ʽ
//[part 1 end]

//[part 2]
//***�����ֶ�Ϊ������SDK��ʽ��Ҫ
root["devIp"] �豸IP;
root["cmdPort"] ����˿�;
root["dataPort"] ���ݶ˿�;
root["userName"] �û���;
root["userPsw"] ����;
root["playType"] ��Ƶ���ͣ�0-�ֳ����� 1-�ط�
//[part 2 end]

//[part 2_1]
//���ֳ���ʱ�Ĳ���
root["real"]["channel"] ͨ��;
root["real"]["subStream"] �������ͣ�0-��ʾ��������1-��ʾ��������
root["real"]["transProc"] ����Э��: 1-TCP��4-RTP
root["real"]["transMode"] ����ģʽ��1-������2-����
root["real"]["ip"]  IP��ַ,����ʱΪ��

//[part 2_2]
//�طŲ���
root["record"]["channel"] ͨ��;
root["record"]["protocol"] ����Э��: 1-TCP��4-RTP
root["record"]["ip"] IP��ַ,����ʱΪ��
root["record"]["idxType"]     1��file, ��DVR6ʹ�� 2��event��DVR7���¼���������Ϣ 3��clip����ֹʱ�������
root["record"]["beginTime"]  ¼��ʼʱ��
root["record"]["peroid"] �������ŵ�������ʱ��(��)
root["record"]["byteRate"]  ʹ�õĴ���(�ֽ�ÿ�룬0Ϊ����) 
//***�����ֶ�Ϊ������SDK��ʽ��Ҫ


//***�����ֶ�Ϊ��rest����ʽ��Ҫ
//[part 3]
//REST���� Json����
root["DestinationIP"] = "";  //�豸Ŀ��ID
root["RestServIp"] = "192.168.8.239"; //REST�����ip
root["RestServPort"] = 8006;                //REST����˿�
root["LoginModel"]["Address"] = "192.168.15.3"; //��½�豸ip
root["LoginModel"]["AddressType"] = 0;          //�豸��ַ����
root["LoginModel"]["CmdPort"] = "3721";          //��½�豸���ƶ˿�
root["LoginModel"]["DataPort"] = "3720";         //�豸���ݶ˿�
root["LoginModel"]["DeviceType"] = "Bsr.LimitDevice"; //�豸����
root["LoginModel"]["Password"] = "123456";             //��½�豸����
root["LoginModel"]["UserName"] = "admin";                //��½�豸�û���


//[part 3_1]
//�ֳ���Ƶjson����
root["RealStreamModel"]["Channel"] = 1;       //�ֳ���Ƶ�����豸ͨ����
root["RealStreamModel"]["SubStream"] = 0;    //��������������
root["RealStreamModel"]["TransProc"] = 0;   //
root["RealStreamModel"]["ChannelId"] = 1;   //
//***�����ֶ�Ϊ��rest����ʽ��Ҫ


//***�����ֶ�Ϊ��ICE��ʽ��Ҫ
//[part 4]
root[iceServAddr"]     //ICE server ��ַ
root["iceServPort"]    //ICE server �˿�
root["iceUser"]        //��½ICE server�û���
root["icePsw"]           //��½ICE server����
root["stun_primaryhost"]   //STUN��͸��ʽ server��ip
root["stun_secodaryhost"]   //STUN��͸��ʽ server��IP
root["stun_user"]           //STUN��͸��ʽ �û���
root["stun_psw"]            //STUN��͸��ʽ ����
root["stun_priport"]         //STUN��͸��ʽ ��port
root["stun_secport"]       //STUN��͸��ʽ  ��port
root["natProtocolType"]     //��͸Э������
root["deviceCode"]           //�豸Ψһ���ʶ
root["devDataPort"]       //�豸���ݶ˿�
root["devCmdPort"]        //�豸���ƶ˿�

//***�����ֶ�Ϊ��ICE��ʽ��Ҫ


***************************************************************************/