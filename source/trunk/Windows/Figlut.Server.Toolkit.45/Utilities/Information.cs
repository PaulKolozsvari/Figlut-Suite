namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Reflection;
    using System.Drawing;
    using System.Linq;
    using Figlut.Server.Toolkit.Data;
    using System.Collections;
    using Microsoft.Win32;
    using System.Threading;
    using System.Management;

    #endregion //Using Directives

    /// <summary>
    /// A helper class helps in retrieving system information.
    /// </summary>
    public class Information
    {
        #region Methods

        /// <summary>
        /// Gets the executing directory of the current application.
        /// </summary>
        public static string GetExecutingDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6);
        }


        //Gets the name of the current executing assembly.
        public static string GetExecutingAssemblyName()
        {
            return Path.GetFileName(Assembly.GetCallingAssembly().GetName().CodeBase).Remove(0, 6);
        }

        //Gets the name of the current executing assembly.
        public static string GetEntryAssemblyVersion()
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            return $"{version}";
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

        public static string GetWindowsDomainAndMachineName()
        {
            string machinerDomainName = GetWindowsMachineDomainName(Environment.MachineName);
            return string.Format(@"{0}\{1}", machinerDomainName, Environment.MachineName);
        }

        internal static string GetWindowsMachineDomainName(string machineName)
        {
            try
            {
                ManagementObject cs = null;
                string key = $"Win32_ComputerSystem.Name='{machineName}'";
                string result = null;
                using (cs = new ManagementObject(key))
                {
                    cs.Get();
                    result = cs["domain"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not get the domain name to which the machine {machineName} belongs to: {ex.Message}.", ex);
            }
        }

        public string GetWindowsMachineId()
        {
            return DecodeProductKey(GetRegistryDigitalProductId(Key.Windows));
        }

        public enum Key { Windows };
        private static byte[] GetRegistryDigitalProductId(Key key)
        {
            byte[] digitalProductId = null;
            RegistryKey registry = null;
            switch (key)
            {
                // Open the Windows subkey readonly.
                case Key.Windows:
                    registry =
                      Registry.LocalMachine.
                        OpenSubKey(
                          @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                            false);
                    break;
            }
            if (registry != null)
            {
                // TODO: For other products, key name maybe different.
                digitalProductId = registry.GetValue("DigitalProductId")
                  as byte[];
                registry.Close();
            }
            return digitalProductId;
        }

        private static string DecodeProductKey(byte[] digitalProductId)
        {
            // Offset of first byte of encoded product key in 
            //  'DigitalProductIdxxx" REG_BINARY value. Offset = 34H.
            const int keyStartIndex = 52;
            // Offset of last byte of encoded product key in 
            //  'DigitalProductIdxxx" REG_BINARY value. Offset = 43H.
            const int keyEndIndex = keyStartIndex + 15;
            // Possible alpha-numeric characters in product key.
            char[] digits = new char[]
              {
                'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'M', 'P', 'Q', 'R', 
                'T', 'V', 'W', 'X', 'Y', '2', '3', '4', '6', '7', '8', '9',
              };
            // Length of decoded product key
            const int decodeLength = 29;
            // Length of decoded product key in byte-form.
            // Each byte represents 2 chars.
            const int decodeStringLength = 15;
            // Array of containing the decoded product key.
            char[] decodedChars = new char[decodeLength];
            // Extract byte 52 to 67 inclusive.
            ArrayList hexPid = new ArrayList();
            for (int i = keyStartIndex; i <= keyEndIndex; i++)
            {
                hexPid.Add(digitalProductId[i]);
            }
            for (int i = decodeLength - 1; i >= 0; i--)
            {
                // Every sixth char is a separator.
                if ((i + 1) % 6 == 0)
                {
                    decodedChars[i] = '-';
                }
                else
                {
                    // Do the actual decoding.
                    int digitMapIndex = 0;
                    for (int j = decodeStringLength - 1; j >= 0; j--)
                    {
                        int byteValue = (digitMapIndex << 8) | (byte)hexPid[j];
                        hexPid[j] = (byte)(byteValue / 24);
                        digitMapIndex = byteValue % 24;
                        decodedChars[i] = digits[digitMapIndex];
                    }
                }
            }
            return new string(decodedChars);
        }

        #endregion //Methods
    }
}