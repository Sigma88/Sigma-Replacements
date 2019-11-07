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
                        case "helmet":
                        case "mesh_female_kerbalAstronaut01_helmet":
                        case "mesh_backpack":
                        case "mesh_hazm_helmet":
                        case "mesh_helmet_support":
                        case "helmetConstr01":
                        case "neckRing":
                            switch (kerbal?.suit)
                            {
                                default:
                                    material.SetTexture(Nyan.nyanSuit);
                                    continue;
                                case ProtoCrewMember.KerbalSuit.Vintage:
                                    material.SetTexture(Nyan.nyanSuit2);
                                    continue;
                                case ProtoCrewMember.KerbalSuit.Future:
                                    material.SetTexture(Nyan.nyanSuit3);
                                    continue;
                            }

                        case "flare1":
                        case "flare2":
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
                            if (material?.shader?.name == "Legacy Shaders/Particles/Alpha Blended Premultiply")
                                material.shader = Shader.Find("Legacy Shaders/Particles/Alpha Blended");

                            if (material.HasProperty("_TintColor"))
                                material.SetTintColor(new Color(1, 0.2f, 0.6f, 0.5f));
                            else
                                material.SetColor(new Color(1, 0.2f, 0.6f, 0.5f));

                            Light lights = renderers[i]?.transform?.parent?.GetComponentInChildren<Light>();
                            if (lights != null) lights.color = new Color(1, 0.2f, 0.6f, 1);
                            continue;
                    }

                    switch (material?.mainTexture?.name)
                    {
                        case "EVAjetpack":
                        case "EVAjetpackscondary":
                        case "ksp_ig_jetpack_diffuse":
                        case "backpack_Diff":
                        case "canopy_Diff":
                        case "cargoContainerPack_diffuse":
                            material.SetColor(new Color(1, 0.2f, 0.6f, 1));
                            continue;

                        case "fairydust":
                            material.SetTintColor(new Color(1, 0.2f, 0.6f, 0.5f));
                            continue;
                    }

                    kerbal.lightR = 1;
                    kerbal.lightG = 0.2f;
                    kerbal.lightB = 0.6f;
                }
            }
        }
    }
}
