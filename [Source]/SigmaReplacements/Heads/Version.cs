using UnityEngine;


namespace SigmaReplacements
{
    namespace Heads
    {
        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class Version : MonoBehaviour
        {
            public static readonly System.Version number = new System.Version("0.2.2");

            void Awake()
            {
                UnityEngine.Debug.Log("[SigmaLog] Version Check:   Sigma Replacements: Heads v" + number);
            }
        }
    }
}
