using UnityEngine;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class Version : MonoBehaviour
        {
            public static readonly System.Version number = new System.Version("0.3.0");

            void Awake()
            {
                UnityEngine.Debug.Log("[SigmaLog] Version Check:   Sigma Replacements: Descriptions v" + number);
            }
        }
    }
}
