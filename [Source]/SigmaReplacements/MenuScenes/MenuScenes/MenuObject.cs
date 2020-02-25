using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class MenuObject : Info
        {
            // Settings
            internal int? index = null;
            internal int? template = null;
            internal bool enabled = true;
            internal bool adjustScale = false;
            internal bool removeJetpack = false;
            internal bool removeHelmet = false;
            internal bool addColliders = false;


            // Physical Parameters
            internal Mesh mesh = null;
            internal Vector3? position = null;
            internal Quaternion? rotation = null;
            internal Vector3? scale = null;


            // Visual Parameters
            internal Material material = null;
            internal Shader shader = null;

            internal Color? color1 = null;
            internal Color? color2 = null;

            internal Texture texture1 = null;
            internal Texture texture2 = null;

            internal Texture normal1 = null;
            internal Texture normal2 = null;

            internal float? brightness = null;

            // Movements
            internal float? rotatoSpeed = null;

            internal string pivotAround = null;
            internal Vector3? pivotPosition = null;
            internal Quaternion? pivotRotation = null;
            internal Vector3? pivotScale = null;
            internal float? pivotDistance = null;
            internal float? pivotRotatoSpeed = null;

            internal float? bobberSeed = null;
            internal Vector3? bobberOFS = null;

            // Lights
            internal ConfigNode[] lights = null;


            // New MenuObject from cfg
            internal MenuObject(ConfigNode node)
            {
                // Settings
                name = node.GetValue("name");

                index = Parse(node.GetValue("index"), index);

                template = Parse(node.GetValue("template"), template);

                if (!bool.TryParse(node.GetValue("enabled"), out enabled))
                {
                    enabled = true;
                }

                useChance = Parse(node.GetValue("useChance"), useChance);

                if (Time.frameCount % 100 >= useChance * 100)
                {
                    enabled = false;
                }

                adjustScale = Parse(node.GetValue("adjustScale"), adjustScale);
                removeJetpack = Parse(node.GetValue("removeJetpack"), removeJetpack);
                removeHelmet = Parse(node.GetValue("removeHelmet"), removeHelmet);
                addColliders = Parse(node.GetValue("addColliders"), addColliders);


                // Physical Parameters
                mesh = Parse(node.GetValue("mesh"), mesh);

                position = Parse(node.GetValue("position"), position);
                rotation = Parse(node.GetValue("rotation"), rotation);
                scale = Parse(node.GetValue("scale"), scale);


                // Visual Parameters
                material = Parse(node.GetValue("material"), material);
                shader = Parse(node.GetValue("shader"), shader);

                color1 = Parse(node.GetValue("color1"), color1);
                color2 = Parse(node.GetValue("color2"), color2);
                texture1 = Parse(node.GetValue("texture1"), texture1);
                texture2 = Parse(node.GetValue("texture2"), texture2);
                normal1 = Parse(node.GetValue("normal1"), normal1);
                normal2 = Parse(node.GetValue("normal2"), normal2);
                brightness = Parse(node.GetValue("brightness"), brightness);


                // Movements
                rotatoSpeed = Parse(node.GetValue("rotatoSpeed"), rotatoSpeed);

                pivotAround = node.GetValue("pivotAround");
                pivotPosition = Parse(node.GetValue("pivotPosition"), pivotPosition);
                pivotRotation = Parse(node.GetValue("pivotRotation"), pivotRotation);
                pivotScale = Parse(node.GetValue("pivotScale"), pivotScale);
                pivotDistance = Parse(node.GetValue("pivotDistance"), pivotDistance);
                pivotRotatoSpeed = Parse(node.GetValue("pivotRotatoSpeed"), pivotRotatoSpeed);

                bobberSeed = Parse(node.GetValue("bobberSeed"), bobberSeed);
                bobberOFS = Parse(node.GetValue("bobberOFS"), bobberOFS);

                // Lights
                lights = node.GetNode("LIGHTS")?.GetNodes("LIGHT");
            }

            // New MenuObject from name
            internal MenuObject(string name, bool enabled = true)
            {
                // Name
                this.name = name;
                this.enabled = enabled;
            }


            // Apply MenuObject to GameObject
            internal void ApplyTo(GameObject obj, float scaleMult = 1)
            {
                if (obj == null) return;
                if (Debug.debug) obj.AddOrGetComponent<LiveDebug>();

                // Edit Position/Rotation/Scale
                obj.transform.position = position ?? obj.transform.position;
                obj.transform.rotation = rotation ?? obj.transform.rotation;
                if (scale != null)
                    obj.transform.localScale = (Vector3)scale * scaleMult;

                // Adjust Scale by Distance
                if (adjustScale)
                    obj.transform.transform.localScale *= (0.25f + (new Vector3(0.7814472f, -0.7841411f, 2.28511f) - obj.transform.transform.position).magnitude / 100);


                // Edit Appearances
                MeshFilter meshFilter = obj.transform.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    meshFilter.mesh = mesh ?? meshFilter.mesh;
                }

                Renderer renderer = obj.transform.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = material ?? renderer.material;
                    renderer.material.shader = shader ?? renderer.material.shader;

                    renderer.material.SetTexture(texture1);
                    renderer.material.SetColor(color1);
                    renderer.material.SetNormal(normal1);
                }


                // Rotato
                if (rotatoSpeed != null)
                {
                    Rotato rotato = obj.AddOrGetComponent<Rotato>();
                    rotato.speed = (float)rotatoSpeed;
                }


                // Pivot
                if (!string.IsNullOrEmpty(pivotAround))
                {
                    GameObject scene = obj;

                    while (scene != null && scene.name != "OrbitScene" && scene.name != "MunScene")
                    {
                        scene = scene?.transform?.parent?.gameObject;
                    }

                    if (scene == null) return;

                    GameObject parent = null;

                    if (pivotAround != null)
                        parent = scene.GetChild(pivotAround);

                    parent = parent ?? obj?.transform?.parent?.gameObject;


                    if (parent != null)
                    {
                        GameObject pivot = new GameObject(obj.name + "_Pivot");
                        pivot.AddOrGetComponent<LiveDebug>();

                        pivot.transform.SetParent(parent.transform);

                        pivot.transform.position = pivotPosition ?? parent.transform.position;
                        pivot.transform.localRotation = pivotRotation ?? Quaternion.identity;
                        pivot.transform.localScale = pivotScale ?? Vector3.one;

                        obj.transform.SetParent(pivot.transform);

                        if (pivotDistance.HasValue)
                        {
                            obj.transform.position = pivot.transform.position + pivot.transform.right * pivotDistance.Value;
                        }
                    }
                }

                if (pivotRotatoSpeed != null)
                {
                    if (obj?.transform?.parent?.gameObject != null)
                    {
                        Rotato pivotRotato = obj.transform.parent.gameObject.AddOrGetComponent<Rotato>();
                        pivotRotato.speed = (float)pivotRotatoSpeed;
                    }
                }


                // Bobber
                if (bobberSeed != null || bobberOFS != null)
                {
                    Bobber bobber = obj.AddOrGetComponent<Bobber>();

                    bobber.seed = bobberSeed ?? bobber.seed;

                    bobber.ofs1 = bobberOFS?.x ?? bobber.ofs1;
                    bobber.ofs2 = bobberOFS?.y ?? bobber.ofs2;
                    bobber.ofs3 = bobberOFS?.z ?? bobber.ofs3;
                }
            }


            // Coronas
            internal void AddCoronas(GameObject body, GameObject template)
            {
                SunCoronas[] coronas = template?.GetComponentsInChildren<SunCoronas>();

                for (int i = 0; i < coronas?.Length; i++)
                {
                    GameObject corona = coronas[i].gameObject;
                    GameObject cloneCorona = Object.Instantiate(corona.gameObject);

                    cloneCorona.name = corona.name;
                    cloneCorona.transform.SetParent(body.transform);
                    cloneCorona.transform.localPosition = corona.transform.localPosition;
                    cloneCorona.transform.localRotation = corona.transform.localRotation;
                    cloneCorona.transform.localScale = corona.transform.localScale;
                    cloneCorona.SetLayerRecursive(15);
                }
            }

            // SunFlare
            internal void AddFlare(GameObject body, CelestialBody template)
            {
                SunFlare[] flares = Resources.FindObjectsOfTypeAll<SunFlare>();

                for (int i = 0; i < flares?.Length; i++)
                {
                    // Flare
                    SunFlare flare = flares[i];

                    if (flare.enabled && flare.sun == template)
                    {
                        // Disable the original SunFlare component
                        flare.enabled = false;

                        // Instantiate LensFlare
                        int oldDelegates = Camera.onPreCull?.GetInvocationList()?.Length ?? 0;              // KopernicusSunFlare compatibility

                        LensFlare lensFlare = Object.Instantiate(flare.sunFlare);

                        // Disable the clone
                        lensFlare.gameObject.SetActive(false);

                        // Add FlareRemover component to the real flare
                        flare.sunFlare.gameObject.AddOrGetComponent<FlareRemover>();

                        // Remove SunFlare component from the clone
                        Object.DestroyImmediate(lensFlare.GetComponent<SunFlare>());

                        System.Delegate[] newDelegates = Camera.onPreCull?.GetInvocationList();             // KopernicusSunFlare compatibility
                        if (newDelegates?.Length == oldDelegates + 1)                                       // KopernicusSunFlare compatibility
                            Camera.onPreCull -= (Camera.CameraCallback)newDelegates[oldDelegates];          // KopernicusSunFlare compatibility

                        // Reposition the GameObject
                        lensFlare.transform.SetParent(flare.transform.parent);
                        lensFlare.transform.position = flare.transform.position;

                        // Add FlareCamera component
                        FlareCamera camera = body.AddOrGetComponent<FlareCamera>();
                        camera.maxBrightness = brightness ?? lensFlare.brightness;
                        camera.flare = lensFlare;
                        lensFlare.brightness = 0;

                        // Re-enable the original SunFlare component
                        flare.enabled = true;

                        // Activate LensFlare
                        lensFlare.gameObject.SetActive(true);
                        lensFlare.enabled = true;

                        return;
                    }
                }
            }


            // Parsers
            Mesh Parse(string s, Mesh defaultValue)
            {
                if (string.IsNullOrEmpty(s)) return defaultValue;
                Debug.Log("Mesh.Parse", "Parsing mesh from string = " + s);

                Mesh output = null;

                if (s.StartsWith("BUILTIN/"))
                {
                    string[] path = s.Split('/');

                    Debug.Log("Mesh.Parse", "BUILTIN mesh. path.Length = " + path.Length);
                    if (path.Length > 1)
                    {
                        CelestialBody planet = FlightGlobals.Bodies.FirstOrDefault(b => b.transform.name == path[1]);
                        Debug.Log("Mesh.Parse", "planet = " + planet);

                        if (path.Length > 4 && path[2] == "PQSLandControl" && path[3] == "scatters")
                        {
                            PQSLandControl landControl = planet?.pqsController?.GetComponentsInChildren<PQSLandControl>(true)?.FirstOrDefault();
                            Debug.Log("Mesh.Parse", "landControl = " + landControl);

                            PQSLandControl.LandClassScatter scatter = landControl?.scatters?.FirstOrDefault(i => i.scatterName == path[4]);
                            Debug.Log("Mesh.Parse", "scatter = " + scatter);

                            Mesh mesh = scatter?.baseMesh;
                            if (KopernicusFixer.detect)
                                mesh = planet.GetModularScatterMesh(scatter);
                            Debug.Log("Mesh.Parse", "mesh = " + mesh);

                            output = mesh;
                        }
                        else if (path.Length > 3 && path[2] == "PQSCity")
                        {
                            PQSCity[] mods = planet?.pqsController?.GetComponentsInChildren<PQSCity>(true);

                            for (int i = 0; i < mods?.Length; i++)
                            {
                                if (mods[i].name == path[3])
                                {
                                    int index = 0;

                                    if (path.Length > 4) int.TryParse(path[4], out index);

                                    output = mods[i].GetComponentsInChildren<MeshFilter>(true)?.Skip(index)?.FirstOrDefault()?.mesh;

                                    break;
                                }
                            }
                        }
                        else if (path.Length > 3 && path[2] == "PQSCity2")
                        {
                            PQSCity2[] mods = planet?.pqsController?.GetComponentsInChildren<PQSCity2>(true);

                            for (int i = 0; i < mods?.Length; i++)
                            {
                                if (mods[i].name == path[3])
                                {
                                    int index = 0;

                                    if (path.Length > 4) int.TryParse(path[4], out index);

                                    output = mods[i].GetComponentsInChildren<MeshFilter>(true)?.Skip(index)?.FirstOrDefault()?.mesh;

                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    output = Resources.FindObjectsOfTypeAll<Mesh>().FirstOrDefault(m => m.name == s);
                    Debug.Log("Mesh.Parse", "mesh = " + output);
                }

                return output ?? defaultValue;
            }

            Shader Parse(string s, Shader defaultValue)
            {
                if (string.IsNullOrEmpty(s)) return defaultValue;

                Shader output = null;

                if (s.StartsWith("BUILTIN/"))
                {
                    string[] path = s.Split('/');

                    if (path.Length > 1)
                    {
                        CelestialBody planet = FlightGlobals.Bodies.FirstOrDefault(b => b.transform.name == path[1]);

                        if (path.Length > 4 && path[2] == "PQSLandControl" && path[3] == "scatters")
                        {
                            PQSLandControl landControl = planet?.pqsController?.GetComponentsInChildren<PQSLandControl>(true)?.FirstOrDefault();

                            for (int i = 0; i < landControl?.scatters?.Length; i++)
                            {
                                if (landControl.scatters[i].scatterName == path[4])
                                {
                                    output = landControl.scatters[i].material?.shader;
                                    break;
                                }
                            }
                        }
                        else if (path.Length > 3 && path[2] == "PQSCity")
                        {
                            PQSCity[] mods = planet?.pqsController?.GetComponentsInChildren<PQSCity>(true);

                            for (int i = 0; i < mods?.Length; i++)
                            {
                                if (mods[i].name == path[3])
                                {
                                    int index = 0;

                                    if (path.Length > 4) int.TryParse(path[4], out index);

                                    output = mods[i].GetComponentsInChildren<Renderer>(true)?.Skip(index)?.FirstOrDefault()?.material?.shader;

                                    break;
                                }
                            }
                        }
                        else if (path.Length > 3 && path[2] == "PQSCity2")
                        {
                            PQSCity2[] mods = planet?.pqsController?.GetComponentsInChildren<PQSCity2>(true);

                            for (int i = 0; i < mods?.Length; i++)
                            {
                                if (mods[i].name == path[3])
                                {
                                    int index = 0;

                                    if (path.Length > 4) int.TryParse(path[4], out index);

                                    output = mods[i].GetComponentsInChildren<Renderer>(true)?.Skip(index)?.FirstOrDefault()?.material?.shader;

                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    output = Resources.FindObjectsOfTypeAll<Shader>().FirstOrDefault(h => h.name == s);
                }

                return output ?? defaultValue;
            }

            Material Parse(string s, Material defaultValue)
            {
                if (string.IsNullOrEmpty(s)) return defaultValue;

                Material output = null;

                if (s.StartsWith("BUILTIN/"))
                {
                    string[] path = s.Split('/');

                    if (path.Length > 1)
                    {
                        CelestialBody planet = FlightGlobals.Bodies.FirstOrDefault(b => b.transform.name == path[1]);

                        if (path.Length > 4 && path[2] == "PQSLandControl" && path[3] == "scatters")
                        {
                            PQSLandControl landControl = planet?.pqsController?.GetComponentsInChildren<PQSLandControl>(true)?.FirstOrDefault();

                            for (int i = 0; i < landControl?.scatters?.Length; i++)
                            {
                                if (landControl.scatters[i].scatterName == path[4])
                                {
                                    output = landControl.scatters[i].material;
                                    break;
                                }
                            }
                        }
                        else if (path.Length > 3 && path[2] == "PQSCity")
                        {
                            PQSCity[] mods = planet?.pqsController?.GetComponentsInChildren<PQSCity>(true);

                            for (int i = 0; i < mods?.Length; i++)
                            {
                                if (mods[i].name == path[3])
                                {
                                    int index = 0;

                                    if (path.Length > 4) int.TryParse(path[4], out index);

                                    output = mods[i].GetComponentsInChildren<Renderer>(true)?.Skip(index)?.FirstOrDefault()?.material;
                                    break;
                                }
                            }
                        }
                        else if (path.Length > 3 && path[2] == "PQSCity2")
                        {
                            PQSCity2[] mods = planet?.pqsController?.GetComponentsInChildren<PQSCity2>(true);

                            for (int i = 0; i < mods?.Length; i++)
                            {
                                if (mods[i].name == path[3])
                                {
                                    int index = 0;

                                    if (path.Length > 4) int.TryParse(path[4], out index);

                                    output = mods[i].GetComponentsInChildren<Renderer>(true)?.Skip(index)?.FirstOrDefault()?.material;

                                    break;
                                }
                            }
                        }
                    }
                }

                return output ?? defaultValue;
            }
        }
    }
}
