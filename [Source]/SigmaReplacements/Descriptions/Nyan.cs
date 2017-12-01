using System;
using System.Resources;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        internal class Nyan : MonoBehaviour
        {
            internal static bool nyan = false;
            internal static bool forever = false;
            static Texture[] sprites = new Texture[18] { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
            static string[] names = new string[18] { "bob", "bunny", "cat", "coin", "game", "grumpy", "jack", "jazz", "mex", "mummy", "ninja", "party", "pirate", "stpat", "tacnayn", "vday", "xmas", "zombie" };

            private static ResourceManager resourceMan;

            private Nyan()
            {
            }

            private static ResourceManager ResourceManager
            {
                get
                {
                    if (ReferenceEquals(resourceMan, null))
                    {
                        ResourceManager temp = new ResourceManager("SigmaReplacements.Descriptions.Nyan", typeof(Nyan).Assembly);
                        resourceMan = temp;
                    }
                    return resourceMan;
                }
            }

            internal static Texture[] nyanSprites
            {
                get
                {
                    for (int i = 0; i < sprites.Length; i++)
                    {
                        if (sprites[i] == null)
                        {
                            byte[] bytes = (byte[])ResourceManager.GetObject(names[i]);
                            sprites[i] = bytes.ToDDS();
                            sprites[i].name = "nyan-" + names[i];
                            DontDestroyOnLoad(sprites[i]);
                        }
                    }

                    return sprites;
                }
            }
        }
    }
}
