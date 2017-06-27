namespace Figlut.Mobile.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    #endregion //Using Directives

    public class StreamHelper
    {
        #region Methods

        public static byte[] GetBytesFromStream(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static string GetStringFromStream(Stream stream, Encoding encoding)
        {
            byte[] data = GetBytesFromStream(stream);
            return encoding.GetString(data, 0, data.Length);
        }

        public static Stream GetStreamFromString(string text, Encoding encoding)
        {
            byte[] result = encoding.GetBytes(text);
            return new MemoryStream(result);
        }

        #endregion //Methods
    }
}
