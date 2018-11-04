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
        private static Dictionary<string, string> pairs = new Dictionary<string, string>();

        static Datastorage()
        {
            //Load Data
            if(!ValidateStorageFile("Datastorage.json")) return;
            string json = File.ReadAllText("Datastorage.json");
            pairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static void SaveData()
        {
            //Save Data
            string json = JsonConvert.SerializeObject(pairs, Formatting.Indented);
            File.WriteAllText("Datastorage.json", json);
        }

        private static bool ValidateStorageFile(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveData();
                return false;
            }

            return true;

        }

        public static void AddPairToStorage(string key, string value)
        {
            pairs.Add(key, value);
            SaveData();
        }

        public static int GetPairsCount()
        {
            return pairs.Count;
        }
    }
}
