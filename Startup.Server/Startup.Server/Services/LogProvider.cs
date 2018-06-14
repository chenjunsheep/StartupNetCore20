using System;

namespace Startp.Server.Services
{
    public class LogProvider
    {
        public static void Log(string msg)
        {
            try
            {
                ////for testing purpose
                //var log = new CountryDataLog();
                //log.LstUpdt = DateTime.Now;
                //log.Comment = msg;
                //log.Create();
            }
            catch (Exception) { }
        }
    }
}
