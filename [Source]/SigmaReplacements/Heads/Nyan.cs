using System.Resources;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Heads
    {
        internal class Nyan
        {
            internal static bool nyan = false;
            internal static bool forever = false;
            static Texture head;

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
                        ResourceManager temp = new ResourceManager("SigmaReplacements.Heads.Nyan", typeof(Nyan).Assembly);
                        resourceMan = temp;
                    }
                    return resourceMan;
                }
            }

            internal static Texture nyanHead
            {
                get
                {
                    if (head == null)
                    {
                        byte[] bytes = (byte[])ResourceManager.GetObject("nyanHead");
                        head = bytes.ToDDS();
                    }

                    return head;
                }
            }
        }
    }
}
