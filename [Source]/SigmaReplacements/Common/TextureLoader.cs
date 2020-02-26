using System;
using System.IO;
using DDSHeaders;
using UnityEngine;


namespace SigmaReplacements
{
    internal static class TextureLoader
    {
        internal static Texture2D Load(string name)
        {
            string path = KSPUtil.ApplicationRootPath + "GameData/" + name;

            Texture2D texture = null;

            if (File.Exists(path))
            {
                if (name.EndsWith(".dds", StringComparison.InvariantCultureIgnoreCase))
                {
                    texture = LoadDDS(path);
                }
                else
                {
                    texture = LoadImage(path);
                }

                if (texture != null)
                {
                    texture.name = name;
                }
            }

            return texture;
        }

        static Texture2D LoadImage(string path)
        {
            Texture2D texture = new Texture2D(1, 1);

            if (ImageConversion.LoadImage(texture, File.ReadAllBytes(path), true))
            {
                return texture;
            }

            return null;
        }

        /// This method has been adapted from Kopernicus and it is licensed LPGL.
        /// (https://github.com/Kopernicus/Kopernicus/blob/master/src/Kopernicus/OnDemand/OnDemandStorage.cs)
        /// <summary>
        /// Loads a texture directly from the DDS file.
        /// </summary>
        static Texture2D LoadDDS(string path)
        {
            try
            {
                // Borrowed from stock KSP 1.0 DDS loader (hi Mike!)
                // Also borrowed the extra bits from Sarbian.
                BinaryReader binaryReader = new BinaryReader(File.OpenRead(path));
                UInt32 num = binaryReader.ReadUInt32();

                if (num == DDSValues.uintMagic)
                {
                    Texture2D map = null;
                    DDSHeader ddsHeader = new DDSHeader(binaryReader);

                    if (ddsHeader.ddspf.dwFourCC == DDSValues.uintDX10)
                    {
                        // ReSharper disable once ObjectCreationAsStatement
                        new DDSHeaderDX10(binaryReader);
                    }

                    Boolean alpha = (ddsHeader.ddspf.dwFlags & 0x00000002) != 0;
                    Boolean fourcc = (ddsHeader.ddspf.dwFlags & 0x00000004) != 0;
                    Boolean rgb = (ddsHeader.ddspf.dwFlags & 0x00000040) != 0;
                    Boolean alphapixel = (ddsHeader.ddspf.dwFlags & 0x00000001) != 0;
                    Boolean luminance = (ddsHeader.ddspf.dwFlags & 0x00020000) != 0;
                    Boolean rgb888 = ddsHeader.ddspf.dwRBitMask == 0x000000ff &&
                                     ddsHeader.ddspf.dwGBitMask == 0x0000ff00 &&
                                     ddsHeader.ddspf.dwBBitMask == 0x00ff0000;
                    Boolean rgb565 = ddsHeader.ddspf.dwRBitMask == 0x0000F800 &&
                                     ddsHeader.ddspf.dwGBitMask == 0x000007E0 &&
                                     ddsHeader.ddspf.dwBBitMask == 0x0000001F;
                    Boolean argb4444 = ddsHeader.ddspf.dwABitMask == 0x0000f000 &&
                                       ddsHeader.ddspf.dwRBitMask == 0x00000f00 &&
                                       ddsHeader.ddspf.dwGBitMask == 0x000000f0 &&
                                       ddsHeader.ddspf.dwBBitMask == 0x0000000f;
                    Boolean rbga4444 = ddsHeader.ddspf.dwABitMask == 0x0000000f &&
                                       ddsHeader.ddspf.dwRBitMask == 0x0000f000 &&
                                       ddsHeader.ddspf.dwGBitMask == 0x000000f0 &&
                                       ddsHeader.ddspf.dwBBitMask == 0x00000f00;

                    Boolean mipmap = (ddsHeader.dwCaps & DDSPixelFormatCaps.MIPMAP) != 0u;


                    Byte[] data = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);

                    if (fourcc)
                    {
                        if (ddsHeader.ddspf.dwFourCC == DDSValues.uintDXT1)
                        {
                            map = new Texture2D((Int32)ddsHeader.dwWidth, (Int32)ddsHeader.dwHeight,
                                TextureFormat.DXT1, mipmap);
                            map.LoadRawTextureData(data);
                        }
                        else if (ddsHeader.ddspf.dwFourCC == DDSValues.uintDXT3)
                        {
                            map = new Texture2D((Int32)ddsHeader.dwWidth, (Int32)ddsHeader.dwHeight,
                                (TextureFormat)11, mipmap);
                            map.LoadRawTextureData(data);
                        }
                        else if (ddsHeader.ddspf.dwFourCC == DDSValues.uintDXT5)
                        {
                            map = new Texture2D((Int32)ddsHeader.dwWidth, (Int32)ddsHeader.dwHeight,
                                TextureFormat.DXT5, mipmap);
                            map.LoadRawTextureData(data);
                        }
                        else if (ddsHeader.ddspf.dwFourCC == DDSValues.uintDXT2)
                        {
                            UnityEngine.Debug.Log("[SigmaLog SR] LoadDDS: DXT2 not supported" + path);
                        }
                        else if (ddsHeader.ddspf.dwFourCC == DDSValues.uintDXT4)
                        {
                            UnityEngine.Debug.Log("[SigmaLog SR] LoadDDS: DXT4 not supported: " + path);
                        }
                        else if (ddsHeader.ddspf.dwFourCC == DDSValues.uintDX10)
                        {
                            UnityEngine.Debug.Log("[SigmaLog SR] LoadDDS: DX10 dds not supported: " + path);
                        }
                        else if (ddsHeader.ddspf.dwFourCC == DDSValues.uintMagic)
                        {
                            UnityEngine.Debug.Log("[SigmaLog SR] LoadDDS: Magic dds not supported: " + path);
                        }
                        else if (ddsHeader.ddspf.dwRGBBitCount == 4 || ddsHeader.ddspf.dwRGBBitCount == 8)
                        {
                            try
                            {
                                Int32 bpp = (Int32)ddsHeader.ddspf.dwRGBBitCount;
                                Int32 colors = (Int32)Math.Pow(2, bpp);
                                Int32 width = (Int32)ddsHeader.dwWidth;
                                Int32 height = (Int32)ddsHeader.dwHeight;
                                Int64 length = new FileInfo(path).Length;
                                Int32 pixels = width * height * bpp / 8 + 4 * colors;

                                if (length - 128 >= pixels)
                                {
                                    Color[] palette = new Color[colors];
                                    Color[] image = new Color[width * height];

                                    for (Int32 i = 0; i < 4 * colors; i += 4)
                                    {
                                        palette[i / 4] = new Color32(data[i + 0], data[i + 1], data[i + 2],
                                            data[i + 3]);
                                    }

                                    for (Int32 i = 4 * colors; i < data.Length; i++)
                                    {
                                        image[(i - 4 * colors) * 8 / bpp] = palette[data[i] * colors / 256];
                                        if (bpp == 4)
                                        {
                                            image[(i - 64) * 2 + 1] = palette[data[i] % 16];
                                        }
                                    }

                                    map = new Texture2D(width, height, TextureFormat.ARGB32, false);
                                    map.SetPixels(image);
                                }
                                else
                                {
                                    fourcc = false;
                                }
                            }
                            catch
                            {
                                fourcc = false;
                            }
                        }
                        else
                        {
                            fourcc = false;
                        }
                    }

                    if (!fourcc)
                    {
                        TextureFormat textureFormat = TextureFormat.ARGB32;
                        Boolean ok = true;
                        if (rgb && rgb888)
                        {
                            // RGB or RGBA format
                            textureFormat = alphapixel
                                ? TextureFormat.RGBA32
                                : TextureFormat.RGB24;
                        }
                        else if (rgb && rgb565)
                        {
                            // Nvidia texconv B5G6R5_UNORM
                            textureFormat = TextureFormat.RGB565;
                        }
                        else if (rgb && alphapixel && argb4444)
                        {
                            // Nvidia texconv B4G4R4A4_UNORM
                            textureFormat = TextureFormat.ARGB4444;
                        }
                        else if (rgb && alphapixel && rbga4444)
                        {
                            textureFormat = TextureFormat.RGBA4444;
                        }
                        else if (!rgb && alpha != luminance && (ddsHeader.ddspf.dwRGBBitCount == 8 || ddsHeader.ddspf.dwRGBBitCount == 16))
                        {
                            if (ddsHeader.ddspf.dwRGBBitCount == 8)
                            {
                                // A8 format or Luminance 8
                                if (alpha)
                                    textureFormat = TextureFormat.Alpha8;
                                else
                                    textureFormat = TextureFormat.R8;
                            }
                            else if (ddsHeader.ddspf.dwRGBBitCount == 16)
                            {
                                // R16 format
                                textureFormat = TextureFormat.R16;
                            }
                        }
                        else
                        {
                            ok = false;
                            UnityEngine.Debug.Log("[SigmaLog SR] LoadDDS: Only DXT1, DXT5, A8, R8, R16, RGB24, RGBA32, RGB565, ARGB4444, RGBA4444, 4bpp palette and 8bpp palette are supported");
                            return null;
                        }

                        if (ok)
                        {
                            map = new Texture2D((Int32)ddsHeader.dwWidth, (Int32)ddsHeader.dwHeight,
                                textureFormat, mipmap);
                            map.LoadRawTextureData(data);
                        }

                    }

                    if (map != null)
                    {
                        map.Apply();
                    }

                    return map;
                }
                else
                {
                    UnityEngine.Debug.Log("[SigmaLog SR] LoadDDS: Bad DDS header.");
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
