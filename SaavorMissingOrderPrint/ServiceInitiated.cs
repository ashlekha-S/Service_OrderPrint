using SaavorMissingOrderPrint;
using SaavorMissingOrderPrint.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Timers;

namespace SaavorMissingOrderPrint
{
    /// <summary>
    /// ServiceInitiated
    /// </summary>
    public class ServiceInitiated
    {
        private readonly Timer _timer;
        //private ServiceHttpServer serviceHttpServer;

        /// <summary>
        /// ServiceInitiated
        /// </summary>
        public ServiceInitiated()
        {
            _timer = new Timer(60000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
            //serviceHttpServer = new ServiceHttpServer();
            //serviceHttpServer.Start("http://localhost:8080/");
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            DateTime utcdate = DateTime.UtcNow;
            DateTime istDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcdate,
                                TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

            string currentTime = Convert.ToDateTime(istDateTime).ToString("hh:mm tt");
            string filePath = Path.Combine(ConfigurationManager.AppSettings["LogFilePath"], DateTime.Now.ToString("MM-dd-yyyy") + "_Logs.txt");
            System.IO.File.AppendAllLines(filePath, new List<string> { string.Format("Service running in India Time {0} UTC Time {1} ", currentTime, utcdate.ToString()) });
            OrderService orderService = new OrderService();
            orderService.GetInvoke();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            //serviceHttpServer.Stop();
        }
    }
}

 
