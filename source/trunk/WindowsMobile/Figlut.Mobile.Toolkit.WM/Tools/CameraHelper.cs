namespace Figlut.Mobile.Toolkit.Tools
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.WindowsMobile.Forms;
    using Microsoft.WindowsMobile.Status;
    using System.Windows.Forms;
    using System.Drawing;
    using System.IO;
    using Figlut.Mobile.Toolkit.Utilities;

    #endregion //Using Directives

    /// <summary>
    /// A helper class that uses the Windows Mobile CameraCaptureDialog to take pictures.
    /// http://breathingtech.com/2011/working-with-camera-on-windows-mobile-applications-net-compact-framework/
    /// </summary>
    public class CameraHelper
    {
        #region Methods

        /// <summary>
        /// Uses a Windows Mobile CameraCaptureDialog to take a picture or video.
        /// Returns true if the picture was taken successfully, otherwise false
        /// if the user cancelled out of the dialog.
        /// </summary>
        /// <param name="video">Whether or not to capture video instead of pictures.</param>
        /// <param name="owner">The form that should own the CameraCaptureDialog. Would typically be the form form from which this method is being called.</param>
        /// <param name="title">The title to be shown on the CameraCaptureDialog.</param>
        /// <param name="result">The image result after a picture has been taken will be placed into this parameter.</param>
        /// <param name="resultData">The binary data of the image result after a picture has been taken will be placed into this parameter.</param>
        /// <returns></returns>
        public bool CameraCapture(
            bool video, 
            Form owner, 
            string title,
            out Image result,
            out byte[] resultData)
        {
            if (!SystemState.CameraPresent)
            {
                throw new Exception("No camera detected on device or camera is not supported.");
            }
            using (CameraCaptureDialog cameraCapture = new CameraCaptureDialog())
            {
                cameraCapture.Owner = owner;
                cameraCapture.InitialDirectory = @"\My Documents";
                cameraCapture.Title = title;
                cameraCapture.VideoTypes = CameraCaptureVideoTypes.Messaging;
                cameraCapture.Resolution = new Size(480, 640);
                cameraCapture.VideoTimeLimit = new TimeSpan(0, 0, 15);
                if (video)
                {
                    cameraCapture.Mode = CameraCaptureMode.VideoWithAudio;
                    cameraCapture.DefaultFileName = @"videotest.3gp";
                }
                else
                {
                    cameraCapture.Mode = CameraCaptureMode.Still;
                    cameraCapture.DefaultFileName = @"FiglutCameraImage.jpg";
                }
                string imageFilePath = Path.Combine(cameraCapture.InitialDirectory, cameraCapture.DefaultFileName);
                if (File.Exists(imageFilePath))
                {
                    File.Delete(imageFilePath);
                }
                DialogResult captureResult = cameraCapture.ShowDialog();
                if (File.Exists(cameraCapture.FileName) || File.Exists(imageFilePath))
                {
                    result = new Bitmap(imageFilePath);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        result.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        resultData = ms.ToArray();
                    }
                    return true;
                }
                result = null;
                resultData = null;
                return false;
            }
        }

        #endregion //Methods
    }
}