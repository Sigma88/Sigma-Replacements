using System.Resources;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Heads
    {
        internal class Nyan
        {
            private static ResourceManager resourceMan;

            internal Nyan()
            {
            }

            internal static ResourceManager ResourceManager
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
