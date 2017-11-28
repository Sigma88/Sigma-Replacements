using UnityEngine;


namespace SigmaReplacements
{
    namespace SkyBox
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        class MenuTriggers : MonoBehaviour
        {
            void Start()
            {
                Debug.Log("MenuTriggers", "");
                Debug.Log("MenuTriggers", ">>>>>  START  <<<<<");

                Debug.Log("MenuTriggers.Start", "Resetting GalaxyCubeControl");
                GalaxyCubeControl.Instance?.Reset();

                GameObject galaxy = GameObject.Find("MainMenuGalaxy");
                Debug.Log("MenuTriggers.Start", "MainMenuGalaxy = " + galaxy);

                CustomSkyBox skybox = new CustomSkyBox(Mode.MAINMENU, "Menu".GetHashCode());
                skybox.ApplyTo(galaxy);
            }
        }

        [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
        class KSCTriggers : MonoBehaviour
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
        class NyanSettings : MonoBehaviour
        {
            void Start()
            {
                //Nyan.nyan = (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) || (Environment.GetCommandLineArgs().Contains("-nyan-nyan") && !Environment.GetCommandLineArgs().Contains("-nyan-not"));
                //Nyan.forever = Nyan.nyan && Environment.GetCommandLineArgs().Contains("-nyan-4ever");
            }
        }
    }
}
