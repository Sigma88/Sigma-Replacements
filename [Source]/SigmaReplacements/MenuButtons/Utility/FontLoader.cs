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
            static Dictionary<string, FontComponents> AssetDictionary = new Dictionary<string, FontComponents>();

            internal static void ChangeFont(this TextMeshPro text, string newFont)
            {
                Debug.Log("ChangeFont", "text = " + text + ", newFont = " + newFont);

                if (text?.font == null)
                    return;

                Debug.Log("ChangeFont", "oldFont = " + text.font.name);

                if (text.font.name == newFont)
                    return;

                GetFontAssets(newFont, out Texture2D Atlas, out FaceInfo FontInfo, out TMP_Glyph[] Glyphs);

                Debug.Log("ChangeFont", "Atlas = " + Atlas + ", Glyphs = " + Glyphs?.Length);

                if (Atlas != null && Glyphs?.Length > 0)
                {
                    TMP_FontAsset newFontAsset = DuplicateFontAsset(text.font);
                    newFontAsset.name = newFont;
                    newFontAsset.atlas = Atlas;
                    newFontAsset.material.mainTexture = Atlas;
                    newFontAsset.AddFaceInfo(FontInfo);
                    newFontAsset.AddGlyphInfo(Glyphs);
                    text.font = newFontAsset;

                    Debug.Log("ChangeFont", "New Font = " + text.font.name);
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

            static void GetFontAssets(string newFont, out Texture2D Atlas, out FaceInfo FontInfo, out TMP_Glyph[] Glyphs)
            {
                Atlas = null;
                FontInfo = null;
                Glyphs = null;

                if (AssetDictionary.ContainsKey(newFont))
                {
                    Atlas = AssetDictionary[newFont].Atlas;
                    FontInfo = AssetDictionary[newFont].FontInfo;
                    Glyphs = AssetDictionary[newFont].Glyphs;
                }

                else

                if (Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault(f => f.name == newFont) is TMP_FontAsset fontAsset)
                {
                    Atlas = fontAsset.atlas;
                    FontInfo = fontAsset.fontInfo;
                    Glyphs = fontAsset.characterDictionary.Values.ToArray();

                    AssetDictionary.Add(newFont, new FontComponents(Atlas, FontInfo, Glyphs));
                }

                else

                if (File.Exists("GameData/" + newFont))
                {
                    FontFromStringArray(File.ReadAllLines("GameData/" + newFont), out Atlas, out FontInfo, out Glyphs);

                    AssetDictionary.Add(newFont, new FontComponents(Atlas, FontInfo, Glyphs));
                }
            }

            static void FontFromStringArray(string[] asset, out Texture2D Atlas, out FaceInfo FontInfo, out TMP_Glyph[] Glyphs)
            {
                Atlas = null;
                FontInfo = null;
                Glyphs = null;

                List<TMP_Glyph> GlyphsList = new List<TMP_Glyph>();

                for (int i = 0; i < asset.Length; i++)
                {
                    if (asset[i].StartsWith("  _typelessdata: "))
                    {
                        string hex = asset[i].Replace("  _typelessdata: ", "");

                        Atlas = TextureFromString(hex);
                    }

                    if (asset[i] == "  m_fontInfo:")
                    {
                        FontInfo = FaceInfoFromStringArray(asset, i);
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

            static FaceInfo FaceInfoFromStringArray(string[] asset, int i)
            {
                FaceInfo FontInfo = new FaceInfo
                {
                    Name = asset[i + 1].Split(':')[1],
                    PointSize = float.Parse(asset[i + 2].Split(':')[1]),
                    Scale = float.Parse(asset[i + 3].Split(':')[1]),
                    CharacterCount = int.Parse(asset[i + 4].Split(':')[1]),
                    LineHeight = float.Parse(asset[i + 5].Split(':')[1]),
                    Baseline = float.Parse(asset[i + 6].Split(':')[1]),
                    Ascender = float.Parse(asset[i + 7].Split(':')[1]),
                    CapHeight = float.Parse(asset[i + 8].Split(':')[1]),
                    Descender = float.Parse(asset[i + 9].Split(':')[1]),
                    CenterLine = float.Parse(asset[i + 10].Split(':')[1]),
                    SuperscriptOffset = float.Parse(asset[i + 11].Split(':')[1]),
                    SubscriptOffset = float.Parse(asset[i + 12].Split(':')[1]),
                    SubSize = float.Parse(asset[i + 13].Split(':')[1]),
                    Underline = float.Parse(asset[i + 14].Split(':')[1]),
                    UnderlineThickness = float.Parse(asset[i + 15].Split(':')[1]),
                    strikethrough = float.Parse(asset[i + 16].Split(':')[1]),
                    strikethroughThickness = float.Parse(asset[i + 17].Split(':')[1]),
                    TabWidth = float.Parse(asset[i + 18].Split(':')[1]),
                    Padding = float.Parse(asset[i + 19].Split(':')[1]),
                    AtlasWidth = float.Parse(asset[i + 20].Split(':')[1]),
                    AtlasHeight = float.Parse(asset[i + 21].Split(':')[1])
                };

                return FontInfo;
            }
        }

        internal class FontComponents
        {
            internal Texture2D Atlas;
            internal FaceInfo FontInfo;
            internal TMP_Glyph[] Glyphs;

            internal FontComponents(Texture2D Atlas, FaceInfo FontInfo, TMP_Glyph[] Glyphs)
            {
                this.Atlas = Atlas;
                this.FontInfo = FontInfo;
                this.Glyphs = Glyphs;
            }
        }
    }
}
