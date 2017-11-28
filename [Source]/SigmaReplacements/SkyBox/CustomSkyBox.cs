using System;
using System.Collections.Generic;
using UnityEngine;


namespace SigmaReplacements
{
    namespace SkyBox
    {
        public class CustomSkyBox
        {
            // Settings
            bool? rotate = null;
            bool? mirror = null;

            // Textures
            Texture[] SkyBox = null;

            internal CustomSkyBox(Mode gameMode, int seed)
            {
                Debug.Log("new CustomSkyBox", "Generating new CustomSkyBox for game mode = " + gameMode);

                int? useChance = null;
                string collection = "";
                List<Texture[]> SkyBoxList = new List<Texture[]>();

                for (int i = 0; i < SkyBoxInfo.DataBase?.Count; i++)
                {
                    SkyBoxInfo info = ((SkyBoxInfo)SkyBoxInfo.DataBase[i]).GetFor(gameMode);

                    if (info != null)
                    {
                        if (string.IsNullOrEmpty(collection) || collection == info.collection)
                        {
                            if (info.useChance != 1)
                            {
                                useChance = Math.Abs(seed % 100);
                                seed = seed.ToString().GetHashCode();
                            }

                            if (info.useChance == 1 || useChance < info.useChance * 100)
                            {
                                Debug.Log("new CustomSkyBox", "SkyBoxInfo nr. " + i + ", matched useChance = " + info.useChance + " to generated chance = " + useChance + " %");
                                Debug.Log("new CustomSkyBox", "SkyBoxInfo nr. " + i + ", matched collection = " + info.collection + " to current collection = " + collection);

                                // Collection
                                collection = info.collection;

                                // Settings
                                rotate = rotate ?? info.rotate;
                                mirror = mirror ?? info.mirror == true ? (Math.Abs(seed % 2) == 1) : info.mirror;

                                // Textures
                                if (info.SkyBox?.Count > 0)
                                    SkyBoxList.AddRange(info.SkyBox);
                            }
                        }
                    }
                }

                SkyBox = SkyBoxList.Pick();
                Debug.Log("new CustomSkyBox", "Generated new CustomSkyBox. rotate = " + rotate + ", mirror = " + (mirror == true) + ", SkyBox = " + (SkyBox == null ? "STOCK" : ((SkyBoxList.IndexOf(SkyBox) + 1) + "/" + SkyBoxList.Count)));
            }

            internal void ApplyTo(GameObject skybox)
            {
                Debug.Log("CustomSkyBox.ApplyTo", "Applying to skybox = " + skybox);
                if (skybox == null) return;

                // Set Textures
                Renderer[] renderers = skybox?.GetComponentsInChildren<Renderer>();

                for (int i = 0; i < renderers?.Length; i++)
                {
                    Debug.Log("CustomSkyBox.ApplyTo", "Renderer = " + renderers[i]);
                    string name = renderers[i]?.name;
                    Material material = renderers[i]?.material;

                    if (material == null) continue;

                    // Select Texture
                    if (SkyBox != null)
                    {
                        Debug.Log("CustomSkyBox.ApplyTo", "Old Texture = " + material.mainTexture);

                        if (name == "XP")
                            material.SetTexture(SkyBox[0]);
                        else if (name == "XN")
                            material.SetTexture(SkyBox[1]);
                        else if (name == "YP") // SQUAD inverts YP with YN
                            material.SetTexture(SkyBox[3]);
                        else if (name == "YN") // SQUAD inverts YN with YP
                            material.SetTexture(SkyBox[2]);
                        else if (name == "ZP")
                            material.SetTexture(SkyBox[4]);
                        else if (name == "ZN")
                            material.SetTexture(SkyBox[5]);

                        Debug.Log("CustomSkyBox.ApplyTo", "New Texture = " + material.mainTexture);
                    }
                }


                // Mirror CubeMap
                if (mirror == true)
                {
                    Debug.Log("CustomSkyBox.ApplyTo", "Mirroring SkyBox Texture");

                    GameObject cube = skybox.GetChild("GalaxyCube");
                    if (cube != null)
                        cube.transform.localScale *= -1;
                    else
                        skybox.transform.localScale *= -1;

                    GalaxyCubeControl control = skybox.GetComponent<GalaxyCubeControl>();
                    if (control != null)
                        control.initRot = Quaternion.AngleAxis(180, Vector3.forward);
                    else
                        skybox.transform.Rotate(cube.transform.forward, 180);
                }


                // Rotate CubeMap
                if (rotate == true)
                {
                    Debug.Log("CustomSkyBox.ApplyTo", "Rotating SkyBox Transform");

                    string hash = "";
                    if (HighLogic.LoadedScene == GameScenes.SPACECENTER && HighLogic.CurrentGame != null)
                        hash += HighLogic.CurrentGame.Seed;
                    else
                        hash += DateTime.Today;

                    int x = hash.GetHashCode();
                    hash = x.ToString();
                    int y = hash.GetHashCode();
                    hash = y.ToString();
                    int z = hash.GetHashCode();
                    hash = z.ToString();

                    Debug.Log("CustomSkyBox.ApplyTo", "Rotatation = {" + (x % 360) + "°, " + (y % 360) + "°, " + (z % 360) + "°}");

                    GalaxyCubeControl cube = skybox.GetComponent<GalaxyCubeControl>();
                    if (cube != null)
                        cube.initRot = Quaternion.Euler(x % 360, y % 360, z % 360);
                    else
                        skybox.transform.rotation = Quaternion.Euler(x % 360, y % 360, z % 360);
                }
            }
        }
    }
}
