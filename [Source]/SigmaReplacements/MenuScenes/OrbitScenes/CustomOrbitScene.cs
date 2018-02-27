using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class CustomOrbitScene : CustomMenuScene
        {
            // Sky
            MenuObject planet = null;
            MenuObject[] moons = null;

            // Kerbals
            MenuObject[] maleKerbals = null;
            MenuObject[] femaleKerbals = null;

            internal CustomOrbitScene(MenuSceneInfo info)
            {
            }

            internal void ApplyTo(GameObject scene)
            {
            }

            void EditPlanet(MenuObject info, GameObject scene)
            {
                if (info == null) return;

                GameObject planet = scene.GetChild("Kerbin");

                if (!info.enabled)
                {
                    planet.GetComponent<Renderer>().enabled = false;
                    return;
                }
            }

            void EditMoons(MenuObject[] moons, GameObject scene)
            {
                if (moons == null) return;

                // Get Stock Body
                GameObject kerbin = scene.GetChild("Kerbin");
                Debug.Log("EditMoons", "Kerbin position = " + (Vector3d)kerbin.transform.position);
                Debug.Log("EditMoons", "Kerbin rotation = " + kerbin.transform.eulerAngles);
                Debug.Log("EditMoons", "Kerbin scale = " + kerbin.transform.localScale);
                Debug.Log("EditMoons", "Kerbin rotatoSpeed = " + kerbin?.GetComponent<Rotato>()?.speed);

                for (int i = moons.Length; i > 0; i--)
                {
                    MenuObject info = moons[i - 1];
                    GameObject body;

                    // Clone or Select Stock Body
                    if (i - 1 > 0 && info.enabled)
                    {
                        if (string.IsNullOrEmpty(info.name)) continue;

                        body = Instantiate(kerbin);
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
                    //if (info.scale != null)
                    //    body.transform.localScale = (Vector3)info.scale * 0.1483542f;

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
                }
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

                        kerbal = Instantiate(template);
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
        }
    }
}
