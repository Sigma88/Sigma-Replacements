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
                        ResourceManager temp = new ResourceManager("SigmaReplacements.Heads.Nyan", typeof(Nyan).Assembly);
                        resourceMan = temp;
                    }
                    return resourceMan;
                }
            }

            internal static Texture2D nyanHead
            {
                get
                {
                    byte[] bytes = (byte[])ResourceManager.GetObject("nyanHead");
                    return bytes.ToDDS();
                }
            }
        }
    }
}
