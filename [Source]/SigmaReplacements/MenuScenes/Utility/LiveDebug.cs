using System.IO;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class LiveDebug : MonoBehaviour
        {
            // Debug
            bool debug = false;

            // Original values
            internal int? index = null;
            Vector3 originalPosition;
            Quaternion originalRotation;
            Vector3 originalScale;

            // Settings
            float moveby;
            float rotateby;
            float scaleby;

            void Start()
            {
                // Original values
                originalPosition = transform.position;
                originalRotation = transform.localRotation;
                originalScale = transform.localScale;

                // Settings
                moveby = 1;
                rotateby = 50;
                scaleby = 0.5f;

                // SAVE
                StartCoroutine(CallbackUtil.DelayedCallback(1, Save));
            }

            void Update()
            {
                if (Input.anyKey)
                {
                    if (debug)
                    {
                        // POSITION
                        if (Input.GetKey(KeyCode.LeftArrow))
                            transform.position += Vector3.left * moveby * Time.deltaTime;
                        if (Input.GetKey(KeyCode.RightArrow))
                            transform.position += Vector3.right * moveby * Time.deltaTime;
                        if (Input.GetKey(KeyCode.DownArrow))
                            transform.position += Vector3.down * moveby * Time.deltaTime;
                        if (Input.GetKey(KeyCode.UpArrow))
                            transform.position += Vector3.up * moveby * Time.deltaTime;
                        if (Input.GetKey(KeyCode.RightControl))
                            transform.position += Vector3.back * moveby * Time.deltaTime;
                        if (Input.GetKey(KeyCode.RightShift))
                            transform.position += Vector3.forward * moveby * Time.deltaTime;

                        // ROTATION
                        if (Input.GetKey(KeyCode.S))
                            transform.Rotate(Vector3.left, rotateby * Time.deltaTime);
                        if (Input.GetKey(KeyCode.W))
                            transform.Rotate(Vector3.right, rotateby * Time.deltaTime);
                        if (Input.GetKey(KeyCode.D))
                            transform.Rotate(Vector3.down, rotateby * Time.deltaTime);
                        if (Input.GetKey(KeyCode.A))
                            transform.Rotate(Vector3.up, rotateby * Time.deltaTime);
                        if (Input.GetKey(KeyCode.E))
                            transform.Rotate(Vector3.back, rotateby * Time.deltaTime);
                        if (Input.GetKey(KeyCode.Q))
                            transform.Rotate(Vector3.forward, rotateby * Time.deltaTime);

                        // SCALE
                        if (Input.GetKey(KeyCode.Period))
                            transform.localScale *= 1 + scaleby * Time.deltaTime;
                        if (Input.GetKey(KeyCode.Comma))
                            transform.localScale /= 1 + scaleby * Time.deltaTime;
                    }
                }
                else
                {
                    // SAVE
                    if (Input.GetKeyUp(KeyCode.F5))
                    {
                        Save();
                    }

                    // LOAD
                    else if (Input.GetKeyUp(KeyCode.F9))
                    {
                        Load();
                    }

                    // RESET
                    else if (Input.GetKeyUp(KeyCode.R))
                    {
                        Reset();
                    }
                }
            }

            void Save()
            {
                Directory.CreateDirectory("GameData/Sigma/Replacements/MenuScenes/Debug/");

                ConfigNode SaveData = new ConfigNode("SaveData");

                SaveData.AddValue("enabled", debug);
                SaveData.AddValue("moveby", moveby);
                SaveData.AddValue("rotateby", rotateby);
                SaveData.AddValue("scaleby", scaleby);
                SaveData.AddValue("position", transform.position);
                SaveData.AddValue("rotation", transform.localEulerAngles);
                SaveData.AddValue("scale", transform.localScale);

                if (GetComponent<FlareCamera>() is FlareCamera flare)
                {
                    SaveData.AddValue("brightness", flare.maxBrightness);
                }
                if (GetComponent<Bobber>() is Bobber bobber)
                {
                    float[] values = new float[] { bobber.ofs1, bobber.ofs2, bobber.ofs3, bobber.seed };

                    DestroyImmediate(bobber);

                    bobber = gameObject.AddComponent<Bobber>();
                    bobber.ofs1 = values[0];
                    bobber.ofs2 = values[1];
                    bobber.ofs3 = values[2];
                    bobber.seed = values[3];
                }

                SaveData.Save("GameData/Sigma/Replacements/MenuScenes/Debug/" + name + index + ".txt");
            }

            void Load()
            {
                string path = "GameData/Sigma/Replacements/MenuScenes/Debug/";

                if (Directory.Exists(path))
                {
                    path += name + index + ".txt";

                    if (File.Exists(path))
                    {
                        ConfigNode LoadData = ConfigNode.Load(path);

                        if (bool.TryParse(LoadData.GetValue("enabled"), out debug) && debug)
                        {
                            moveby = float.Parse(LoadData.GetValue("moveby"));
                            rotateby = float.Parse(LoadData.GetValue("rotateby"));
                            scaleby = float.Parse(LoadData.GetValue("scaleby"));
                            transform.position = ConfigNode.ParseVector3(LoadData.GetValue("position"));
                            transform.localEulerAngles = ConfigNode.ParseVector3(LoadData.GetValue("rotation"));
                            transform.localScale = ConfigNode.ParseVector3(LoadData.GetValue("scale"));

                            if (GetComponent<FlareCamera>() is FlareCamera flare)
                            {
                                flare.maxBrightness = float.Parse(LoadData.GetValue("brightness"));
                            }
                            if (GetComponent<Bobber>() is Bobber bobber)
                            {
                                bobber.enabled = false;
                            }
                        }
                        else
                        {
                            if (GetComponent<Bobber>() is Bobber bobber)
                            {
                                float[] values = new float[] { bobber.ofs1, bobber.ofs2, bobber.ofs3, bobber.seed };

                                DestroyImmediate(bobber);

                                bobber = gameObject.AddComponent<Bobber>();
                                bobber.ofs1 = values[0];
                                bobber.ofs2 = values[1];
                                bobber.ofs3 = values[2];
                                bobber.seed = values[3];
                            }
                        }
                    }
                }
            }

            void Reset()
            {
                if (debug)
                {
                    transform.position = originalPosition;
                    transform.localRotation = originalRotation;
                    transform.localScale = originalScale;

                    Bobber bobber = transform.GetComponent<Bobber>();
                    if (bobber != null)
                    {
                        bobber.enabled = false;
                    }
                }
            }
        }
    }
}
