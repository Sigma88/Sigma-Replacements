using UnityEngine;


namespace SigmaReplacements
{
    namespace Heads
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, true)]
        internal class SettingsLoader : MonoBehaviour
        {
            void Start()
            {
                // User Settings
                ConfigNode[] InfoNodes = UserSettings.ConfigNode.GetNodes("Kerbal");

                for (int i = 0; i < InfoNodes?.Length; i++)
                {
                    ConfigNode[] requirements = InfoNodes[i].GetNodes("Requirements");
                    ConfigNode[] info = InfoNodes[i].GetNodes("Head");

                    if (requirements.Length == 0)
                        requirements = new[] { new ConfigNode() };

                    for (int j = 0; j < requirements.Length; j++)
                    {
                        for (int k = 0; k < info.Length; k++)
                        {
                            HeadInfo.List.Add(new HeadInfo(requirements[j], info[k]));
                        }
                    }
                }

                if (HeadInfo.List?.Count > 0) HeadInfo.DataBase = HeadInfo.List.Order();
            }
        }
    }
}
