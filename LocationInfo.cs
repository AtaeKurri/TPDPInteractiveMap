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
        public int Index { get; set; }
        public Dictionary<string, EncounterInfo> Encounters { get; set; }
        public Dictionary<string, EncounterInfo> SpeEncounters { get; set; }
    }

    public class EncounterInfo
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string Style { get; set; }
    }
}
