using UnityEngine;
using KSP.UI.Screens;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
        internal class KSCTriggers : MonoBehaviour
        {
            void Start()
            {
                AstronautComplex AC = FindObjectOfType<ACSceneSpawner>()?.ACScreenPrefab?.canvas?.GetComponent<AstronautComplex>();
                AC?.Get_widgetApplicants()?.gameObject?.AddOrGetComponent<CustomDescription>();
                AC?.Get_widgetEnlisted()?.gameObject?.AddOrGetComponent<CustomDescription>();
                AC?.gameObject?.AddOrGetComponent<AstronautComplexFix>();
            }
        }
    }
}
