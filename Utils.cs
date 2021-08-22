using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace TPDPInteractiveMap
{
    public class Utils
    {
        public static string startupPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string startupResources = $"{startupPath}\\Resources";
        public static string configFile = $"{startupResources}\\config.json";

        public Utils(){}

        public List<LocationInfo> readJson()
        {
            checkConfigFileExists();
            string fileName = configFile;
            string jsonString = File.ReadAllText(fileName);
            List<LocationInfo> locationInfo = JsonSerializer.Deserialize<List<LocationInfo>>(jsonString);
            return locationInfo;
        }

        public static void checkConfigFileExists()
        {
            if (!File.Exists(configFile))
            {
                using (FileStream fs = File.Create(configFile))
                {
                    List<LocationInfo> infos = new List<LocationInfo>();

                    infos.Add(createLocationInfo("0", 0));
                    infos.Add(createLocationInfo("1", 1));

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(infos, options);
                    byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public static LocationInfo createLocationInfo(string name, int index)
        {
            LocationInfo locationInfo = new LocationInfo
            {
                Name = name,
                Index = index,
                Encounters = new Dictionary<string, EncounterInfo>
                {
                    ["Encounter1"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["Encounter2"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["Encounter3"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["Encounter4"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["Encounter5"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["Encounter6"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["Encounter7"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["Encounter8"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["Encounter9"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["Encounter10"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" }
                },
                SpeEncounters = new Dictionary<string, EncounterInfo>
                {
                    ["SpeEncounter1"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["SpeEncounter2"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["SpeEncounter3"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["SpeEncounter4"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" },
                    ["SpeEncounter5"] = new EncounterInfo { Id = 0, Level = 0, Style = "Normal" }
                }
            };
            return locationInfo;
        }
    }
}
