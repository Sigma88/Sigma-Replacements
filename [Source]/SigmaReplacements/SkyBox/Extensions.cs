﻿using System;
using System.Collections.Generic;
using UnityEngine;


namespace SigmaReplacements
{
    namespace SkyBox
    {
        internal static class Extensions
        {
            internal static Texture[] Pick(this List<Texture[]> list)
            {
                if (list?.Count > 0)
                {
                    if (HighLogic.LoadedScene == GameScenes.SPACECENTER && HighLogic.CurrentGame != null)
                        return list[Math.Abs(HighLogic.CurrentGame.Seed) % list.Count];
                    else
                        return list[Math.Abs(DateTime.Today.GetHashCode()) % list.Count];
                }

                return null;
            }

            internal static Texture[] At(this List<Texture[]> list, Texture[] element, List<Texture[]> reference)
            {
                if (reference.Contains(element) && list?.Count > reference.IndexOf(element))
                    return list[reference.IndexOf(element)];
                else
                    return list.Pick();
            }
        }
    }
}
