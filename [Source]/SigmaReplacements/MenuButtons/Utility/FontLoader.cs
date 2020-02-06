using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;


namespace SigmaReplacements
{
    namespace MenuButtons
    {
        internal static class FontLoader
        {
            static Dictionary<string, KeyValuePair<Texture2D, TMP_Glyph[]>> AssetDictionary = new Dictionary<string, KeyValuePair<Texture2D, TMP_Glyph[]>>();

            internal static void ChangeFont(this TextMeshPro text, string newFontName)
            {
                if (text?.font == null)
                    return;

                if (text.font.name == newFontName)
                    return;

                Texture2D Atlas = null;
                TMP_Glyph[] Glyphs = null;

                GetFontAssets(newFontName, out Atlas, out Glyphs);

                if (Atlas != null && Glyphs?.Length > 0)
                {
                    TMP_FontAsset newFont = DuplicateFontAsset(text.font);
                    newFont.atlas = Atlas;
                    newFont.AddGlyphInfo(Glyphs);
                    newFont.material.mainTexture = Atlas;
                    text.font = newFont;
                }
            }

            static TMP_FontAsset DuplicateFontAsset(TMP_FontAsset original)
            {
                TMP_FontAsset fa = ScriptableObject.CreateInstance<TMP_FontAsset>();
                TMP_FontAsset o = original;

                fa.atlas = o.atlas;
                fa.boldSpacing = o.boldSpacing;
                fa.boldStyle = o.boldStyle;

                fa.fallbackFontAssets = o.fallbackFontAssets;
                fa.fontAssetType = o.fontAssetType;
                fa.fontCreationSettings = o.fontCreationSettings;
                fa.AddFaceInfo(o.fontInfo);
                fa.fontWeights = o.fontWeights;
                fa.italicStyle = o.italicStyle;
                fa.AddKerningInfo(o.kerningInfo);
                fa.material = DuplicateMaterial(o.material);
                fa.materialHashCode = o.materialHashCode;
                fa.name = o.name + "(Copy)";
                fa.normalSpacingOffset = o.normalSpacingOffset;
                fa.normalStyle = o.normalStyle;
                fa.tabSize = o.tabSize;

                //Glyph dic
                TMP_Glyph[] tmpg = new TMP_Glyph[o.characterDictionary.Count];
                int i = 0;
                foreach (KeyValuePair<int, TMP_Glyph> k in o.characterDictionary)
                {
                    tmpg[i] = k.Value;
                    i++;
                }
                fa.AddGlyphInfo(tmpg);

                TMP_Glyph gl = new TMP_Glyph();

                return fa;
            }

            static Material DuplicateMaterial(Material original)
            {
                Material m = new Material(original);
                return m;
            }

            static void GetFontAssets(string fontName, out Texture2D Atlas, out TMP_Glyph[] Glyphs)
            {
                Atlas = null;
                Glyphs = null;

                if (AssetDictionary.ContainsKey(fontName))
                {
                    Atlas = AssetDictionary[fontName].Key;
                    Glyphs = AssetDictionary[fontName].Value;
                }

                else

                if (Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault(f => f.name == fontName) is TMP_FontAsset fontAsset)
                {
                    Atlas = fontAsset.atlas;
                    Glyphs = fontAsset.characterDictionary.Values.ToArray();

                    AssetDictionary.Add(fontName, new KeyValuePair<Texture2D, TMP_Glyph[]>(Atlas, Glyphs));
                }

                else

                if (File.Exists(fontName))
                {
                    FontFromStringArray(File.ReadAllLines(fontName), out Atlas, out Glyphs);

                    AssetDictionary.Add(fontName, new KeyValuePair<Texture2D, TMP_Glyph[]>(Atlas, Glyphs));
                }
            }

            static void FontFromStringArray(string[] asset, out Texture2D Atlas, out TMP_Glyph[] Glyphs)
            {
                Atlas = null;
                Glyphs = null;

                List<TMP_Glyph> GlyphsList = new List<TMP_Glyph>();

                for (int i = 0; i < asset.Length; i++)
                {
                    if (asset[i].StartsWith("  _typelessdata: "))
                    {
                        string hex = asset[i].Replace("  _typelessdata: ", "");

                        Atlas = TextureFromString(hex);
                    }

                    if (Glyphs == null && asset[i].StartsWith("  - id:"))
                    {
                        GlyphsList.Add(GlyphFromStringArray(asset, i));
                    }
                }

                if (GlyphsList.Count > 0)
                {
                    Glyphs = GlyphsList.ToArray();
                }
            }

            static Texture2D TextureFromString(string hex)
            {
                Texture2D texture = null;
                int size = (int)Mathf.Sqrt(hex.Length * 0.5f);

                if (hex.Length == size * size * 2)
                {
                    texture = new Texture2D(size, size);
                    texture.SetPixels32(ColorArrayFromString(hex));
                    texture.Apply();
                    Object.DontDestroyOnLoad(texture);
                }
                else
                {
                    Debug.Log("TextureFromString", "String of length " + hex.Length + " cannot be a square image.");
                }

                return texture;
            }

            static Color32[] ColorArrayFromString(string hex)
            {
                Color32[] array = new Color32[hex.Length >> 1];

                for (int i = 0; i < hex.Length >> 1; ++i)
                {
                    byte b = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
                    array[i] = new Color32(0, 0, 0, b);
                }

                return array;
            }

            static int GetHexVal(char hex)
            {
                int val = (int)hex;
                return val - (val < 58 ? 48 : 87);
            }

            static TMP_Glyph GlyphFromStringArray(string[] asset, int i)
            {
                TMP_Glyph Glyph = new TMP_Glyph
                {
                    id = int.Parse(asset[i + 0].Split(':')[1]),
                    x = float.Parse(asset[i + 1].Split(':')[1]),
                    y = float.Parse(asset[i + 2].Split(':')[1]),
                    width = float.Parse(asset[i + 3].Split(':')[1]),
                    height = float.Parse(asset[i + 4].Split(':')[1]),
                    xOffset = float.Parse(asset[i + 5].Split(':')[1]),
                    yOffset = float.Parse(asset[i + 6].Split(':')[1]),
                    xAdvance = float.Parse(asset[i + 7].Split(':')[1]),
                    scale = float.Parse(asset[i + 8].Split(':')[1])
                };

                return Glyph;
            }
        }
    }
}
