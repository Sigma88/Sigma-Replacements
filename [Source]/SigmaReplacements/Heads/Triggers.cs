﻿using System;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Heads
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
                        orbitScene.transform.GetChild(i).gameObject.AddOrGetComponent<CustomHead>();
                    }
                }

                GameObject munScene = GameObject.Find("MunScene")?.GetChild("Kerbals");

                if (munScene != null)
                {
                    for (int i = 0; i < munScene?.transform?.childCount; i++)
                    {
                        munScene.transform.GetChild(i).gameObject.AddOrGetComponent<CustomHead>();
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
                            CustomHead wernerHead = transform.gameObject.AddOrGetComponent<CustomHead>();
                            break;
                        case "Strategy_Mortimer":
                        case "Strategy_ScienceGuy":
                        case "Strategy_PRGuy":
                        case "Strategy_MechanicGuy":
                            UIKerbalStrategy strategy = transform?.gameObject?.AddOrGetComponent<UIKerbalStrategy>();
                            CustomHead adminHead = transform?.gameObject?.AddOrGetComponent<CustomHead>();
                            break;
                        case "instructor_Gene":
                            if (transform?.parent?.gameObject?.name == "Instructor_Gene")
                            {
                                UIKerbalGene gene = transform?.gameObject?.AddOrGetComponent<UIKerbalGene>();
                                CustomHead geneHead = transform?.gameObject?.AddOrGetComponent<CustomHead>();
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

                    if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                    {
                        GameObject gene = Resources.FindObjectsOfTypeAll<MCAvatarController>().FirstOrDefault().gameObject.GetChild("instructor_Gene");
                        GeneHead head = gene.AddOrGetComponent<GeneHead>();
                    }
                }
            }
        }

        internal class GeneHead : MonoBehaviour
        {
            void Start()
            {
                ProtoCrewMember kerbal = UIKerbals.instructors[0];
                CustomHead head = gameObject.AddOrGetComponent<CustomHead>();
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
                GameEvents.onCrewBoardVessel.Add(OnCrewBoardVessel);
            }

            void OnVesselLoaded(Vessel vessel)
            {
                if (vessel == null) return;

                Debug.Log("FlightTriggers.OnVesselLoaded", "Vessel = " + vessel);
                vessel?.gameObject?.AddOrGetComponent<IVAHeadFinder>();

                if (vessel.isEVA)
                {
                    KerbalEVA kerbalEVA = vessel?.evaController;
                    kerbalEVA?.gameObject?.AddOrGetComponent<CustomHead>();
                }
            }

            void OnCrewOnEva(GameEvents.FromToAction<Part, Part> action)
            {
                Debug.Log("FlightTriggers.OnCrewOnEva", "Part = " + action.to);

                KerbalEVA kerbalEVA = action.to.GetComponent<KerbalEVA>();
                CustomHead customHead = kerbalEVA.gameObject.AddOrGetComponent<CustomHead>();
            }

            void OnCrewBoardVessel(GameEvents.FromToAction<Part, Part> action)
            {
                Debug.Log("FlightTriggers.OnCrewOnEva", "Part = " + action.to);

                Vessel vessel = action.to?.vessel;
                vessel?.gameObject?.AddOrGetComponent<IVAHeadFinder>()?.UpdateIVAs();
            }
        }

        internal class IVAHeadFinder : IVAFinder
        {
            internal override void AddOrGetComponent(GameObject gameObject)
            {
                gameObject.AddOrGetComponent<CustomHead>();
            }
        }

        // Nyan Settings
        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class NyanSettings : MonoBehaviour
        {
            void Start()
            {
                string[] args = Environment.GetCommandLineArgs();

                Nyan.nyan = (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) || (args.Contains("-nyan-nyan") && !args.Contains("-nyan-not") || args.Contains("-nyan-heads"));
                Nyan.forever = Nyan.nyan && (args.Contains("-nyan-4ever") || args.Contains("-nyan-heads"));
            }
        }
    }
}
