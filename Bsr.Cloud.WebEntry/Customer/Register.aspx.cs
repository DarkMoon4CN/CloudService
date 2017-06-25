using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Text;
using System.Collections;
using System.Web.Services;

namespace Bsr.Cloud.WebEntry.CustomerReg
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string SendMail(string toMail)
        {
            string title = "星际云注册";
            string content = CreateNum();
            //读取保存的Cookie信息
            HttpCookie cookie = HttpContext.Current.Request.Cookies["RegCode"];
            if (cookie != null)
            {
                cookie.Value = content;
            }
            else
            {
                cookie = new HttpCookie("RegCode");
                cookie.Value = content;

                DateTime dt = DateTime.Now;
                TimeSpan ts = new TimeSpan(0, 0, 1, 0, 0);//过期时间为1分钟
                cookie.Expires = dt.Add(ts);//设置过期时间
            }

            MailService mail = new MailService();
            mail.SendMail(toMail, title, content);
            return content;
        }

        /// <summary>
        /// 生成6位随机数字
        /// </summary>
        /// <returns></returns>
        public static string CreateNum()
        {
            ArrayList MyArray = new ArrayList();
            Random random = new Random();
            string str = null;
            //循环的次数   
            int Nums = 6;
            while (Nums > 0)
            {
                int i = random.Next(1, 9);
                if (!MyArray.Contains(i))
                {
                    if (MyArray.Count < 6)
                    {
                        MyArray.Add(i);
                        Nums -= 1;
                    }
                }
                continue;
            }

            for (int j = 0; j <= MyArray.Count - 1; j++)
            {
                str += MyArray[j].ToString();
            }
            return str;
        }
    }


    /// <summary>
    ///发送邮件类
    /// </summary>
    public class MailService
    {
        /// <summary>
        /// 发送邮件的方法
        /// </summary>
        /// <param name="toMail">目的邮件地址</param>
        /// <param name="title">发送邮件的标题</param>
        /// <param name="content">发送邮件的内容</param>
        public void SendMail(string toMail, string title, string content)
        {
            // 邮件服务设置
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            smtpClient.Host = "smtp.bstar.com.cn"; //指定SMTP服务器
            smtpClient.Port = 25;//端口
            smtpClient.Credentials = new System.Net.NetworkCredential("zhangmz@bstar.com.cn", "newday2010");//用户名和密码

            // 发送邮件设置        
            MailMessage mailMessage = new MailMessage(); // 发送人和收件人
            mailMessage.From = new MailAddress("zhangmz@bstar.com.cn");//发件人
            //收件人邮箱地址
            //第一个参数是发信人邮件地址
            //第二参数是发信人显示的名称
            //第三个参数是 第二个参数所使用的编码，如果指定不正确，则对方收到后显示乱码
            mailMessage.To.Add(new MailAddress(toMail, toMail.ToString(), Encoding.UTF8));
            //邮件标题编码
            mailMessage.SubjectEncoding = Encoding.UTF8;
            //邮件主题
            mailMessage.Subject = title;
            //邮件内容
            mailMessage.Body = content;
            //邮件内容编码
            mailMessage.BodyEncoding = Encoding.UTF8;

            //设置正文内容是否是包含Html的格式
            mailMessage.IsBodyHtml = true;
            //发送邮件的优先等级（有效值为High,Low,Normal）
            mailMessage.Priority = MailPriority.Normal;
            smtpClient.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
            //发送邮件
            smtpClient.Send(mailMessage);   //同步发送
            //smtpClient.SendAsync(mailMessage, mailMessage.To); //异步发送 （异步发送时页面上要加上Async="true" ）
        }
        void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                System.Web.HttpContext.Current.Response.Write("<script>alert('Sending of email message was cancelled.');</script>");
            }
            if (e.Error == null)
            {
                System.Web.HttpContext.Current.Response.Write("<script>alert('Mail sent successfully');</script>");
            }
            else
            {
                System.Web.HttpContext.Current.Response.Write("<script>alert('Error occured, info=" + e.Error.Message + "');</script>");
            }

        }
    }

}