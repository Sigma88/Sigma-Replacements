using UnityEngine;


namespace SigmaReplacements
{
    namespace Textures
    {
        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class Version : MonoBehaviour
        {
            public static readonly System.Version number = new System.Version("0.2.0");

            void Awake()
            {
                UnityEngine.Debug.Log("[SigmaLog] Version Check:   Sigma Replacements: Textures v" + number);
            }
        }
    }
}
