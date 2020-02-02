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
                // Orbit Scene Settings
                ConfigNode[] OrbitInfoNodes = UserSettings.ConfigNode?.GetNodes("OrbitScene");
                for (int i = 0; i < OrbitInfoNodes?.Length; i++)
                {
                    if (OrbitSceneInfo.DataBase == null)
                        OrbitSceneInfo.DataBase = new List<MenuSceneInfo>();

                    OrbitSceneInfo info = new OrbitSceneInfo(OrbitInfoNodes[i]);
                    AddUnique(OrbitSceneInfo.DataBase, info);
                }

                // Mun Scene Settings
                ConfigNode[] MunInfoNodes = UserSettings.ConfigNode?.GetNodes("MunScene");
                for (int i = 0; i < MunInfoNodes?.Length; i++)
                {
                    if (MunSceneInfo.DataBase == null)
                        MunSceneInfo.DataBase = new List<MenuSceneInfo>();

                    MunSceneInfo info = new MunSceneInfo(MunInfoNodes[i]);
                    AddUnique(MunSceneInfo.DataBase, info);
                }

                // Removed Non-Enabled
                if (OrbitSceneInfo.DataBase?.Count > 0)
                    OrbitSceneInfo.DataBase.RemoveAll(i => !i.enabled);
                if (MunSceneInfo.DataBase?.Count > 0)
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
