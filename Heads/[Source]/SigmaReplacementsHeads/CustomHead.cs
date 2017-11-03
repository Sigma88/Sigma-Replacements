using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Heads
    {
        internal class CustomHead : MonoBehaviour
        {
            // Colors
            Color? pupilLeft = null;
            Color? pupilRight = null;
            Color? eyeballLeft = null;
            Color? eyeballRight = null;
            Color? upTeeth01 = null;
            Color? upTeeth02 = null;
            Color? head = null;
            Color? hair = null;

            // Textures
            Texture pupilLeftTex = null;
            Texture pupilRightTex = null;
            Texture eyeballLeftTex = null;
            Texture eyeballRightTex = null;
            Texture upTeeth01Tex = null;
            Texture upTeeth02Tex = null;
            Texture headTex = null;
            Texture hairTex = null;

            // Normals
            Texture pupilLeftNrm = null;
            Texture pupilRightNrm = null;
            Texture eyeballLeftNrm = null;
            Texture eyeballRightNrm = null;
            Texture upTeeth01Nrm = null;
            Texture upTeeth02Nrm = null;
            Texture headNrm = null;
            Texture hairNrm = null;

            void Start()
            {
                KerbalEVA kerbalEVA = GetComponent<KerbalEVA>();
                ProtoCrewMember kerbal = kerbalEVA?.part?.protoModuleCrew?.FirstOrDefault();
                if (kerbal == null) kerbal = GetComponent<CrewMember>();
                if (kerbal == null) return;

                LoadFor(kerbal);

                ApplyTo(kerbal);
            }

            void LoadFor(ProtoCrewMember kerbal)
            {
                HeadInfo.hash = "";
                int? useChance = null;

                for (int i = 0; i < HeadInfo.DataBase?.Count; i++)
                {
                    HeadInfo info = HeadInfo.DataBase[i].GetFor(kerbal);

                    if (info != null)
                    {
                        if (info.useChance != 1)
                            useChance = kerbal.Hash(info.useGameSeed) % 100;

                        if (info.useChance == 1 || useChance < info.useChance * 100)
                        {
                            // Colors
                            pupilLeft = pupilLeft ?? info.pupilLeft.Pick(kerbal, info.useGameSeed);
                            pupilRight = pupilRight ?? info.pupilRight.Pick(kerbal, info.useGameSeed);
                            eyeballLeft = eyeballLeft ?? info.eyeballLeft.Pick(kerbal, info.useGameSeed);
                            eyeballRight = eyeballRight ?? info.eyeballRight.Pick(kerbal, info.useGameSeed);
                            upTeeth01 = upTeeth01 ?? info.upTeeth01.Pick(kerbal, info.useGameSeed);
                            upTeeth02 = upTeeth02 ?? info.upTeeth02.Pick(kerbal, info.useGameSeed);
                            head = head ?? info.head.Pick(kerbal, info.useGameSeed);
                            hair = hair ?? info.hair.Pick(kerbal, info.useGameSeed);

                            // Textures
                            pupilLeftTex = pupilLeftTex ?? info.pupilLeftTex.Pick(kerbal, info.useGameSeed);
                            pupilRightTex = pupilRightTex ?? info.pupilRightTex.Pick(kerbal, info.useGameSeed);
                            eyeballLeftTex = eyeballLeftTex ?? info.eyeballLeftTex.Pick(kerbal, info.useGameSeed);
                            eyeballRightTex = eyeballRightTex ?? info.eyeballRightTex.Pick(kerbal, info.useGameSeed);
                            upTeeth01Tex = upTeeth01Tex ?? info.upTeeth01Tex.Pick(kerbal, info.useGameSeed);
                            upTeeth02Tex = upTeeth02Tex ?? info.upTeeth02Tex.Pick(kerbal, info.useGameSeed);
                            headTex = headTex ?? info.headTex.Pick(kerbal, info.useGameSeed);
                            hairTex = hairTex ?? info.hairTex.Pick(kerbal, info.useGameSeed);

                            // Normals
                            pupilLeftNrm = pupilLeftNrm ?? info.pupilLeftNrm.Pick(kerbal, info.useGameSeed);
                            pupilRightNrm = pupilRightNrm ?? info.pupilRightNrm.Pick(kerbal, info.useGameSeed);
                            eyeballLeftNrm = eyeballLeftNrm ?? info.eyeballLeftNrm.Pick(kerbal, info.useGameSeed);
                            eyeballRightNrm = eyeballRightNrm ?? info.eyeballRightNrm.Pick(kerbal, info.useGameSeed);
                            upTeeth01Nrm = upTeeth01Nrm ?? info.upTeeth01Nrm.Pick(kerbal, info.useGameSeed);
                            upTeeth02Nrm = upTeeth02Nrm ?? info.upTeeth02Nrm.Pick(kerbal, info.useGameSeed);
                            headNrm = headNrm ?? info.headNrm.Pick(kerbal, info.useGameSeed);
                            hairNrm = hairNrm ?? info.hairNrm.Pick(kerbal, info.useGameSeed);
                        }
                    }
                }
            }

            void ApplyTo(ProtoCrewMember kerbal)
            {
                Renderer[] renderers = GetComponentsInChildren<Renderer>();

                for (int i = 0; i < renderers?.Length; i++)
                {
                    string name = renderers[i]?.name;
                    Material material = renderers[i]?.material;
                    if (material == null) continue;

                    if (name == "pupilLeft")
                    {
                        if (pupilLeft != null)
                        {
                            material.color = (Color)pupilLeft;
                        }
                        if (pupilLeftTex != null)
                            material.mainTexture = pupilLeftTex;
                        if (pupilLeftNrm != null && material.HasProperty("_BumpMap"))
                            material.SetTexture("_BumpMap", pupilLeftNrm);
                    }

                    if (name == "pupilRight")
                    {
                        if (pupilRight != null)
                            material.color = (Color)pupilRight;
                        if (pupilRightTex != null)
                            material.mainTexture = pupilRightTex;
                        if (pupilRightNrm != null && material.HasProperty("_BumpMap"))
                            material.SetTexture("_BumpMap", pupilRightNrm);
                    }

                    if (name == "eyeballLeft")
                    {
                        if (eyeballLeft != null)
                            material.color = (Color)eyeballLeft;
                        if (eyeballLeftTex != null)
                            material.mainTexture = eyeballLeftTex;
                        if (eyeballLeftNrm != null && material.HasProperty("_BumpMap"))
                            material.SetTexture("_BumpMap", eyeballLeftNrm);
                    }

                    if (name == "eyeballRight")
                    {
                        if (eyeballRight != null)
                            material.color = (Color)eyeballRight;
                        if (eyeballRightTex != null)
                            material.mainTexture = eyeballRightTex;
                        if (eyeballRightNrm != null && material.HasProperty("_BumpMap"))
                            material.SetTexture("_BumpMap", eyeballRightNrm);
                    }

                    if (name == "upTeeth01" || name == "downTeeth01")
                    {
                        if (upTeeth01 != null)
                            material.color = (Color)upTeeth01;
                        if (upTeeth01Tex != null)
                            material.mainTexture = upTeeth01Tex;
                        if (upTeeth01Nrm != null && material.HasProperty("_BumpMap"))
                            material.SetTexture("_BumpMap", upTeeth01Nrm);
                    }

                    if (name == "upTeeth02" || name == "upTeeth01")
                    {
                        if (upTeeth02 != null)
                            material.color = (Color)upTeeth02;
                        if (upTeeth02Tex != null)
                            material.mainTexture = upTeeth02Tex;
                        if (upTeeth02Nrm != null && material.HasProperty("_BumpMap"))
                            material.SetTexture("_BumpMap", upTeeth02Nrm);
                    }

                    if (name == "headMesh01" || name == "headMesh")
                    {
                        if (head != null)
                            material.color = (Color)head;
                        if (headTex != null)
                            material.mainTexture = headTex;
                        if (headNrm != null && material.HasProperty("_BumpMap"))
                            material.SetTexture("_BumpMap", headNrm);
                    }

                    if (name == "ponytail")
                    {
                        if (hair != null)
                            material.color = (Color)hair;
                        if (hairTex != null)
                            material.mainTexture = hairTex;
                        if (hairNrm != null && material.HasProperty("_BumpMap"))
                            material.SetTexture("_BumpMap", hairNrm);
                    }
                }
            }
        }
    }
}
