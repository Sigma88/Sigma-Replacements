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
            Debug.Log("List<" + List?.FirstOrDefault()?.GetType()?.Name + ">.Order", "Total count = " + List?.Count);

            List<Info> DataBase = new List<Info>();

            DataBase.AddRange(List.Where(h => !string.IsNullOrEmpty(h?.name) && !string.IsNullOrEmpty(h?.collection)));
            DataBase.AddRange(List.Where(h => !string.IsNullOrEmpty(h?.name) && string.IsNullOrEmpty(h?.collection)));
            DataBase.AddRange(List.Where(h => string.IsNullOrEmpty(h?.name) && !string.IsNullOrEmpty(h?.collection)));
            DataBase.AddRange(List.Where(h => h != null && string.IsNullOrEmpty(h?.name) && string.IsNullOrEmpty(h?.collection)));

            Debug.Log("List<" + List?.FirstOrDefault()?.GetType()?.Name + ">.Order", "Valid count = " + DataBase?.Count);

            return DataBase;
        }

        internal static void SetColor(this Material material, Color? color)
        {
            if (material != null && color != null)
            {
                if (material.shader?.name == "Mobile/Diffuse")
                    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                material.color = (Color)color;
            }
        }

        internal static void SetTexture(this Material material, Texture newTex, bool flip = false)
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

                if (flip)
                {
                    Vector2 scale = material.GetTextureScale("_MainTex");
                    scale.x *= -1;
                    material.SetTextureScale("_MainTex", scale);
                }
            }
        }

        internal static void SetNormal(this Material material, Texture newTex)
        {
            if (material != null && newTex != null)
            {
                if (!material.HasProperty("_BumpMap"))
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

        internal static void SetEmissive(this Material material, Texture newTex, bool flip = false)
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

                if (flip)
                {
                    Vector2 scale = material.GetTextureScale("_Emissive");
                    scale.x *= -1;
                    material.SetTextureScale("_Emissive", scale);
                }
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

        internal static void SetTexture(this Image image, Texture newTex, Image stockImage = null, bool fit = false, bool resize = false, Vector2? resolution = null)
        {
            if (image?.sprite != null)
            {
                Vector2 scale = new Vector2(1, 1);
                if (stockImage?.sprite?.rect != null)
                {
                    scale.x = stockImage.sprite.rect.width;
                    scale.y = stockImage.sprite.rect.height;
                }

                image.sprite = image.sprite.SetTexture(newTex ?? stockImage?.sprite?.texture, fit);

                if (resize)
                {
                    scale.x /= image.sprite.rect.width / (resolution?.x ?? 1);
                    scale.y /= image.sprite.rect.height / (resolution?.y ?? 1);

                    if (stockImage?.rectTransform?.localScale != null)
                        image.rectTransform.localScale = stockImage.rectTransform.localScale;

                    image.rectTransform.localScale = new Vector3(image.rectTransform.localScale.x / scale.x, image.rectTransform.localScale.y / scale.y, image.rectTransform.localScale.z);
                }
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

        internal static Sprite SetTexture(this Sprite sprite, Texture newTex, bool fit = false)
        {
            if (sprite == null || newTex == null) return sprite;
            if (sprite.texture == newTex) return sprite;

            Texture oldTex = sprite?.texture;

            if (oldTex != null)
            {
                newTex.anisoLevel = oldTex.anisoLevel;
                newTex.wrapMode = oldTex.wrapMode;
            }

            Rect rect = sprite.rect;
            Vector2 pivot = sprite.pivot;

            if (fit)
            {
                rect.width = newTex.width;
                rect.height = newTex.height;
                pivot = new Vector2(newTex.width * 0.5f, newTex.height * 0.5f);
            }

            return Sprite.Create((Texture2D)newTex, rect, pivot);
        }

        internal static Color? At(this List<Color?> list, Color? element, List<Color?> reference, ProtoCrewMember kerbal, bool useGameSeed, string name = "")
        {
            if (reference.Contains(element) && list?.Count > reference.IndexOf(element))
                return list[reference.IndexOf(element)];
            else
                return list.Pick(kerbal, useGameSeed, name);
        }

        internal static Texture At(this List<Texture> list, Texture element, List<Texture> reference, ProtoCrewMember kerbal, bool useGameSeed, string name = "")
        {
            if (reference.Contains(element) && list?.Count > reference.IndexOf(element))
                return list[reference.IndexOf(element)];
            else
                return list.Pick(kerbal, useGameSeed, name);
        }

        internal static Vector2? At(this List<Vector2?> list, Texture element, List<Texture> reference, ProtoCrewMember kerbal, bool useGameSeed, string name = "")
        {
            if (reference.Contains(element) && list?.Count > reference.IndexOf(element))
                return list[reference.IndexOf(element)];
            else
                return list.Pick(kerbal, useGameSeed, name);
        }

        internal static Texture Pick(this List<Texture> list, ProtoCrewMember kerbal, bool useGameSeed, string name = "")
        {
            if (list?.Count > 1 && kerbal != null)
                return list[kerbal.Hash(useGameSeed) % list.Count];
            else if (list?.Count > 0)
                return list[name.Hash(useGameSeed) % list.Count];
            else
                return null;
        }

        internal static Color? Pick(this List<Color?> list, ProtoCrewMember kerbal, bool useGameSeed, string name = "")
        {
            if (list?.Count > 1 && kerbal != null)
                return list[kerbal.Hash(useGameSeed) % list.Count];
            else if (list?.Count > 0)
                return list[name.Hash(useGameSeed) % list.Count];
            else
                return null;
        }

        internal static Vector2? Pick(this List<Vector2?> list, ProtoCrewMember kerbal, bool useGameSeed, string name = "")
        {
            if (list?.Count > 1 && kerbal != null)
                return list[kerbal.Hash(useGameSeed) % list.Count];
            else if (list?.Count > 0)
                return list[name.Hash(useGameSeed) % list.Count];
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

        internal static int Hash(this string name, bool useGameSeed)
        {
            name = name ?? "";

            int h = Math.Abs(name.GetHashCode());

            if (useGameSeed && HighLogic.CurrentGame != null) h += Math.Abs(HighLogic.CurrentGame.Seed);

            return h;
        }

        internal static Texture2D ToDDS(this byte[] bytes)
        {
            try
            {
                if (bytes[4] != 124) return null; //this byte should be 124 for DDS image files

                int height = bytes[13] * 256 + bytes[12];
                int width = bytes[17] * 256 + bytes[16];

                int header = 128;
                byte[] data = new byte[bytes.Length - header];
                Buffer.BlockCopy(bytes, header, data, 0, bytes.Length - header);

                TextureFormat format;
                switch ((double)height * width / data.Length)
                {
                    case 1d:
                        format = TextureFormat.DXT5;
                        break;
                    case 2d:
                        format = TextureFormat.DXT1;
                        break;
                    default:
                        return null;
                }

                Texture2D texture = new Texture2D(width, height, format, false);

                texture.LoadRawTextureData(data);
                texture.Apply();

                return (texture);
            }
            catch
            {
                return null;
            }
        }
    }
}
