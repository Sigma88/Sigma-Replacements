using System;
using System.Collections.Generic;
using UnityEngine;


namespace SigmaReplacements
{
    internal static class Extensions
    {
        internal static void Load(this CrewMember[] array, ConfigNode node, int index)
        {
            CrewMember kerbal = array[index];
            Info stats = new Info(node.GetNode("Stats") ?? new ConfigNode(), new ConfigNode());

            array[index] = new CrewMember
            (
                stats?.rosterStatus ?? kerbal.type,
                !string.IsNullOrEmpty(stats?.name) ? stats.name : kerbal.name,
                stats?.gender ?? kerbal.gender,
                stats?.trait?.Length > 0 && !string.IsNullOrEmpty(stats.trait[0]) ? stats.trait[0] : kerbal.trait,
                stats?.veteran ?? kerbal.veteran,
                stats?.isBadass ?? kerbal.isBadass,
                stats?.courage ?? kerbal.courage,
                stats?.stupidity ?? kerbal.stupidity,
                stats?.experienceLevel ?? kerbal.experienceLevel
            );
        }
    }
}
