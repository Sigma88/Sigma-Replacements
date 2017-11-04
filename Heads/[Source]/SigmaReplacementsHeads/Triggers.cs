using System.Linq;
using UnityEngine;
using KSP.UI.Screens.Flight;


namespace SigmaReplacements
{
    namespace Heads
    {
        [KSPAddon(KSPAddon.Startup.Flight, false)]
        class FlightTriggers : MonoBehaviour
        {
            void Start()
            {
                GameEvents.onCrewOnEva.Add(OnCrewOnEva);
                TimingManager.LateUpdateAdd(TimingManager.TimingStage.Normal, Add);
            }

            void OnCrewOnEva(GameEvents.FromToAction<Part, Part> action)
            {
                KerbalEVA kerbalEVA = action.to.GetComponent<KerbalEVA>();
                if (kerbalEVA.GetComponent<CustomHead>() == null)
                    kerbalEVA.gameObject.AddComponent<KerbalEVA>();
            }

            void Add()
            {
                TimingManager.LateUpdateRemove(TimingManager.TimingStage.Normal, Add);
                KerbalEVA[] kerbalEVAs = Resources.FindObjectsOfTypeAll<KerbalEVA>();

                for (int i = 0; i < kerbalEVAs.Length; i++)
                {
                    if (kerbalEVAs[i].GetComponent<CustomHead>() == null)
                        kerbalEVAs[i].gameObject.AddComponent<CustomHead>();
                }

                kerbalExpressionSystem[] kerbalIVAs = Resources.FindObjectsOfTypeAll<kerbalExpressionSystem>();

                for (int i = 0; i < kerbalIVAs.Length; i++)
                {
                    if (kerbalIVAs[i]?.GetComponent<CustomHead>() == null)
                        kerbalIVAs[i].gameObject.AddComponent<CustomHead>();
                }
            }
        }
    }
}
