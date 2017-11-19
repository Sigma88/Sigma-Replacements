using UnityEngine;


namespace SigmaReplacements
{
    namespace SkyBox
    {
        public static class DefaultSkyBox
        {
            internal static Texture XP;
            internal static Texture XN;
            static Texture YP;
            static Texture YN;
            static Texture ZP;
            static Texture ZN;
            static QuaternionD initRot;

            public static void Reset(this GalaxyCubeControl skybox)
            {
                KSCTriggers.skip = false;

                if (skybox != null)
                {
                    if (initRot == null)
                        initRot = skybox.initRot;
                    else
                        skybox.initRot = initRot;

                    Renderer[] renderers = skybox?.gameObject?.GetComponentsInChildren<Renderer>();

                    for (int i = 0; i < renderers?.Length; i++)
                    {
                        string name = renderers[i]?.name;
                        Material material = renderers[i]?.material;
                        if (material == null) continue;

                        if (name == "XP")
                        {
                            if (XP == null)
                                XP = material.mainTexture;
                            else
                                material.mainTexture = XP;
                        }

                        else

                        if (name == "XN")
                        {
                            if (XN == null)
                                XN = material.mainTexture;
                            else
                                material.mainTexture = XN;
                        }

                        else

                        if (name == "YP")
                        {
                            if (YP == null)
                                YP = material.mainTexture;
                            else
                                material.mainTexture = YP;
                        }

                        else

                        if (name == "YN")
                        {
                            if (YN == null)
                                YN = material.mainTexture;
                            else
                                material.mainTexture = YN;
                        }

                        else

                        if (name == "ZP")
                        {
                            if (ZP == null)
                                ZP = material.mainTexture;
                            else
                                material.mainTexture = ZP;
                        }

                        else

                        if (name == "ZN")
                        {
                            if (ZN == null)
                                ZN = material.mainTexture;
                            else
                                material.mainTexture = ZN;
                        }
                    }
                }
            }
        }
    }
}
