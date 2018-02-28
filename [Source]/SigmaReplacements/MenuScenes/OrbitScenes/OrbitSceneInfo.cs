using System.Collections.Generic;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class OrbitSceneInfo : MenuSceneInfo
        {
            // Static
            internal static List<MenuSceneInfo> DataBase = null;

            // Bodies
            internal ConfigNode planet = null;
            internal ConfigNode[] moons = null;

            // Kerbals
            internal ConfigNode[] kerbals = null;

            // Scatters
            internal ConfigNode[] scatter = null;


            // New MenuScenesInfo From Config
            internal OrbitSceneInfo(ConfigNode info)
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

                // Bodies
                planet = info?.GetNode("BODIES")?.GetNode("PLANET");
                moons = info?.GetNode("BODIES")?.GetNodes("MOON");

                // Kerbals
                kerbals = info?.GetNode("KERBALS")?.GetNodes("KERBAL");

                // Scatter
                scatter = info?.GetNode("SCATTER")?.GetNodes("OBJECT");
            }
        }
    }
}
