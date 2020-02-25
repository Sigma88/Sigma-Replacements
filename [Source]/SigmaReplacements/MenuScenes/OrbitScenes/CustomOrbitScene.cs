using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class CustomOrbitScene : CustomMenuScene
        {
            // Bodies
            MenuObject planet = null;
            MenuObject[] moons = null;

            // Scatter
            MenuObject[] scatter = null;

            // Kerbals
            MenuObject[] kerbals = null;

            // Lights
            MenuLight[] lights = null;
            static string[] lightNames = null;
            static Dictionary<string, GameObject> lightTemplates = null;

            internal CustomOrbitScene(OrbitSceneInfo info)
            {
                Debug.Log("CustomOrbitScene", "Custom Orbit Scene name = " + info.name);

                // Bodies
                planet = Parse(info.planet, planet);
                moons = Parse(info.moons, moons);

                // Scatter
                scatter = Parse(info.scatter, scatter);

                // Kerbals
                kerbals = Parse(info.kerbals, kerbals);

                // Lights
                lights = Parse(info.lights, lights);
            }

            internal void ApplyTo(GameObject[] scenes)
            {
                GameObject scene = scenes[1];

                // Bodies
                EditPlanet(planet, scene);
                EditMoons(moons, scene);

                // Scatter
                AddScatter(scatter, scene, scenes[0].GetChild("sandcastle"));

                // Kerbals
                EditKerbals(kerbals, scene);

                // Lights
                EditLights(lights, scene);
                CleanUpLights();
            }

            void EditPlanet(MenuObject info, GameObject scene)
            {
                // If info is null generate a default one
                info = info ?? new MenuObject("Kerbin");

                // Find the Planet GameObject
                GameObject planet = scene.GetChild("Kerbin");

                if (!info.enabled)
                {
                    planet.GetComponent<Renderer>().enabled = false;
                    Debug.Log("EditPlanet", "Disabled Kerbin Renderer");
                    return;
                }

                Debug.Log("EditPlanet", "Kerbin position = " + (Vector3d)planet.transform.position);
                Debug.Log("EditPlanet", "Kerbin rotation = " + (Vector3d)planet.transform.eulerAngles);
                Debug.Log("EditPlanet", "Kerbin scale = " + (Vector3d)planet.transform.localScale);
                Debug.Log("EditPlanet", "Kerbin rotatoSpeed = " + planet?.GetComponent<Rotato>()?.speed);

                // Edit Visual Parameters
                Renderer renderer = planet.GetComponent<Renderer>();

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
                    MeshFilter meshFilter = planet.GetComponent<MeshFilter>();
                    meshFilter.mesh = template?.GetComponent<MeshFilter>()?.mesh ?? meshFilter.mesh;

                    // Coronas
                    info.AddCoronas(planet, template);

                    // Flare
                    if (info.brightness != 0 && !Debug.debug)
                    {
                        FlareFixer flare = planet.AddComponent<FlareFixer>();
                        flare.template = cb;
                        flare.info = info;
                    }
                }

                info.ApplyTo(planet, 1.4987610578537f);

                // Add Colliders
                if (info.addColliders)
                {
                    planet.layer = 15;
                    if (planet.GetComponent<Collider>() == null)
                    {
                        MeshCollider collider = planet.AddComponent<MeshCollider>();
                        collider.isTrigger = true;
                    }
                }

                // Add Lights
                for (int l = 0; l < info.lights?.Length; l++)
                {
                    ConfigNode node = info.lights[l];

                    MenuLight menuLight = new MenuLight(node);

                    GameObject lightObj = GetLight(menuLight, scene);

                    if (!lightTemplates.ContainsKey(lightObj.name))
                    {
                        lightObj.transform.SetParent(planet.transform);
                        lightObj.transform.localPosition = Vector3.zero;
                        lightObj.transform.localRotation = Quaternion.identity;
                        lightObj.transform.localScale = Vector3.one;

                        menuLight.ApplyTo(lightObj, scene);
                    }
                }
            }

            void EditMoons(MenuObject[] moons, GameObject scene)
            {
                if (!(moons?.Length > 0))
                {
                    CelestialBody defaultMoon = FlightGlobals.Bodies?.Where(b => b?.referenceBody?.transform?.name == "Kerbin")?.OrderBy(b => b?.Radius)?.LastOrDefault();
                    moons = new MenuObject[] { new MenuObject(defaultMoon?.name ?? "Mun", defaultMoon != null) };
                }

                // Get Stock Body
                GameObject mun = scene.GetChild("Mun");
                GameObject clone = Instantiate(mun);
                Debug.Log("EditMoons", "Mun position = " + (Vector3d)mun.transform.position);
                Debug.Log("EditMoons", "Mun rotation = " + (Vector3d)mun.transform.eulerAngles);
                Debug.Log("EditMoons", "Mun scale = " + (Vector3d)mun.transform.localScale);
                Debug.Log("EditMoons", "Mun rotatoSpeed = " + mun?.GetComponent<Rotato>()?.speed);

                for (int i = 0; i < moons.Length; i++)
                {
                    MenuObject info = moons[i];
                    GameObject body;

                    // Clone or Select Stock Body 
                    if (info.name == "Mun")
                    {
                        body = mun;
                        body.SetActive(info.enabled);

                        if (!info.enabled) continue;
                    }
                    else if (info.enabled)
                    {
                        if (string.IsNullOrEmpty(info.name)) continue;

                        body = Instantiate(clone);
                        body.name = "NewMoon_" + info.name;
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

                        // Mesh
                        MeshFilter meshFilter = body.GetComponent<MeshFilter>();
                        meshFilter.mesh = template?.GetComponent<MeshFilter>()?.mesh ?? meshFilter.mesh;

                        // Coronas
                        info.AddCoronas(body, template);

                        // Flare
                        if (info.brightness != 0 || Debug.debug)
                        {
                            FlareFixer flare = body.AddComponent<FlareFixer>();
                            flare.template = cb;
                            flare.info = info;
                        }
                    }

                    // Edit Textures
                    renderer.material.SetTexture(info.texture1);
                    renderer.material.SetNormal(info.normal1);
                    renderer.material.SetColor(info.color1);

                    // Edit Physical Parameters
                    info.scale = info.scale ?? Vector3.one;
                    info.ApplyTo(body, 0.209560245275497f);

                    // Add Colliders
                    if (info.addColliders)
                    {
                        body.layer = 15;
                        if (body.GetComponent<Collider>() == null)
                        {
                            MeshCollider collider = body.AddComponent<MeshCollider>();
                            collider.isTrigger = true;
                        }
                    }

                    // Add Lights
                    for (int l = 0; l < info.lights?.Length; l++)
                    {
                        ConfigNode node = info.lights[l];

                        MenuLight menuLight = new MenuLight(node);

                        GameObject lightObj = GetLight(menuLight, scene);

                        if (!lightTemplates.ContainsKey(lightObj.name))
                        {
                            lightObj.transform.SetParent(body.transform);
                            lightObj.transform.localPosition = Vector3.zero;
                            lightObj.transform.localRotation = Quaternion.identity;
                            lightObj.transform.localScale = Vector3.one;

                            menuLight.ApplyTo(lightObj, scene);
                        }
                    }
                }

                // CleanUp
                Object.DestroyImmediate(clone);
            }

            void AddScatter(MenuObject[] scatters, GameObject scene, GameObject template)
            {
                Debug.Log("AddScatter", "scatters count = " + scatters?.Length);
                if (!(scatters?.Length > 0)) return;

                Debug.Log("AddScatter", "template position = " + (Vector3d)template.transform.position);
                Debug.Log("AddScatter", "template rotation = " + (Vector3d)template.transform.eulerAngles);
                Debug.Log("AddScatter", "template scale = " + (Vector3d)template.transform.localScale);

                for (int i = 0; i < scatters?.Length; i++)
                {
                    if (!scatters[i].enabled) continue;

                    MenuObject info = scatters[i];
                    GameObject scatter = Object.Instantiate(template);
                    scatter.transform.SetParent(scene.transform);
                    Object.DestroyImmediate(scatter.GetComponent<SandCastleLogic>());
                    scatter.name = info.name;

                    info.ApplyTo(scatter);
                }
            }

            void EditKerbals(MenuObject[] info, GameObject scene)
            {
                Debug.Log("EditKerbals", "info count = " + info?.Length);
                if (!(info?.Length > 0)) return;

                // Get Stock Kerbal
                Transform kerbals = scene.GetChild("Kerbals").transform;
                Debug.Log("EditKerbals", "kerbal templates count = " + kerbals?.childCount);

                if (kerbals == null || kerbals.childCount < 4) return;

                GameObject[] templates = GetOrbitKerbals();
                GameObject[] munTemplates = GetMunKerbals();

                if (Debug.debug)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Debug.Log("EditKerbals", "template[" + i + "] position = " + (Vector3d)templates[i].transform.position);
                        Debug.Log("EditKerbals", "template[" + i + "] rotation = " + (Vector3d)templates[i].transform.eulerAngles);
                        Debug.Log("EditKerbals", "template[" + i + "] scale = " + (Vector3d)templates[i].transform.localScale);

                        if (templates[i].GetComponent<Bobber>() is Bobber bobber)
                        {
                            Debug.Log("EditKerbals", "template[" + i + "] bobberSeed = " + (double)bobber.seed);
                            Debug.Log("EditKerbals", "template[" + i + "] bobberOFS = " + (Vector3d)(new Vector3(bobber.ofs1, bobber.ofs2, bobber.ofs3)));
                            Debug.Log("EditKerbals", "template[" + i + "] bobberVAL = " + (Vector3d)(new Vector3(bobber.val1, bobber.val2, bobber.val3)));
                        }
                    }
                }

                for (int i = 0; i < info?.Length; i++)
                {
                    GameObject kerbal;

                    // Clone or Select Stock newGuy
                    if (info[i].index >= 0 && info[i].index <= 3)
                    {
                        kerbal = kerbals.GetChild(i).gameObject;
                        kerbal.SetActive(info[i].enabled);
                        if (!info[i].enabled) continue;
                    }
                    else if (info[i].enabled)
                    {
                        if (string.IsNullOrEmpty(info[i].name)) continue;

                        int? template = info[i].template;

                        if (template >= 0 && template <= 3)
                        {
                            kerbal = Instantiate(templates[template.Value]);
                        }
                        else if (template >= -2 && template <= -1)
                        {
                            kerbal = Instantiate(munTemplates[-template.Value - 1]);
                            kerbal.transform.SetParent(templates[0].transform.parent);
                        }
                        else
                        {
                            continue;
                        }

                        kerbal.name = info[i].name;
                    }
                    else
                    {
                        continue;
                    }

                    // Apply Physical Parameters
                    info[i].ApplyTo(kerbal);

                    // Remove JetPack
                    GameObject jetpack = kerbal?.GetChild("jetpack01");
                    if (jetpack != null && info[i].removeJetpack)
                    {
                        jetpack.SetActive(false);
                    }

                    // Remove Helmet
                    GameObject helmet = kerbal.GetChild("helmet01") ?? kerbal.GetChild("mesh_female_kerbalAstronaut01_helmet01");
                    if (helmet != null)
                    {
                        helmet.SetActive(!info[i].removeHelmet);
                    }

                    // Add Colliders
                    if (info[i].addColliders)
                    {
                        KerbalColliders(kerbal, helmet);
                    }
                }

                // CleanUp
                for (int i = 0; i < 4; i++)
                {
                    Object.DestroyImmediate(templates[i]);
                }
                for (int i = 0; i < 2; i++)
                {
                    Object.DestroyImmediate(munTemplates[i]);
                }
            }

            void EditLights(MenuLight[] info, GameObject scene)
            {
                if (!(info?.Length > 0)) return;

                if (scene == null) return;

                for (int i = 0; i < info?.Length; i++)
                {
                    GameObject lightObj = GetLight(info[i], scene);

                    // Apply MenuLight
                    info[i].ApplyTo(lightObj, scene);
                }
            }

            void CleanUpLights()
            {
                if (lightTemplates != null)
                {
                    // CleanUp
                    for (int i = 0; i < lightNames.Length; i++)
                    {
                        Object.DestroyImmediate(lightTemplates[lightNames[i]]);
                    }

                    lightTemplates = null;
                }
            }

            GameObject GetLight(MenuLight info, GameObject scene)
            {
                GameObject lightObj = null;

                if (lightTemplates == null)
                    GetLightTemplates(scene);

                // Clone or Select Stock Light
                if (lightTemplates.ContainsKey(info.name))
                {
                    lightObj = scene.GetChild(info.name);
                    lightObj.SetActive(info.enabled);
                }
                else if (info.enabled)
                {
                    if (!string.IsNullOrEmpty(info.name))
                    {
                        if (lightTemplates.ContainsKey(info.template))
                        {
                            lightObj = Instantiate(lightTemplates[info.template]);
                            lightObj.name = info.name;
                        }
                    }
                }

                return lightObj;
            }

            void GetLightTemplates(GameObject scene)
            {
                lightNames = new string[] { "BackLight", "FillLight", "KeyLight", "PlanetLight" };

                lightTemplates = new Dictionary<string, GameObject>
                {
                    { lightNames[0], Instantiate(scene.GetChild(lightNames[0])) },
                    { lightNames[1], Instantiate(scene.GetChild(lightNames[1])) },
                    { lightNames[2], Instantiate(scene.GetChild(lightNames[2])) },
                    { lightNames[3], Instantiate(scene.GetChild(lightNames[3])) }
                };

                if (Debug.debug)
                {
                    for (int i = 0; i < lightNames.Length; i++)
                    {
                        GameObject light = lightTemplates[lightNames[i]];
                        Debug.Log("GetLightTemplates", "template[" + i + "] = " + light);
                        Debug.Log("GetLightTemplates", "    position = " + (Vector3d)light.transform.position);
                        Debug.Log("GetLightTemplates", "    rotation = " + (Vector3d)light.transform.eulerAngles);
                        Debug.Log("GetLightTemplates", "    scale = " + (Vector3d)light.transform.localScale);
                    }
                }
            }
        }
    }
}
