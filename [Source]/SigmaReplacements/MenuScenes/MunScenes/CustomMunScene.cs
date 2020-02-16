using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class CustomMunScene : CustomMenuScene
        {
            // Sky
            MenuObject atmosphere = null;
            Color? haze = null;
            MenuObject[] bodies = null;

            // Terrain
            MenuObject terrain = null;

            // Scatter
            MenuObject[] boulders = null;
            MenuObject[] scatter = null;

            // ShipWreck
            MenuObject wreck = null;
            MenuObject ground = null;

            // Kerbals
            MenuObject[] kerbals = null;

            internal CustomMunScene(MunSceneInfo info)
            {
                // Sky
                if (info.atmosphere != null)
                    atmosphere = new MenuObject(info.atmosphere);

                bodies = Parse(info.bodies, bodies);

                // Terrain
                if (info.terrain != null)
                    terrain = new MenuObject(info.terrain);

                // Scatter
                boulders = ParseBoulders(info.scatter);
                scatter = ParseScatter(info.scatter);

                // ShipWreck
                if (info.wreck != null)
                    wreck = new MenuObject(info.wreck);
                if (info.ground != null)
                    ground = new MenuObject(info.ground);

                // Kerbals
                kerbals = Parse(info.kerbals, kerbals);
            }

            internal void ApplyTo(GameObject scene)
            {
                // Sky
                AddAtmosphere(atmosphere, scene);
                EditBodies(bodies, scene);

                // Terrain
                EditTerrain(terrain, scene);

                // Scatter
                EditBoulders(boulders, scene);
                AddScatter(scatter, scene);

                // ShipWreck
                EditWreck(wreck, scene);
                EditGround(ground, scene);

                // Kerbals
                EditKerbals(kerbals, scene);
            }

            void AddAtmosphere(MenuObject info, GameObject scene)
            {
                if (info?.color1 != null && info?.color2 != null)
                {
                    // Colors
                    Color from = (Color)info.color1;
                    Color to = (Color)info.color2;
                    haze = Color.Lerp(from, to, 0.5f).A(0.65f);

                    // Atmosphere Top
                    Texture2D top = new Texture2D(1, 1);
                    top.SetPixel(1, 1, to);
                    top.Apply();

                    // Atmosphere Gradient
                    Texture2D gradient = new Texture2D(1, 256);

                    for (int y = 0; y < 256; y++)
                        gradient.SetPixel(0, y, Color.Lerp(from, to, y / 256f));

                    gradient.Apply();

                    // Create Atmosphere SkyBox
                    GameObject sky = GameObject.Find("MainMenuGalaxy");
                    GameObject atmosphere = Instantiate(sky);

                    atmosphere.GetChild("XN").GetComponent<Renderer>().material.mainTexture = gradient;
                    atmosphere.GetChild("XP").GetComponent<Renderer>().material.mainTexture = gradient;
                    atmosphere.GetChild("YN").GetComponent<Renderer>().material.mainTexture = top;
                    atmosphere.GetChild("YP").GetComponent<Renderer>().material.mainTexture = top;
                    atmosphere.GetChild("ZP").GetComponent<Renderer>().material.mainTexture = gradient;
                    atmosphere.GetChild("ZN").GetComponent<Renderer>().material.mainTexture = gradient;

                    atmosphere.transform.position = sky.transform.position;
                    atmosphere.transform.localScale = sky.transform.localScale * 0.577f;
                    atmosphere.transform.eulerAngles = Vector3.zero;
                }
            }

            void EditBodies(MenuObject[] bodies, GameObject scene)
            {
                if (!(bodies?.Length > 0))
                {
                    bodies = new MenuObject[] { new MenuObject("Kerbin") };
                }

                // Get Stock Body
                GameObject kerbin = scene.GetChild("Kerbin");
                GameObject clone = Instantiate(kerbin);
                Debug.Log("EditBodies", "Kerbin position = " + (Vector3d)kerbin.transform.position);
                Debug.Log("EditBodies", "Kerbin rotation = " + (Vector3d)kerbin.transform.eulerAngles);
                Debug.Log("EditBodies", "Kerbin scale = " + (Vector3d)kerbin.transform.localScale);
                Debug.Log("EditBodies", "Kerbin rotatoSpeed = " + kerbin?.GetComponent<Rotato>()?.speed);

                for (int i = 0; i < bodies.Length; i++)
                {
                    MenuObject info = bodies[i];
                    GameObject body;

                    // Clone or Select Stock Body
                    if (info.name == "Kerbin")
                    {
                        body = kerbin;
                        body.SetActive(info.enabled);
                        if (!info.enabled) continue;
                    }
                    else if (info.enabled)
                    {
                        if (string.IsNullOrEmpty(info.name)) continue;

                        body = Instantiate(clone);
                        body.name = "NewBody_" + info.name;
                    }
                    else
                    {
                        continue;
                    }

                    // Edit Visual Parameters
                    Renderer renderer = body.GetComponent<Renderer>();

                    CelestialBody cb = FlightGlobals.Bodies?.FirstOrDefault(b => b.transform.name == info.name);
                    GameObject template = cb?.scaledBody;

                    if (template != null)
                    {
                        // OnDemand
                        if (KopernicusFixer.detect)
                            OnDemandFixer.LoadTextures(template);

                        // Material
                        renderer.material = template?.GetComponent<Renderer>()?.material ?? renderer.material;
                        renderer.material.SetTexture(info.texture1);
                        renderer.material.SetNormal(info.normal1);
                        renderer.material.SetColor(info.color1);

                        // Mesh
                        MeshFilter meshFilter = body.GetComponent<MeshFilter>();
                        meshFilter.mesh = template?.GetComponent<MeshFilter>()?.mesh ?? meshFilter.mesh;

                        // Coronas
                        AddCoronas(body, template);
                    }

                    // Edit Physical Parameters
                    info.scale = info.scale ?? Vector3.one;
                    info.ApplyTo(body, 0.188336193561554f);

                    // Add Atmospheric Haze
                    if (haze != null)
                    {
                        renderer.materials = new Material[] { renderer.material, new Material(Shader.Find("KSP/Scenery/Unlit/Transparent")) };
                        renderer.materials[1].color = (Color)haze;
                    }

                    // CleanUp
                    Object.Destroy(clone);
                }
            }

            void EditTerrain(MenuObject info, GameObject scene)
            {
                if (info == null) return;

                // Select Terrain
                Terrain terrain = scene.GetChild("Terrain").GetComponent<Terrain>();
                TerrainLayer[] layers = terrain.terrainData.terrainLayers;

                // Change Terrain Textures
                if (info.texture1 != null)
                    layers[0].diffuseTexture = (Texture2D)info.texture1;
                if (info.texture2 != null)
                    layers[1].diffuseTexture = (Texture2D)info.texture2;

                // Change Terrain Normals
                if (info.normal1 != null)
                    layers[0].normalMapTexture = (Texture2D)info.normal1;
                if (info.normal2 != null)
                    layers[1].normalMapTexture = (Texture2D)info.normal2;

                // Save Terrain Changes
                terrain.terrainData.terrainLayers = layers;
            }

            void EditBoulders(MenuObject[] info, GameObject scene)
            {
                Transform[] boulders = scene.GetComponentsInChildren<Transform>(true).Where(t => t.name == "boulder" && t.parent == scene.transform).ToArray();

                if (Debug.debug)
                {
                    for (int i = 0; i < boulders.Length; i++)
                    {
                        Debug.Log("EditBoulders", "boulder index = " + i);
                        Debug.Log("EditBoulders", "     position = " + (Vector3d)boulders[i].transform.position);
                        Debug.Log("EditBoulders", "     rotation = " + (Vector3d)boulders[i].transform.eulerAngles);
                        Debug.Log("EditBoulders", "        scale = " + (Vector3d)boulders[i].transform.localScale);
                    }
                }

                for (int i = 0; i < info?.Length; i++)
                {
                    if (info[i].index >= boulders.Length) continue;

                    for (int j = info[i].index ?? 0; j <= (info[i].index ?? boulders.Length - 1); j++)
                    {
                        // Enable
                        boulders[j].gameObject.SetActive(info[i].enabled);
                        if (!boulders[j].gameObject.activeSelf) continue;

                        // Debug
                        if (Debug.debug) boulders[j].gameObject.AddOrGetComponent<LiveDebug>().index = j;

                        // Edit Body Position/Rotation/Scale
                        boulders[j].position += info[i].position ?? Vector3.zero;
                        boulders[j].rotation = info[i].rotation ?? boulders[j].rotation;
                        boulders[j].localScale = info[i].scale ?? boulders[j].localScale;

                        // Adjust Scale by Distance
                        if (info[i].adjustScale)
                            boulders[j].transform.localScale *= (0.5f + (new Vector3(0.7814472f, -0.7841411f, 2.28511f) - boulders[j].transform.position).magnitude / 100);

                        // Edit Appearances
                        Renderer renderer = boulders[j].GetComponent<Renderer>();
                        renderer.material = info[i].material ?? renderer.material;
                        renderer.material.shader = info[i].shader ?? renderer.material.shader;

                        MeshFilter meshFilter = boulders[j].GetComponent<MeshFilter>();
                        meshFilter.mesh = info[i].mesh ?? meshFilter.mesh;

                        renderer.material.SetTexture(info[i].texture1);
                        renderer.material.SetNormal(info[i].normal1);
                        renderer.material.SetColor(info[i].color1);
                    }
                }
            }

            void AddScatter(MenuObject[] scatters, GameObject scene)
            {
                int? sandcastle = null;
                GameObject template = scene.GetChild("sandcastle");
                Debug.Log("AddScatter", "template position = " + (Vector3d)template.transform.position);
                Debug.Log("AddScatter", "template rotation = " + (Vector3d)template.transform.eulerAngles);
                Debug.Log("AddScatter", "template scale = " + (Vector3d)template.transform.localScale);

                for (int i = 0; i < scatters?.Length; i++)
                {
                    if (!scatters[i].enabled) continue;

                    if (scatters[i].name == "sandcastle")
                    {
                        sandcastle = sandcastle ?? i;
                        continue;
                    }

                    MenuObject info = scatters[i];
                    GameObject scatter = Instantiate(template);
                    Object.DestroyImmediate(scatter.GetComponent<SandCastleLogic>());
                    scatter.name = info.name;

                    info.ApplyTo(scatter);
                }

                if (sandcastle != null)
                {
                    scatters[(int)sandcastle].ApplyTo(template);
                }
            }

            void EditWreck(MenuObject info, GameObject scene)
            {
                if (info == null) return;

                GameObject wreck = scene.GetChild("wreckedShip");
                if (Debug.debug)
                {
                    Debug.Log("EditWreck", "wreck position = " + (Vector3d)wreck.transform.position);
                    Debug.Log("EditWreck", "wreck rotation = " + (Vector3d)wreck.transform.eulerAngles);
                    Debug.Log("EditWreck", "wreck scale = " + (Vector3d)wreck.transform.localScale);

                    GameObject ground = wreck.GetChild("boulder");
                    Debug.Log("EditWreck", "ground position = " + (Vector3d)ground.transform.position);
                    Debug.Log("EditWreck", "ground rotation = " + (Vector3d)ground.transform.eulerAngles);
                    Debug.Log("EditWreck", "ground scale = " + (Vector3d)ground.transform.localScale);
                }

                // Enable
                wreck.SetActive(info.enabled);
                if (!wreck.activeSelf) return;

                // Edit Position/Rotation/Scale
                wreck.transform.position = info.position ?? wreck.transform.position;
                wreck.transform.rotation = info.rotation ?? wreck.transform.rotation;
                wreck.transform.localScale = info.scale ?? wreck.transform.localScale;

                if (Debug.debug) wreck.AddComponent<LiveDebug>();
            }

            void EditGround(MenuObject info, GameObject scene)
            {
                if (info == null) return;

                GameObject ground = scene.GetChild("wreckedShip").GetChild("boulder");
                Debug.Log("EditGround", "ground position = " + (Vector3d)ground.transform.position);
                Debug.Log("EditGround", "ground rotation = " + (Vector3d)ground.transform.eulerAngles);
                Debug.Log("EditGround", "ground scale = " + (Vector3d)ground.transform.localScale);

                // Enable
                ground.SetActive(info.enabled);
                if (!ground.activeSelf) return;

                // Edit Position/Rotation/Scale
                ground.transform.position = info.position ?? ground.transform.position;
                ground.transform.rotation = info.rotation ?? ground.transform.rotation;
                ground.transform.localScale = info.scale ?? ground.transform.localScale;

                // Edit Appearances
                MeshFilter meshFilter = ground.transform.GetComponent<MeshFilter>();
                meshFilter.mesh = info.mesh ?? meshFilter.mesh;

                Renderer renderer = ground.transform.GetComponent<Renderer>();
                renderer.material.shader = info.shader ?? renderer.material.shader;
                renderer.material.SetTexture(info.texture1);
                renderer.material.SetNormal(info.normal1);
                renderer.material.SetColor(info.color1);
            }

            void EditKerbals(MenuObject[] info, GameObject scene)
            {
                if (!(info?.Length > 0)) return;

                // Get Stock Kerbal
                Transform kerbals = scene?.GetChild("Kerbals")?.transform;

                if (kerbals == null || kerbals.childCount < 1) return;

                GameObject[] templates = new GameObject[] { Instantiate(kerbals.GetChild(0).gameObject), InstantiateFemale(kerbals.GetChild(0).gameObject) };

                if (Debug.debug)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Debug.Log("EditKerbals", "template[" + i + "] position = " + (Vector3d)templates[i].transform.position);
                        Debug.Log("EditKerbals", "template[" + i + "] rotation = " + (Vector3d)templates[i].transform.eulerAngles);
                        Debug.Log("EditKerbals", "template[" + i + "] scale = " + (Vector3d)templates[i].transform.localScale);
                    }
                }

                for (int i = 0; i < info?.Length; i++)
                {
                    GameObject kerbal;

                    // Clone or Select Stock newGuy
                    if (info[i].index == 0)
                    {
                        kerbal = kerbals.GetChild(0).gameObject;
                        kerbal.SetActive(info[i].enabled);
                        if (!info[i].enabled) continue;
                    }
                    else if (info[i].enabled)
                    {
                        if (string.IsNullOrEmpty(info[i].name)) continue;

                        if (info[i].template == 1)
                            kerbal = Instantiate(templates[1]);
                        else
                            kerbal = Instantiate(templates[0]);

                        kerbal.name = info[i].name;
                    }
                    else
                    {
                        continue;
                    }

                    // Apply Physical Parameters
                    info[i].ApplyTo(kerbal);

                    // Remove Helmet
                    GameObject helmet = kerbal?.GetChild("helmet01");
                    if (helmet != null && info[i].removeHelmet)
                    {
                        helmet.SetActive(false);

                        MenuRandomKerbalAnims mrka = kerbal.GetComponent<MenuRandomKerbalAnims>();
                        if (mrka != null)
                        {
                            List<string> list = mrka.anims.ToList();
                            if (list.Remove("idle_c"))
                            {
                                mrka.anims = list.ToArray();
                            }
                        }
                    }
                }

                // CleanUp
                for (int i = 0; i < 2; i++)
                {
                    Object.DestroyImmediate(templates[i]);
                }
            }

            MenuObject[] ParseBoulders(ConfigNode[] input)
            {
                if (input == null) return null;

                MenuObject[] data = Parse(input, null);

                List<MenuObject> output = data.Where(i => i.name == "boulder" && i.index == null).ToList();
                output.AddRange(data.Where(i => i.name == "boulder" && i.index != null).OrderBy(i => i.index));

                return output.ToArray();
            }

            MenuObject[] ParseScatter(ConfigNode[] input)
            {
                MenuObject[] output = null;

                if (input != null)
                {
                    output = Parse(input, output);
                }

                return output?.Where(i => i.name != "boulder")?.ToArray();
            }

            GameObject InstantiateFemale(GameObject maleEVA)
            {
                GameObject[] scenes = Object.FindObjectOfType<MainMenu>()?.envLogic?.areas;

                GameObject femaleEVA = Instantiate(maleEVA);
                femaleEVA.name = "femaleEVA";
                GameObject maleHead = femaleEVA.GetChild("head02");

                GameObject valHead = Object.Instantiate(scenes[1].GetChild("mesh_female_kerbalAstronaut01_kerbalGirl_mesh_kerbalGirl_base"), maleHead.transform.parent);

                valHead.SetActive(true);
                valHead.transform.rotation = maleHead.transform.rotation;
                valHead.transform.position = maleHead.transform.position;
                maleHead.SetActive(false);

                GameObject leftEye = null;
                GameObject rightEye = null;

                foreach (var renderer in valHead.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {
                    if (renderer?.bones?.Length > 0)
                    {
                        var newBones = new Transform[renderer.bones.Length];

                        for (int i = 0; i < renderer.bones.Length; i++)
                        {
                            newBones[i] = femaleEVA?.GetChild(renderer?.bones?[i]?.name)?.transform;

                            if (newBones[i].name == "jntDrv_l_eye01")
                            {
                                if (leftEye == null)
                                {
                                    leftEye = new GameObject("jntDrv_l_eye01_pivot");
                                    leftEye.transform.parent = newBones[i];
                                    leftEye.transform.localPosition = Vector3.zero;
                                    leftEye.transform.localEulerAngles = new Vector3(356, 11, 17);
                                    leftEye.transform.localScale = Vector3.one;
                                }
                                newBones[i] = leftEye.transform;
                            }

                            if (newBones[i].name == "jntDrv_r_eye01")
                            {
                                if (rightEye == null)
                                {
                                    rightEye = new GameObject("jntDrv_r_eye01_pivot");
                                    rightEye.transform.parent = newBones[i];
                                    rightEye.transform.localPosition = Vector3.zero;
                                    rightEye.transform.localEulerAngles = new Vector3(3, 348, 17);
                                    rightEye.transform.localScale = Vector3.one;
                                }
                                newBones[i] = rightEye.transform;
                            }
                        }

                        renderer.rootBone = femaleEVA?.GetChild(renderer?.rootBone?.name)?.transform;

                        if (renderer?.rootBone?.name == "jntDrv_l_eye01")
                            renderer.rootBone = leftEye.transform;

                        if (renderer?.rootBone?.name == "jntDrv_r_eye01")
                            renderer.rootBone = rightEye.transform;

                        renderer.bones = newBones;
                    }
                }

                return femaleEVA;
            }
        }
    }
}
