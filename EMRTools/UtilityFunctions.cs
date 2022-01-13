using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;

namespace EMRTools
{
    public static class UtilityFunctions
    {
        public static string GetConnString()
        {
            string versionDependent = Application.UserAppDataRegistry.Name;
            string versionIndependent = versionDependent.Substring(0, versionDependent.LastIndexOf("\\"));

            string ConnectionString = String.Empty;
            if (Registry.GetValue(versionIndependent, "ConnectionString", null) != null)
            {
                ConnectionString = Registry.GetValue(versionIndependent, "ConnectionString", null).ToString();
                return ConnectionString;
            }
            else
            {
                return ConnectionString;
            }
        }

        public static string GetSMSUsername()
        {
            string versionDependent = Application.UserAppDataRegistry.Name;
            string versionIndependent = versionDependent.Substring(0, versionDependent.LastIndexOf("\\"));

            string ConnectionString = String.Empty;
            if (Registry.GetValue(versionIndependent, "sms_username", null) != null)
            {
                ConnectionString = Registry.GetValue(versionIndependent, "sms_username", null).ToString();
                return ConnectionString;
            }
            else
            {
                return ConnectionString;
            }
        }

        public static string GetSMSAPIKey()
        {
            string versionDependent = Application.UserAppDataRegistry.Name;
            string versionIndependent = versionDependent.Substring(0, versionDependent.LastIndexOf("\\"));

            string ConnectionString = String.Empty;
            if (Registry.GetValue(versionIndependent, "sms_apikey", null) != null)
            {
                ConnectionString = Registry.GetValue(versionIndependent, "sms_apikey", null).ToString();
                return ConnectionString;
            }
            else
            {
                return ConnectionString;
            }
        }
    }
}
