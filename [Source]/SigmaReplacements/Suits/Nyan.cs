using System.Resources;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Suits
    {
        internal class Nyan
        {
            internal static bool nyan = false;
            internal static bool forever = false;

            private static ResourceManager resourceMan;

            private Nyan()
            {
            }

            private static ResourceManager ResourceManager
            {
                get
                {
                    if (object.ReferenceEquals(resourceMan, null))
                    {
                        ResourceManager temp = new ResourceManager("SigmaReplacements.Suits.Nyan", typeof(Nyan).Assembly);
                        resourceMan = temp;
                    }
                    return resourceMan;
                }
            }

            internal static Texture2D nyanHelmet
            {
                get
                {
                    byte[] bytes = (byte[])ResourceManager.GetObject("nyanHelmet");
                    return bytes.ToDDS();
                }
            }

            internal static Texture2D nyanSuit
            {
                get
                {
                    byte[] bytes = (byte[])ResourceManager.GetObject("nyanSuit");
                    return bytes.ToDDS();
                }
            }
        }
    }
}
