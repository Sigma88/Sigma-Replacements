using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class Version : MonoBehaviour
        {
            public static readonly System.Version number = new System.Version("0.3.1");

            void Awake()
            {
                UnityEngine.Debug.Log("[SigmaLog] Version Check:   Sigma Replacements: MenuScenes v" + number);
            }
        }
    }
}
