using UnityEngine;


namespace SigmaReplacements
{
    namespace Heads
    {
        internal class NyanHead
        {
            internal static void ApplyTo(ProtoCrewMember kerbal, CustomHead head)
            {
                Debug.Log("NyanHead.ApplyTo", "kerbal = " + kerbal);

                Renderer[] renderers = head.GetComponentsInChildren<Renderer>();

                for (int i = 0; i < renderers?.Length; i++)
                {
                    string name = renderers[i]?.name;
                    Material material = renderers[i]?.material;
                    if (material == null) continue;


                    switch (name)
                    {
                        case "headMesh":
                        case "headMesh01":
                        case "headMesh02":
                        case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_polySurface51":
                            material.SetTexture(Nyan.nyanHead);
                            if (kerbal.gender == ProtoCrewMember.Gender.Female)
                            {
                                material.SetTextureOffset("_MainTex", new Vector2(-0.225f, -0.5f));
                                material.SetTextureScale("_MainTex", new Vector2(1.5f, 1.75f));
                            }
                            continue;
                    }
                }
            }
        }
    }
}
