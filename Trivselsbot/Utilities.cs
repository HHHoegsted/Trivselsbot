using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography.X509Certificates;


namespace Trivselsbot
{
    class Utilities
    {
        private static Dictionary<string, string> alerts;

        static Utilities()
        {
            string json = File.ReadAllText("Systemlang/alerts.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            alerts = data.ToObject<Dictionary<string, string>>();
        }

        public static string getAlert(string key)
        {
            if (alerts.ContainsKey(key))
                return alerts[key];
            return "";

        }

        public static string getFormattedAlert(string key, params object[] parameter)
        {
            if (alerts.ContainsKey(key))
            {
                return String.Format(alerts[key], parameter);
            }

            return "";
        }


    }
}
