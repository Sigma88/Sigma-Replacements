using UnityEngine;


namespace SigmaReplacements
{
    namespace SkyBox
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, true)]
        class SettingsLoader : MonoBehaviour
        {
            void Start()
            {
                // User Settings
                ConfigNode[] InfoNodes = UserSettings.ConfigNode.GetNodes("SkyBox");

                for (int i = 0; i < InfoNodes?.Length; i++)
                {
                    ConfigNode[] requirements = InfoNodes[i].GetNodes("Requirements");
                    ConfigNode[] heads = InfoNodes[i].GetNodes("CubeMap");

                    if (requirements.Length == 0)
                        requirements = new[] { new ConfigNode() };

                    for (int j = 0; j < requirements.Length; j++)
                    {
                        for (int k = 0; k < heads.Length; k++)
                        {
                            SkyBoxInfo.List.Add(new SkyBoxInfo(requirements[j], heads[k]));
                        }
                    }
                }

                if (SkyBoxInfo.List?.Count > 0) SkyBoxInfo.DataBase = SkyBoxInfo.List.Order();
            }
        }
    }
}
