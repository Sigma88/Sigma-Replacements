using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class CustomMenuScene
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

            internal CustomMenuScene(MenuSceneInfo info)
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
                    GameObject atmosphere = Object.Instantiate(sky);

                    atmosphere.GetChild("XN").GetComponent<Renderer>().material.mainTexture = gradient;
                    atmosphere.GetChild("XP").GetComponent<Renderer>().material.mainTexture = gradient;
                    atmosphere.GetChild("YN").GetComponent<Renderer>().material.mainTexture = top;
                    atmosphere.GetChild("YP").GetComponent<Renderer>().material.mainTexture = top;
                    atmosphere.GetChild("ZP").GetComponent<Renderer>().material.mainTexture = gradient;
                    atmosphere.GetChild("ZN").GetComponent<Renderer>().material.mainTexture = gradient;

                    atmosphere.transform.position = sky.transform.position;
                    atmosphere.transform.localScale = sky.transform.localScale * 0.9f;
                }
            }

            void EditBodies(MenuObject[] bodies, GameObject scene)
            {
                if (bodies == null) return;

                // Get Stock Body
                GameObject kerbin = scene.GetChild("Kerbin");
                Debug.Log("EditBodies", "Kerbin position = " + (Vector3d)kerbin.transform.position);
                Debug.Log("EditBodies", "Kerbin rotation = " + kerbin.transform.eulerAngles);
                Debug.Log("EditBodies", "Kerbin scale = " + (Vector3d.one * 1.2695035));

                for (int i = bodies.Length; i > 0; i--)
                {
                    MenuObject info = bodies[i - 1];
                    GameObject body;

                    // Clone or Select Stock Body
                    if (i - 1 > 0 && info.enabled)
                    {
                        if (string.IsNullOrEmpty(info.name)) continue;

                        body = Object.Instantiate(kerbin);
                        body.name = "NewBody_" + info.name;
                    }
                    else if (i - 1 == 0)
                    {
                        body = kerbin;
                        body.SetActive(info.enabled);
                        if (!info.enabled) continue;
                    }
                    else
                    {
                        continue;
                    }


                    // Edit Body Position/Rotation/Scale
                    body.transform.position = info.position ?? body.transform.position;
                    body.transform.rotation = info.rotation ?? body.transform.rotation;
                    if (info.scale != null)
                        body.transform.localScale = (Vector3)info.scale * 0.1483542f;


                    // Edit Body Appearance
                    Renderer renderer = body.GetComponent<Renderer>();
                    GameObject template = FlightGlobals.Bodies?.FirstOrDefault(b => b.transform.name == info.name)?.scaledBody;
                    if (template != null)
                    {
                        // Material
                        renderer.material = template?.GetComponent<Renderer>()?.material ?? renderer.material;
                        renderer.material.SetTexture(info.texture1);
                        renderer.material.SetNormal(info.normal1);
                        renderer.material.SetColor(info.color1);

                        // Mesh
                        MeshFilter meshFilter = body.GetComponent<MeshFilter>();
                        meshFilter.mesh = template?.GetComponent<MeshFilter>()?.mesh ?? meshFilter.mesh;
                    }


                    // Add Atmospheric Haze
                    if (haze != null)
                    {
                        renderer.materials = new Material[] { renderer.material, new Material(Shader.Find("KSP/Scenery/Unlit/Transparent")) };
                        renderer.materials[1].color = (Color)haze;
                    }
                }
            }

            void EditTerrain(MenuObject info, GameObject scene)
            {
                if (info == null) return;

                // Select Terrain
                Terrain terrain = scene.GetChild("Terrain").GetComponent<Terrain>();
                SplatPrototype[] splats = terrain.terrainData.splatPrototypes;


                // Change Terrain Textures
                if (info.texture1 != null)
                    splats[0].texture = (Texture2D)info.texture1;
                if (info.normal1 != null)
                    splats[0].normalMap = (Texture2D)info.normal1;

                // Change Terrain Normals
                if (info.texture2 != null)
                    splats[1].texture = (Texture2D)info.texture2;
                if (info.normal2 != null)
                    splats[1].normalMap = (Texture2D)info.normal2;

                // Save Terrain Changes
                terrain.terrainData.splatPrototypes = splats;
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
                        Debug.Log("EditBoulders", "     rotation = " + boulders[i].transform.eulerAngles);
                        Debug.Log("EditBoulders", "        scale = " + (Vector3d)boulders[i].transform.localScale);
                    }
                }

                for (int i = 0; i < info.Length; i++)
                {
                    if (info[i].index >= boulders.Length) continue;

                    for (int j = info[i].index ?? 0; j <= (info[i].index ?? boulders.Length - 1); j++)
                    {
                        // Enable
                        boulders[j].gameObject.SetActive(info[i].enabled);
                        if (!boulders[j].gameObject.activeSelf) continue;

                        // Edit Body Position/Rotation/Scale
                        boulders[j].position += info[i].position ?? Vector3.zero;
                        boulders[j].rotation = info[i].rotation ?? boulders[j].rotation;
                        boulders[j].localScale = info[i].scale ?? boulders[j].localScale;

                        // Adjust Scale by Distance
                        if (info[i].adjustScale)
                            boulders[j].transform.localScale *= (0.25f + (new Vector3(0.7814472f, -0.7841411f, 2.28511f) - boulders[j].transform.position).magnitude / 100);

                        // Edit Appearances
                        MeshFilter meshFilter = boulders[j].GetComponent<MeshFilter>();
                        meshFilter.mesh = info[i].mesh ?? meshFilter.mesh;

                        Renderer renderer = boulders[j].GetComponent<Renderer>();
                        renderer.material.shader = info[i].shader ?? renderer.material.shader;
                        renderer.material.SetTexture(info[i].texture1);
                        renderer.material.SetNormal(info[i].normal1);
                        renderer.material.SetColor(info[i].color1);
                    }
                }
            }

            void AddScatter(MenuObject[] scatters, GameObject scene)
            {
                GameObject template = scene.GetChild("sandcastle");
                Debug.Log("AddScatter", "template position = " + (Vector3d)template.transform.position);
                Debug.Log("AddScatter", "template rotation = " + template.transform.eulerAngles);
                Debug.Log("AddScatter", "template scale = " + (Vector3d)template.transform.localScale);

                for (int i = 0; i < scatters?.Length; i++)
                {
                    if (!scatters[i].enabled) continue;

                    MenuObject info = scatters[i];
                    GameObject scatter = Object.Instantiate(template);
                    Object.DestroyImmediate(scatter.GetComponent<SandCastleLogic>());
                    scatter.name = info.name;

                    // Edit Position/Rotation/Scale
                    scatter.transform.position = info.position ?? scatter.transform.position;
                    scatter.transform.rotation = info.rotation ?? scatter.transform.rotation;
                    scatter.transform.localScale = info.scale ?? scatter.transform.localScale;

                    // Adjust Scale by Distance
                    if (info.adjustScale)
                        scatter.transform.transform.localScale *= (0.25f + (new Vector3(0.7814472f, -0.7841411f, 2.28511f) - scatter.transform.transform.position).magnitude / 100);

                    // Edit Appearances
                    MeshFilter meshFilter = scatter.transform.GetComponent<MeshFilter>();
                    meshFilter.mesh = info.mesh ?? meshFilter.mesh;

                    Renderer renderer = scatter.transform.GetComponent<Renderer>();
                    renderer.material.shader = info.shader ?? renderer.material.shader;
                    renderer.material.SetTexture(info.texture1);
                    renderer.material.SetNormal(info.normal1);
                    renderer.material.SetColor(info.color1);
                }
            }

            void EditWreck(MenuObject info, GameObject scene)
            {
                if (info == null) return;

                GameObject wreck = scene.GetChild("wreckedShip");
                if (Debug.debug)
                {
                    Debug.Log("EditWreck", "wreck position = " + (Vector3d)wreck.transform.position);
                    Debug.Log("EditWreck", "wreck rotation = " + wreck.transform.eulerAngles);
                    Debug.Log("EditWreck", "wreck scale = " + (Vector3d)wreck.transform.localScale);

                    GameObject ground = wreck.GetChild("boulder");
                    Debug.Log("EditWreck", "ground position = " + (Vector3d)ground.transform.position);
                    Debug.Log("EditWreck", "ground rotation = " + ground.transform.eulerAngles);
                    Debug.Log("EditWreck", "ground scale = " + (Vector3d)ground.transform.localScale);
                }

                // Enable
                wreck.SetActive(info.enabled);
                if (!wreck.activeSelf) return;

                // Edit Position/Rotation/Scale
                wreck.transform.position = info.position ?? wreck.transform.position;
                wreck.transform.rotation = info.rotation ?? wreck.transform.rotation;
                wreck.transform.localScale = info.scale ?? wreck.transform.localScale;
            }

            void EditGround(MenuObject info, GameObject scene)
            {
                if (info == null) return;

                GameObject ground = scene.GetChild("wreckedShip").GetChild("boulder");
                Debug.Log("EditGround", "ground position = " + (Vector3d)ground.transform.position);
                Debug.Log("EditGround", "ground rotation = " + ground.transform.eulerAngles);
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

            void EditKerbals(MenuObject[] kerbals, GameObject scene)
            {
                if (kerbals == null) return;

                // Get Stock Kerbal
                GameObject template = scene.GetChild("Kerbals").transform.GetChild(0).gameObject;
                Debug.Log("EditKerbals", "template position = " + (Vector3d)template.transform.position);
                Debug.Log("EditKerbals", "template rotation = " + template.transform.eulerAngles);
                Debug.Log("EditKerbals", "template scale = " + (Vector3d)template.transform.localScale);

                for (int i = kerbals.Length; i > 0; i--)
                {
                    MenuObject info = kerbals[i - 1];
                    GameObject kerbal;

                    // Clone or Select Stock newGuy
                    if (i - 1 > 0 && info.enabled)
                    {
                        if (string.IsNullOrEmpty(info.name)) continue;

                        kerbal = Object.Instantiate(template);
                        kerbal.name = info.name;
                    }
                    else if (i - 1 == 0)
                    {
                        kerbal = template;
                        kerbal.SetActive(info.enabled);
                        if (!info.enabled) continue;
                    }
                    else
                    {
                        continue;
                    }


                    // Edit newGuy Position/Rotation/Scale
                    kerbal.transform.position = info.position ?? kerbal.transform.position;
                    kerbal.transform.rotation = info.rotation ?? kerbal.transform.rotation;
                    kerbal.transform.localScale = info.scale ?? kerbal.transform.localScale;

                    // Remove Helmet
                    kerbal.GetChild("helmet01").SetActive(!info.removeHelmet);
                }
            }

            MenuObject[] Parse(ConfigNode[] nodes, MenuObject[] array)
            {
                for (int i = 0; i < nodes?.Length; i++)
                {
                    if (array == null) array = new MenuObject[nodes.Length];

                    array[i] = new MenuObject(nodes[i]);
                }

                return array;
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

            internal class MenuObject : Info
            {
                internal bool enabled = true;
                internal int? index = null;

                internal Vector3? position = null;
                internal Quaternion? rotation = null;
                internal Vector3? scale = null;

                internal Texture texture1 = null;
                internal Texture texture2 = null;

                internal Color? color1 = null;
                internal Color? color2 = null;

                internal Texture normal1 = null;
                internal Texture normal2 = null;

                internal Shader shader = null;
                internal Mesh mesh = null;

                internal bool adjustScale = false;

                internal bool removeHelmet = false;

                internal MenuObject(ConfigNode node)
                {
                    name = node.GetValue("name");

                    index = Parse(node.GetValue("index"), index);

                    if (!bool.TryParse(node.GetValue("enabled"), out enabled))
                    {
                        enabled = true;
                    }

                    position = Parse(node.GetValue("position"), position);
                    rotation = Parse(node.GetValue("rotation"), rotation);
                    scale = Parse(node.GetValue("scale"), scale);

                    texture1 = Parse(node.GetValue("texture1"), texture1);
                    texture2 = Parse(node.GetValue("texture2"), texture2);
                    color1 = Parse(node.GetValue("color1"), color1);
                    color2 = Parse(node.GetValue("color2"), color2);
                    normal1 = Parse(node.GetValue("normal1"), normal1);
                    normal2 = Parse(node.GetValue("normal2"), normal2);

                    mesh = Parse(node.GetValue("mesh"), mesh);
                    shader = Parse(node.GetValue("shader"), shader);

                    bool.TryParse(node.GetValue("adjustScale"), out adjustScale);
                    bool.TryParse(node.GetValue("removeHelmet"), out removeHelmet);
                }

                Mesh Parse(string s, Mesh defaultValue)
                {
                    if (string.IsNullOrEmpty(s)) return defaultValue;

                    Mesh output = null;

                    if (s.StartsWith("BUILTIN/"))
                    {
                        string[] path = s.Split('/');

                        if (path.Length > 4)
                        {
                            CelestialBody planet = FlightGlobals.Bodies.FirstOrDefault(b => b.transform.name == path[1]);

                            if (path[2] == "PQSLandControl" && path[3] == "scatters")
                            {
                                PQSLandControl landControl = planet?.pqsController?.GetComponentsInChildren<PQSLandControl>(true)?.FirstOrDefault();

                                for (int i = 0; i < landControl?.scatters?.Length; i++)
                                {
                                    if (landControl.scatters[i].scatterName == path[4])
                                    {
                                        output = landControl.scatters[i].baseMesh;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        output = Resources.FindObjectsOfTypeAll<Mesh>().FirstOrDefault(m => m.name == s);
                    }

                    return output ?? defaultValue;
                }

                Shader Parse(string s, Shader defaultValue)
                {
                    if (string.IsNullOrEmpty(s)) return defaultValue;

                    return Resources.FindObjectsOfTypeAll<Shader>().FirstOrDefault(h => h.name == s) ?? defaultValue;
                }
            }
        }
    }
}
