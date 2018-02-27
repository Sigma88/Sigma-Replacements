using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class CustomMunScene
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
                Debug.Log("EditBodies", "Kerbin rotatoSpeed = " + kerbin?.GetComponent<Rotato>()?.speed);

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

                    // Debug
                    if (info.debug) body.AddOrGetComponent<LiveDebug>();

                    // Edit Body Position/Rotation/Scale
                    body.transform.position = info.position ?? body.transform.position;
                    body.transform.rotation = info.rotation ?? body.transform.rotation;
                    if (info.scale != null)
                        body.transform.localScale = (Vector3)info.scale * 0.1483542f;

                    // Edit Body Rotation Speed
                    Rotato rotato = body.GetComponent<Rotato>();
                    rotato.speed = info.rotatoSpeed ?? rotato.speed;

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

                        // Debug
                        if (info[i].debug) boulders[j].gameObject.AddOrGetComponent<LiveDebug>();

                        // Edit Body Position/Rotation/Scale
                        boulders[j].position += info[i].position ?? Vector3.zero;
                        boulders[j].rotation = info[i].rotation ?? boulders[j].rotation;
                        boulders[j].localScale = info[i].scale ?? boulders[j].localScale;

                        // Adjust Scale by Distance
                        if (info[i].adjustScale)
                            boulders[j].transform.localScale *= (0.25f + (new Vector3(0.7814472f, -0.7841411f, 2.28511f) - boulders[j].transform.position).magnitude / 100);

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
                Debug.Log("AddScatter", "template rotation = " + template.transform.eulerAngles);
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
                    GameObject scatter = Object.Instantiate(template);
                    Object.DestroyImmediate(scatter.GetComponent<SandCastleLogic>());
                    scatter.name = info.name;

                    EditObject(scatter, info);
                }

                if (sandcastle != null)
                {
                    EditObject(template, scatters[(int)sandcastle]);
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

                if (info.debug) wreck.AddComponent<LiveDebug>();
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

            void EditObject(GameObject obj, MenuObject info)
            {
                if (info.debug) obj.AddOrGetComponent<LiveDebug>();

                // Edit Position/Rotation/Scale
                obj.transform.position = info.position ?? obj.transform.position;
                obj.transform.rotation = info.rotation ?? obj.transform.rotation;
                obj.transform.localScale = info.scale ?? obj.transform.localScale;

                // Adjust Scale by Distance
                if (info.adjustScale)
                    obj.transform.transform.localScale *= (0.25f + (new Vector3(0.7814472f, -0.7841411f, 2.28511f) - obj.transform.transform.position).magnitude / 100);

                // Edit Appearances
                Renderer renderer = obj.transform.GetComponent<Renderer>();
                renderer.material = info.material ?? renderer.material;
                renderer.material.shader = info.shader ?? renderer.material.shader;

                MeshFilter meshFilter = obj.transform.GetComponent<MeshFilter>();
                meshFilter.mesh = info.mesh ?? meshFilter.mesh;

                renderer.material.SetTexture(info.texture1);
                renderer.material.SetColor(info.color1);
                renderer.material.SetNormal(info.normal1);
            }
        }
    }
}
