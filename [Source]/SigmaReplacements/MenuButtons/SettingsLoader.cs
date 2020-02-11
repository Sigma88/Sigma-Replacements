using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuButtons
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, true)]
        internal class SettingsLoader : MonoBehaviour
        {
            void Start()
            {
                if (!Nyan.nyan || Debug.debug)
                {
                    // User Settings
                    ConfigNode[] InfoNodes = UserSettings.ConfigNode.GetNodes("MenuButton");

                    for (int i = 0; i < InfoNodes?.Length; i++)
                    {
                        MenuButtonInfo.List.Add(new MenuButtonInfo(InfoNodes[i]));
                    }

                    if (MenuButtonInfo.List?.Count > 0)
                    {
                        MenuButtonInfo.DataBase = MenuButtonInfo.List.Order();
                    }
                }
            }
        }
    }
}
