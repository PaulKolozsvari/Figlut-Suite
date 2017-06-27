namespace Figlut.Mobile.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Win32;

    #endregion //Using Directives

    public class WmConfig
    {
        #region Constants

        public const string SIM_TOOL_KIT_UI_REG_KEY_PATH = "SimToolkit.UI";
        public const string CLSID_REG_KEY_PATH = @"SimToolkit.UI\CLSID";
        public const string SIM_TOOL_KIT_KEY_DEFAULT_VALUE = "Toolkit UI";
        public const string CLSID_DEFAULT_VALUE = "{7B58F1D9-1C13-440f-894B-B90680570A2D}";

        #endregion //Constants

        #region Methods

        #region Sim Toolkit UI

        public static bool GetSimToolKitUIEnabled()
        {
            bool result;
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(CLSID_REG_KEY_PATH, true))
            {
                try
                {
                    result = key != null;
                }
                finally
                {
                    if (key != null)
                    {
                        key.Close();
                    }
                }
            }
            return result;
        }

        public static string DisableSimToolKitUI()
        {
            string result = string.Empty;
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(CLSID_REG_KEY_PATH, true))
            {
                try
                {
                    if (key == null)
                    {
                        throw new ArgumentException("Sim Tool Kit UI already disabled.");
                    }
                    result = key.GetValue("Default").ToString();
                }
                finally
                {
                    if (key != null)
                    {
                        key.Close();
                    }
                }
            }
            Registry.ClassesRoot.DeleteSubKey(CLSID_REG_KEY_PATH, true);
            return result;
        }

        public static void EnableSimToolKitUI()
        {
            EnableSimToolKitUI(null, null);
        }

        public static void EnableSimToolKitUI(string originalCLSIDDefaultValue)
        {
            EnableSimToolKitUI(null, originalCLSIDDefaultValue);
        }

        public static void EnableSimToolKitUI(string simToolkitUiKeyValue, string originalCLSIDDefaultValue)
        {
            if (string.IsNullOrEmpty(simToolkitUiKeyValue))
            {
                simToolkitUiKeyValue = SIM_TOOL_KIT_KEY_DEFAULT_VALUE;
            }
            if (string.IsNullOrEmpty(originalCLSIDDefaultValue))
            {
                originalCLSIDDefaultValue = CLSID_DEFAULT_VALUE;
            }
            if (GetSimToolKitUIEnabled())
            {
                throw new ArgumentException("Sim Tool Kit UI already enabled.");
            }
            using (RegistryKey simToolKitUiKey = Registry.ClassesRoot.CreateSubKey(SIM_TOOL_KIT_UI_REG_KEY_PATH))
            {
                try
                {
                    simToolKitUiKey.SetValue("Default", SIM_TOOL_KIT_KEY_DEFAULT_VALUE);
                }
                finally
                {
                    if (simToolKitUiKey != null)
                    {
                        simToolKitUiKey.Close();
                    }
                }
            }
            using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(CLSID_REG_KEY_PATH))
            {
                try
                {
                    key.SetValue("Default", CLSID_DEFAULT_VALUE, RegistryValueKind.String);
                }
                finally
                {
                    if (key != null)
                    {
                        key.Close();
                    }
                }
            }
        }

        #endregion //Sim Toolkit UI

        #region User Agent

        public const string POST_PLATFORM_REGISTRY_KEY_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\5.0\User Agent\Post Platform";
        public const string USER_AGENT_DEFAULT_VALUE = "FiglutUserAgentSetter";

        public static void SaveUserAgent(List<string> tokens)
        {
            SaveUserAgent(null, tokens);
        }

        /// <summary>
        /// Understanding User Agent Strings : http://msdn.microsoft.com/en-us/library/ms537503(v=vs.85).aspx
        /// </summary>
        /// <param name="userAgentValue"></param>
        /// <param name="tokens"></param>
        public static void SaveUserAgent(string userAgentValue, List<string> tokens)
        {
            try
            {
                if (string.IsNullOrEmpty(userAgentValue))
                {
                    userAgentValue = USER_AGENT_DEFAULT_VALUE;
                }
                RegistryKey key = Registry.LocalMachine.CreateSubKey(POST_PLATFORM_REGISTRY_KEY_PATH);
                string[] valueNames = key.GetValueNames();
                foreach (string vn in valueNames)
                {
                    if (tokens.Contains(vn))
                    {
                        tokens.Remove(vn);
                    }
                }
                foreach (string token in tokens)
                {
                    key.SetValue(token, userAgentValue, RegistryValueKind.String);
                }
                key.Close();
                tokens.Clear();
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, true, true);
            }
        }

        #endregion //User Agent

        #endregion //Methods
    }
}