using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPDPInteractiveMap
{
    public class LocationInfo
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public Dictionary<string, EncounterInfo> Encounters { get; set; }
        public Dictionary<string, EncounterInfo> SpeEncounters { get; set; }
    }

    public class EncounterInfo
    {
        public string id { get; set; }
        public string level { get; set; }
        public string style { get; set; }
        public string weight { get; set; }
    }

    public class MapData
    {
        public string location_name { get; set; }
        public string[] tilesets { get; set; }
        public string weather { get; set; }
        public string overworld_theme { get; set; }
        public string battle_background { get; set; }
        public string forbid_bike { get; set; }
        public string encounter_type { get; set; }
        public string unknown { get; set; }
        public string forbid_gap_map { get; set; }
        public EncounterInfo[] normal_encounters { get; set; }
        public EncounterInfo[] special_encounters { get; set; }
    }
}
