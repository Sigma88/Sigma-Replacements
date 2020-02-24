using UnityEngine;
using TMPro;


namespace SigmaReplacements
{
    namespace MenuButtons
    {
        internal class CustomMenuButton : MonoBehaviour
        {
            // Colors
            internal Color? normalColor = null;
            internal Color? hoverColor = null;
            internal Color? downColor = null;
            internal Color? disabledColor = null;

            // Border
            internal float? borderSize = null;
            internal Color? borderColor = null;

            // Text
            internal string text = null;
            internal string font = null;
            internal float? fontSize = null;

            // Vectors
            internal Vector3? position = null;
            internal Quaternion? rotation = null;
            internal Vector3? scale = null;

            void Start()
            {
                Customize();

                if (Debug.debug)
                {
                    gameObject.AddOrGetComponent<LiveDebug>();
                }
            }

            void OnEnable()
            {
                Customize();
            }

            void Customize()
            {
                LoadFor();
                ApplyTo();
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
                                disabledColor = disabledColor ?? info.disabledColor;

                                // Border
                                borderSize = borderSize ?? info.borderSize;
                                borderColor = borderColor ?? info.borderColor;

                                // Text
                                if (string.IsNullOrEmpty(text))
                                    text = info.text;
                                if (string.IsNullOrEmpty(font))
                                    font = info.font;
                                fontSize = fontSize ?? info.fontSize;

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

                transform.position = position ?? transform.position;
                transform.localRotation = rotation ?? transform.localRotation;
                transform.localScale = scale ?? transform.localScale;

                TextMeshPro mesh = GetComponent<TextMeshPro>();
                Debug.Log("CustomMenuButton.ApplyTo", "mesh = " + mesh);

                if (mesh != null)
                {
                    if (!string.IsNullOrEmpty(text))
                        mesh.text = text;

                    if (!string.IsNullOrEmpty(font))
                        mesh.ChangeFont(font);

                    mesh.fontSize = fontSize ?? mesh.fontSize;
                    mesh.outlineWidth = borderSize ?? mesh.outlineWidth;
                    mesh.outlineColor = borderColor ?? mesh.outlineColor;

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
                        button.disabledColor = disabledColor ?? (normalColor.HasValue ? normalColor.Value * 0.5f : button.disabledColor);

                        button.OnMouseExit();
                    }
                }
            }
        }
    }
}
