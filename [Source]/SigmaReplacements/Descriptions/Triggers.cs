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
        [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
        public class KSCTriggers : MonoBehaviour
        {
            static bool skip = false;

            void Start()
            {
                if (!skip)
                {
                    skip = true;
                    CrewListItem[] items = Resources.FindObjectsOfTypeAll<CrewListItem>();
                    for (int i = 0; i < items?.Length; i++)
                    {
                        items[i].gameObject.AddOrGetComponent<CustomDescription>();
                    }
                    
                    CrewWidget[] widgets = Resources.FindObjectsOfTypeAll<CrewWidget>();
                    for (int i = 0; i < widgets?.Length; i++)
                    {
                        widgets[i].gameObject.AddOrGetComponent<CustomDescription>();
                    }
                }

                AstronautComplex ac = Resources.FindObjectsOfTypeAll<AstronautComplex>()?.FirstOrDefault();
                if (ac?.gameObject != null)
                    ac.gameObject.AddOrGetComponent<AstronautComplexFix>();
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
