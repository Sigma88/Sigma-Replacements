﻿using System;
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
                    int index = MenuSceneInfo.DataBase.Choose(Math.Abs(hash.GetHashCode()));
                    CustomMenuScene scene = new CustomMenuScene(MenuSceneInfo.DataBase[index]);
                    scene.ApplyTo(scenes[0]);
                }
            }
        }

        internal static class PseudoRandom
        {
            static int max = 3;
            static double type = 0.5;
            static double[] chances = null;

            internal static int Scene(int hash)
            {
                if (Math.Pow(hash, 0.5) % 1 < type)
                {
                    type = Math.Max(0, type - 1d / max);

                    return 0;
                }
                else
                {
                    type = Math.Min(1, type + 1d / max);

                    return 1;
                }
            }

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
