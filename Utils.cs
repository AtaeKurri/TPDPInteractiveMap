using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.VisualBasic.FileIO;

namespace TPDPInteractiveMap
{
    public class Utils
    {
        public static string startupPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string startupResources = $"{startupPath}\\Resources";
        public static string configFile = $"{startupResources}\\config.json";
        public static string mapDataPath = $"{startupResources}\\mapDatas";

        public Utils(){}

        public List<MapData> readJson()
        {
            checkConfigFileExists();
            string fileName = configFile;
            string jsonString = File.ReadAllText(fileName);
            List<MapData> mapData = JsonSerializer.Deserialize<List<MapData>>(jsonString);
            return mapData;
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
                ID = index,
                Encounters = new Dictionary<string, EncounterInfo>
                {
                    ["Encounter1"] = new EncounterInfo { id = "0", level = "0", style = "0", weight="0" },
                    ["Encounter2"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" },
                    ["Encounter3"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" },
                    ["Encounter4"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" },
                    ["Encounter5"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" },
                    ["Encounter6"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" },
                    ["Encounter7"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" },
                    ["Encounter8"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" },
                    ["Encounter9"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" },
                    ["Encounter10"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" }
                },
                SpeEncounters = new Dictionary<string, EncounterInfo>
                {
                    ["SpeEncounter1"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" },
                    ["SpeEncounter2"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" },
                    ["SpeEncounter3"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" },
                    ["SpeEncounter4"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" },
                    ["SpeEncounter5"] = new EncounterInfo { id = "0", level = "0", style = "0", weight = "0" }
                }
            };
            return locationInfo;
        }

        public static void copyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", System.IO.SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", System.IO.SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        public static void createConfigurationFile(string path)
        {
            List<MapData> mapDatas = new List<MapData>();
            using (TextFieldParser parser = new TextFieldParser($"{startupResources}\\MapToTownMapData.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadLine();
                while (!parser.EndOfData)
                {
                    string pathToJson;
                    string[] fields = parser.ReadFields();
                    pathToJson = $"{mapDataPath}\\{string.Format("{0:D3}", Convert.ToInt32(fields[0]))}\\{string.Format("{0:D3}", Convert.ToInt32(fields[0]))}.json";
                    string pathToJsonDir = $"{mapDataPath}\\{string.Format("{0:D3}", Convert.ToInt32(fields[0]))}";
                    if (Directory.Exists(pathToJsonDir))
                    {
                        if (File.Exists(pathToJson))
                        {
                            if (!mapDatas.Any(n => n.location_name == fields[1]))
                            {
                                Console.WriteLine(pathToJsonDir);
                                string jsonString = File.ReadAllText(pathToJson);
                                mapDatas.Add(JsonSerializer.Deserialize<MapData>(jsonString));
                            }
                        }
                    }
                }
            }

            using (FileStream file = new FileStream(configFile, FileMode.OpenOrCreate))
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(mapDatas, options);
                byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
                file.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
