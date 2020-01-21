using UnityEngine;


namespace SigmaReplacements
{
    namespace SkyBox
    {
        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class Version : MonoBehaviour
        {
            public static readonly System.Version number = new System.Version("0.5.1");

            void Awake()
            {
                UnityEngine.Debug.Log("[SigmaLog] Version Check:   Sigma Replacements: SkyBox v" + number);
            }
        }
    }
}
