using UnityEngine;


namespace SigmaReplacements
{
    namespace Textures
    {
        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        public class Version : MonoBehaviour
        {
            public static readonly System.Version number = new System.Version("0.1.1");

            void Awake()
            {
                UnityEngine.Debug.Log("[SigmaLog] Version Check:   Sigma Replacements: Textures v" + number);
            }
        }
    }
}
