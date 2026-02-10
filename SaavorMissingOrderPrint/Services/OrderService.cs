using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System;
using SaavorMissingOrderPrint.DBContext;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web.UI.WebControls;
using System.Security.Policy;

namespace SaavorMissingOrderPrint.Services
{
    /// <summary>
    /// OrderService
    /// </summary>
    public class OrderService
    {
        /// <summary>
        /// OrderService
        /// </summary>
        public OrderService() { }

        public void GetInvoke()
        {
            string filePath = Path.Combine(ConfigurationManager.AppSettings["LogFilePath"], DateTime.Now.ToString("MM-dd-yyyy") + "_Logs.txt");
            List<string> lines = new List<string>();
            lines.Add(string.Format("********* {0}: Saavor Missing Order Print Reader Status **************\n", DateTime.UtcNow.ToString()));
            string message = string.Empty;
            AppDBContext appDBContext = new AppDBContext();
            try
            {
                var orders = appDBContext.GetOrders(1);
                StringBuilder stringBuilder = new StringBuilder();
                string orderNumber = string.Empty;
                int profileId;
                string apiURL = string.Empty;
                foreach (DataRow row in orders.Rows)
                {
                    orderNumber = Convert.ToString(row["OrderNumber"]);
                    profileId = Convert.ToInt32(row["ProfileId"]);
                    lines.Add(string.Format("Process Order Number - {0}\n", orderNumber));
                    using (HttpClient client = new HttpClient())
                    {
                        apiURL = string.Format("{0}?orderNumber={1},{2}", Convert.ToString(ConfigurationManager.AppSettings["BroadCastURL"]), orderNumber, profileId);
                        lines = new List<string> { string.Format("API URL - {0}", apiURL) };
                        HttpResponseMessage response = client.GetAsync(apiURL).GetAwaiter().GetResult();
                        if (!response.IsSuccessStatusCode)
                        {
                            lines.Add(string.Format("Error - {0}\n", response.StatusCode));
                        }
                        else
                        {
                            lines.Add(string.Format("Success Order Number - {0}\n", orderNumber));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            lines.Add(string.Format("{0}\n************END**********\n", message));
            System.IO.File.AppendAllLines(filePath, lines);
        }

    }
}
