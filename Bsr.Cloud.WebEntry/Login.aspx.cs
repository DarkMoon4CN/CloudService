using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Bsr.Cloud.BLogic;
using Bsr.Cloud.Model;
using Bsr.Cloud.Core;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Text;
using Bsr.Cloud.Model.Entities;
using System.Xml;
using Bsr.Cloud.WebEntry.RestService;
using System.Web.Security;

namespace Bsr.Cloud.WebEntry
{
    public partial class Login : System.Web.UI.Page
    {
        private ILogger myLog = new Logger<Login>();
        protected void Page_Load(object sender, EventArgs e)
        {
            myLog.InfoFormat("Index页面初始化");
            //string token = (string)Session["token"];
            //string host = "http://" + Request.Url.Host;
            //string jumpURL = host + "/Pages/Index.aspx?token=" + token;
            //if (token != null)
            //{
            //    try
            //    {
            //        string url = host + "/ServiceCustomer.svc/GetSelfCustomer";
            //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //        request.Method = "POST";//必填
            //        request.ContentType = "application/json";//必填
            //        request.Headers.Add("BstarCloud-User-Token", token);//必填
            //        request.ContentLength = 0;//必填，发送长度。post情况下，不设置会有异常，
            //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //        Stream receiveStream = response.GetResponseStream();
            //        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            //        string sourceCode = readStream.ReadToEnd();
            //        response.Close();
            //        readStream.Close();
            //        int subIndnex = sourceCode.IndexOf("\"SignInType\":");
            //        string indexSource = sourceCode.Substring(subIndnex);
            //        int subLast = indexSource.IndexOf(",");
            //        string subData = indexSource.Substring(0, subLast);
            //        string[] splitData = subData.Split(':');
            //        if (splitData[1] == "1")
            //        {
            //            jumpURL = host + "/Pages/AccountManagement_Admin.aspx?token=" + token;
            //        }
            //    }
            //    catch
            //    {
            //        jumpURL = host + "/Login.aspx";
            //    }
            //Response.Redirect(jumpURL, false);
        }
    }
}