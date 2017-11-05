﻿using UnityEngine;


namespace SigmaReplacements
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    class SettingsLoader : MonoBehaviour
    {
        void Start()
        {
            // Debug Spam
            if (bool.TryParse(UserSettings.ConfigNode?.GetValue("debug"), out bool debug) && debug) Debug.debug = true;
        }
    }

    namespace Heads
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
                    ConfigNode head = InfoNodes[i].GetNode("Head");

                    if (requirements.Length == 0)
                        requirements = new[] { new ConfigNode() };

                    for (int j = 0; j < requirements.Length; j++)
                    {
                        HeadInfo.List.Add(new HeadInfo(requirements[j], head));
                    }
                }

                if (HeadInfo.List?.Count > 0)
                    HeadInfo.OrderDB();
            }
        }
    }
}