using System;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using NHibernate;
using NHibernate.Cfg;
using Bsr.Cloud.BLogic;
using Bsr.Cloud.Model;
using Bsr.Cloud.Model.Entities;
using Bsr.Cloud.BLogic.BLL;
using System.Collections.Generic;
using System.Reflection;
using Bsr.DeviceAdapter.Model;
using System.Xml;
using System.Web;


namespace Bsr.Cloud.BLogic
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {


                //获取当前用户访问权限 测试模型
                PermissionServer permissionServer = PermissionServer.GetInstance();
                List<string> permissionNames = new List<string>();
                foreach (string name in Enum.GetNames(typeof(PermissionCustomerEnum)))
                {
                    permissionNames.Add(name);
                }
                IList<Permission> permissionFlag = permissionServer.SelectPermissionByCidAndPname(120, permissionNames);









                //xml 测试模型
                //IDictionary<int, AVEncoderInfoDto> avEncoderInfo = new Dictionary<int, AVEncoderInfoDto>();
                //string xmlPath = AppDomain.CurrentDomain.BaseDirectory;
                //string xmlName = "ChannlEncoder.xml";
                //string fullPath = xmlPath + "\\" + xmlName;
                //XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.Load(fullPath);
                //XmlNodeList nodeList = xmlDoc.SelectSingleNode("ChannlEncoder").ChildNodes;
                //foreach (XmlNode xn in nodeList)
                //{
                //    AVEncoderInfoDto avInfo = new AVEncoderInfoDto();
                //    XmlElement xe = (XmlElement)xn;

                //    string streamType = xe.Attributes["StreamType"].Value;
                //    int subStream = 0;
                //    int.TryParse(streamType, out subStream);
                //    XmlNodeList resultList = xe.ChildNodes;
                //    if (resultList != null && resultList.Count == 5)
                //    {
                //        XmlElement resolution = (XmlElement)resultList[0];
                //        XmlElement frameRate = (XmlElement)resultList[1];
                //        XmlElement bitrateMode = (XmlElement)resultList[2];
                //        XmlElement bitrate = (XmlElement)resultList[3];
                //        XmlElement iFrameInterval = (XmlElement)resultList[4];

                //        byte btRes;
                //        byte.TryParse(resolution.InnerText.Trim(), out btRes);
                //        avInfo.Resolution = btRes;

                //        byte btFra;
                //        byte.TryParse(frameRate.InnerText.Trim(), out btFra);
                //        avInfo.FrameRate = btFra;

                //        byte btMode;
                //        byte.TryParse(bitrateMode.InnerText.Trim(), out btMode);
                //        avInfo.BitrateMode = btMode;

                //        byte btIFr;
                //        byte.TryParse(iFrameInterval.InnerText.Trim(), out btIFr);
                //        avInfo.IFrameInterval = btIFr;

                //        uint uiBit;
                //        uint.TryParse(bitrate.InnerText.Trim(),out uiBit);
                //        avInfo.Bitrate = uiBit;

                //        avEncoderInfo.Add(subStream,avInfo);
                //    }
                //} 

                //byte b = (1 << 1);
                //Console.WriteLine(b);

                //DeviceServer deviceServer = DeviceServer.GetInstance();
                //string keyWord = "M2111B02015102466700530362";
                //List<int> deviceIdList = new List<int>();
                //deviceIdList.Add(94);
                //deviceServer.SelectDeviceSerialNumber(keyWord, deviceIdList);

                // ChannelBLL  cs=new ChannelBLL();
                // IList<GroupChannelResponse> gcList=null;
                // int total=0;
                // cs.GetChannelByPage(1, 20, "", "",ref gcList,ref total);


                ////CheckDeviceBySN测试模型
                //DeviceBLL dBLL = new DeviceBLL();
                //int isAdd = 0;
                //int hardwareType = 0;
                //int isOnline = 0;
                //string sn = "M2111B02015102466700530362";
                //dBLL.CheckDeviceBySN(sn, ref isAdd, ref isOnline, ref hardwareType);



                //memCache测试模型
                //UserTokenCache utc = UserTokenCache.GetInstance();
                //int i = 0;
                //while (true)
                //{
                //    i++;
                //    IList<TokenCacheProperty> resultFlag = utc.FindAll();
                //    Console.WriteLine(resultFlag.Count);
                //    for (int j = 0; j < resultFlag.Count; j++)
                //    {
                //        Console.WriteLine(resultFlag[j].CustomerName);
                //    }
                //    Console.WriteLine(" ------------------ " + i);
                //    Thread.Sleep(2000);
                //}

                //ChannelServer cs = ChannelServer.GetInstance();
                //IList<Channel> channelFlag= cs.SelectChannelByChannelId(new Channel() {ChannelId=451 });
                //MyOutMsg.ReturnValidate();
                // string TestGuid = "efcf2461-7d43-461c-a726-dcbc38f7701c"; System.Guid.NewGuid().ToString();
                //子用户查询节点
                //ResourceGroupBLL rgb = new ResourceGroupBLL();
                //IList<ResourceGroup> resourceGroupFlag = new List<ResourceGroup>();
                //rgb.GetGroupByCustomerId("", ref resourceGroupFlag);
                //权限删除模型
                //PermissionServer ps = PermissionServer.GetInstance();
                //Permission permission = new Permission();
                //permission.CustomerId = 65;
                //ps.DeletePermissionByCustomerId(permission);
                ////测试流媒体参数
                //DeviceBLL deviceBLL = new DeviceBLL();
                //Channel channel=new Channel();
                //channel.ChannelId=319;
                //BP4StreamerParameter BPstreamer=new BP4StreamerParameter();
                //deviceBLL.GetStreamerParameterByCustomerToken(channel, "", ref BPstreamer);


                //调试设备状态模型
                //DeviceBLL deviceBLL = new DeviceBLL();
                //int[] deviceIdList= new int[]{60,61};
                //List<Device> deviceFlag=new List<Device>();
                //deviceBLL.GetServerGetDeviceState(deviceIdList, "",ref  deviceFlag);

                //调试主用户对子用户的授权模型

                //CustomerBLL cb = new CustomerBLL();
                //Customer customer=new Customer();
                //AuthorizeSubResponse sub=new AuthorizeSubResponse();
                //cb.GetAuthorizeSubCustomer(customer, "", ref sub);
                 
                // string stop = "Stop";
                // Console.Write(stop);
                // 日志分页模型
                //OperaterLogServer ol=OperaterLogServer.GetInstance();
                //List<int> testlogList=new List<int>();
                //testlogList.Add(66);
                //int startCount=0;
                //int resquestCount=5;
                //ol.OperaterLogForPage(testlogList, resquestCount, startCount, "登录");
                //ol.OperaterLogForPageCount(testlogList, "登录");
                //ChannelServer channelServer=ChannelServer.GetInstance();
                //Device device =new Device();
                //Customer customer=new Customer();
                //Channel channel=new Channel();

                //device.CustomerId = 32;
                //channel.ChannelId = 335;
                
                //IList result=channelServer.SelectChannelAndDeviceBySome(device, channel);
                
                //for (int i = 0; i < result.Count; i++)
                //{
                //    object[] obj =(object[])result[i];
                //    int rs = (int)obj[3];
                //    Console.Write(rs);
                      
                //}
                //前台管理员查询主账号信息模型
                //CustomerBLL customerBLL = new CustomerBLL();
                //IList<Customer> customerFlag=null;
                //CustomerServer cs=CustomerServer.GetInstance();
                //Customer customer = new Customer();
                //customer.CustomerId = 31;
                //string keyWord = "";
                //cs.SearchCustomerByParentId(customer,keyWord);

                //BPServer服务器验证token模型
                //CustomerBLL customerBLL = new CustomerBLL();
                //int nodeId = 66;
                //int nodeType = 2;
                //customerBLL.CheckServerCustomerToken(PermissionNameTypeEnum.Video.ToString(), nodeId, nodeType, "");


                //设备搜索模型
                //DeviceBLL deviceBLL = new DeviceBLL();
                //string nameKey = "b";
                //string SNKey = "";
                //string token = "";
                //IList<Device> deviceFlag = null;

                // deviceBLL.SearchDevice(nameKey,SNKey,token,ref deviceFlag);

                //通道分页查询模型
                // ChannelBLL channelBLL = new ChannelBLL();
                //int requestCount=50;

                //int startCount=5;
                // Total=0;
                //IList<Channel> channelFlag=null; 
                //channelBLL.GetChannelByPage(startCount,requestCount,"", ref Total,ref channelFlag);

                //查询某个组下所有分组
                //ResourceGroupBLL rg = new ResourceGroupBLL();
                //ResourceGroup resourceGroup = new ResourceGroup();
                //resourceGroup.ResourceGroupId = 86;
                //int[] channelIdList = new int[] { 52,53};
                //rg.UpdateChannelListByResourceGroupId(resourceGroup, channelIdList,"");
                //IList<GroupChannel> groupChannelFlag = new List<GroupChannel>();
                //rg.GetChannelByResourceGroupIdList(resourceGroup, "", ref groupChannelFlag);
                //resourceGroup.ResourceGroupId = 6;
                //查询用户通道 模型
                //IList<Channel> deviceChannel = null;
                //Customer customerChannelTest = new Customer();
                //customerChannelTest.CustomerId = 10;
                //rg.SelectChannelByCustomerId(customerChannelTest,"",ref deviceChannel);
                //rg.RemoveResourceGroup(customerChannelTest, resourceGroup, "");
                //DeviceBLL deviceBLL = new DeviceBLL();
                //Customer customer = new Customer();
                //customer.CustomerId = 7;
               // deviceBLL.AddDevice(customer,"test","192.168.50.140",0,"",null);
               ////添加Receiver模型
               // MCache mc = new MCache();
               // Customer customer = new Customer();
               // CustomerBLL cb = new CustomerBLL();
               // //添加主用户模型
               // Customer customerTwo = new Customer();
               // customerTwo.CustomerName = "bstar_tt";
               // customerTwo.Password="123456";
               // customerTwo.SignInType = 2;
               // customerTwo.ReceiverCellPhone = "13888888887";
               // customerTwo.ReceiverEmail = "bstar_tt@bstar.com.cn";
               // customerTwo.ReceiverName = "蓝色星河";
               // customerTwo.ParentId = 0;
               // customerTwo.AccountCompany = "北京蓝色星河软件技术发展有限公司";
               // customerTwo.AccountCompanyAddress = "北京市海淀区北太平庄路18号城建大厦A座24层";
               // customerTwo.AccountTelephone = "010-82255955";
               // customerTwo.AccountIDNumber = "";
               // customerTwo.IsEnable = 0;
               // int CustomerId = 0;
               // cb.AddParentCustomerInfo(customerTwo,ref CustomerId);

               // Console.Write(CustomerId.ToString());

                //用户查询
               // customer.CustomerName = "13888888888";
               // customer.Password = "123456";
               // string CustomerName="";
               // string CustomerToken = "";
               // int loginType =1;
               // var response = cb.Login(customer.CustomerName, customer.Password,loginType, ref  CustomerId, ref  
               //CustomerName, ref  CustomerToken);

               // Console.Write(response);

                //测试MCache
                //UserTokenCache utc = UserTokenCache.GetInstance();
                //while (true)
                //{
                //    Thread.Sleep(5000);
                //    TokenCacheProperty tcp = utc.FindByCustomerToken(CustomerToken);
                //    if (tcp != null)
                //        Console.Write(tcp.CustomerName);
                //    else Console.Write("已被移除");
                //}
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //ToWCFScoket scoket = new ToWCFScoket();

           /// scoket.SocketServer();
            Console.ReadKey();
        }


        [AttributeUsage(AttributeTargets.Class | 
            AttributeTargets.Method | 
            AttributeTargets.ReturnValue | 
            AttributeTargets.Property,
            AllowMultiple = true, Inherited = true)]
        public class ValidateAttribute: Attribute
        {
        }

        public  class Test
        {
            [Validate]   
            public void Demo() { }
        }
        //标记控制目标类
        public static class MyOutMsg
        {
            public static object ReturnValidate()
             {
                 Test test = new Test();
                 MemberInfo[] members = test.GetType().GetMembers();
                 
                 foreach (MemberInfo member in members)
                 {
                     Attribute testAttr = Attribute.GetCustomAttribute(member, typeof(ValidateAttribute));
                     if (testAttr != null && testAttr is ValidateAttribute)
                     {
                         Console.WriteLine(((ValidateAttribute)testAttr));
                     }
                     Attribute[] attributes = Attribute.GetCustomAttributes(member);
                 }
                 return null;
             }

        }
    }
}
