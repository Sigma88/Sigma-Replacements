using UnityEngine;
using System.Linq;

namespace SigmaReplacements
{
    namespace Heads
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        class MenuTriggers : MonoBehaviour
        {
            void Start()
            {
                Transform[] transforms = Resources.FindObjectsOfTypeAll<Transform>();

                int menu = 0;
                int rnd = 0;

                for (int i = 0; i < transforms?.Length; i++)
                {
                    Transform transform = transforms[i];

                    if (transform?.name == "Kerbals")
                    {
                        foreach (Transform child in transform)
                        {
                            if (child?.gameObject != null && child?.GetComponent<UIKerbal>() == null)
                                child.gameObject.AddComponent<UIKerbal>();

                            UIKerbal kerbal = child.GetComponent<UIKerbal>();
                            kerbal.crewMember = UIKerbal.menuKerbals[menu];
                            menu++;

                            if (child?.gameObject != null && child?.GetComponent<CustomHead>() == null)
                                child.gameObject.AddComponent<CustomHead>();
                        }
                    }
                }
            }
        }


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
