using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace KofCWSC.API.Utils
{
    public class Helper
    {
        public static string FormatLogEntry(object thisme, Exception ex,string addData = "")
        {
            //***********************************************************************************************
            // 12/05/2024 Tim Philomeno
            // trying to get a consistant logging string
            // usage: Log.Error(Helper.FormatLogEntry(this, ex));
            var method = new StackTrace(ex, true).GetFrame(0)?.GetMethod();
            var className = method?.DeclaringType?.FullName;
            DateTime date = DateTime.Now;
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return "API" + " - " + env + " - " + date + " - " + thisme.GetType().Name + " - in method " + method + " - in class " + className + ex.Message + " - " + ex.InnerException + " - ***" + addData + "*** - " + ex.StackTrace;
            //-----------------------------------------------------------------------------------------------
        }
        public static string GetIPAddress(string hostname)
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(hostname);

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    //System.Diagnostics.Debug.WriteLine("LocalIPadress: " + ip);
                    return ip.ToString();
                }
            }
            return string.Empty;
        }
        public static string FormatPhoneNumber(string? phoneNumber)
        {
            if (phoneNumber.IsNullOrEmpty())
            {
                return null;
                //return "(000) 000-0000";
            }
            else
            {
                // Remove any non-numeric characters
                string cleanedPhoneNumber = Regex.Replace(phoneNumber, @"\D", "");

                // Ensure the phone number has 10 digits
                if (cleanedPhoneNumber.Length == 10)
                {
                    // Format the phone number
                    string formattedPhoneNumber = $"({cleanedPhoneNumber.Substring(0, 3)}) {cleanedPhoneNumber.Substring(3, 3)}-{cleanedPhoneNumber.Substring(6, 4)}";
                    return formattedPhoneNumber;
                }
                else 
                {
                    // Return the original input if it doesn't have 10 digits
                    return phoneNumber;
                }
            }
        }
        public static string GetStateAbbr(string stateName)
        {
            if (stateName.Length == 2) { return stateName; }
            var states = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Alabama", "AL" },
                { "Alaska", "AK" },
                { "Arizona", "AZ" },
                { "Arkansas", "AR" },
                { "California", "CA" },
                { "Colorado", "CO" },
                { "Connecticut", "CT" },
                { "Delaware", "DE" },
                { "Florida", "FL" },
                { "Georgia", "GA" },
                { "Hawaii", "HI" },
                { "Idaho", "ID" },
                { "Illinois", "IL" },
                { "Indiana", "IN" },
                { "Iowa", "IA" },
                { "Kansas", "KS" },
                { "Kentucky", "KY" },
                { "Louisiana", "LA" },
                { "Maine", "ME" },
                { "Maryland", "MD" },
                { "Massachusetts", "MA" },
                { "Michigan", "MI" },
                { "Minnesota", "MN" },
                { "Mississippi", "MS" },
                { "Missouri", "MO" },
                { "Montana", "MT" },
                { "Nebraska", "NE" },
                { "Nevada", "NV" },
                { "New Hampshire", "NH" },
                { "New Jersey", "NJ" },
                { "New Mexico", "NM" },
                { "New York", "NY" },
                { "North Carolina", "NC" },
                { "North Dakota", "ND" },
                { "Ohio", "OH" },
                { "Oklahoma", "OK" },
                { "Oregon", "OR" },
                { "Pennsylvania", "PA" },
                { "Rhode Island", "RI" },
                { "South Carolina", "SC" },
                { "South Dakota", "SD" },
                { "Tennessee", "TN" },
                { "Texas", "TX" },
                { "Utah", "UT" },
                { "Vermont", "VT" },
                { "Virginia", "VA" },
                { "Washington", "WA" },
                { "West Virginia", "WV" },
                { "Wisconsin", "WI" },
                { "Wyoming", "WY" }
            };

            if (states.TryGetValue(stateName, out string abbreviation))
            {
                return abbreviation;
            }
            else
            {
                throw new ArgumentException($"State name '{stateName}' is not recognized.");
            }
        }
        public static string CUpLow(string instr)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(instr.ToLower());
        }
    }
}
