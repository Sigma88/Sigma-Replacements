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
            static Texture suit;
            static Texture helmet;

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
                        ResourceManager temp = new ResourceManager("SigmaReplacements.Suits.Nyan", typeof(Nyan).Assembly);
                        resourceMan = temp;
                    }
                    return resourceMan;
                }
            }

            internal static Texture nyanHelmet
            {
                get
                {
                    if (helmet == null)
                    {
                        byte[] bytes = (byte[])ResourceManager.GetObject("nyanHelmet");
                        helmet = bytes.ToDDS();
                    }

                    return helmet;
                }
            }

            internal static Texture nyanSuit
            {
                get
                {
                    if (suit == null)
                    {
                        byte[] bytes = (byte[])ResourceManager.GetObject("nyanSuit");
                        suit = bytes.ToDDS();
                    }

                    return suit;
                }
            }
        }
    }
}
