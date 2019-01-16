#region Using Directives

using System;

#endregion //Using Directives

namespace Figlut.MonoDroid.Toolkit.Utilities.SettingsFile.Default
{
    public class WebServiceMobileClientAppSettings : WebServiceClientAppSettings
    {
        #region FTP

        /// <summary>
        /// The base URI of the FTP site.
        /// </summary>
        [SettingInfo("FTP", DisplayName ="FTP Base URI", Description = "The base URI of the FTP site.", CategorySequenceId = 0)]
        public string FtpBaseUri { get; set; }

        [SettingInfo("FTP", DisplayName ="FTP Username", Description = "The Username to use to connect to the FTP site.", CategorySequenceId = 1)]
        public string FtpUsername { get; set; }

        [SettingInfo("FTP", DisplayName = "FTP Password", Description = "The Password to use to connect to the FTP site.", CategorySequenceId = 2)]
        public string FtpPassword { get; set; }

        [SettingInfo("FTP", DisplayName = "FTP File Transfer Chunk Size", Description = "The size of the buffer to use when transferring files to and from the FTP site.", CategorySequenceId = 3)]
        public int FtpFileTransferChunkSize { get; set; }

        #endregion //FTP
    }
}