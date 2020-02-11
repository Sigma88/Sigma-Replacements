using System.IO;
using UnityEngine;
using TMPro;


namespace SigmaReplacements
{
    namespace MenuButtons
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
                Directory.CreateDirectory("GameData/Sigma/Replacements/MenuButtons/Debug/");

                ConfigNode SaveData = new ConfigNode("LiveDebug");

                SaveData.AddValue("enabled", debug);
                SaveData.AddValue("position", transform.position);
                SaveData.AddValue("rotation", transform.localEulerAngles);
                SaveData.AddValue("scale", transform.localScale);
                SaveData.AddValue("moveby", moveby);
                SaveData.AddValue("rotateby", rotateby);
                SaveData.AddValue("scaleby", scaleby);

                TextMeshPro mesh = GetComponent<TextMeshPro>();

                if (mesh != null)
                {
                    SaveData.AddValue("text", mesh.text);
                    SaveData.AddValue("font", mesh.font.name);
                    SaveData.AddValue("fontSize", mesh.fontSize);
                    SaveData.AddValue("borderSize", mesh.outlineWidth);
                    SaveData.AddValue("borderColor", (Color)mesh.outlineColor);
                    SaveData.AddValue("normalColor", mesh.color);
                }

                TextProButton3D button = GetComponent<TextProButton3D>();

                if (button != null)
                {
                    SaveData.RemoveValue("normalColor");
                    SaveData.AddValue("normalColor", button.normalColor);
                    SaveData.AddValue("hoverColor", button.hoverColor);
                    SaveData.AddValue("downColor", button.downColor);
                    SaveData.AddValue("disabledColor", button.disabledColor);
                }

                SaveData.Save("GameData/Sigma/Replacements/MenuButtons/Debug/" + name + index + ".txt");
            }

            void Load()
            {
                string path = "GameData/Sigma/Replacements/MenuButtons/Debug/";

                if (Directory.Exists(path))
                {
                    if (File.Exists(path + name + index + ".txt"))
                    {
                        ConfigNode LoadData = ConfigNode.Load(path + name + index + ".txt");

                        if (bool.TryParse(LoadData.GetValue("enabled"), out debug) && debug)
                        {
                            transform.position = ConfigNode.ParseVector3(LoadData.GetValue("position"));
                            transform.localEulerAngles = ConfigNode.ParseVector3(LoadData.GetValue("rotation"));
                            transform.localScale = ConfigNode.ParseVector3(LoadData.GetValue("scale"));
                            moveby = float.Parse(LoadData.GetValue("moveby"));
                            rotateby = float.Parse(LoadData.GetValue("rotateby"));
                            scaleby = float.Parse(LoadData.GetValue("scaleby"));

                            TextMeshPro mesh = GetComponent<TextMeshPro>();

                            if (mesh != null)
                            {
                                mesh.text = LoadData.GetValue("text");
                                mesh.font.name = LoadData.GetValue("font");
                                mesh.fontSize = float.Parse(LoadData.GetValue("fontSize"));
                                mesh.outlineWidth = float.Parse(LoadData.GetValue("borderSize"));
                                mesh.outlineColor = ConfigNode.ParseColor(LoadData.GetValue("borderColor"));
                                mesh.color = ConfigNode.ParseColor(LoadData.GetValue("normalColor"));
                            }

                            TextProButton3D button = GetComponent<TextProButton3D>();

                            if (button != null)
                            {
                                button.normalColor = ConfigNode.ParseColor(LoadData.GetValue("normalColor"));
                                button.hoverColor = ConfigNode.ParseColor(LoadData.GetValue("hoverColor"));
                                button.downColor = ConfigNode.ParseColor(LoadData.GetValue("downColor"));
                                button.disabledColor = ConfigNode.ParseColor(LoadData.GetValue("disabledColor"));
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
                }
            }
        }
    }
}
