using UnityEngine;


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
                    ConfigNode Suit = InfoNodes[i].GetNode("Suit");

                    if (requirements.Length == 0)
                        requirements = new[] { new ConfigNode() };

                    for (int j = 0; j < requirements.Length; j++)
                    {
                        SuitInfo.List.Add(new SuitInfo(requirements[j], Suit));
                    }
                }

                if (SuitInfo.List?.Count > 0) SuitInfo.DataBase = SuitInfo.List.Order();
            }
        }
    }
}
