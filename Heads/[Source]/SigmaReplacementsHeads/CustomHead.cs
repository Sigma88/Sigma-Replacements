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
            Color? tongue = null;
            Color? head = null;
            Color? hair = null;
            Color? arm = null;

            // Textures
            Texture pupilLeftTex = null;
            Texture pupilRightTex = null;
            Texture eyeballLeftTex = null;
            Texture eyeballRightTex = null;
            Texture upTeeth01Tex = null;
            Texture upTeeth02Tex = null;
            Texture tongueTex = null;
            Texture headTex = null;
            Texture hairTex = null;
            Texture armTex = null;

            // Normals
            Texture pupilLeftNrm = null;
            Texture pupilRightNrm = null;
            Texture eyeballLeftNrm = null;
            Texture eyeballRightNrm = null;
            Texture upTeeth01Nrm = null;
            Texture upTeeth02Nrm = null;
            Texture tongueNrm = null;
            Texture headNrm = null;
            Texture hairNrm = null;
            Texture armNrm = null;


            void Start()
            {
                Apply();
            }

            internal void Apply()
            {
                ProtoCrewMember kerbal = GetComponent<KerbalEVA>()?.part?.protoModuleCrew?.FirstOrDefault();
                if (kerbal == null) kerbal = GetComponent<kerbalExpressionSystem>()?.protoCrewMember;
                if (kerbal == null) kerbal = GetComponent<UIKerbalMenu>()?.crewMember;
                if (kerbal == null) kerbal = GetComponent<UIKerbalWerner>()?.crewMember;
                if (kerbal == null) kerbal = GetComponent<UIKerbalGene>()?.crewMember;
                if (kerbal == null) kerbal = GetComponent<UIKerbalStrategy>()?.crewMember;
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
                    string collection = "";

                    if (info != null)
                    {
                        if (string.IsNullOrEmpty(collection) || collection == info.collection)
                        {
                            if (info.useChance != 1)
                                useChance = kerbal.Hash(info.useGameSeed) % 100;

                            if (info.useChance == 1 || useChance < info.useChance * 100)
                            {
                                // Collection
                                collection = info.collection;

                                // Colors
                                pupilLeft = pupilLeft ?? info.pupilLeft.Pick(kerbal, info.useGameSeed);
                                pupilRight = pupilRight ?? info.pupilRight.At(pupilLeft, info.pupilLeft) ?? info.pupilRight.Pick(kerbal, info.useGameSeed);
                                eyeballLeft = eyeballLeft ?? info.eyeballLeft.Pick(kerbal, info.useGameSeed);
                                eyeballRight = eyeballRight ?? info.eyeballRight.At(eyeballLeft, info.eyeballLeft) ?? info.eyeballRight.Pick(kerbal, info.useGameSeed);
                                upTeeth01 = upTeeth01 ?? info.upTeeth01.Pick(kerbal, info.useGameSeed);
                                upTeeth02 = upTeeth02 ?? info.upTeeth02.At(upTeeth01, info.upTeeth01) ?? info.upTeeth02.Pick(kerbal, info.useGameSeed);
                                tongue = tongue ?? info.tongue.Pick(kerbal, info.useGameSeed);
                                head = head ?? info.head.Pick(kerbal, info.useGameSeed);
                                hair = hair ?? info.hair.At(head, info.head) ?? info.hair.Pick(kerbal, info.useGameSeed);

                                // Textures
                                pupilLeftTex = pupilLeftTex ?? info.pupilLeftTex.Pick(kerbal, info.useGameSeed);
                                pupilRightTex = pupilRightTex ?? info.pupilRightTex.At(pupilLeftTex, info.pupilLeftTex) ?? info.pupilRightTex.Pick(kerbal, info.useGameSeed);
                                eyeballLeftTex = eyeballLeftTex ?? info.eyeballLeftTex.Pick(kerbal, info.useGameSeed);
                                eyeballRightTex = eyeballRightTex ?? info.eyeballRightTex.At(eyeballLeftTex, info.eyeballLeftTex) ?? info.eyeballRightTex.Pick(kerbal, info.useGameSeed);
                                upTeeth01Tex = upTeeth01Tex ?? info.upTeeth01Tex.Pick(kerbal, info.useGameSeed);
                                upTeeth02Tex = upTeeth02Tex ?? info.upTeeth02Tex.At(upTeeth01Tex, info.upTeeth01Tex) ?? info.upTeeth02Tex.Pick(kerbal, info.useGameSeed);
                                tongueTex = tongueTex ?? info.tongueTex.Pick(kerbal, info.useGameSeed);
                                headTex = headTex ?? info.headTex.Pick(kerbal, info.useGameSeed);
                                hairTex = hairTex ?? info.hairTex.At(headTex, info.headTex) ?? info.hairTex.Pick(kerbal, info.useGameSeed);

                                // Normals
                                pupilLeftNrm = pupilLeftNrm ?? info.pupilLeftNrm.Pick(kerbal, info.useGameSeed);
                                pupilRightNrm = pupilRightNrm ?? info.pupilRightNrm.At(pupilLeftNrm, info.pupilLeftNrm) ?? info.pupilRightNrm.Pick(kerbal, info.useGameSeed);
                                eyeballLeftNrm = eyeballLeftNrm ?? info.eyeballLeftNrm.Pick(kerbal, info.useGameSeed);
                                eyeballRightNrm = eyeballRightNrm ?? info.eyeballRightNrm.At(eyeballLeftNrm, info.eyeballLeftNrm) ?? info.eyeballRightNrm.Pick(kerbal, info.useGameSeed);
                                upTeeth01Nrm = upTeeth01Nrm ?? info.upTeeth01Nrm.Pick(kerbal, info.useGameSeed);
                                upTeeth02Nrm = upTeeth02Nrm ?? info.upTeeth02Nrm.At(upTeeth01Nrm, info.upTeeth01Nrm) ?? info.upTeeth02Nrm.Pick(kerbal, info.useGameSeed);
                                tongueNrm = tongueNrm ?? info.tongueNrm.Pick(kerbal, info.useGameSeed);
                                headNrm = headNrm ?? info.headNrm.Pick(kerbal, info.useGameSeed);
                                hairNrm = hairNrm ?? info.hairNrm.At(headNrm, info.headNrm) ?? info.hairNrm.Pick(kerbal, info.useGameSeed);
                            }
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

                    if (name == "pupilLeft" || name == "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_pupilLeft")
                    {
                        material.SetColor(pupilLeft);
                        material.SetTexture(pupilLeftTex);
                        material.SetNormal(pupilLeftNrm);
                    }

                    if (name == "pupilRight" || name == "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_pupilRight")
                    {
                        material.SetColor(pupilRight);
                        material.SetTexture(pupilRightTex);
                        material.SetNormal(pupilRightNrm);
                    }

                    if (name == "eyeballLeft" || name == "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_eyeballLeft")
                    {
                        material.SetColor(eyeballLeft);
                        material.SetTexture(eyeballLeftTex);
                        material.SetNormal(eyeballLeftNrm);
                    }

                    if (name == "eyeballRight" || name == "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_eyeballRight")
                    {
                        material.SetColor(eyeballRight);
                        material.SetTexture(eyeballRightTex);
                        material.SetNormal(eyeballRightNrm);
                    }

                    if (name == "upTeeth01" || name == "downTeeth01" || name == "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_downTeeth01")
                    {
                        material.SetColor(upTeeth01);
                        material.SetTexture(upTeeth01Tex);
                        material.SetNormal(upTeeth01Nrm);
                    }

                    if (name == "upTeeth02" || name == "upTeeth01" || name == "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_upTeeth01")
                    {
                        material.SetColor(upTeeth02);
                        material.SetTexture(upTeeth02Tex);
                        material.SetNormal(upTeeth02Nrm);
                    }

                    if (name == "tongue")
                    {
                        material.SetColor(tongue);
                        material.SetTexture(tongueTex);
                        material.SetNormal(tongueNrm);
                    }

                    if (name == "headMesh01" || name == "headMesh" || name == "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_polySurface51")
                    {
                        material.SetColor(head);
                        material.SetTexture(headTex);
                        material.SetNormal(headNrm);
                    }

                    if (name == "ponytail" || name == "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_pCube1")
                    {
                        material.SetColor(hair);
                        material.SetTexture(hairTex);
                        material.SetNormal(hairNrm);
                    }

                    if (name == "arm")
                    {
                        material.SetColor(arm);
                        material.SetTexture(armTex);
                        material.SetNormal(armNrm);
                    }
                }
            }
        }
    }
}
