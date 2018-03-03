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

            internal CustomOrbitScene(OrbitSceneInfo info)
            {
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
                if (info == null) return;

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
                if (!(moons?.Length > 0)) return;

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
                        // Material
                        renderer.material = template?.GetComponent<Renderer>()?.material ?? renderer.material;
                        renderer.material.SetTexture(info.texture1);
                        renderer.material.SetNormal(info.normal1);
                        renderer.material.SetColor(info.color1);

                        // Mesh
                        MeshFilter meshFilter = body.GetComponent<MeshFilter>();
                        meshFilter.mesh = template?.GetComponent<MeshFilter>()?.mesh ?? meshFilter.mesh;
                    }

                    // Edit Physical Parameters
                    info.scale = info.scale ?? Vector3.one;
                    float mult = (float)((cb?.Radius ?? 200000) / 200000);
                    info.ApplyTo(body, 0.209560245275497f * mult);

                    // CleanUp
                    Object.Destroy(clone);
                }
            }

            void AddScatter(MenuObject[] scatters, GameObject scene, GameObject template)
            {
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
                if (!(info?.Length > 0)) return;

                // Get Stock Kerbal
                Transform kerbals = scene.GetChild("Kerbals").transform;

                if (kerbals == null || kerbals.childCount != 4) return;

                GameObject[] templates = new GameObject[] { Instantiate(kerbals.GetChild(0).gameObject), Instantiate(kerbals.GetChild(1).gameObject), Instantiate(kerbals.GetChild(2).gameObject), Instantiate(kerbals.GetChild(3).gameObject) };

                if (Debug.debug)
                {
                    for (int i = 0; i < 4; i++)
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
                    if (i > 3 && info[i].enabled)
                    {
                        if (string.IsNullOrEmpty(info[i].name)) continue;
                        if (!(info[i]?.template >= 0 && info[i]?.template < 4)) continue;

                        kerbal = Instantiate(templates[(int)info[i].template]);
                        kerbal.name = info[i].name;
                    }
                    else if (i < 3)
                    {
                        kerbal = kerbals.GetChild(i).gameObject;
                        kerbal.SetActive(info[i].enabled);
                        if (!info[i].enabled) continue;
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
                    Object.Destroy(templates[i]);
                }
            }
        }
    }
}
