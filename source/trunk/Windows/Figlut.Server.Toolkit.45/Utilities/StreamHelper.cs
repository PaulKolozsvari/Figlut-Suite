namespace Figlut.Server.Toolkit.Utilities
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

        public static Stream GetStreamFromBytes(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        public static Stream GetByteStreamFromFile(string filePath)
        {
            FileSystemHelper.ValidateFileExists(filePath);
            MemoryStream result = new MemoryStream();
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                result.Write(buffer, 0, buffer.Length);
            }
            return result;
        }

        public static string GetStringFromStream(Stream stream, Encoding encoding)
        {
            byte[] data = GetBytesFromStream(stream);
            return encoding.GetString(data);
        }

        public static Stream GetStreamFromString(string text, Encoding encoding)
        {
            if (text == null)
            {
                text = string.Empty;
            }
            byte[] result = encoding.GetBytes(text);
            return new MemoryStream(result);
        }

        #endregion //Methods
    }
}
