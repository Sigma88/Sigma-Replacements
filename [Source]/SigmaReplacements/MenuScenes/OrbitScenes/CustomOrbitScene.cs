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
            }

            internal void ApplyTo(GameObject[] scenes)
            {
                // Bodies
                EditPlanet(planet, scenes[1]);
                EditMoons(moons, scenes[1]);

                // Scatter
                AddScatter(scatter, scenes[1], scenes[0].GetChild("sandcastle"));

                // Kerbals
                EditKerbals(kerbals, scenes[1]);
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
                }

                info.ApplyTo(planet, 1.4987610578537f);
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
                    if (i > 0 && info.enabled)
                    {
                        if (string.IsNullOrEmpty(info.name)) continue;

                        body = Instantiate(clone);
                        body.name = "NewMoon_" + info.name;
                    }
                    else if (i == 0)
                    {
                        body = mun;
                        body.SetActive(info.enabled);

                        if (!info.enabled) continue;
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
                    }

                    // Edit Textures
                    renderer.material.SetTexture(info.texture1);
                    renderer.material.SetNormal(info.normal1);
                    renderer.material.SetColor(info.color1);

                    // Edit Physical Parameters
                    info.scale = info.scale ?? Vector3.one;
                    float mult = (float)((cb?.Radius ?? 200000) / 200000);
                    info.ApplyTo(body, 0.209560245275497f * mult);
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

                GameObject[] templates = new GameObject[] { Instantiate(kerbals.GetChild(0).gameObject), Instantiate(kerbals.GetChild(1).gameObject), Instantiate(kerbals.GetChild(2).gameObject), Instantiate(kerbals.GetChild(3).gameObject) };

                if (Debug.debug)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Debug.Log("EditKerbals", "template[" + i + "] position = " + (Vector3d)templates[i].transform.position);
                        Debug.Log("EditKerbals", "template[" + i + "] rotation = " + (Vector3d)templates[i].transform.eulerAngles);
                        Debug.Log("EditKerbals", "template[" + i + "] scale = " + (Vector3d)templates[i].transform.localScale);

                        Bobber bobber = templates[i].GetComponent<Bobber>();
                        if (bobber != null)
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
                    else if (info[i].enabled && info[i].template >= 0 && info[i].template <= 3)
                    {
                        if (string.IsNullOrEmpty(info[i].name)) continue;

                        kerbal = Instantiate(templates[(int)info[i].template]);
                        kerbal.name = info[i].name;
                    }
                    else
                    {
                        continue;
                    }

                    // Apply Physical Parameters
                    info[i].ApplyTo(kerbal);

                    // Remove Helmet
                    GameObject helmet = kerbal.GetChild("helmet01") ?? kerbal.GetChild("mesh_female_kerbalAstronaut01_helmet01");
                    if (helmet != null)
                    {
                        helmet.SetActive(!info[i].removeHelmet);
                    }
                }

                // CleanUp
                for (int i = 0; i < 4; i++)
                {
                    Object.DestroyImmediate(templates[i]);
                }
            }
        }
    }
}
