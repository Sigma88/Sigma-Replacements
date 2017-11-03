using System.Linq;
using UnityEngine;
using KSP.UI.Screens;


namespace SigmaReplacements
{
    namespace Heads
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        class MainMenuTrigger : MonoBehaviour
        {/*
            void Awake()
            {
                Settings.menuVets = (new[] { Settings.menuVet2, Settings.menuVet3, Settings.menuVet1 }).ToList();

                var orbitScene = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(o => o.name == "OrbitScene");

                var Kerbals = orbitScene.GetChild("Kerbals");

                foreach (Transform child in Kerbals.transform)
                {
                    child.gameObject.AddComponent<Head>();
                }
            }*/
        }

        [KSPAddon(KSPAddon.Startup.Flight, false)]
        class FlightTriggers : MonoBehaviour
        {
            void Start()
            {
                GameEvents.onCrewOnEva.Add(OnCrewOnEva);
                Add();
            }

            void OnCrewOnEva(GameEvents.FromToAction<Part, Part> action)
            {
                Add();
            }

            void Add()
            {
                KerbalEVA[] kerbals = Resources.FindObjectsOfTypeAll<KerbalEVA>();

                for (int i = 0; i < kerbals.Length; i++)
                {
                    kerbals[i].gameObject.AddComponent<CustomHead>();
                }
            }
        }
    }
}
