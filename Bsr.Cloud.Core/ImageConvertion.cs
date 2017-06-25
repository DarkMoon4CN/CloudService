using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageMagick;
using System.IO;
using System.Drawing;
using Bsr.Cloud.Model;

namespace Bsr.Cloud.Core
{

    public class ImageConvertibleInfo
    {
        public byte[] srcByte;  // 图片源数据
        public int dstWidth;    // 目标宽
        public int dstHeight;   // 目标高
        public string dstFmt;   // 目标格式：png/jpg
    }

    /// <summary>
    /// 图片转换的工具类，该类库依赖：Magick.NET-AnyCPU.dll 这个c#库，
    /// 这个库又依赖于 vcomp110.dll, msvcp110.dll, msvcr110.dll 这几个vs运行时库
    /// </summary>
    public class ImageConvertion
    {

        /// <summary>
        /// 转换图片文件
        /// </summary>
        /// <param name="icInfo">待处理的图片数据和目标格式描述</param>
        /// <param name="dstByte">处理好的图片数据, 函数返回0时表示有效</param>
        /// <returns>0表示成功，其它值为失败</returns>
        public static int Convert(ImageConvertibleInfo icInfo, out byte[] dstByte)
        {
            MagickImage image = null;
            dstByte = null;

            if (icInfo.srcByte == null || icInfo.dstWidth <= 0 || icInfo.dstHeight <= 0)
            {
                return (int)CodeEnum.NoComplete;
            }

            try
            {
                image = new MagickImage(icInfo.srcByte);

                // 重新设置图片的尺寸和格式信息，
                image.Scale(icInfo.dstWidth, icInfo.dstHeight);
                switch (icInfo.dstFmt)
                {
                    case "png":
                        image.Format = MagickFormat.Png;
                        break;
                    case "jpg":
                    case "jpeg":
                        image.Format = MagickFormat.Jpg;
                        break;
                    default:
                        image.Format = MagickFormat.Png;
                        break;
                }

                dstByte = image.ToByteArray();
            }
            catch (Exception ex)
            {
                myLog.ErrorFormat("图片转换失败，目标信息：{0} * {1}, 格式：{2}", ex,
                                  icInfo.dstWidth, icInfo.dstHeight, icInfo.dstFmt);
                return (int)CodeEnum.ApplicationErr;
            }
            finally
            {
                if (image != null)
                {
                    image.Dispose();
                }
            }

            return 0;
        }


        #region

        static private ILogger myLog = new Logger<ImageConvertion>();

        #endregion

    }
}


/*
        static void Main(string[] args)
        {
            //var path = string.Format(@"{0}", AppDomain.CurrentDomain.BaseDirectory);
            //MagickAnyCPU.CacheDirectory = path;


            ImageConvertibleInfo icInfo = new ImageConvertibleInfo();

            FileStream f = new FileStream(@"d:\s1.png", FileMode.Open, FileAccess.Read);
            icInfo.srcByte = new byte[f.Length];
            f.Read(icInfo.srcByte, 0, (Int32)f.Length);
            f.Close();


            icInfo.dstHeight = 1240;
            icInfo.dstWidth = 1320;
            icInfo.dstFmt = "jpg";

            byte[] dstByte = null;
            bool rs = Convert(icInfo, out dstByte);
            if (rs)
            {
                FileStream f2 = new FileStream(@"d:\d1.png", FileMode.Create, FileAccess.Write);
                f2.Write(dstByte, 0, dstByte.Length);
                f2.Close();
            }

        }
 
 */