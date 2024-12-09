using System.Net;

namespace KofCWSC.API.Utils
{
    public class Helper
    {
        public static string FormatLogEntry(object thisme, Exception ex)
        {
            //***********************************************************************************************
            // 12/05/2024 Tim Philomeno
            // trying to get a consistant logging string
            // usage: Log.Error(Helper.FormatLogEntry(this, ex));
            return thisme.GetType().Name + " - " + ex.Message + " - " + ex.InnerException;
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
    }
}
