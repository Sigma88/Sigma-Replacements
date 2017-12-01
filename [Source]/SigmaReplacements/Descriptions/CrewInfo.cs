using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        class CrewInfo : Info
        {
            // Static
            internal static List<CrewInfo> List = new List<CrewInfo>();
            internal static List<CrewInfo> DataBase = new List<CrewInfo>();

            // CrewInfo Specific Requirements
            internal int? index = null;
            internal bool unique = false;
            internal bool last = false;

            // Define
            string displayName = null;
            string tooltipName = null;
            Texture sprite = null;
            string[] informations = new string[] { };


            // New CrewInfo
            internal CrewInfo(ConfigNode requirements, ConfigNode info) : base(requirements, info)
            {
                // Parse Requirements
                Parse(requirements, info);

                // CrewInfo Specific Requirements
                unique = Parse(requirements.GetValue("unique"), unique);
                last = Parse(requirements.GetValue("last"), last);
                index = Parse(requirements.GetValue("index"), index);

                // Parse Informations
                displayName = info.GetValue("displayName");
                tooltipName = info.GetValue("tooltipName");
                sprite = Parse(info.GetValue("sprite"), sprite);
                informations = info.GetValues("info");
            }

            internal static void OrderDB()
            {
                Debug.Log("CrewInfo.OrderDB", "Total Information Nodes Loaded = " + List.Count);
                List = List.Where(i => i.informations.Length > 0 || !string.IsNullOrEmpty(i.tooltipName) || !string.IsNullOrEmpty(i.displayName) || i.sprite != null).ToList();
                Debug.Log("CrewInfo.OrderDB", "Valid Information Nodes Loaded = " + List.Count);
                Debug.Log("CrewInfo.OrderDB", "Initial DataBase count = " + DataBase.Count);
                DataBase.AddRange(List.Where(i => i.name != null && i.index != null).OrderBy(i => i.index).ThenBy(i => i.useChance));
                Debug.Log("CrewInfo.OrderDB", "Added withName, withIndex to DataBase count = " + DataBase.Count);
                DataBase.AddRange(List.Where(i => i.name != null && i.index == null).OrderBy(i => i.index).ThenBy(i => i.useChance));
                Debug.Log("CrewInfo.OrderDB", "Added withName, noIndex to DataBase count = " + DataBase.Count);
                DataBase.AddRange(List.Where(i => i.name == null && i.index != null).OrderBy(i => i.index).ThenBy(i => i.useChance));
                Debug.Log("CrewInfo.OrderDB", "Added noName, withIndex to DataBase count = " + DataBase.Count);
                DataBase.AddRange(List.Where(i => i.name == null && i.index == null).OrderBy(i => i.index).ThenBy(i => i.useChance));
                Debug.Log("CrewInfo.OrderDB", "Added noName, noIndex to DataBase count = " + DataBase.Count);
            }
        }
    }
}
