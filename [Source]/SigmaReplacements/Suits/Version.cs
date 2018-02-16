using UnityEngine;


namespace SigmaReplacements
{
    namespace Suits
    {
        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class Version : MonoBehaviour
        {
            public static readonly System.Version number = new System.Version("0.2.3");

            void Awake()
            {
                UnityEngine.Debug.Log("[SigmaLog] Version Check:   Sigma Replacements: Suits v" + number);
            }
        }
    }
}
