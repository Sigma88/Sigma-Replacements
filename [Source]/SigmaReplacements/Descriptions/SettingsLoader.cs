using UnityEngine;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, true)]
        class SettingsLoader : MonoBehaviour
        {
            void Start()
            {
                // User Settings
                ConfigNode[] InfoNodes = UserSettings.ConfigNode.GetNodes("Kerbal");

                for (int i = 0; i < InfoNodes?.Length; i++)
                {
                    ConfigNode[] requirements = InfoNodes[i].GetNodes("Requirements");
                    ConfigNode[] info = InfoNodes[i].GetNodes("Description");

                    if (requirements.Length == 0)
                        requirements = new[] { new ConfigNode() };

                    for (int j = 0; j < requirements.Length; j++)
                    {
                        for (int k = 0; k < info.Length; k++)
                        {
                            CrewInfo.List.Add(new CrewInfo(requirements[j], info[k]));
                        }
                    }
                }

                if (CrewInfo.List?.Count > 0) CrewInfo.OrderDB();
            }
        }
    }
}
