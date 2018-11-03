using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Trivselsbot
{
    class Datastorage
    {
        public static Dictionary<string, string> pairs = new Dictionary<string, string>();

        static Datastorage()
        {
            //Load Data
            ValidateStorageFile("Datastorage.json");
            string json = File.ReadAllText("Datastorage.json");
            pairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static void SaveData()
        {
            //Save Data
        }

        private static void ValidateStorageFile(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveData();
            }

        }
    }
}
