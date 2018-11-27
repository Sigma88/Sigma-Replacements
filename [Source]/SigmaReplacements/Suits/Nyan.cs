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
            static Texture suit2;

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

            internal static Texture nyanSuit2
            {
                get
                {
                    if (suit2 == null)
                    {
                        byte[] bytes = (byte[])ResourceManager.GetObject("nyanSuit2");
                        suit2 = bytes.ToDDS();
                    }

                    return suit2;
                }
            }
        }
    }
}
