using UnityEngine;
using System.Linq;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, true)]
        internal class SettingsLoader : MonoBehaviour
        {
            void Awake()
            {
                // Mun Scene Settings
                ConfigNode[] InfoNodes = UserSettings.ConfigNode.GetNodes("MunScene");

                for (int i = 0; i < InfoNodes?.Length; i++)
                {
                    MenuSceneInfo info = new MenuSceneInfo(InfoNodes[i]);
                    AddUnique(info);
                }

                // Add Stock MunScene
                AddUnique(new MenuSceneInfo("MunScene"));

                // Removed Non-Enabled
                MenuSceneInfo.DataBase.RemoveAll(i => !i.enabled);
            }

            void AddUnique(MenuSceneInfo info)
            {
                if (string.IsNullOrEmpty(info.name)) return;
                if (MenuSceneInfo.DataBase.Any(i => i.name == info.name)) return;
                MenuSceneInfo.DataBase.Add(info);
            }
        }
    }
}
