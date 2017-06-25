using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bsr.Cloud.Model;
using Bsr.Cloud.Core;
using Bsr.Cloud.Model.Entities;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
namespace Bsr.Cloud.BLogic.BLL
{
    public class BLLHelper
    {
        CustomerServer customerServer = CustomerServer.GetInstance();
        BPServerConfigServer bpServerConfigServer = BPServerConfigServer.GetInstance();
        static private ILogger myLog = new Logger<BLLHelper>();

        #region 记录用户日志 AddOperaterLog
        /// <summary>
        ///  记录用户日志
        /// </summary>
        /// <param name="customerToken">token</param>
        /// <param name="oLog">日志 实体</param>
        internal void AddOperaterLog(OperaterLog oLog,TokenCacheProperty tcp)
        {
            try
            {
                //memcache中搜寻 customerToken 进行组织数据 
                //UserTokenCache utc = UserTokenCache.GetInstance();
                StringBuilder builder = new StringBuilder();
                if (tcp != null)
                {
                    oLog.CustomerId = tcp.CustomerId;
                    
                    oLog.OperaterTime = DateTime.Now;
                    builder.Append("用户：");
                    builder.Append(tcp.CustomerName);
                    builder.Append(" ");
                    builder.Append(oLog.Action);

                    string AgentType = "";
                    if (tcp.LoginType == (int)LoginTypeEnum.web)
                    {
                        AgentType = "web";
                    }
                    else if (tcp.LoginType == (int)LoginTypeEnum.window)
                    {
                        AgentType = "桌面客户端";
                    }
                    else if (tcp.LoginType == (int)LoginTypeEnum.android)
                    {
                        AgentType = "android";
                    }
                    else if (tcp.LoginType == (int)LoginTypeEnum.ios)
                    {
                        AgentType = "IOS";
                    }
                    else
                    {
                        AgentType = "未知";
                    }
                    builder.Append(AgentType);
                    if (tcp.AgentVersion != null && tcp.AgentVersion != "")
                    {
                        builder.Append(" 版本：" + tcp.AgentVersion + " ");
                    }
                    oLog.AgentType = AgentType;
                    oLog.AgentVersion=tcp.AgentVersion;
                    builder.Append(oLog.Remarks);
                    oLog.Remarks = builder.ToString();
                    OperaterLogServer.GetInstance().InsertOperaterLog(oLog);
                 }
            }
            catch(Exception ex)
            {
                myLog.WarnFormat("AddOperaterLog方法异常,Customer 日志未能写入",ex);
            }
        }
        #endregion

        #region 验证用户 CheckCustomer ref用户缓存对象
        internal ResponseBaseDto CheckCustomer(ResponseBaseDto dto, string customerToken, ref TokenCacheProperty tcp)
        {
            UserTokenCache utc = UserTokenCache.GetInstance();
            tcp= utc.FindByCustomerToken(customerToken);
            Customer customer = new Customer();
            if (tcp == null)
            {
                dto.Message = "请求的用户未找到";
                dto.Code = (int)CodeEnum.NoUser;
                return dto;
            }
            else if (tcp.IsEnable == 0)
            {
                dto.Message = "此账户已被冻结,无法使用此功能";
                dto.Code = (int)CodeEnum.NoAuthorization;
                return dto;
            }//Wait....
            return dto;
        }
        #endregion

        #region 提供保存图片方法
        /// <summary>
        /// 提供保存图片方法
        /// </summary>
        /// <param name="imageByte">图片byte字节</param>
        /// <param name="folderName">文件夹</param>
        /// <param name="extName">后缀名{jpg,png}....</param>
        /// <param name="oldImageName">旧的文件名遇到default时跳过</param>
        /// <param name="tcp">tcp.CustomerId</param>
        /// <returns>图片名称</returns>
        internal string SaveImage(byte[] imageByte, string folderName, string extName,string oldImageName, TokenCacheProperty tcp) 
        {
            string filePath =string.Empty;
            // 检查存放图片的目录是否存在，不存在则创建
            var path = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory,folderName);
            Directory.CreateDirectory(path);

            // 生成随机且唯一的image文件名，将数据保存
            string imageFile = string.Format("{0}_{1}.{2}",
                               PwdMD5.StringToMD5(DateTime.Now.ToString()), tcp.CustomerId, extName);

            // 打开文件，写入图片数据
            filePath = string.Format(@"{0}\{1}", path, imageFile);
            FileStream fs = new System.IO.FileStream(filePath, FileMode.Create, FileAccess.Write);
            fs.Write(imageByte, 0, imageByte.Length);
            fs.Close();
            if (!oldImageName.ToLower().Contains("default"))
            {
                File.Delete(path + "\\" + oldImageName);
            }

            return imageFile;
        }
        #endregion

        #region 过滤数据和脚本关键字
        /// <summary>
        /// 过滤数据和脚本关键字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckBadWord(string str)
        {
            string pattern = @"<script|select|insert|delete|from|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec   master|netlocalgroup administrators|net user|or|and";
            if (Regex.IsMatch(str, pattern, RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 获取BPServerConfig配置信息
        //获取配置信息
        internal BPServerConfig GetServerConfig()
        {
            BPServerConfig bpServerConfig = new BPServerConfig();
            bpServerConfig.Domain = "default";
            IList<BPServerConfig> bpServerConfigFlag = bpServerConfigServer.GetBPServerConfigByDomain(bpServerConfig);

            if (bpServerConfigFlag == null && bpServerConfigFlag.Count == 0)
            {
                bpServerConfig.BPServerConfigId = 3;
                bpServerConfigFlag.Add(bpServerConfig);
                return GetServerConfig(bpServerConfigFlag[0]);
            }
            else
            {
                return bpServerConfigFlag[0];
            }

        }
        //获取配置信息
        internal BPServerConfig GetServerConfig(BPServerConfig serverConfig)
        {
            IList<BPServerConfig> serverConfigFlag = bpServerConfigServer.GetBPServerConfigByKey(serverConfig);
            if (serverConfigFlag != null && serverConfigFlag.Count > 0)
            {
                return serverConfigFlag[0];
            }
            return null;
        }
        /// <summary>
        ///   获取服务器某块的svc服务是否在本地配置存在
        /// </summary>
        internal string GetServerModelStr()
        {
            BPServerConfig serverConfig = GetServerConfig();
            if (serverConfig != null)
            {
                return serverConfig.BusinessLogicAddress;
            }
            return "";
        }
        #endregion

        #region XML反序列化
        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region  XML序列化
        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }
        #endregion
    }
}
