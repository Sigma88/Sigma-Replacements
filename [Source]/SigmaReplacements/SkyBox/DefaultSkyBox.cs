using UnityEngine;


namespace SigmaReplacements
{
    namespace SkyBox
    {
        internal static class DefaultSkyBox
        {
            internal static Texture XP;
            internal static Texture XN;
            static Texture YP;
            static Texture YN;
            static Texture ZP;
            static Texture ZN;
            static QuaternionD? initRot = null;
            static Vector3? initScale = null;

            public static void Reset(this GalaxyCubeControl skybox)
            {
                Debug.Log("DefaultSkyBox.Reset", "GalaxyCubeControl = " + skybox);
                KSCTriggers.skip = false;

                if (skybox != null)
                {
                    // Rotation
                    if (initRot == null)
                    {
                        initRot = skybox.initRot;
                        Debug.Log("DefaultSkyBox.Reset", "Saved initRot = " + initRot);
                    }
                    else
                    {
                        skybox.initRot = (QuaternionD)initRot;
                        Debug.Log("DefaultSkyBox.Reset", "Loaded initRot = " + skybox.initRot);
                    }

                    // Scale
                    GameObject cube = skybox.gameObject.GetChild("GalaxyCube");
                    if (cube != null)
                    {
                        if (initScale == null)
                        {
                            initScale = cube.transform.localScale;
                            Debug.Log("DefaultSkyBox.Reset", "Saved initScale = " + initScale);
                        }
                        else
                        {
                            cube.transform.localScale = (Vector3)initScale;
                            Debug.Log("DefaultSkyBox.Reset", "Loaded initScale = " + cube.transform.localScale);
                        }
                    }

                    // Textures
                    Renderer[] renderers = skybox?.gameObject?.GetComponentsInChildren<Renderer>(true);

                    for (int i = 0; i < renderers?.Length; i++)
                    {
                        string name = renderers[i]?.name;
                        Material material = renderers[i]?.material;
                        if (material == null) continue;

                        if (name == "XP")
                        {
                            if (XP == null)
                            {
                                XP = material.mainTexture;
                                Debug.Log("DefaultSkyBox.Reset", "Saved Stock XP texture = " + XP);
                            }
                            else
                            {
                                material.mainTexture = XP;
                                Debug.Log("DefaultSkyBox.Reset", "Loaded Stock XP texture = " + material.mainTexture);
                            }
                        }

                        else

                        if (name == "XN")
                        {
                            if (XN == null)
                            {
                                XN = material.mainTexture;
                                Debug.Log("DefaultSkyBox.Reset", "Saved Stock XN texture = " + XN);
                            }
                            else
                            {
                                material.mainTexture = XN;
                                Debug.Log("DefaultSkyBox.Reset", "Loaded Stock XN texture = " + material.mainTexture);
                            }
                        }

                        else

                        if (name == "YP")
                        {
                            if (YP == null)
                            {
                                YP = material.mainTexture;
                                Debug.Log("DefaultSkyBox.Reset", "Saved Stock YP texture = " + YP);
                            }
                            else
                            {
                                material.mainTexture = YP;
                                Debug.Log("DefaultSkyBox.Reset", "Loaded Stock YP texture = " + material.mainTexture);
                            }
                        }

                        else

                        if (name == "YN")
                        {
                            if (YN == null)
                            {
                                YN = material.mainTexture;
                                Debug.Log("DefaultSkyBox.Reset", "Saved Stock YN texture = " + YN);
                            }
                            else
                            {
                                material.mainTexture = YN;
                                Debug.Log("DefaultSkyBox.Reset", "Loaded Stock YN texture = " + material.mainTexture);
                            }
                        }

                        else

                        if (name == "ZP")
                        {
                            if (ZP == null)
                            {
                                ZP = material.mainTexture;
                                Debug.Log("DefaultSkyBox.Reset", "Saved Stock ZP texture = " + ZP);
                            }
                            else
                            {
                                material.mainTexture = ZP;
                                Debug.Log("DefaultSkyBox.Reset", "Loaded Stock ZP texture = " + material.mainTexture);
                            }
                        }

                        else

                        if (name == "ZN")
                        {
                            if (ZN == null)
                            {
                                ZN = material.mainTexture;
                                Debug.Log("DefaultSkyBox.Reset", "Saved Stock ZN texture = " + ZN);
                            }
                            else
                            {
                                material.mainTexture = ZN;
                                Debug.Log("DefaultSkyBox.Reset", "Loaded Stock ZN texture = " + material.mainTexture);
                            }
                        }
                    }
                }
            }
        }
    }
}
