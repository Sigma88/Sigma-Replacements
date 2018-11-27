using UnityEngine;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        internal class AstronautComplexFix : MonoBehaviour
        {
            void Start()
            {
                Events.onAstronautComplexEnter.Fire();
            }
        }
    }
}
