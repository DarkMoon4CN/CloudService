using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using Bsr.Cloud.Model;
using Bsr.Cloud.Core;
using System.Xml;

namespace Bsr.Cloud.WebEntry.RestService
{
    public class Common : ICommon
    {
        static private ILogger myLog = new Logger<Common>();

        public CheckUpdateResponseDto CheckUpdate(CheckUpdateRequestDto req)
        {
            CheckUpdateResponseDto res = new CheckUpdateResponseDto();

            try
            {
                res.SoftwareId = req.SoftwareId;
                res.LatestBuildVersion = req.BuildVersion;
                string path = "MobileClinetDownLoad/Android/";
                string WhereToUpdate = "";
                string MajorVersion = "";
                string MinorVersion = "";
                switch (req.AgentType)
                {
                    case (int)LoginTypeEnum.android:
                        myLog.Info(" android");
                        res.Expired = ReadXmlAndVerifyVersion(path,req.SoftwareId, req.MajorVersion, req.MinorVersion, out WhereToUpdate,out MajorVersion,out MinorVersion);
                        res.LatestMajorVersion = MajorVersion;
                        res.LatestMinorVersion = MinorVersion;
                        res.WhereToUpdate = WhereToUpdate;
                        break;
                    case (int)LoginTypeEnum.ios:
                        myLog.Info(" ios");
                        break;

                    default:
                        myLog.WarnFormat("未知的终端类型：{0}", req.AgentType);
                        break;
                }
            }
            catch (Exception ex)
            {
                myLog.Error("检测客户端版本时有异常: ", ex);
            }

            return res;
        }

        /// <summary>
        /// 读取软件下载版本的配置文件并获取软件更新的标识
        /// </summary>
        /// <param name="path">配置文件的路径</param>
        /// <param name="SoftwareId">软件唯一标识</param>
        /// <param name="rec_majorVersion">主版本</param>
        /// <param name="rec_minorVersion">子版本</param>
        /// <param name="WhereToUpdate">下载路径</param>
        /// <param name="MajorVersion">最新主版本</param>
        /// <param name="MinorVersion">最新子版本</param>
        /// <returns>软件更新标识;1 表示已过期，0表示可继续使用</returns>
        private int ReadXmlAndVerifyVersion(string path,string SoftwareId, string rec_majorVersion, string rec_minorVersion, out string WhereToUpdate,out string MajorVersion,out string MinorVersion)
        {
            int flag = 0;
            string version_Major = "";
            string version_Minor = "";
            WhereToUpdate = "";
            MajorVersion = "";
            MinorVersion = "";
            XmlDocument xmlmodel = null;
            string XMLParam = AppDomain.CurrentDomain.BaseDirectory + path + "DownConfig.xml";
            if (System.IO.File.Exists(XMLParam))
            {
                if (xmlmodel == null)
                {
                    xmlmodel = new XmlDocument();
                    xmlmodel.Load(XMLParam);
                }
                foreach (XmlNode xmlNode in xmlmodel.ChildNodes)
                {
                    foreach (XmlNode xnode in xmlNode.ChildNodes)
                    {
                        if (xnode.Attributes["Name"].Value.ToUpper() == "MAJORVERSION")
                        {
                            version_Major = xnode.Attributes["Value"].Value;
                        }
                        else if (xnode.Attributes["Name"].Value.ToUpper() == "LATESTMINORVERSION")
                        {
                            version_Minor = xnode.Attributes["Value"].Value;
                        }
                    }
                }
                if (version_Major == rec_majorVersion)
                {
                    if (version_Minor == rec_minorVersion)
                    {
                        flag = 0;
                    }
                    else 
                    {
                        WhereToUpdate = path;
                        flag = 0;
                    }
                    MinorVersion = version_Minor;
                }
                else
                {
                    MinorVersion = version_Major;
                    MinorVersion = version_Minor;
                    WhereToUpdate = path;
                    flag = 1;
                }
            }
            return flag;
        }
    }
}