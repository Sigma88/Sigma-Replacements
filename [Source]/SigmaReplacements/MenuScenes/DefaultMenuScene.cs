using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal static class DefaultMenuScene
        {
            internal static Texture XP;
            internal static Texture XN;
            static Texture YP;
            static Texture YN;
            static Texture ZP;
            static Texture ZN;
            static QuaternionD? initRot = null;
            static Vector3? initScale = null;

            public static void Reset(this GalaxyCubeControl MenuScenes)
            {
                Debug.Log("DefaultMenuScenes.Reset", "GalaxyCubeControl = " + MenuScenes);
                KSCTriggers.skip = false;

                if (MenuScenes != null)
                {
                    // Rotation
                    if (initRot == null)
                    {
                        initRot = MenuScenes.initRot;
                        Debug.Log("DefaultMenuScenes.Reset", "Saved initRot = " + initRot);
                    }
                    else
                    {
                        MenuScenes.initRot = (QuaternionD)initRot;
                        Debug.Log("DefaultMenuScenes.Reset", "Loaded initRot = " + MenuScenes.initRot);
                    }

                    // Scale
                    GameObject cube = MenuScenes.gameObject.GetChild("GalaxyCube");
                    if (cube != null)
                    {
                        if (initScale == null)
                        {
                            initScale = cube.transform.localScale;
                            Debug.Log("DefaultMenuScenes.Reset", "Saved initScale = " + initScale);
                        }
                        else
                        {
                            cube.transform.localScale = (Vector3)initScale;
                            Debug.Log("DefaultMenuScenes.Reset", "Loaded initScale = " + cube.transform.localScale);
                        }
                    }

                    // Textures
                    Renderer[] renderers = MenuScenes?.gameObject?.GetComponentsInChildren<Renderer>(true);

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
                                Debug.Log("DefaultMenuScenes.Reset", "Saved Stock XP texture = " + XP);
                            }
                            else
                            {
                                material.mainTexture = XP;
                                Debug.Log("DefaultMenuScenes.Reset", "Loaded Stock XP texture = " + material.mainTexture);
                            }
                        }

                        else

                        if (name == "XN")
                        {
                            if (XN == null)
                            {
                                XN = material.mainTexture;
                                Debug.Log("DefaultMenuScenes.Reset", "Saved Stock XN texture = " + XN);
                            }
                            else
                            {
                                material.mainTexture = XN;
                                Debug.Log("DefaultMenuScenes.Reset", "Loaded Stock XN texture = " + material.mainTexture);
                            }
                        }

                        else

                        if (name == "YP")
                        {
                            if (YP == null)
                            {
                                YP = material.mainTexture;
                                Debug.Log("DefaultMenuScenes.Reset", "Saved Stock YP texture = " + YP);
                            }
                            else
                            {
                                material.mainTexture = YP;
                                Debug.Log("DefaultMenuScenes.Reset", "Loaded Stock YP texture = " + material.mainTexture);
                            }
                        }

                        else

                        if (name == "YN")
                        {
                            if (YN == null)
                            {
                                YN = material.mainTexture;
                                Debug.Log("DefaultMenuScenes.Reset", "Saved Stock YN texture = " + YN);
                            }
                            else
                            {
                                material.mainTexture = YN;
                                Debug.Log("DefaultMenuScenes.Reset", "Loaded Stock YN texture = " + material.mainTexture);
                            }
                        }

                        else

                        if (name == "ZP")
                        {
                            if (ZP == null)
                            {
                                ZP = material.mainTexture;
                                Debug.Log("DefaultMenuScenes.Reset", "Saved Stock ZP texture = " + ZP);
                            }
                            else
                            {
                                material.mainTexture = ZP;
                                Debug.Log("DefaultMenuScenes.Reset", "Loaded Stock ZP texture = " + material.mainTexture);
                            }
                        }

                        else

                        if (name == "ZN")
                        {
                            if (ZN == null)
                            {
                                ZN = material.mainTexture;
                                Debug.Log("DefaultMenuScenes.Reset", "Saved Stock ZN texture = " + ZN);
                            }
                            else
                            {
                                material.mainTexture = ZN;
                                Debug.Log("DefaultMenuScenes.Reset", "Loaded Stock ZN texture = " + material.mainTexture);
                            }
                        }
                    }
                }
            }
        }
    }
}
