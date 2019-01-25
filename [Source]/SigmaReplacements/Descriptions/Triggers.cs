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
                    CrewListItem[] items = Resources.FindObjectsOfTypeAll<CrewListItem>();
                    int? n1 = items?.Length;
                    for (int i = 0; i < n1; i++)
                    {
                        CrewListItem item = items[i];
                        GameObject gameObject = item?.gameObject;
                        if (gameObject != null)
                        {
                            if (gameObject.GetComponent<CustomDescription>() != null)
                            {
                                gameObject.AddComponent<CustomDescription>();
                            }
                        }
                    }

                    CrewWidget[] widgets = Resources.FindObjectsOfTypeAll<CrewWidget>();
                    int? n2 = widgets?.Length;
                    for (int i = 0; i < n2; i++)
                    {
                        CrewWidget widget = widgets[i];
                        GameObject gameObject = widget?.gameObject;
                        if (gameObject != null)
                        {
                            if (gameObject.GetComponent<CustomDescription>() != null)
                            {
                                gameObject.AddComponent<CustomDescription>();
                            }
                        }
                    }

                    AstronautComplex ac = Resources.FindObjectsOfTypeAll<AstronautComplex>()?.FirstOrDefault();
                    if (ac != null)
                    {
                        GameObject gameObject = ac?.gameObject;
                        if (gameObject != null)
                        {
                            if (gameObject.GetComponent<AstronautComplexFix>() != null)
                            {
                                gameObject.AddComponent<AstronautComplexFix>();
                            }
                        }
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
