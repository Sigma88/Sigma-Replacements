using System;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Suits
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        internal class MenuKerbals : MonoBehaviour
        {
            void Start()
            {
                GameObject orbitScene = GameObject.Find("OrbitScene")?.GetChild("Kerbals");

                if (orbitScene != null)
                {
                    for (int i = 0; i < orbitScene?.transform?.childCount; i++)
                    {
                        orbitScene.transform.GetChild(i).gameObject.AddOrGetComponent<CustomSuit>();
                    }
                }

                GameObject munScene = GameObject.Find("MunScene")?.GetChild("Kerbals");

                if (munScene != null)
                {
                    for (int i = 0; i < munScene?.transform?.childCount; i++)
                    {
                        munScene.transform.GetChild(i).gameObject.AddOrGetComponent<CustomSuit>();
                    }
                }
            }
        }

        [KSPAddon(KSPAddon.Startup.MainMenu, true)]
        internal class MenuTriggers : MonoBehaviour
        {
            void Start()
            {
                Debug.Log("MenuTriggers", "Start");

                Transform[] transforms = Resources.FindObjectsOfTypeAll<Transform>();

                for (int i = 0; i < transforms?.Length; i++)
                {
                    Transform transform = transforms[i];

                    switch (transform?.name)
                    {
                        case "WernerVonKerman":
                            UIKerbalWerner werner = transform.gameObject.AddOrGetComponent<UIKerbalWerner>();
                            CustomSuit wernerSuit = transform.gameObject.AddOrGetComponent<CustomSuit>();
                            break;
                        case "Strategy_Mortimer":
                        case "Strategy_ScienceGuy":
                        case "Strategy_PRGuy":
                        case "Strategy_MechanicGuy":
                            UIKerbalStrategy strategy = transform?.gameObject?.AddOrGetComponent<UIKerbalStrategy>();
                            CustomSuit adminSuit = transform?.gameObject?.AddOrGetComponent<CustomSuit>();
                            break;
                        case "instructor_Gene":
                            if (transform?.parent?.gameObject?.name == "Instructor_Gene")
                            {
                                UIKerbalGene gene = transform?.gameObject?.AddOrGetComponent<UIKerbalGene>();
                                CustomSuit geneSuit = transform?.gameObject?.AddOrGetComponent<CustomSuit>();
                            }
                            break;
                    }
                }
            }
        }

        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        internal class ResetTriggers : MonoBehaviour
        {
            void Start()
            {
                KSCTriggers.trigger = true;
            }
        }

        [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
        internal class KSCTriggers : MonoBehaviour
        {
            internal static bool trigger = true;

            void Start()
            {
                Debug.Log("KSCTriggers", "Start");

                if (trigger)
                {
                    trigger = false;

                    if (trigger && HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                    {
                        GameObject gene = Resources.FindObjectsOfTypeAll<MCAvatarController>().FirstOrDefault().gameObject.GetChild("instructor_Gene");
                        GeneSuit suit = gene.AddOrGetComponent<GeneSuit>();
                    }
                }
            }
        }

        internal class GeneSuit : MonoBehaviour
        {
            void Start()
            {
                ProtoCrewMember kerbal = UIKerbals.instructors[0];
                CustomSuit head = gameObject.AddOrGetComponent<CustomSuit>();
                head.LoadFor(kerbal);
                head.ApplyTo(kerbal);
            }
        }

        [KSPAddon(KSPAddon.Startup.MainMenu, true)]
        internal class FlightTriggers : MonoBehaviour
        {
            void Start()
            {
                DontDestroyOnLoad(this);

                Debug.Log("FlightTriggers", "Start");

                GameEvents.onVesselLoaded.Add(OnVesselLoaded);
                GameEvents.onVesselCreate.Add(OnVesselLoaded);
                GameEvents.onCrewOnEva.Add(OnCrewOnEva);
            }

            void OnVesselLoaded(Vessel vessel)
            {
                if (vessel == null) return;

                Debug.Log("FlightTriggers.OnVesselLoaded", "Vessel = " + vessel);

                if (vessel.isEVA)
                {
                    KerbalEVA kerbalEVA = vessel?.evaController;
                    kerbalEVA.gameObject.AddOrGetComponent<CustomSuit>().situation = vessel?.situation;
                }
                else
                {
                    vessel?.gameObject?.AddOrGetComponent<IVASuitFinder>();
                }
            }

            void OnCrewOnEva(GameEvents.FromToAction<Part, Part> action)
            {
                Debug.Log("FlightTriggers.OnCrewOnEva", "Part = " + action.to);

                KerbalEVA kerbalEVA = action.to.GetComponent<KerbalEVA>();
                kerbalEVA.gameObject.AddOrGetComponent<CustomSuit>().situation = action.from?.vessel?.situation;
            }
        }

        internal class IVASuitFinder : IVAFinder
        {
            internal override void AddOrGetComponent(GameObject gameObject)
            {
                gameObject.AddOrGetComponent<CustomSuit>();
            }
        }

        // Nyan Settings
        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class NyanSettings : MonoBehaviour
        {
            void Start()
            {
                string[] args = Environment.GetCommandLineArgs();

                Nyan.nyan = (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) || (args.Contains("-nyan-nyan") && !args.Contains("-nyan-not") || args.Contains("-nyan-suits"));
                Nyan.forever = Nyan.nyan && (args.Contains("-nyan-4ever") || args.Contains("-nyan-suits"));
            }
        }
    }
}
