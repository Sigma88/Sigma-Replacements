using UnityEngine;


namespace SigmaReplacements
{
    namespace Navigation
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
                    ConfigNode[] info = InfoNodes[i].GetNodes("NavBall");

                    if (requirements.Length == 0)
                        requirements = new[] { new ConfigNode() };

                    for (int j = 0; j < requirements.Length; j++)
                    {
                        for (int k = 0; k < info.Length; k++)
                        {
                            NavBallInfo.List.Add(new NavBallInfo(requirements[j], info[k]));
                        }
                    }
                }

                if (NavBallInfo.List?.Count > 0) NavBallInfo.DataBase = NavBallInfo.List.Order();
            }
        }
    }
}
