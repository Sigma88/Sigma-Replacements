using System;
using System.Linq;
using UnityEngine;
using KSP.UI;
using KSP.UI.Screens;
using KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        [KSPAddon(KSPAddon.Startup.FlightEditorAndKSC, false)]
        internal class KSCTriggers : MonoBehaviour
        {
            void Start()
            {
                if (HighLogic.LoadedScene != GameScenes.FLIGHT)
                {
                    CrewListItem[] items = FindObjectsOfType<CrewListItem>();
                    for (int i = 0; i < items?.Length; i++)
                    {
                        items[i]?.gameObject?.AddOrGetComponent<CustomDescription>();
                    }

                    CrewWidget[] widgets = FindObjectsOfType<CrewWidget>();
                    for (int i = 0; i < widgets?.Length; i++)
                    {
                        widgets[i]?.gameObject?.AddOrGetComponent<CustomDescription>();
                    }

                    AstronautComplex ac = FindObjectOfType<AstronautComplex>();
                    if (ac != null)
                    {
                        ac?.gameObject?.AddOrGetComponent<AstronautComplexFix>();
                    }
                }
            }
        }

        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class NyanSettings : MonoBehaviour
        {
            void Start()
            {
                string[] args = Environment.GetCommandLineArgs();

                Nyan.nyan = (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) || (args.Contains("-nyan-nyan") && !args.Contains("-nyan-not") || args.Contains("-nyan-descr"));
                Nyan.forever = Nyan.nyan && (args.Contains("-nyan-4ever") || args.Contains("-nyan-descr"));
            }
        }
    }
}
