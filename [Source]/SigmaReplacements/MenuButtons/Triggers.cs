using System;
using System.Linq;
using TMPro;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuButtons
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        class MainMenuTrigger : MonoBehaviour
        {
            void Start()
            {
                if (!Nyan.nyan || Debug.debug)
                {
                    TextMeshPro[] meshes = Resources.FindObjectsOfTypeAll<TextMeshPro>();

                    for (int i = 0; i < meshes.Length; i++)
                    {
                        meshes[i].gameObject.AddComponent<CustomMenuButton>();
                    }

                    GameObject logo = GameObject.Find("logo");

                    if (logo != null)
                    {
                        logo.AddComponent<CustomMenuButton>();
                    }
                }
            }
        }

        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        class NyanSettings : MonoBehaviour
        {
            void Start()
            {
                string[] args = Environment.GetCommandLineArgs();

                Nyan.nyan = (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) || (args.Contains("-nyan-nyan") && !args.Contains("-nyan-not") || args.Contains("-nyan-buttons"));
                Nyan.forever = Nyan.nyan && (args.Contains("-nyan-4ever") || args.Contains("-nyan-buttons"));
            }
        }
    }
}
