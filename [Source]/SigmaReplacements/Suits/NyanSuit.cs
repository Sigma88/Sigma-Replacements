using UnityEngine;


namespace SigmaReplacements
{
    namespace Suits
    {
        internal class NyanSuit
        {
            internal static void ApplyTo(ProtoCrewMember kerbal, CustomSuit suit)
            {
                Debug.Log("NyanHead.ApplyTo", "kerbal = " + kerbal);

                Renderer[] renderers = suit.GetComponentsInChildren<Renderer>();

                for (int i = 0; i < renderers?.Length; i++)
                {
                    string name = renderers[i]?.name;
                    Material material = renderers[i]?.material;
                    if (material == null) continue;

                    switch (name)
                    {
                        case "body01":
                        case "mesh_female_kerbalAstronaut01_body01":
                        case "coat01":
                        case "pants01":
                        case "mesh_bowTie01":
                            if (kerbal?.suit == ProtoCrewMember.KerbalSuit.Vintage)
                                material.SetTexture(Nyan.nyanSuit2);
                            else
                                material.SetTexture(Nyan.nyanSuit);
                            continue;

                        case "helmet":
                        case "mesh_female_kerbalAstronaut01_helmet":
                        case "mesh_backpack":
                        case "mesh_hazm_helmet":
                        case "mesh_helmet_support":
                        case "helmetConstr01":
                            if (kerbal?.suit != ProtoCrewMember.KerbalSuit.Vintage)
                                material.SetTexture(Nyan.nyanSuit);
                            continue;

                        case "visor":
                        case "mesh_female_kerbalAstronaut01_visor":
                        case "mesh_hazm_visor":
                            material.SetColor(new Color(0, 0, 0, 0));
                            continue;

                        case "flareL1":
                        case "flareR1":
                        case "flareL2":
                        case "flareR2":
                        case "flare1L":
                        case "flare1R":
                        case "flare2L":
                        case "flare2R":
                        case "EVALight":
                        case "lightPlane":
                            material.shader = Shader.Find("Particles/Alpha Blended");
                            material.SetTintColor(new Color(1, 0.2f, 0.6f, 0.5f));
                            Light lights = renderers[i]?.transform?.parent?.GetComponentInChildren<Light>();
                            if (lights != null) lights.color = new Color(1, 0.2f, 0.6f, 1);
                            continue;
                    }

                    switch (material?.mainTexture?.name)
                    {
                        case "EVAjetpack":
                        case "EVAjetpackscondary":
                            material.SetColor(new Color(1, 0.2f, 0.6f, 1));
                            continue;

                        case "fairydust":
                            material.SetTintColor(new Color(1, 0.2f, 0.6f, 0.5f));
                            continue;
                    }
                }
            }
        }
    }
}
