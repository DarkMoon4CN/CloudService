using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace Bsr.Cloud.WebEntry.PageHandler
{
    /// <summary>
    /// UpLoadImage 的摘要说明
    /// </summary>
    public class UpLoadImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";

            HttpFileCollection files = context.Request.Files;

            if (files.Count > 0)
            {
                HttpPostedFile file = files[0];
                string fileName = file.FileName;
                Stream fs = file.InputStream;
                BinaryReader br = new BinaryReader(fs);

                byte[] bytes = br.ReadBytes((Int32)fs.Length);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                //string res = "{ msg:'转换成功',base64Code:'" + Convert.ToBase64String(bytes) + "'}";
                context.Response.Write(Convert.ToBase64String(bytes));
                context.Response.End();
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}