using UnityEngine;
using UnityEngine.UI;


namespace SigmaReplacements
{
    namespace Navigation
    {
        class NyanNavBall
        {
            internal static void ApplyTo(FlightUIModeController controller)
            {
                Debug.Log("NyanNavBall.ApplyTo", "controller = " + controller);

                if (controller == null) return;

                Material newNavball = controller?.gameObject?.GetChild("NavBall")?.GetComponent<Renderer>()?.material;
                if (newNavball != null)
                {
                    newNavball.SetMainTexture(Nyan.nyanBall);
                }


                Image newCursor = controller?.gameObject?.GetChild("NavBallCursor")?.GetComponent<Image>();
                if (newCursor != null)
                {
                    newCursor.SetColor(new Color(1, 0.6f, 1, 1));
                }


                Renderer[] newVectors = controller?.gameObject?.GetChild("NavBallVectorsPivot")?.GetComponentsInChildren<Renderer>(true);

                for (int i = 0; i < newVectors?.Length; i++)
                {
                    string name = newVectors[i]?.name;

                    Material material = newVectors[i]?.material;
                    if (material == null) continue;


                    if (name == "ProgradeVector")
                    {
                        material.SetTintColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "ProgradeWaypoint")
                    {
                        material.SetTintColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "RetrogradeVector")
                    {
                        material.SetTintColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "RetrogradeWaypoint")
                    {
                        material.SetTintColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "RadialInVector")
                    {
                        material.SetTintColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "RadialOutVector")
                    {
                        material.SetTintColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "NormalVector")
                    {
                        material.SetTintColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "AntiNormalVector")
                    {
                        material.SetTintColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "BurnVector")
                    {
                        material.SetTintColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "BurnVectorArrow")
                    {
                        material.SetTintColor(new Color(0, 0, 0, 1));
                    }
                }
            }

            internal static void FixIVA(Renderer[] renderers)
            {
                Debug.Log("NyanNavBall.FixIVA", "renderers count = " + renderers?.Length);

                for (int i = 0; i < renderers?.Length; i++)
                {
                    string name = renderers[i]?.name;

                    Material material = renderers[i]?.material;
                    if (material == null) continue;


                    if (name == "NavSphere")
                    {
                        material.SetTexture(Nyan.nyanBall, true);
                    }

                    else

                    if (name == "IVAprograde")
                    {
                        material.SetColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "progradeVector")
                    {
                        material.SetColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "progradeWaypoint")
                    {
                        material.SetColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "retrogradeVector")
                    {
                        material.SetColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "retrogradeWaypoint" || name == "retrogradeDeltaV")
                    {
                        material.SetColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "RadialInVector")
                    {
                        material.SetColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "RadialOutVector")
                    {
                        material.SetColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "NormalVector")
                    {
                        material.SetColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "antiNormalVector")
                    {
                        material.SetColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "NavWaypointVector")
                    {
                        material.SetColor(new Color(0, 0, 0, 1));
                    }

                    else

                    if (name == "ManeuverArrow")
                    {
                        material.SetColor(new Color(0, 0, 0, 1));
                    }
                }
            }
        }
    }
}
