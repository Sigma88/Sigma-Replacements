using System;
using System.Collections.Generic;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Heads
    {
        internal static class Extensions
        {
            internal static void SetColor(this Material material, Color? color)
            {
                if (material != null && color != null)
                {
                    material.color = (Color)color;
                }
            }

            internal static void SetTexture(this Material material, Texture newTex)
            {
                if (material != null && newTex != null)
                {
                    Texture oldTex = material?.mainTexture;

                    if (oldTex != null)
                    {
                        newTex.anisoLevel = oldTex.anisoLevel;
                        newTex.wrapMode = oldTex.wrapMode;
                    }

                    material.mainTexture = newTex;
                }
            }

            internal static void SetNormal(this Material material, Texture newTex)
            {
                if (material != null && newTex != null && material.HasProperty("_BumpMap"))
                {
                    Texture oldTex = material.GetTexture("_BumpMap");

                    if (oldTex != null)
                    {
                        newTex.anisoLevel = oldTex.anisoLevel;
                        newTex.wrapMode = oldTex.wrapMode;
                    }

                    material.SetTexture("_BumpMap", newTex);
                }
            }

            internal static Texture At(this List<Texture> right, Texture item, List<Texture> left)
            {
                if (item != null && left.Contains(item) && right?.Count > left.IndexOf(item))
                    return right[left.IndexOf(item)];
                return null;
            }

            internal static Color? At(this List<Color> right, Color? item, List<Color> left)
            {
                if (item != null && left.Contains((Color)item) && right?.Count > left.IndexOf((Color)item))
                    return right[left.IndexOf((Color)item)];
                return null;
            }

            internal static Texture Pick(this List<Texture> list, ProtoCrewMember kerbal, bool useGameSeed)
            {
                if (list.Count == 1)
                    return list[0];
                else if (list.Count > 1)
                    return list[kerbal.Hash(useGameSeed) % list.Count];
                else
                    return null;
            }

            internal static Color? Pick(this List<Color> list, ProtoCrewMember kerbal, bool useGameSeed)
            {
                if (list.Count == 1)
                    return list[0];
                else if (list.Count > 1)
                    return list[kerbal.Hash(useGameSeed) % list.Count];
                else
                    return null;
            }

            internal static int Hash(this ProtoCrewMember kerbal, bool useGameSeed)
            {
                string hash = HeadInfo.hash;

                if (string.IsNullOrEmpty(hash)) hash = kerbal.name;

                int h = Math.Abs(hash.GetHashCode());

                if (useGameSeed) h += Math.Abs(HighLogic.CurrentGame.Seed);

                HeadInfo.hash = h.ToString();

                return h;
            }
        }
    }
}
