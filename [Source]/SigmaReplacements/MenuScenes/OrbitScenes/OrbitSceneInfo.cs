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

            // Scatters
            internal ConfigNode[] scatter = null;

            // Kerbals
            internal ConfigNode[] kerbals = null;

            // Lights
            internal ConfigNode[] lights = null;


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

                // Scatter
                scatter = info?.GetNode("SCATTER")?.GetNodes("OBJECT");

                // Kerbals
                kerbals = info?.GetNode("KERBALS")?.GetNodes("KERBAL");

                // Lights
                lights = info?.GetNode("LIGHTS")?.GetNodes("LIGHT");
            }
        }
    }
}
