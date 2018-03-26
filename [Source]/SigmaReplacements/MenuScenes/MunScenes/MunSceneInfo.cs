using System.Collections.Generic;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class MunSceneInfo : MenuSceneInfo
        {
            // Static
            internal static List<MenuSceneInfo> DataBase = new List<MenuSceneInfo>();

            // Sky
            internal ConfigNode atmosphere = null;
            internal ConfigNode[] bodies = null;

            // Terrain
            internal ConfigNode terrain = null;

            // Scatter
            internal ConfigNode[] scatter = null;

            // ShipWreck
            internal ConfigNode wreck = null;
            internal ConfigNode ground = null;

            // Kerbals
            internal ConfigNode[] kerbals = null;

            // New MenuScenesInfo From Config
            internal MunSceneInfo(ConfigNode info)
            {
                // Identifiers
                name = info?.GetValue("name");

                if (string.IsNullOrEmpty(name)) return;

                // Requirements
                if (!bool.TryParse(info.GetValue("enabled"), out enabled))
                {
                    enabled = true;
                }

                if (!enabled) return;

                // Sky
                atmosphere = info?.GetNode("SKY")?.GetNode("ATMOSPHERE");
                bodies = info?.GetNode("SKY")?.GetNodes("BODY");

                // Terrain
                terrain = info?.GetNode("TERRAIN");

                // Scatter
                scatter = info?.GetNode("SCATTER")?.GetNodes("OBJECT");

                // ShipWreck
                wreck = info?.GetNode("SHIPWRECK")?.GetNode("SHIP");
                ground = info?.GetNode("SHIPWRECK")?.GetNode("GROUND");

                // Kerbals
                kerbals = info?.GetNode("KERBALS")?.GetNodes("KERBAL");
            }

            // New MenuScenesInfo From Name
            internal MunSceneInfo(string name) : base(name)
            {
            }
        }
    }
}
