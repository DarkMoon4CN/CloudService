using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using System.Net;
using System.Text;

namespace Bsr.Cloud.WebEntry.PageHandler
{
    /// <summary>
    /// LoginHandler 的摘要说明
    /// </summary>
    public class LoginHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request.Params["action"];//请求方法
            //登陆
            if (action == "login")
            {
                string Userid = context.Request.Params["id"];
                FormsAuthentication.RedirectFromLoginPage(Userid, true);
                context.Session["token"] = context.Request.Params["token"];
                context.Session["signInType"] = context.Request.Params["signInType"];
                context.Response.End();
            }
            //注销
            else if (action == "logout")
            {
                context.Session.Remove("token");
                context.Session.Remove("signInType");
                FormsAuthentication.SignOut();
            }
            //判断系统是否登陆
            else if (action == "isLogin")
            {
                bool isLogin = HttpContext.Current.User.Identity.IsAuthenticated;
                if (isLogin)
                {
                    string result = "{\"logined\":true,\"user\":{\"token\":\"" + context.Session["token"] + "\",\"signInType\":" + (context.Session["signInType"] != null ? context.Session["signInType"] : 0) + "}}";
                    context.Response.Write(result);
                }
                else
                {
                    string result = "{\"logined\":false}";
                    context.Response.Write(result);
                }
            }
            else if (action == "ImgToBase64")
            {
                string Imagefilename = context.Request.Params["Imagefilename"];
                if (Imagefilename != null && Imagefilename != "")
                {
                    context.Response.Write(ImgToBase64String(Imagefilename));
                }
                else
                {
                    context.Response.Write("false");
                }
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        //图片转为base64编码的字符串   
        public string ImgToBase64String(string Imagefilename)
        {
            try
            {
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Imagefilename);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch// (Exception ex)
            {
                return "";
            }
        }

        //base64编码的字符串转为图片   
        public System.Drawing.Bitmap Base64StringToImage(string strbase64)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);
                //bmp.Save("test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);   
                ms.Close();
                return bmp;
            }
            catch// (Exception ex)
            {
                return null;
            }
        }
    }
}