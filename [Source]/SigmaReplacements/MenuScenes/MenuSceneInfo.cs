using System.Collections.Generic;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class MenuSceneInfo
        {
            // Static
            internal static List<MenuSceneInfo> DataBase = new List<MenuSceneInfo>();

            // Requirements
            internal string name = "";
            internal bool enabled = true;

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

            // Get
            internal MenuSceneInfo GetScene()
            {
                return null;
            }

            // New MenuScenesInfo From Config
            internal MenuSceneInfo(ConfigNode info)
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
            internal MenuSceneInfo(string name)
            {
                this.name = name;
            }
        }
    }
}
