using System.Linq;
using TMPro;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuButtons
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        internal class NyanMenusButtons : MonoBehaviour
        {
            internal static Color defaultColor = new Color(1, 0.8f, 0.7f);
            internal static Color[] rainbow = new[] { new Color(1, 0.5f, 0.5f, 1), new Color(1, 0.8f, 0.5f, 1), new Color(1, 1, 0.5f, 1), new Color(0.6f, 1, 0.5f, 1), new Color(0.5f, 0.8f, 1, 1), new Color(0.7f, 0.6f, 1, 1) };
            double time = 0;

            void Start()
            {
                if (Nyan.nyan)
                {
                    TextProButton3D[] buttons = Resources.FindObjectsOfTypeAll<TextProButton3D>();

                    for (int i = 0; i < buttons.Length; i++)
                    {
                        switch (buttons[i]?.name)
                        {
                            case "Start Game":
                                buttons[i].gameObject.AddOrGetComponent<NyanMenuButton>().i = 0;
                                continue;
                            case "Settings":
                                buttons[i].gameObject.AddOrGetComponent<NyanMenuButton>().i = 1;
                                continue;
                            case "Community":
                                buttons[i].gameObject.AddOrGetComponent<NyanMenuButton>().i = 2;
                                continue;
                            case "Addons":
                                buttons[i].gameObject.AddOrGetComponent<NyanMenuButton>().i = 3;
                                continue;
                            case "Credits":
                                buttons[i].gameObject.AddOrGetComponent<NyanMenuButton>().i = 4;
                                continue;
                            case "Quit":
                                buttons[i].gameObject.AddOrGetComponent<NyanMenuButton>().i = 5;
                                continue;
                            case "Continue Game":
                                buttons[i].gameObject.AddOrGetComponent<NyanMenuButton>().i = 6;
                                continue;
                            case "New Game":
                                buttons[i].gameObject.AddOrGetComponent<NyanMenuButton>().i = 7;
                                continue;
                            case "Training":
                                buttons[i].gameObject.AddOrGetComponent<NyanMenuButton>().i = 8;
                                continue;
                            case "Scenarios":
                                buttons[i].gameObject.AddOrGetComponent<NyanMenuButton>().i = 9;
                                continue;
                            case "Back":
                                buttons[i].gameObject.AddOrGetComponent<NyanMenuButton>().i = 10;
                                continue;
                            case "BuyMakingHistory":
                            case "MissionBuilder":
                            case "PlayMissions":
                                buttons[i].normalColor = defaultColor;
                                buttons[i].hoverColor = Color.white;
                                buttons[i].downColor = defaultColor;
                                buttons[i].disabledColor = defaultColor * 0.5f;
                                buttons[i].OnMouseExit();
                                continue;
                        }
                    }

                    TextMeshPro mesh = Resources.FindObjectsOfTypeAll<TextMeshPro>().FirstOrDefault(m => m.name == "Header");

                    if (mesh != null)
                    {
                        mesh.color = defaultColor;
                    }
                }
            }

            void Update()
            {
                if (Nyan.forever)
                {
                    if (time > 0.3)
                    {
                        time = 0;
                        NyanMenuButton.offset = (NyanMenuButton.offset + 5) % 6;
                    }
                    else
                    {
                        time += Time.deltaTime;
                    }
                }
            }
        }

        internal class NyanMenuButton : MonoBehaviour
        {
            internal static int offset = 0;

            internal int i;
            TextProButton3D button;
            bool update = true;

            void Awake()
            {
                button = GetComponent<TextProButton3D>();
            }

            void Update()
            {
                if (button != null)
                {
                    button.normalColor = NyanMenusButtons.rainbow[(i + offset) % 6];
                    button.hoverColor = Color.white;
                    button.downColor = button.normalColor;
                    button.disabledColor = button.normalColor * 0.5f;

                    if (update)
                    {
                        button.OnMouseExit();
                    }

                    if (!Nyan.forever)
                    {
                        DestroyImmediate(this);
                    }
                }
            }

            void OnMouseEnter()
            {
                update = false;
            }

            void OnMouseExit()
            {
                update = true;
            }
        }
    }
}
