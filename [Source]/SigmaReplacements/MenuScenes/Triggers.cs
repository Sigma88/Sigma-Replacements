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
                Debug.Log("MenuTriggers.Awake", "Nyan.nyan = " + Nyan.nyan);
                Debug.Log("MenuTriggers.Awake", "MunSceneInfo.DataBase.Count = " + MunSceneInfo.DataBase?.Count);
                Debug.Log("MenuTriggers.Awake", "OrbitSceneInfo.DataBase.Count = " + OrbitSceneInfo.DataBase?.Count);
                if (!Nyan.nyan && MunSceneInfo.DataBase == null && OrbitSceneInfo.DataBase == null) return;
                if (MunSceneInfo.DataBase.Count == 0 && OrbitSceneInfo.DataBase.Count == 0) return;

                GameObject[] scenes = FindObjectOfType<MainMenu>()?.envLogic?.areas;

                Debug.Log("MenuTriggers.Awake", "scenes = " + scenes?.Length);
                if (scenes == null) return;

                int i = 1;
                string hash = DateTime.Now.ToLongTimeString();
                hash = Math.Abs(hash.GetHashCode()).ToString();

                i = PseudoRandom.Scene(Math.Abs(hash.GetHashCode()));
                Debug.Log("MenuTriggers.Awake", "random scene = " + i);


                if (Nyan.nyan)
                {
                    Debug.Log("MenuTriggers.Awake", "Loading nyan scene");

                    // Activate Scene
                    scenes[i].SetActive(true);
                    scenes[(i + 1) % 2].SetActive(false);

                    // Nyan-ify the OrbitScene
                    Renderer mun = scenes[1].GetChild("Mun").GetComponent<Renderer>();
                    mun.material.SetTexture(Nyan.nyanGround);

                    // Nyan-ify the MunScene
                    Terrain terrain = scenes[0].GetChild("Terrain").GetComponent<Terrain>();
                    TerrainLayer[] layers = terrain.terrainData.terrainLayers;
                    layers[0].diffuseTexture = layers[1].diffuseTexture = (Texture2D)Nyan.nyanGround;
                    layers[0].normalMapTexture = layers[1].normalMapTexture = null;
                    terrain.terrainData.terrainLayers = layers;
                    return;
                }

                // Force Mun Scene
                if (OrbitSceneInfo.DataBase?.Count == 0)
                {
                    i = 0;
                }

                // Force Orbit Scene
                if (MunSceneInfo.DataBase?.Count == 0)
                {
                    i = 1;
                }

                // Activate Scene
                scenes[i].SetActive(true);
                scenes[(i + 1) % 2].SetActive(false);

                Debug.Log("MenuTriggers.Awake", "chosen scene = " + i);
                if (i == 0)
                {
                    Debug.Log("MenuTriggers.Awake", "Loading mun scene");

                    if (MunSceneInfo.DataBase != null)
                    {
                        int index = MunSceneInfo.DataBase.Choose(Math.Abs(hash.GetHashCode()));
                        CustomMunScene scene = new CustomMunScene((MunSceneInfo)MunSceneInfo.DataBase[index]);
                        scene.ApplyTo(scenes[0]);
                    }
                }
                else
                {
                    Debug.Log("MenuTriggers.Awake", "Loading orbit scene");

                    if (OrbitSceneInfo.DataBase != null)
                    {
                        int index = OrbitSceneInfo.DataBase.Choose(Math.Abs(hash.GetHashCode()));
                        CustomOrbitScene scene = new CustomOrbitScene((OrbitSceneInfo)OrbitSceneInfo.DataBase[index]);
                        scene.ApplyTo(scenes);
                    }
                }

                if (KopernicusFixer.detect)
                {
                    Debug.Log("MenuTriggers.Awake", "Kopernicus has been detected");

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
