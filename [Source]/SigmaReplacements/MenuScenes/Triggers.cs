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
                GameObject[] scenes = FindObjectOfType<MainMenu>().envLogic.areas;

                int i = 1;
                string hash = DateTime.Now.ToLongTimeString();
                hash = Math.Abs(hash.GetHashCode()).ToString();
                i = PseudoRandom.Scene(Math.Abs(hash.GetHashCode()));

                // Choose Scene
                scenes[i].SetActive(true);
                scenes[(i + 1) % 2].SetActive(false);

                if (Nyan.nyan)
                {
                    Renderer mun = scenes[1].GetChild("Mun").GetComponent<Renderer>();
                    mun.material.SetTexture(Nyan.nyanGround);

                    Terrain terrain = scenes[0].GetChild("Terrain").GetComponent<Terrain>();
                    SplatPrototype[] splats = terrain.terrainData.splatPrototypes;
                    splats[0].texture = (Texture2D)Nyan.nyanGround;
                    splats[1].texture = (Texture2D)Nyan.nyanGround;
                    terrain.terrainData.splatPrototypes = splats;
                    return;
                }

                if (MunSceneInfo.DataBase.Count > 0)
                {
                    if (OrbitSceneInfo.DataBase?.Count == 0)
                    {
                        i = 0;
                    }
                }
                else
                {
                    i = 1;
                }

                if (i == 0)
                {
                    int index = MunSceneInfo.DataBase.Choose(Math.Abs(hash.GetHashCode()));
                    CustomMunScene scene = new CustomMunScene((MunSceneInfo)MunSceneInfo.DataBase[index]);
                    scene.ApplyTo(scenes[0]);
                }
                else if (OrbitSceneInfo.DataBase?.Count > 0)
                {
                    int index = OrbitSceneInfo.DataBase.Choose(Math.Abs(hash.GetHashCode()));
                    CustomOrbitScene scene = new CustomOrbitScene((OrbitSceneInfo)OrbitSceneInfo.DataBase[index]);
                    scene.ApplyTo(scenes);
                }

                if (KopernicusFixer.detect)
                {
                    gameObject.AddOrGetComponent<KopernicusFixer>();
                }
            }
        }

        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class NyanSettings : MonoBehaviour
        {
            void Start()
            {
                string[] args = Environment.GetCommandLineArgs();

                Nyan.nyan = (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) || (args.Contains("-nyan-nyan") && !args.Contains("-nyan-not") || args.Contains("-nyan-scenes"));
                Nyan.forever = Nyan.nyan && (args.Contains("-nyan-4ever") || args.Contains("-nyan-scenes"));
            }
        }
    }
}
