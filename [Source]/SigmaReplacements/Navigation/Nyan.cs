using System.Resources;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Navigation
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
                        ResourceManager temp = new ResourceManager("SigmaReplacements.Navigation.Nyan", typeof(Nyan).Assembly);
                        resourceMan = temp;
                    }
                    return resourceMan;
                }
            }

            internal static Texture2D nyanBall
            {
                get
                {
                    byte[] bytes = (byte[])ResourceManager.GetObject("nyanBall");
                    return bytes.ToDDS();
                }
            }
        }
    }
}
