using System.Resources;
using UnityEngine;


namespace SigmaReplacements
{
    namespace SkyBox
    {
        internal class Nyan
        {
            internal static bool nyan = false;
            internal static bool forever = false;
            static Texture frame0;
            static Texture frame1;
            static Texture frame2;
            static Texture frame3;

            static ResourceManager resourceMan;

            Nyan()
            {
            }

            static ResourceManager ResourceManager
            {
                get
                {
                    if (ReferenceEquals(resourceMan, null))
                    {
                        ResourceManager temp = new ResourceManager("SigmaReplacements.SkyBox.Nyan", typeof(Nyan).Assembly);
                        resourceMan = temp;
                    }
                    return resourceMan;
                }
            }

            internal static Texture[] nyanSkyBox
            {
                get
                {
                    if (frame0 == null)
                    {
                        byte[] bytes0 = (byte[])ResourceManager.GetObject("nyanXP0");
                        frame0 = bytes0.ToDDS();
                    }
                    if (frame1 == null)
                    {
                        byte[] bytes1 = (byte[])ResourceManager.GetObject("nyanXP1");
                        frame1 = bytes1.ToDDS();
                    }
                    if (frame2 == null)
                    {
                        byte[] bytes2 = (byte[])ResourceManager.GetObject("nyanXP2");
                        frame2 = bytes2.ToDDS();
                    }
                    if (frame3 == null)
                    {
                        byte[] bytes3 = (byte[])ResourceManager.GetObject("nyanXP3");
                        frame3 = bytes3.ToDDS();
                    }

                    return new Texture[] { frame0, frame1, frame2, frame3 };
                }
            }
        }
    }
}
