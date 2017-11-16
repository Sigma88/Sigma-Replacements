using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using KSP.UI;
using IState = KSP.UI.UIStateImage.ImageState;
using BState = KSP.UI.UIStateToggleButton.ButtonState;


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

        internal static void SetEmissive(this Material material, Texture newTex)
        {
            if (material?.HasProperty("_Emissive") == true && newTex != null)
            {
                Texture oldTex = material.GetTexture("_Emissive");

                if (oldTex != null)
                {
                    newTex.anisoLevel = oldTex.anisoLevel;
                    newTex.wrapMode = oldTex.wrapMode;
                }

                material.SetTexture("_Emissive", newTex);
            }
        }

        internal static void SetTintColor(this Material material, Color? color, Material stockMaterial = null)
        {
            if (material != null)
            {
                if (color != null)
                    material.SetColor("_TintColor", (Color)color);

                else

                if (stockMaterial?.HasProperty("_TintColor") == true)
                    material.SetColor("_TintColor", stockMaterial.GetColor("_TintColor"));
            }
        }

        internal static void SetMainTexture(this Material material, Texture newTex, Material stockMaterial = null)
        {
            if (material != null)
            {
                if (newTex == null)
                    newTex = stockMaterial?.GetTexture("_MainTexture");

                if (newTex == null) return;

                Texture oldTex = material?.GetTexture("_MainTexture");

                if (oldTex == newTex) return;

                if (oldTex != null)
                {
                    newTex.anisoLevel = oldTex.anisoLevel;
                    newTex.wrapMode = oldTex.wrapMode;
                }

                material.SetTexture("_MainTexture", newTex);
            }
        }

        internal static void SetColor(this Image image, Color? color, Image stockImage = null)
        {
            if (image != null)
            {
                image.color = color ?? stockImage?.color ?? image.color;
            }
        }

        internal static void SetTexture(this Image image, Texture newTex, Image stockImage = null)
        {
            if (image?.sprite != null)
            {
                image.sprite = image.sprite.SetTexture(newTex ?? stockImage?.sprite?.texture);
            }
        }

        internal static void SetColor(this UIStateImage button, Color? color, UIStateImage stockButton)
        {
            Image image = button?.GetComponent<Image>();

            if (image != null)
            {
                Image stockImage = stockButton?.GetComponent<Image>();
                image.SetColor(color, stockImage);
            }
        }

        internal static void SetTexture(this UIStateImage button, Texture newTex, UIStateImage stockButton = null)
        {
            for (int i = 0; i < button?.states?.Length; i++)
            {
                IState state = button?.states[i];

                if (state != null)
                {
                    Texture stockTexture = stockButton?.states?.Length > i ? stockButton.states[i]?.sprite?.texture : null;
                    state.sprite = state.sprite.SetTexture(newTex ?? stockTexture);
                }
            }
        }

        internal static void SetColor(this UIStateToggleButton toggle, Color? color, UIStateToggleButton stockButton = null)
        {
            Image image = toggle?.GetComponent<Image>();

            if (image != null && color != null)
            {
                Image stockImage = stockButton?.GetComponent<Image>();
                image.SetColor(color, stockImage);
            }
        }

        internal static void SetTexture(this UIStateToggleButton toggle, Texture newTex, UIStateToggleButton stockToggle = null)
        {
            if (toggle != null)
            {
                BState[] states = { toggle.stateTrue, toggle.stateFalse };
                BState[] stockStates = { stockToggle?.stateTrue, stockToggle?.stateFalse };
                for (int i = 0; i < states?.Length; i++)
                {
                    BState stock = stockStates[i];
                    states[i].disabled = states[i].disabled.SetTexture(newTex ?? stock?.disabled?.texture);
                    states[i].normal = states[i].normal.SetTexture(newTex ?? stock?.normal?.texture);
                    states[i].highlight = states[i].highlight.SetTexture(newTex ?? stock?.highlight?.texture);
                    states[i].pressed = states[i].pressed.SetTexture(newTex ?? stock?.pressed?.texture);
                }
            }
        }

        internal static Sprite SetTexture(this Sprite sprite, Texture newTex)
        {
            if (sprite == null || newTex == null) return sprite;
            if (sprite.texture == newTex) return sprite;

            Texture oldTex = sprite?.texture;

            if (oldTex != null)
            {
                newTex.anisoLevel = oldTex.anisoLevel;
                newTex.wrapMode = oldTex.wrapMode;
            }

            return Sprite.Create((Texture2D)newTex, sprite.rect, sprite.pivot);
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
            if (list.Count > 1 && kerbal != null)
                return list[kerbal.Hash(useGameSeed) % list.Count];
            else if (list.Count > 0)
                return list[0];
            else
                return null;
        }

        internal static Color? Pick(this List<Color> list, ProtoCrewMember kerbal, bool useGameSeed)
        {
            if (list.Count > 1 && kerbal != null)
                return list[kerbal.Hash(useGameSeed) % list.Count];
            else if (list.Count > 0)
                return list[0];
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
