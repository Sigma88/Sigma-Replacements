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
                GalaxyCubeControl.Instance?.Reset();
            }
        }

        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        class MainMenuGalaxy : MonoBehaviour
        {
            void Start()
            {
                Debug.Log("MainMenuGalaxy", "Start");
                GameObject galaxy = GameObject.Find("MainMenuGalaxy");

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
                if (!skip && HighLogic.CurrentGame != null)
                {
                    Debug.Log("KSCTriggers.Start", "Current game = " + HighLogic.CurrentGame + ", mode = " + HighLogic.CurrentGame?.Mode);
                    CustomSkyBox skybox = new CustomSkyBox((Mode)HighLogic.CurrentGame.Mode, HighLogic.CurrentGame.Seed);
                    skybox.ApplyTo(GalaxyCubeControl.Instance?.gameObject);
                }
            }
        }
    }
}
