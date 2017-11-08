using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    internal static class Extensions
    {
        internal static List<Info> Order(this List<Info> List)
        {
            Debug.Log(List?.GetType()?.Name + ".Order", "Total count = " + List?.Count);

            List<Info> DataBase = new List<Info>();

            DataBase.AddRange(List.Where(h => !string.IsNullOrEmpty(h?.name) && !string.IsNullOrEmpty(h?.collection)));
            DataBase.AddRange(List.Where(h => !string.IsNullOrEmpty(h?.name) && string.IsNullOrEmpty(h?.collection)));
            DataBase.AddRange(List.Where(h => string.IsNullOrEmpty(h?.name) && !string.IsNullOrEmpty(h?.collection)));
            DataBase.AddRange(List.Where(h => h != null && string.IsNullOrEmpty(h?.name) && string.IsNullOrEmpty(h?.collection)));

            Debug.Log(List?.GetType()?.Name + ".Order", "Valid count = " + List?.Count);

            return DataBase;
        }

        internal static void Load(this CrewMember[] array, ConfigNode node, int index)
        {
            CrewMember kerbal = array[index];
            Info stats = new Info(node.GetNode("Stats") ?? new ConfigNode(), new ConfigNode());

            array[index] = new CrewMember
            (
                stats?.rosterStatus ?? kerbal.type,
                !string.IsNullOrEmpty(stats?.name) ? stats.name : kerbal.name,
                stats?.gender ?? kerbal.gender,
                stats?.trait?.Length > 0 && !string.IsNullOrEmpty(stats.trait[0]) ? stats.trait[0] : kerbal.trait,
                stats?.veteran ?? kerbal.veteran,
                stats?.isBadass ?? kerbal.isBadass,
                stats?.courage ?? kerbal.courage,
                stats?.stupidity ?? kerbal.stupidity,
                stats?.experienceLevel ?? kerbal.experienceLevel
            );
        }

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
            if (material != null && newTex != null)
            {
                material.shader = Shader.Find("Bumped Diffuse");

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
            string hash = Info.hash;

            if (string.IsNullOrEmpty(hash)) hash = kerbal.name;

            int h = Math.Abs(hash.GetHashCode());

            if (useGameSeed && HighLogic.CurrentGame != null) h += Math.Abs(HighLogic.CurrentGame.Seed);

            Info.hash = h.ToString();

            return h;
        }
    }
}
