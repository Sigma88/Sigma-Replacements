using System;
using System.Linq;
using UnityEngine;
using KSP.UI.Screens;


namespace SigmaReplacements
{
    namespace Heads
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, true)]
        class MenuTriggers : MonoBehaviour
        {
            static string[] names = new string[] { "Strategy_Mortimer", "Strategy_ScienceGuy", "Strategy_PRGuy", "Strategy_MechanicGuy" };

            void Start()
            {
                Debug.Log("MenuTriggers", "Start");

                Transform[] transforms = Resources.FindObjectsOfTypeAll<Transform>();

                int menu = 0;

                for (int i = 0; i < transforms?.Length; i++)
                {
                    Transform transform = transforms[i];

                    if (transform?.name == "Kerbals")
                    {
                        int? kerbals = transform?.childCount;
                        if (kerbals > 4) kerbals = 4;

                        for (int j = 0; j < kerbals; j++)
                        {
                            Transform child = transform.GetChild(j);

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

                    if (names.Contains(transform?.name))
                    {
                        UIKerbalStrategy strategy = transform?.gameObject?.AddOrGetComponent<UIKerbalStrategy>();
                        CustomHead head = transform?.gameObject?.AddOrGetComponent<CustomHead>();
                    }

                    if (transform?.name == "instructor_Gene")
                    {
                        if (transform?.parent?.gameObject?.name == "Instructor_Gene")
                        {
                            UIKerbalGene strategy = transform?.gameObject?.AddOrGetComponent<UIKerbalGene>();
                            CustomHead head = transform?.gameObject?.AddOrGetComponent<CustomHead>();
                        }
                    }
                }
            }
        }

        [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
        class KSCTriggers : MonoBehaviour
        {
            void Start()
            {
                Debug.Log("KSCTriggers", "Start");

                if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                {
                    GameObject admin = Resources.FindObjectsOfTypeAll<Administration>().FirstOrDefault().gameObject;
                    UIKerbalsTrigger heads = admin.AddOrGetComponent<UIKerbalsTrigger>();

                    GameObject gene = Resources.FindObjectsOfTypeAll<MCAvatarController>().FirstOrDefault().gameObject.GetChild("instructor_Gene");
                    GeneHead head = gene.AddOrGetComponent<GeneHead>();
                }
            }
        }

        class GeneHead : MonoBehaviour
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
        class FlightTriggers : MonoBehaviour
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
                Debug.Log("FlightTriggers.OnVesselLoaded", "Vessel = " + vessel);

                KerbalEVA[] kerbalEVAs = vessel.GetComponentsInChildren<KerbalEVA>(true);

                for (int i = 0; i < kerbalEVAs?.Length; i++)
                {
                    if (kerbalEVAs[i]?.GetComponent<CustomHead>() == null)
                        kerbalEVAs[i].gameObject.AddComponent<CustomHead>();
                }

                kerbalExpressionSystem[] kerbalIVAs = Resources.FindObjectsOfTypeAll<kerbalExpressionSystem>();

                for (int i = 0; i < kerbalIVAs?.Length; i++)
                {
                    if (kerbalIVAs[i]?.GetComponent<CustomHead>() == null)
                        kerbalIVAs[i].gameObject.AddComponent<CustomHead>();
                }
            }

            void OnCrewOnEva(GameEvents.FromToAction<Part, Part> action)
            {
                Debug.Log("FlightTriggers.OnCrewOnEva", "Part = " + action.to);

                KerbalEVA kerbalEVA = action.to.GetComponent<KerbalEVA>();
                if (kerbalEVA.GetComponent<CustomHead>() == null)
                    kerbalEVA.gameObject.AddComponent<CustomHead>();
            }
        }

        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        class NyanSettings : MonoBehaviour
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
