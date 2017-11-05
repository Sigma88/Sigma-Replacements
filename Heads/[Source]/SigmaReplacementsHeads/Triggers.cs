using UnityEngine;
using System.Linq;
using KSP.UI.Screens;


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

                for (int i = 0; i < transforms?.Length; i++)
                {
                    Transform transform = transforms[i];

                    if (transform?.name == "Kerbals")
                    {
                        foreach (Transform child in transform)
                        {
                            if (child?.gameObject != null && child?.GetComponent<UIKerbalMenu>() == null)
                                child.gameObject.AddComponent<UIKerbalMenu>();

                            UIKerbalMenu kerbal = child.GetComponent<UIKerbalMenu>();
                            kerbal.crewMember = UIKerbals.menuKerbals[menu];
                            menu++;

                            if (child?.gameObject != null && child?.GetComponent<CustomHead>() == null)
                                child.gameObject.AddComponent<CustomHead>();
                        }
                    }

                    if (transform?.name == "WernerVonKerman")
                    {
                        if (transform?.gameObject != null && transform?.GetComponent<UIKerbalWerner>() == null)
                            transform.gameObject.AddComponent<UIKerbalWerner>();

                        if (transform?.gameObject != null && transform?.GetComponent<CustomHead>() == null)
                            transform.gameObject.AddComponent<CustomHead>();
                    }
                }
            }
        }

        [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
        class KSCTriggers : MonoBehaviour
        {
            void Start()
            {
                Administration admin = Resources.FindObjectsOfTypeAll<Administration>().FirstOrDefault();

                if (admin != null && admin.GetComponent<AdminTrigger>() == null)
                {
                    admin.gameObject.AddComponent<AdminTrigger>();
                }


                string[] names = new string[] { "Strategy_Mortimer", "Strategy_ScienceGuy", "Strategy_PRGuy", "Strategy_MechanicGuy" };

                Transform[] transforms = Resources.FindObjectsOfTypeAll<Transform>();

                for (int i = 0; i < transforms?.Length; i++)
                {
                    Transform transform = transforms[i];

                    if (names.Contains(transform?.name))
                    {
                        int index = names.IndexOf(transform?.name);

                        if (transform?.gameObject != null && transform?.GetComponent<UIKerbalStrategy>() == null)
                            transform.gameObject.AddComponent<UIKerbalStrategy>();

                        if (transform?.gameObject != null && transform?.GetComponent<CustomHead>() == null)
                            transform.gameObject.AddComponent<CustomHead>();
                    }

                    if (transform?.name == "instructor_Gene")
                    {
                        if (transform?.gameObject != null && transform?.GetComponent<UIKerbalGene>() == null)
                            transform.gameObject.AddComponent<UIKerbalGene>();

                        if (transform?.gameObject != null && transform?.GetComponent<CustomHead>() == null)
                            transform.gameObject.AddComponent<CustomHead>();
                    }
                }
            }
        }

        internal class AdminTrigger : MonoBehaviour
        {
            void Start()
            {
                UIKerbalStrategy[] strategies = GetComponentsInChildren<UIKerbalStrategy>();

                for (int i = 0; i < strategies?.Length; i++)
                {
                    if (strategies[i] != null)
                        strategies[i].Apply();
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
                GameEvents.onCrewOnEva.Remove(OnCrewOnEva);
                KerbalEVA kerbalEVA = action.to.GetComponent<KerbalEVA>();
                if (kerbalEVA.GetComponent<CustomHead>() == null)
                    kerbalEVA.gameObject.AddComponent<CustomHead>();
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
