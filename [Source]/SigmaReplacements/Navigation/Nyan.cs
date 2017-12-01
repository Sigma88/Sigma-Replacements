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
            static Texture ball;

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
                        ResourceManager temp = new ResourceManager("SigmaReplacements.Navigation.Nyan", typeof(Nyan).Assembly);
                        resourceMan = temp;
                    }
                    return resourceMan;
                }
            }

            internal static Texture nyanBall
            {
                get
                {
                    if (ball == null)
                    {
                        byte[] bytes = (byte[])ResourceManager.GetObject("nyanBall");
                        ball = bytes.ToDDS();
                    }

                    return ball;
                }
            }
        }
    }
}
