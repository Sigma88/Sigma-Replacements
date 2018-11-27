using System.Resources;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class Nyan
        {
            internal static bool nyan = false;
            internal static bool forever = false;
            static Texture ground;

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
                        ResourceManager temp = new ResourceManager("SigmaReplacements.MenuScenes" +
                            ".Nyan", typeof(Nyan).Assembly);
                        resourceMan = temp;
                    }
                    return resourceMan;
                }
            }

            internal static Texture nyanGround
            {
                get
                {
                    if (ground == null)
                    {
                        byte[] bytes = (byte[])ResourceManager.GetObject("nyanGround");
                        ground = bytes.ToDDS();
                    }

                    return ground;
                }
            }
        }
    }
}
