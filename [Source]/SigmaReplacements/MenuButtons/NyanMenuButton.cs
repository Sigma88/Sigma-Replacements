using System.Linq;
using TMPro;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuButtons
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        internal class NyanMenusButton : MonoBehaviour
        {
            static Color[] rainbow = new[] { new Color(1, 0.5f, 0.5f, 1), new Color(1, 0.8f, 0.5f, 1), new Color(1, 1, 0.5f, 1), new Color(0.6f, 1, 0.5f, 1), new Color(0.5f, 0.8f, 1, 1), new Color(0.7f, 0.6f, 1, 1) };
            TextProButton3D[] ordered = new TextProButton3D[11];
            double time = 0;
            int offset = 0;

            void Start()
            {
                if (Nyan.nyan && !Debug.debug)
                {
                    TextProButton3D[] buttons = Resources.FindObjectsOfTypeAll<TextProButton3D>();

                    for (int i = 0; i < buttons.Length; i++)
                    {
                        if (buttons[i].name == "Start Game")
                            ordered[0] = buttons[i];
                        else if (buttons[i].name == "Settings")
                            ordered[1] = buttons[i];
                        else if (buttons[i].name == "Community")
                            ordered[2] = buttons[i];
                        else if (buttons[i].name == "Addons")
                            ordered[3] = buttons[i];
                        else if (buttons[i].name == "Credits")
                            ordered[4] = buttons[i];
                        else if (buttons[i].name == "Quit")
                            ordered[5] = buttons[i];
                        else if (buttons[i].name == "Continue Game")
                            ordered[6] = buttons[i];
                        else if (buttons[i].name == "New Game")
                            ordered[7] = buttons[i];
                        else if (buttons[i].name == "Training")
                            ordered[8] = buttons[i];
                        else if (buttons[i].name == "Scenarios")
                            ordered[9] = buttons[i];
                        else if (buttons[i].name == "Back")
                            ordered[10] = buttons[i];
                    }

                    for (int i = 0; i < 11; i++)
                    {
                        if (ordered[i] == null)
                            return;
                    }

                    for (int i = 0; i < 11; i++)
                    {
                        ordered[i].normalColor = rainbow[i % 6];
                        ordered[i].hoverColor = Color.white;
                        ordered[i].downColor = rainbow[i % 6];
                        ordered[i].OnMouseExit();
                    }

                    TextMeshPro mesh = Resources.FindObjectsOfTypeAll<TextMeshPro>().FirstOrDefault(m => m.name == "Header");

                    if (mesh != null)
                    {
                        mesh.color = new Color(1, 0.8f, 0.7f);
                    }
                }
            }

            void Update()
            {
                if (Nyan.forever && !Debug.debug)
                {
                    if (time > 0.3)
                    {
                        time = 0;
                        for (int i = 0; i < 11; i++)
                        {
                            ordered[i].normalColor = rainbow[(i + offset) % 6];
                            ordered[i].downColor = rainbow[(i + offset) % 6];
                            ordered[i].OnMouseExit();
                        }
                        offset = (offset + 1) % 6;
                    }
                    else
                    {
                        time += Time.deltaTime;
                    }
                }
            }
        }
    }
}
