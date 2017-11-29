using System;
using System.Linq;
using UnityEngine;
using KSP.UI.Screens;


namespace SigmaReplacements
{
    namespace Suits
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        class MenuTriggers : MonoBehaviour
        {
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

                            if (child?.gameObject != null && child?.GetComponent<CustomSuit>() == null)
                                child.gameObject.AddComponent<CustomSuit>();
                        }
                    }

                    if (transform?.name == "WernerVonKerman")
                    {
                        if (transform?.gameObject != null && transform?.GetComponent<UIKerbalWerner>() == null)
                            transform.gameObject.AddComponent<UIKerbalWerner>();

                        if (transform?.gameObject != null && transform?.GetComponent<CustomSuit>() == null)
                            transform.gameObject.AddComponent<CustomSuit>();
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

                Administration admin = Resources.FindObjectsOfTypeAll<Administration>().FirstOrDefault();

                if (admin?.gameObject != null && admin.GetComponent<AdminTrigger>() == null)
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

                        if (transform?.gameObject != null && transform?.GetComponent<CustomSuit>() == null)
                            transform.gameObject.AddComponent<CustomSuit>();
                    }

                    if (transform?.name == "instructor_Gene")
                    {
                        if (transform?.gameObject != null && transform?.GetComponent<UIKerbalGene>() == null)
                            transform.gameObject.AddComponent<UIKerbalGene>();

                        if (transform?.gameObject != null && transform?.GetComponent<CustomSuit>() == null)
                            transform.gameObject.AddComponent<CustomSuit>();
                    }
                }
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
                    if (kerbalEVAs[i]?.GetComponent<CustomSuit>() == null)
                        kerbalEVAs[i].gameObject.AddComponent<CustomSuit>();
                }

                kerbalExpressionSystem[] kerbalIVAs = Resources.FindObjectsOfTypeAll<kerbalExpressionSystem>();

                for (int i = 0; i < kerbalIVAs?.Length; i++)
                {
                    if (kerbalIVAs[i]?.GetComponent<CustomSuit>() == null)
                        kerbalIVAs[i].gameObject.AddComponent<CustomSuit>();
                }
            }

            void OnCrewOnEva(GameEvents.FromToAction<Part, Part> action)
            {
                Debug.Log("FlightTriggers.OnCrewOnEva", "Part = " + action.to);

                KerbalEVA kerbalEVA = action.to.GetComponent<KerbalEVA>();
                if (kerbalEVA.GetComponent<CustomSuit>() == null)
                    kerbalEVA.gameObject.AddComponent<CustomSuit>();
            }
        }

        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        class NyanSettings : MonoBehaviour
        {
            void Start()
            {
                Nyan.nyan = (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) || (Environment.GetCommandLineArgs().Contains("-nyan-nyan") && !Environment.GetCommandLineArgs().Contains("-nyan-not"));
                Nyan.forever = Nyan.nyan && Environment.GetCommandLineArgs().Contains("-nyan-4ever");
            }
        }
    }
}
