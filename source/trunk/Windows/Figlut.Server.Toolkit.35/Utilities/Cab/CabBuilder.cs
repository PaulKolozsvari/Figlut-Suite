namespace Figlut.Server.Toolkit.Utilities.Cab
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Diagnostics;

    #endregion //Using Directives

    public class CabBuilder
    {
        #region Constants

        public const string CAB_WIZ_LOG_FILE_NAME = "Figlut.Cab.Builder.log";
        public const string DEFAULT_CAB_WIZ_FILE_NAME = "Cabwiz.exe";

        #endregion //Constants

        #region Methods

        /// <summary>
        /// Cab Wizard: http://msdn.microsoft.com/en-us/library/aa924359.aspx
        /// </summary>
        /// <param name="cabInfo"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="cabWizFilePath"></param>
        /// <param name="deleteINFAfterBuild"></param>
        /// <returns>Returns the contents of CabWiz's log file.</returns>
        public static bool BuildCab(
            CabInfo cabInfo,
            string outputFilePath,
            string cabWizFilePath,
            bool deleteINFAfterBuild,
            out string cabWizLogFileContents,
            out string outputAbsoluteFilePath)
        {
            outputAbsoluteFilePath = Path.IsPathRooted(outputFilePath) ? outputFilePath : Path.GetFullPath(outputFilePath);
            string outputFileName = Path.GetFileName(outputAbsoluteFilePath);
            string outputDirectory = Path.GetDirectoryName(outputAbsoluteFilePath);
            string cabWizAbsoluteFilePath = Path.IsPathRooted(cabWizFilePath) ? cabWizFilePath : Path.GetFullPath(cabWizFilePath);
            string cabWizLogFileAbsoluteFilePath = Path.Combine(outputDirectory, CAB_WIZ_LOG_FILE_NAME);
            string cabWizINFAbsoluteFilePath = Path.Combine(outputDirectory, Path.ChangeExtension(outputFileName, ".inf"));
            if (!Directory.Exists(outputDirectory))
            {
                throw new DirectoryNotFoundException(string.Format("Could not find CAB output directory: {0}", outputDirectory));
            }
            if (Path.GetExtension(outputAbsoluteFilePath).ToLower() != ".cab")
            {
                throw new ArgumentException(string.Format("Output file {0} must be a CAB file with the cab extension.", outputAbsoluteFilePath));
            }
            FileSystemHelper.ValidateFileExists(cabWizAbsoluteFilePath);
            if (File.Exists(outputAbsoluteFilePath))
            {
                File.Delete(outputAbsoluteFilePath);
            }
            if (File.Exists(cabWizLogFileAbsoluteFilePath))
            {
                File.Delete(cabWizLogFileAbsoluteFilePath);
            }
            cabInfo.SaveINFFile(cabWizINFAbsoluteFilePath);
            string cabWizCommandLine = string.Format(
                "\"{0}\" /dest \"{1}\" /err {2}",
                cabWizINFAbsoluteFilePath,
                outputDirectory,
                CAB_WIZ_LOG_FILE_NAME);
            using (Process p = new Process())
            {
                p.StartInfo = new ProcessStartInfo(cabWizAbsoluteFilePath, cabWizCommandLine)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                p.Start();
                p.WaitForExit(60000); //Wait for one minute.
                if (!p.HasExited)
                {
                    p.Kill();
                }
                p.Close();
            }
            if (!File.Exists(cabWizLogFileAbsoluteFilePath))
            {
                cabWizLogFileContents = string.Format("Expected log file {0} was not generated.", cabWizLogFileAbsoluteFilePath);
            }
            else
            {
                cabWizLogFileContents = File.ReadAllText(cabWizLogFileAbsoluteFilePath);
            }
            if (!File.Exists(outputAbsoluteFilePath))
            {
                return false;
            }
            if (deleteINFAfterBuild)
            {
                File.Delete(cabWizINFAbsoluteFilePath);
            }
            return true;
        }

        #endregion //Methods
    }
}