using System;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Textures
    {
        [KSPAddon(KSPAddon.Startup.EveryScene, false)]
        class SceneTrigger : MonoBehaviour
        {
            static bool update = false;

            void Start()
            {
                update = true;
                Replace();
            }

            void Update()
            {
                if (update)
                {
                    update = false;
                    Replace();
                }
            }

            void Replace()
            {
                Material[] materials = Resources.FindObjectsOfTypeAll<Material>();
                Debug.Log(HighLogic.LoadedScene + ".Start", "Replacing textures for " + materials?.Length + " materials");

                for (int i = 0; i < materials?.Length; i++)
                {
                    Material material = materials[i];

                    if (material == null) continue;

                    Texture oldTex = null;

                    if (material.HasProperty("_MainTexture"))
                    {
                        oldTex = material.GetTexture("_MainTexture");
                        if (oldTex != null && TextureInfo.Database.ContainsKey(oldTex))
                        {
                            Debug.Log(HighLogic.LoadedScene + ".Start", "Replacing textures for material = " + material);
                            material.SetMainTexture(TextureInfo.Database[oldTex]);
                            Debug.Log(HighLogic.LoadedScene + ".Start", "Replaced old _MainTexture = " + oldTex + " with new _MainTexture = " + material.GetTexture("_MainTexture"));
                        }
                    }
                    else if (material.HasProperty("_MainTex"))
                    {
                        oldTex = material.GetTexture("_MainTex");
                        if (oldTex != null && TextureInfo.Database.ContainsKey(oldTex))
                        {
                            Debug.Log(HighLogic.LoadedScene + ".Start", "Replacing textures for material = " + material);
                            material.SetTexture(TextureInfo.Database[oldTex]);
                            Debug.Log(HighLogic.LoadedScene + ".Start", "Replaced old _MainTex = " + oldTex + " with new _MainTex = " + material.GetTexture("_MainTex"));
                        }
                    }

                    oldTex = null;

                    if (material.HasProperty("_BumpMap"))
                    {
                        oldTex = material.GetTexture("_BumpMap");
                        if (oldTex != null && TextureInfo.Database.ContainsKey(oldTex))
                        {
                            Debug.Log(HighLogic.LoadedScene + ".Start", "Replacing normals for material = " + material);
                            material.SetNormal(TextureInfo.Database[oldTex]);
                            Debug.Log(HighLogic.LoadedScene + ".Start", "Replaced old _BumpMap = " + oldTex + " with new _BumpMap = " + material.GetTexture("_BumpMap"));
                        }
                    }
                }
            }
        }

        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        class NyanSettings : MonoBehaviour
        {
            static Texture2D empty;

            void Start()
            {
                string[] args = Environment.GetCommandLineArgs();

                Nyan.nyan = (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) || (args.Contains("-nyan-nyan") && !args.Contains("-nyan-not") || args.Contains("-nyan-tex"));
                Nyan.forever = Nyan.nyan && (args.Contains("-nyan-4ever") || args.Contains("-nyan-tex"));
            }

            void OnDestroy()
            {
                if (Nyan.forever) LoadNyan();
            }

            void LoadNyan()
            {
                Info info = new Info(new ConfigNode(), new ConfigNode());

                empty = new Texture2D(1, 1);
                empty.SetPixel(1, 1, new Color(0, 0, 0, 0));
                empty.Apply();

                string[] names = { "01_sun", "02_eve", "03_kerbin" };
                for (int i = 0; i < 3; i++)
                {
                    Texture oldTex = null;
                    oldTex = info.Parse(names[i], oldTex);
                    if (oldTex != null)
                        TextureInfo.Database.Add(oldTex, empty);
                }

                Texture oldJool = null;
                oldJool = info.Parse("04_jool", oldJool);
                if (oldJool != null)
                {
                    TextureInfo.Database.Add(oldJool, Nyan.nyanLoad);
                }
            }
        }
    }
}
