using UnityEngine;


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
}
