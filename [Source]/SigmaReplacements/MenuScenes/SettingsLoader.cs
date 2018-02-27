using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
                    MunSceneInfo info = new MunSceneInfo(InfoNodes[i]);
                    AddUnique(MunSceneInfo.DataBase, info);
                }

                // Add Stock MunScene
                AddUnique(MunSceneInfo.DataBase, new MunSceneInfo("MunScene"));

                // Removed Non-Enabled
                MunSceneInfo.DataBase.RemoveAll(i => !i.enabled);
            }

            static void AddUnique(List<MenuSceneInfo> list, MenuSceneInfo info)
            {
                if (string.IsNullOrEmpty(info.name)) return;
                if (list.Any(i => i.name == info.name)) return;
                list.Add(info);
            }
        }
    }
}
