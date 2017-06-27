namespace Figlut.Mobile.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Reflection;
    using System.Drawing;
    using System.Linq;
    using PsionTeklogix.SystemPTX;
    using Figlut.Mobile.Toolkit.Data;
    using System.Runtime.InteropServices;

    #endregion //Using Directives

    /// <summary>
    /// A helper class helps in retrieving system information.
    /// </summary>
    public class Information
    {
        #region Methods

        #region Windows Mobile

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/ms893522.aspx
        /// </summary>
        /// <param name="appdata">[in] Application-provided data, which is hashed with the device identifier.
        /// As long as the application provides the same data as this input, the same hash 
        /// will always be returned on the same device, even if the device is re-flashed and/
        /// or cold-booted. This application data must have a minimum length of 8 bytes and 
        /// should be unique to the application.</param>
        /// 
        /// <param name="cbApplictionData">[in] Length of the application data, pbApplicationData.
        /// This parameter must be at least 8 bytes or the function call will fail.</param>
        /// 
        /// <param name="dwDeviceIDVersion">[in] Version number of the algorithm used to calculate 
        /// the device identifier returned from GetDeviceUniqueID. Currently, the only defined 
        /// version number is 1. This parameter must use this value or the function call will fail.</param>
        /// 
        /// <param name="deviceIDOuput">[out] Application-provided output buffer for the device identifier. 
        /// This buffer should be at least equal to GETDEVICEUNIQUEID_VI_OUTPUT (20 bytes). If the provided 
        /// buffer is smaller than the complete hash, the hash will be truncated and only the bytes that fit 
        /// will be copied into the output buffer. If the buffer is not large enough, GetDeviceUniqueID 
        /// truncates the output data to fit the buffer, and then returns HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER).</param>
        /// 
        /// <param name="pcbDeviceIDOutput">[in, out] Specifies the length of the device identifier. The input parameter 
        /// is the length of the application-supplied buffer. The output parameter is the number of bytes written 
        /// into the output buffer. If this pointer is equal to NULL, the call to GetDeviceUniqueID will fail.</param>
        /// <returns></returns>
        [DllImport("coredll.dll")]
        private extern static int GetDeviceUniqueID([In, Out] byte[] appdata,
                                                    int cbApplictionData,
                                                    int dwDeviceIDVersion,
                                                    [In, Out] byte[] deviceIDOuput,
                                                    out uint pcbDeviceIDOutput);

        public static string GetDeviceIdBase64String(string appString)
        {
            byte[] appData = new byte[appString.Length];
            for (int i = 0; i < appString.Length; i++)
            {
                appData[i] = (byte)appString[i];
            }
            int appDataSize = appData.Length;
            byte[] deviceOutput = new byte[20]; //Device Unique Id
            uint sizeOut = 20;
            GetDeviceUniqueID(appData, appDataSize, 1, deviceOutput, out sizeOut);
            return Convert.ToBase64String(deviceOutput);
        }

        #endregion //Windows Mobile

        #region Windows CE

        private static Int32 FILE_DEVICE_HAL = 0x00000101;
        private static Int32 FILE_ANY_ACCESS = 0x0;
        private static Int32 METHOD_BUFFERED = 0x0;

        private static Int32 IOCTL_HAL_GET_DEVICEID =
            ((FILE_DEVICE_HAL) << 16) | ((FILE_ANY_ACCESS) << 14)
            | ((21) << 2) | (METHOD_BUFFERED);

        [DllImport("coredll.dll")]
        private static extern bool KernelIoControl(Int32 IoControlCode, IntPtr
          InputBuffer, Int32 InputBufferSize, byte[] OutputBuffer, Int32
          OutputBufferSize, ref Int32 BytesReturned);

        public static string GetCEDeviceID()
        {
            byte[] OutputBuffer = new byte[256];
            Int32 OutputBufferSize, BytesReturned;
            OutputBufferSize = OutputBuffer.Length;
            BytesReturned = 0;

            // Call KernelIoControl passing the previously defined
            // IOCTL_HAL_GET_DEVICEID parameter
            // We don’t need to pass any input buffers to this call
            // so InputBuffer and InputBufferSize are set to their null
            // values
            bool retVal = KernelIoControl(IOCTL_HAL_GET_DEVICEID,
                    IntPtr.Zero,
                    0,
                    OutputBuffer,
                    OutputBufferSize,
                    ref BytesReturned);

            // If the request failed, exit the method now
            if (retVal == false)
            {
                return null;
            }

            // Examine the OutputBuffer byte array to find the start of the 
            // Preset ID and Platform ID, as well as the size of the
            // PlatformID. 
            // PresetIDOffset – The number of bytes the preset ID is offset
            //                  from the beginning of the structure
            // PlatformIDOffset - The number of bytes the platform ID is
            //                    offset from the beginning of the structure
            // PlatformIDSize - The number of bytes used to store the
            //                  platform ID
            // Use BitConverter.ToInt32() to convert from byte[] to int
            Int32 PresetIDOffset = BitConverter.ToInt32(OutputBuffer, 4);
            Int32 PlatformIDOffset = BitConverter.ToInt32(OutputBuffer, 0xc);
            Int32 PlatformIDSize = BitConverter.ToInt32(OutputBuffer, 0x10);

            // Convert the Preset ID segments into a string so they can be 
            // displayed easily.
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("{0:X8}-{1:X4}-{2:X4}-{3:X4}-",
                 BitConverter.ToInt32(OutputBuffer, PresetIDOffset),
                 BitConverter.ToInt16(OutputBuffer, PresetIDOffset + 4),
                 BitConverter.ToInt16(OutputBuffer, PresetIDOffset + 6),
                 BitConverter.ToInt16(OutputBuffer, PresetIDOffset + 8)));

            // Break the Platform ID down into 2-digit hexadecimal numbers
            // and append them to the Preset ID. This will result in a 
            // string-formatted Device ID
            for (int i = PlatformIDOffset;
                 i < PlatformIDOffset + PlatformIDSize;
                 i++)
            {
                sb.Append(String.Format("{0:X2}", OutputBuffer[i]));
            }

            // return the Device ID string
            return sb.ToString();
        }

        #endregion //Windows CE

        /// <summary>
        /// Gets the Figlut Device Id
        /// </summary>
        /// <returns>The Figlut device ID</returns>
        public static string GetPsionDeviceId()
        {
            if (string.IsNullOrEmpty(SystemInformation.MachineUID))
            {
                throw new Exception("This is not a Psion device.");
            }
            return SystemInformation.MachineUID;
        }

        /// <summary>
        /// Gets the executing directory of the current application.
        /// </summary>
        public static string GetExecutingDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        }

        /// <summary>
        /// Returns a dictionary of all the system colors with their names as the keys
        /// to the dictionary.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Color> GetSystemColors()
        {
            Type colorType = typeof(Color);
            Dictionary<string, Color> result = new Dictionary<string, Color>();
            foreach (PropertyInfo p in colorType.GetProperties().Where(p => p.PropertyType == colorType))
            {
                Color c = (Color)p.GetValue(null, null);
                result.Add(p.Name, c);
            }
            return result;
        }

        #endregion //Methods
    }
}