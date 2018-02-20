using System;
using System.Collections.Generic;
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

                if (MenuSceneInfo.DataBase.Count > 0)
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
                    UnityEngine.Debug.Log("SigmaLog: db count = " + MenuSceneInfo.DataBase.Count);
                    int index = MenuSceneInfo.DataBase.Choose(Math.Abs(hash.GetHashCode()));
                    UnityEngine.Debug.Log("SigmaLog: chosen = " + index);
                    CustomMenuScene scene = new CustomMenuScene(MenuSceneInfo.DataBase[index]);
                    scene.ApplyTo(scenes[0]);
                }
            }

            static string s1 = "";

            static int count = 0;

            void Update()
            {
                string s = DateTime.Now.ToLongTimeString();

                if (s != s1)
                {
                    count++;

                    s1 = s;
                    UnityEngine.Debug.Log("SigmaLog: s1 = " + Math.Abs(s1.GetHashCode()) + " -- " + PseudoRandom.Scene(Math.Abs(s1.GetHashCode())));
                    string s2 = Math.Abs(s1.GetHashCode()).ToString();
                    UnityEngine.Debug.Log("SigmaLog: s2 = " + Math.Abs(s2.GetHashCode()) + " -- " + MenuSceneInfo.DataBase.Choose(Math.Abs(s2.GetHashCode())));

                    if (count == 1000)
                    {
                        Application.Quit();
                    }
                }
            }
        }

        internal static class PseudoRandom
        {
            static double type = 0.5;

            internal static int Scene(int hash)
            {
                if (Math.Pow(hash, 0.5) % 1 < type)
                {
                    type = Math.Max(0, type - 0.1);

                    return 0;
                }
                else
                {
                    type = Math.Min(1, type + 0.1);

                    return 1;
                }
            }

            static int max = 3;
            static double[] chances = null;

            internal static int Choose(this List<MenuSceneInfo> list, int hash)
            {
                if (chances == null)
                {
                    chances = new double[list.Count];
                    for (int i = 0; i < chances.Length; i++)
                    {
                        chances[i] = 100d / chances.Length;
                    }
                }

                double random = Math.Pow(hash, 0.5) % 100d;
                double sum = 0;

                for (int i = 0; i < chances.Length; i++)
                {
                    sum += chances[i];
                    if (random < sum)
                    {
                        double penalty = Math.Min(chances[i], (100d / max) / chances.Length);
                        double bonus = penalty / (chances.Length - 1);

                        for (int j = 0; j < chances.Length; j++)
                        {
                            chances[j] += bonus;
                        }

                        chances[i] = chances[i] - bonus - penalty;
                        return i;
                    }
                }

                return 0;
            }
        }
    }
}
