using System.IO;
using TMPro;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuButtons
    {
        public class CustomMenuButton : MonoBehaviour
        {
            // Colors
            internal Color? normalColor = null;
            internal Color? hoverColor = null;
            internal Color? downColor = null;

            // Text
            internal string text = null;

            // Vectors
            internal Vector3? position = null;
            internal Quaternion? rotation = null;
            internal Vector3? scale = null;

            void Start()
            {
                Customize();
            }

            void OnEnable()
            {
                Customize();
            }

            void Customize()
            {
                LoadFor();
                ApplyTo();

                if (Debug.debug)
                {
                    Directory.CreateDirectory("GameData/Sigma/Replacements/MenuButtons/Debug/");
                    Save();
                    TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, Load);
                }
            }

            void LoadFor()
            {
                Debug.Log("CustomMenuButton.LoadFor", "object = " + name);

                string collection = "";

                for (int i = 0; i < MenuButtonInfo.DataBase?.Count; i++)
                {
                    MenuButtonInfo info = (MenuButtonInfo)MenuButtonInfo.DataBase[i];

                    if (info != null)
                    {
                        if (string.IsNullOrEmpty(info.name) || info.name == name)
                        {
                            Debug.Log("CustomMenuButton.LoadFor", "Matched name = " + info.name + " to object name = " + name);

                            if (string.IsNullOrEmpty(collection) || collection == info.collection)
                            {
                                Debug.Log("CustomMenuButton.LoadFor", "Matched collection = " + info.collection + " to current collection = " + collection);
                                // Collection
                                collection = info.collection;

                                // Colors
                                normalColor = normalColor ?? info.normalColor;
                                hoverColor = hoverColor ?? info.hoverColor;
                                downColor = downColor ?? info.downColor;

                                // Text
                                if (string.IsNullOrEmpty(text))
                                    text = info.text;

                                // Vectors
                                position = position ?? info.position;
                                rotation = rotation ?? info.rotation;
                                scale = scale ?? info.scale;
                            }
                        }
                    }
                }
            }

            void ApplyTo()
            {
                Debug.Log("CustomMenuButton.ApplyTo", "object = " + name);

                transform.localPosition = position ?? transform.localPosition;
                transform.localRotation = rotation ?? transform.localRotation;
                transform.localScale = scale ?? transform.localScale;

                TextMeshPro mesh = GetComponent<TextMeshPro>();
                Debug.Log("CustomMenuButton.ApplyTo", "mesh = " + mesh);

                if (mesh != null)
                {
                    if (!string.IsNullOrEmpty(text))
                        mesh.text = text;

                    TextProButton3D button = GetComponent<TextProButton3D>();
                    Debug.Log("CustomMenuButton.ApplyTo", "button = " + button);

                    if (button == null)
                    {
                        mesh.color = normalColor ?? mesh.color;
                    }
                    else
                    {
                        button.normalColor = normalColor ?? button.normalColor;
                        button.hoverColor = hoverColor ?? button.hoverColor;
                        button.downColor = downColor ?? button.downColor;

                        button.OnMouseExit();
                    }
                }
            }

            void Save()
            {
                string[] data = new string[20];

                data[0] = "position";
                data[1] = Print(transform.localPosition);
                data[2] = "";
                data[3] = "rotation";
                data[4] = Print(transform.localRotation.eulerAngles);
                data[5] = "";
                data[6] = "scale";
                data[7] = Print(transform.localScale);

                TextMeshPro mesh = GetComponent<TextMeshPro>();

                if (mesh != null)
                {
                    data[8] = "";
                    data[9] = "text";
                    data[10] = mesh.text;
                    data[11] = "";
                    data[12] = "normalColor";
                    data[13] = Print(mesh.color);

                    TextProButton3D button = GetComponent<TextProButton3D>();

                    if (button != null)
                    {
                        data[13] = "";
                        data[13] = Print(button.normalColor);
                        data[14] = "";
                        data[15] = "hoverColor";
                        data[16] = Print(button.hoverColor);
                        data[17] = "";
                        data[18] = "downColor";
                        data[19] = Print(button.downColor);
                    }
                }

                File.WriteAllLines("GameData/Sigma/Replacements/MenuButtons/Debug/" + name + ".txt", data);
            }

            void Load()
            {
                string[] data = File.ReadAllLines("GameData/Sigma/Replacements/MenuButtons/Debug/" + name + ".txt");

                try { transform.localPosition = ConfigNode.ParseVector3(data[1]); } catch { }
                try { transform.localRotation = Quaternion.Euler(ConfigNode.ParseVector3(data[4])); } catch { }
                try { transform.localScale = ConfigNode.ParseVector3(data[7]); } catch { }

                TextMeshPro mesh = GetComponent<TextMeshPro>();

                if (mesh != null)
                {
                    if (!string.IsNullOrEmpty(data[10]))
                        mesh.text = data[10];

                    TextProButton3D button = GetComponent<TextProButton3D>();

                    if (button == null)
                    {
                        try { mesh.color = ConfigNode.ParseColor(data[13]); } catch { }
                    }
                    else
                    {
                        try { button.normalColor = ConfigNode.ParseColor(data[13]); } catch { }
                        try { button.hoverColor = ConfigNode.ParseColor(data[16]); } catch { }
                        try { button.downColor = ConfigNode.ParseColor(data[19]); } catch { }
                    }
                }
            }

            void OnMouseOver()
            {
                // ROTATION
                if (Input.GetKeyDown(KeyCode.W))
                    transform.localRotation = Quaternion.Euler((transform.localEulerAngles.x + 1) % 360, transform.localEulerAngles.y, transform.localEulerAngles.z);
                else if (Input.GetKeyDown(KeyCode.S))
                    transform.localRotation = Quaternion.Euler((transform.localEulerAngles.x + 359) % 360, transform.localEulerAngles.y, transform.localEulerAngles.z);
                else if (Input.GetKeyDown(KeyCode.A))
                    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, (transform.localEulerAngles.y + 1) % 360, transform.localEulerAngles.z);
                else if (Input.GetKeyDown(KeyCode.D))
                    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, (transform.localEulerAngles.y + 359) % 360, transform.localEulerAngles.z);
                else if (Input.GetKeyDown(KeyCode.Q))
                    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, (transform.localEulerAngles.z + 1) % 360);
                else if (Input.GetKeyDown(KeyCode.E))
                    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, (transform.localEulerAngles.z + 359) % 360);

                // POSITION
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    transform.localPosition = new Vector3(transform.localPosition.x - 0.05f, transform.localPosition.y, transform.localPosition.z);
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    transform.localPosition = new Vector3(transform.localPosition.x + 0.05f, transform.localPosition.y, transform.localPosition.z);
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.05f, transform.localPosition.z);
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.05f, transform.localPosition.z);
                else if (Input.GetKeyDown(KeyCode.RightControl))
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.05f);
                else if (Input.GetKeyDown(KeyCode.RightShift))
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 0.05f);

                // SCALE
                else if (Input.GetKeyDown(KeyCode.KeypadMinus))
                    transform.localScale = transform.localScale * 0.95f;
                else if (Input.GetKeyDown(KeyCode.KeypadPlus))
                    transform.localScale = transform.localScale * 1.05f;

                // NOTHING
                else return;

                Save();
            }

            string Print(Color? color)
            {
                if (color != null)
                {
                    return color.Value.r + "," + color.Value.g + "," + color.Value.b + "," + color.Value.a;
                }

                return "";
            }

            string Print(Vector3? vector)
            {
                if (vector != null)
                {
                    return vector.Value.x + "," + vector.Value.y + "," + vector.Value.z;
                }

                return "";
            }
        }
    }
}
