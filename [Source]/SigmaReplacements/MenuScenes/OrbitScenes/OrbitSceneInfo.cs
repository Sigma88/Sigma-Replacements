using System.Collections.Generic;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class OrbitSceneInfo : MenuSceneInfo
        {
            // Static
            internal static List<MenuSceneInfo> DataBase = new List<MenuSceneInfo>();

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
            }

            // New MenuScenesInfo From Name
            internal OrbitSceneInfo(string name) : base(name)
            {
            }
        }
    }
}
