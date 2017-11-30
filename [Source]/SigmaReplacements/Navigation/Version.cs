using UnityEngine;


namespace SigmaReplacements
{
    namespace Navigation
    {
        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        public class Version : MonoBehaviour
        {
            public static readonly System.Version number = new System.Version("0.1.2");

            void Awake()
            {
                UnityEngine.Debug.Log("[SigmaLog] Version Check:   Sigma Replacements: Navigation v" + number);
            }
        }
    }
}
