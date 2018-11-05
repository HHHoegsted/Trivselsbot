using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;


namespace Trivselsbot
{
    class Utilities
    {
        private static Dictionary<string, string> alerts;
        internal static List<string> profanity = new List<string>(){"fuck"};

        static Utilities()
        {
            string json = File.ReadAllText(Global.pathAlerts);
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            alerts = data.ToObject<Dictionary<string, string>>();

            if (File.Exists(Global.pathProfanity))
            {
                json = File.ReadAllText(Global.pathProfanity);
                var profanitydata = JsonConvert.DeserializeObject<dynamic>(json);
                profanity = profanitydata.ToObject<List<string>>();
            }
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

        public static string formatterDato(uint day, uint month)
        {
            string result = day.ToString();
            switch (month)
            {
                case 1: result += ". januar";
                    break;
                case 2: result += ". februar";
                    break;
                case 3: result += ". marts";
                    break;
                case 4: result += ". april";
                    break;
                case 5: result += ". maj";
                    break;
                case 6: result += ". juni";
                    break;
                case 7: result += ". juli";
                    break;
                case 8: result += ". august";
                    break;
                case 9: result += ". september";
                    break;
                case 10: result += ". oktober";
                    break;
                case 11: result += ". november";
                    break;
                default: result += ". december";
                    break;
             
            }

            return result;
        }



    }
}
