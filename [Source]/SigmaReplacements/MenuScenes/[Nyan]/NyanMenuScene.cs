using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        internal class NyanMenu : MonoBehaviour
        {
            void Start()
            {
            }

            void Update()
            {
            }
        }

        [KSPAddon(KSPAddon.Startup.EveryScene, false)]
        internal class NyanMenuScene : MonoBehaviour
        {
            void Start()
            {
            }

            void OnDestroy()
            {
            }

            void UpdateBox()
            {
            }
        }
    }
}
