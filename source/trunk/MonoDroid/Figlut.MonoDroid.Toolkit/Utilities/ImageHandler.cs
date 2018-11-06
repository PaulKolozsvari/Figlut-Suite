using Android.Graphics;
using Android.Media;

namespace Figlut.MonoDroid.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;
    using System.IO;

    #endregion //Using Directives

	//TODO Implment an Android version of the ImageHandler

    public class ImageHandler
    {
        public static void SaveImage(string filePath, byte[] imageBytes)
        {
//            EncoderParameter param = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 1);
//            ImageCodecInfo jpegCodec = GetEncoderInfo(@"image/bmp");
//            EncoderParameters encoderParams = new EncoderParameters(100);
//            encoderParams.Param[0] = param;
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    fs.Write(imageBytes, 0, imageBytes.Length);
                }
            }
        }

//        private static ImageCodecInfo GetEncoderInfo(String mimeType)
//        {
//            int j;
//            ImageCodecInfo[] encoders;
//            encoders = ImageCodecInfo.GetImageEncoders();
//            for (j = 0; j < encoders.Length; ++j)
//            {
//                if (encoders[j].MimeType == mimeType)
//                    return encoders[j];
//            }
//            return null;
//        }
    }
}