using System;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        internal class MenuTriggers : MonoBehaviour
        {
            void Awake()
            {
                int i = 1;
                string hash = DateTime.Now.ToLongTimeString();

                if (MunSceneInfo.DataBase.Count > 0)
                {
                    hash = Math.Abs(hash.GetHashCode()).ToString();
                    i = PseudoRandom.Scene(Math.Abs(hash.GetHashCode()));
                }

                // Choose Scene
                GameObject[] scenes = FindObjectOfType<MainMenu>().envLogic.areas;
                scenes[i].SetActive(true);
                scenes[(i + 1) % 2].SetActive(false);
                if (i == 0)
                {
                    int index = MunSceneInfo.DataBase.Choose(Math.Abs(hash.GetHashCode()));
                    CustomMunScene scene = new CustomMunScene((MunSceneInfo)MunSceneInfo.DataBase[index]);
                    scene.ApplyTo(scenes[0]);
                }
                else if (OrbitSceneInfo.DataBase?.Count > 0)
                {
                    int index = MunSceneInfo.DataBase.Choose(Math.Abs(hash.GetHashCode()));
                    CustomOrbitScene scene = new CustomOrbitScene((OrbitSceneInfo)OrbitSceneInfo.DataBase[index]);
                    scene.ApplyTo(scenes[1]);
                }

                if (AssemblyLoader.loadedAssemblies.FirstOrDefault(a => a.name == "Kopernicus") != null)
                {
                    new KopernicusFixer();
                }
            }
        }
    }
}
