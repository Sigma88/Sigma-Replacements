﻿using UnityEngine;


namespace SigmaReplacements
{
    namespace Suits
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
                    ConfigNode[] suits = InfoNodes[i].GetNodes("Suit");

                    if (requirements.Length == 0)
                        requirements = new[] { new ConfigNode() };

                    for (int j = 0; j < requirements.Length; j++)
                    {
                        for (int k = 0; k < suits.Length; k++)
                        {
                            SuitInfo.List.Add(new SuitInfo(requirements[j], suits[k]));
                        }
                    }
                }

                if (SuitInfo.List?.Count > 0) SuitInfo.DataBase = SuitInfo.List.Order();
            }
        }
    }
}
