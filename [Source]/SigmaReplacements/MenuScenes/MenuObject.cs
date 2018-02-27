﻿using System.Linq;
using System.IO;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class MenuObject : Info
        {
            internal bool debug = false;
            internal bool enabled = true;
            internal int? index = null;

            internal Vector3? position = null;
            internal Quaternion? rotation = null;
            internal Vector3? scale = null;
            internal float? rotatoSpeed = null;

            internal Material material = null;

            internal Mesh mesh = null;
            internal Shader shader = null;

            internal Color? color1 = null;
            internal Color? color2 = null;

            internal Texture texture1 = null;
            internal Texture texture2 = null;

            internal Texture normal1 = null;
            internal Texture normal2 = null;


            internal bool adjustScale = false;

            internal bool removeHelmet = false;

            internal MenuObject(ConfigNode node)
            {
                name = node.GetValue("name");

                index = Parse(node.GetValue("index"), index);

                bool.TryParse(node.GetValue("debug"), out debug);

                if (!bool.TryParse(node.GetValue("enabled"), out enabled))
                {
                    enabled = true;
                }

                useChance = Parse(node.GetValue("useChance"), useChance);

                if (Time.frameCount % 100 >= useChance * 100)
                {
                    enabled = false;
                }

                position = Parse(node.GetValue("position"), position);
                rotation = Parse(node.GetValue("rotation"), rotation);
                scale = Parse(node.GetValue("scale"), scale);
                
                rotatoSpeed = Parse(node.GetValue("rotatoSpeed"), rotatoSpeed);

                material = Parse(node.GetValue("material"), material);
                shader = Parse(node.GetValue("shader"), shader);
                mesh = Parse(node.GetValue("mesh"), mesh);

                color1 = Parse(node.GetValue("color1"), color1);
                color2 = Parse(node.GetValue("color2"), color2);
                texture1 = Parse(node.GetValue("texture1"), texture1);
                texture2 = Parse(node.GetValue("texture2"), texture2);
                normal1 = Parse(node.GetValue("normal1"), normal1);
                normal2 = Parse(node.GetValue("normal2"), normal2);

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
                                    output = landControl.scatters[i].baseMesh;
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

        internal class LiveDebug : MonoBehaviour
        {
            static double time = 0.1;
            static float moveby = 0.05f;

            void Update()
            {
                time += Time.deltaTime;

                if (time < 0.1) return;
                else time = 0;

                // POSITION
                if (Input.GetKey(KeyCode.LeftArrow))
                    transform.localPosition = new Vector3(transform.localPosition.x - moveby, transform.localPosition.y, transform.localPosition.z);
                else if (Input.GetKey(KeyCode.RightArrow))
                    transform.localPosition = new Vector3(transform.localPosition.x + moveby, transform.localPosition.y, transform.localPosition.z);
                else if (Input.GetKey(KeyCode.DownArrow))
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - moveby, transform.localPosition.z);
                else if (Input.GetKey(KeyCode.UpArrow))
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + moveby, transform.localPosition.z);
                else if (Input.GetKey(KeyCode.RightControl))
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - moveby);
                else if (Input.GetKey(KeyCode.RightShift))
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveby);

                // ROTATION
                else if (Input.GetKey(KeyCode.W))
                    transform.localRotation = Quaternion.Euler((transform.localEulerAngles.x + 1) % 360, transform.localEulerAngles.y, transform.localEulerAngles.z);
                else if (Input.GetKey(KeyCode.S))
                    transform.localRotation = Quaternion.Euler((transform.localEulerAngles.x + 359) % 360, transform.localEulerAngles.y, transform.localEulerAngles.z);
                else if (Input.GetKey(KeyCode.A))
                    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, (transform.localEulerAngles.y + 1) % 360, transform.localEulerAngles.z);
                else if (Input.GetKey(KeyCode.D))
                    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, (transform.localEulerAngles.y + 359) % 360, transform.localEulerAngles.z);
                else if (Input.GetKey(KeyCode.Q))
                    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, (transform.localEulerAngles.z + 1) % 360);
                else if (Input.GetKey(KeyCode.E))
                    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, (transform.localEulerAngles.z + 359) % 360);

                // SCALE
                else if (Input.GetKey(KeyCode.KeypadMinus))
                    transform.localScale = transform.localScale * 0.95f;
                else if (Input.GetKey(KeyCode.KeypadPlus))
                    transform.localScale = transform.localScale * 1.05f;

                // NOTHING
                else return;

                Save();
            }

            void Save()
            {
                Directory.CreateDirectory("GameData/Sigma/Replacements/MenuScenes/Debug/");

                string[] data = new string[3];

                data[0] = "position = " + (Vector3d)transform.position;
                data[1] = "rotation = " + (Vector3d)transform.eulerAngles;
                data[2] = "scale = " + (Vector3d)transform.localScale;

                File.WriteAllLines("GameData/Sigma/Replacements/MenuScenes/Debug/" + name + ".txt", data);
            }
        }
    }
}
