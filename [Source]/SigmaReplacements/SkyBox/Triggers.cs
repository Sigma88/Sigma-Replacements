using System;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace SkyBox
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        internal class MenuTriggers : MonoBehaviour
        {
            void Start()
            {
                Debug.Log("MenuTriggers", "");
                Debug.Log("MenuTriggers", ">>>>>  START  <<<<<");

                Debug.Log("MenuTriggers.Start", "Resetting GalaxyCubeControl");
                GalaxyCubeControl.Instance?.Reset();

                GameObject galaxy = GameObject.Find("MainMenuGalaxy");
                Debug.Log("MenuTriggers.Start", "MainMenuGalaxy = " + galaxy);

                CustomSkyBox skybox = new CustomSkyBox(Mode.MAINMENU, Math.Abs(DateTime.Today.GetHashCode()));
                skybox.ApplyTo(galaxy);
            }
        }

        [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
        internal class KSCTriggers : MonoBehaviour
        {
            internal static bool skip = false;

            void Start()
            {
                Debug.Log("KSCTriggers", "");
                Debug.Log("KSCTriggers", ">>>>>  START  <<<<<");

                if (!skip && HighLogic.CurrentGame != null)
                {
                    Debug.Log("KSCTriggers.Start", "Current game mode = " + HighLogic.CurrentGame?.Mode + ", seed = " + HighLogic.CurrentGame?.Seed);
                    CustomSkyBox skybox = new CustomSkyBox((Mode)HighLogic.CurrentGame.Mode, HighLogic.CurrentGame.Seed);
                    skybox.ApplyTo(GalaxyCubeControl.Instance?.gameObject);
                }
            }
        }

        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class NyanSettings : MonoBehaviour
        {
            void Start()
            {
                string[] args = Environment.GetCommandLineArgs();

                Nyan.nyan = (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) || (args.Contains("-nyan-nyan") && !args.Contains("-nyan-not") || args.Contains("-nyan-sky"));
                Nyan.forever = Nyan.nyan && (args.Contains("-nyan-4ever") || args.Contains("-nyan-sky"));
            }
        }
    }
}
