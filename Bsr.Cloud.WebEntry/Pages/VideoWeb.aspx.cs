using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Bsr.Cloud.WebEntry.Pages
{
    public partial class VideoWeb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string Base64Imge(string bdata, string dataSize, string format)
        {
            byte[] bt = Convert.FromBase64String(bdata);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(bt);
            Bitmap bitmap = new Bitmap(stream);

            // 打开文件，写入图片数据
            var path = string.Format(@"{0}\channelImage", AppDomain.CurrentDomain.BaseDirectory);
            string imageFullPath = string.Format(@"{0}\{1}", path, "test2222.jpg");
            bitmap.Save(imageFullPath, ImageFormat.Jpeg);

            return "";
        }
    }
}