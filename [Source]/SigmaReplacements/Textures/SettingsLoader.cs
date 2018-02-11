using UnityEngine;


namespace SigmaReplacements
{
    namespace Textures
    {
        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        class FirstSettingsLoader : MonoBehaviour
        {
            void OnDestroy()
            {
                // User Settings
                ConfigNode[] InfoNodes = UserSettings.ConfigNode.GetNodes("Texture");

                Debug.Log("SettingsLoader", "Texture nodes found = " + (InfoNodes?.Length ?? 0));

                if (!(InfoNodes?.Length > 0)) return;

                TextureInfo info = new TextureInfo(InfoNodes);
            }
        }

        [KSPAddon(KSPAddon.Startup.MainMenu, true)]
        class SecondSettingsLoader : MonoBehaviour
        {
            void Start()
            {
                // User Settings
                ConfigNode[] InfoNodes = UserSettings.ConfigNode.GetNodes("Texture");

                Debug.Log("SettingsLoader", "Texture nodes found = " + (InfoNodes?.Length ?? 0));

                if (!(InfoNodes?.Length > 0)) return;

                TextureInfo info = new TextureInfo(InfoNodes);
            }
        }
    }
}
