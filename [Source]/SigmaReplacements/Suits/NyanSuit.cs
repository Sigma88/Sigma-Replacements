using UnityEngine;


namespace SigmaReplacements
{
    namespace Suits
    {
        class NyanSuit : CustomObject
        {
            internal static void ApplyTo(ProtoCrewMember kerbal, CustomSuit head)
            {
                Debug.Log("NyanHead.ApplyTo", "kerbal = " + kerbal);

                Renderer[] renderers = head.GetComponentsInChildren<Renderer>();

                for (int i = 0; i < renderers?.Length; i++)
                {
                    string name = renderers[i]?.name;
                    Material material = renderers[i]?.material;
                    if (material == null) continue;


                    if (name == "body01" || name == "mesh_female_kerbalAstronaut01_body01")
                    {
                        material.SetTexture(Nyan.nyanSuit);
                    }

                    else

                    if (name == "helmet" || name == "mesh_female_kerbalAstronaut01_helmet")
                    {
                        material.SetTexture(Nyan.nyanHelmet);
                    }

                    else

                    if (name == "visor" || name == "mesh_female_kerbalAstronaut01_visor")
                    {
                        material.SetColor(new Color(0, 0, 0, 0));
                    }

                    else

                    if (name == "flare1" || name == "flare2")
                    {
                        material.shader = Shader.Find("Particles/Alpha Blended");
                        material.SetTintColor(new Color(1, 0.2f, 0.6f, 0.5f));

                        Light lights = renderers[i].GetComponentInParent<Light>();
                        lights.color = new Color(1, 0.2f, 0.6f, 1);
                    }

                    else

                    if (material.mainTexture?.name == "EVAjetpack")
                    {
                        material.SetColor(new Color(1, 0.2f, 0.6f, 1));
                    }

                    else

                    if (material.mainTexture?.name == "fairydust")
                    {
                        material.SetTintColor(new Color(1, 0.2f, 0.6f, 0.5f));
                    }
                }
            }
        }
    }
}
