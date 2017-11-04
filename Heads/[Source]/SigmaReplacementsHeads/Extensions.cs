using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using KSP.UI;
using KSP.UI.Screens;
using KSP.UI.TooltipTypes;


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

                        material.mainTexture = newTex;
                    }
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

                        material.SetTexture("_BumpMap", newTex);
                    }
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

            internal static int crewLimit(this AstronautComplex complex)
            {
                FieldInfo crewLimit = typeof(AstronautComplex).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(k => k.Name == "crewLimit");

                try
                {
                    return (int)crewLimit.GetValue(complex);
                }
                catch
                {
                    return int.MaxValue;
                }
            }

            internal static CrewListItem crewListItem(this ProtoCrewMember kerbal)
            {
                FieldInfo crew = typeof(CrewListItem).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(k => k.FieldType == typeof(ProtoCrewMember));
                Debug.Log("Kerbal.crewListItem", "Kerbal reflection = " + crew);
                AstronautComplex ac = Resources.FindObjectsOfTypeAll<AstronautComplex>().FirstOrDefault();
                Debug.Log("Kerbal.crewListItem", "AstronautComplex = " + ac);
                if (crew == null || ac == null) return null;

                UIList[] list = new[] { ac?.ScrollListApplicants, ac?.ScrollListAssigned, ac?.ScrollListAvailable, ac?.ScrollListKia };
                CrewListItem item = null;

                for (int i = 0; i < list?.Length; i++)
                {
                    item = list[i]?.GetUiListItems()?.FirstOrDefault(k => crew.GetValue(k.GetComponent<CrewListItem>()) == kerbal)?.GetComponent<CrewListItem>();
                    if (item != null) break;
                }

                Debug.Log("Kerbal.crewListItem", "Item = " + item);
                return item;
            }

            internal static TooltipController_CrewAC GetTooltip(this CrewListItem listItem)
            {
                return listItem?.GetComponent<TooltipController_CrewAC>();
            }

            internal static string PrintFor(this string s, ProtoCrewMember kerbal)
            {
                return s
                    .Replace("&br;", "\n")
                    .Replace("&name;", kerbal.name)
                    .Replace("&trait;", kerbal.trait)
                    .Replace("&seed;", "" + HighLogic.CurrentGame.Seed)
                    .Replace("&visited;", "" + (kerbal?.careerLog?.Entries?.Select(e => e.target)?.Where(t => !string.IsNullOrEmpty(t))?.Distinct()?.Count() ?? 0))
                    .Replace("&missions;", "" + (kerbal?.careerLog?.Entries?.Select(e => e.flight)?.Distinct()?.Count() ?? 0))
                    .GetHashColor();
            }

            internal static string GetHashColor(this string s)
            {
                int start = 0;

                while (s.Substring(start).Contains("&Color"))
                {
                    start = s.IndexOf("&Color");
                    int end = s.Substring(start).IndexOf(";") + 1;
                    if (end > 9)
                    {
                        int add = 0;
                        switch (s.Substring(start + 6, 2))
                        {
                            case "Lo":
                                break;
                            case "Hi":
                                add = 80;
                                break;
                            default:
                                start++;
                                continue;
                        }
                        string text = s.Substring(start, end);
                        int hash = Math.Abs(text.GetHashCode());
                        string color = "#";
                        for (int i = 0; i < 3; i++)
                        {
                            color += (hash % 176 + add).ToString("X");
                            hash = Math.Abs(hash.ToString().GetHashCode());
                        }
                        s = s.Replace(text, "<color=" + color + ">");
                    }
                    else
                    {
                        continue;
                    }
                }
                return s;
            }
        }
    }
}
