using UnityEngine;


namespace SigmaReplacements
{
    namespace Heads
    {
        public class CustomHead : CustomObject
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
                ProtoCrewMember kerbal = Apply();

                if (kerbal == null) return;

                LoadFor(kerbal);
                ApplyTo(kerbal);
            }

            internal override void LoadFor(ProtoCrewMember kerbal)
            {
                Debug.Log("CustomHead.LoadFor", "kerbal = " + kerbal);

                if (kerbal == null) return;

                Info.hash = "";
                int? useChance = null;
                string collection = "";

                for (int i = 0; i < HeadInfo.DataBase?.Count; i++)
                {
                    HeadInfo info = (HeadInfo)HeadInfo.DataBase[i].GetFor(kerbal);

                    Debug.Log("CustomHead.LoadFor", "HeadInfo.DataBase[" + i + "] = " + info);
                    if (info != null)
                    {
                        Debug.Log("CustomHead.LoadFor", "Matching head collection = " + info.collection + " to current collection = " + collection);
                        if (string.IsNullOrEmpty(collection) || collection == info.collection)
                        {
                            if (useChance == null && info.useChance != 1)
                                useChance = kerbal.Hash(info.useGameSeed) % 100;

                            Debug.Log("CustomHead.LoadFor", "Matching head useChance = " + info.useChance + " to generated chance = " + useChance + " %");
                            if (info.useChance == 1 || useChance < info.useChance * 100)
                            {
                                Debug.Log("CustomHead.LoadFor", "Loading HeadInfo.DataBase[" + i + "] = " + info);

                                // Collection
                                collection = info.collection;

                                // Colors
                                pupilLeft = pupilLeft ?? info.pupilLeft.Pick(kerbal, info.useGameSeed);
                                pupilRight = pupilRight ?? info.pupilRight.At(pupilLeft, info.pupilLeft, kerbal, info.useGameSeed);
                                eyeballLeft = eyeballLeft ?? info.eyeballLeft.Pick(kerbal, info.useGameSeed);
                                eyeballRight = eyeballRight ?? info.eyeballRight.At(eyeballLeft, info.eyeballLeft, kerbal, info.useGameSeed);
                                upTeeth01 = upTeeth01 ?? info.upTeeth01.Pick(kerbal, info.useGameSeed);
                                upTeeth02 = upTeeth02 ?? info.upTeeth02.At(upTeeth01, info.upTeeth01, kerbal, info.useGameSeed);
                                tongue = tongue ?? info.tongue.Pick(kerbal, info.useGameSeed);
                                head = head ?? info.head.Pick(kerbal, info.useGameSeed);
                                hair = hair ?? info.hair.At(head, info.head, kerbal, info.useGameSeed) ?? head;
                                arm = arm ?? info.arm.At(head, info.head, kerbal, info.useGameSeed) ?? head;

                                // Textures
                                pupilLeftTex = pupilLeftTex ?? info.pupilLeftTex.Pick(kerbal, info.useGameSeed);
                                pupilRightTex = pupilRightTex ?? info.pupilRightTex.At(pupilLeftTex, info.pupilLeftTex, kerbal, info.useGameSeed);
                                eyeballLeftTex = eyeballLeftTex ?? info.eyeballLeftTex.Pick(kerbal, info.useGameSeed);
                                eyeballRightTex = eyeballRightTex ?? info.eyeballRightTex.At(eyeballLeftTex, info.eyeballLeftTex, kerbal, info.useGameSeed);
                                upTeeth01Tex = upTeeth01Tex ?? info.upTeeth01Tex.Pick(kerbal, info.useGameSeed);
                                upTeeth02Tex = upTeeth02Tex ?? info.upTeeth02Tex.At(upTeeth01Tex, info.upTeeth01Tex, kerbal, info.useGameSeed);
                                tongueTex = tongueTex ?? info.tongueTex.Pick(kerbal, info.useGameSeed);
                                headTex = headTex ?? info.headTex.Pick(kerbal, info.useGameSeed);
                                hairTex = hairTex ?? info.hairTex.At(headTex, info.headTex, kerbal, info.useGameSeed);
                                armTex = armTex ?? info.armTex.At(headTex, info.headTex, kerbal, info.useGameSeed);

                                // Normals
                                pupilLeftNrm = pupilLeftNrm ?? info.pupilLeftNrm.At(pupilLeftTex, info.pupilLeftTex, kerbal, info.useGameSeed);
                                pupilRightNrm = pupilRightNrm ?? info.pupilRightNrm.At(pupilRightTex, info.pupilRightTex, kerbal, info.useGameSeed);
                                eyeballLeftNrm = eyeballLeftNrm ?? info.eyeballLeftNrm.At(eyeballLeftTex, info.eyeballLeftTex, kerbal, info.useGameSeed);
                                eyeballRightNrm = eyeballRightNrm ?? info.eyeballRightNrm.At(eyeballRightTex, info.eyeballRightTex, kerbal, info.useGameSeed);
                                upTeeth01Nrm = upTeeth01Nrm ?? info.upTeeth01Nrm.At(upTeeth01Tex, info.upTeeth01Tex, kerbal, info.useGameSeed);
                                upTeeth02Nrm = upTeeth02Nrm ?? info.upTeeth02Nrm.At(upTeeth02Tex, info.upTeeth02Tex, kerbal, info.useGameSeed);
                                tongueNrm = tongueNrm ?? info.tongueNrm.At(tongueTex, info.tongueTex, kerbal, info.useGameSeed);
                                headNrm = headNrm ?? info.headNrm.At(headTex, info.headTex, kerbal, info.useGameSeed);
                                hairNrm = hairNrm ?? info.hairNrm.At(hairTex, info.hairTex, kerbal, info.useGameSeed);
                                armNrm = armNrm ?? info.armNrm.At(armTex, info.armTex, kerbal, info.useGameSeed);

                                continue;
                            }
                        }
                    }

                    Debug.Log("CustomHead.LoadFor", "Ignoring HeadInfo.DataBase[" + i + "] = " + info);
                }
            }

            internal override void ApplyTo(ProtoCrewMember kerbal)
            {
                Debug.Log("CustomHead.ApplyTo", "kerbal = " + kerbal);

                if (kerbal == null) return;

                if (Nyan.nyan)
                {
                    if (HighLogic.LoadedScene == GameScenes.MAINMENU || Nyan.forever)
                    {
                        NyanHead.ApplyTo(kerbal, this);
                        return;
                    }
                }

                Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
                Debug.Log("CustomHead.ApplyTo", "renderers.Length = " + renderers?.Length);

                for (int i = 0; i < renderers?.Length; i++)
                {
                    string name = renderers[i]?.name;
                    Debug.Log("CustomHead.ApplyTo", "renderers[" + i + "].name = " + name);

                    if (name == "backdrop") continue;

                    Material material = renderers[i]?.material;
                    if (material == null) continue;

                    switch (name)
                    {
                        case "pupilLeft":
                        case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_pupilLeft":
                            material.SetColor(pupilLeft);
                            material.SetTexture(pupilLeftTex);
                            material.SetNormal(pupilLeftNrm);
                            continue;

                        case "pupilRight":
                        case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_pupilRight":
                            material.SetColor(pupilRight);
                            material.SetTexture(pupilRightTex);
                            material.SetNormal(pupilRightNrm);
                            continue;

                        case "eyeballLeft":
                        case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_eyeballLeft":
                            material.SetColor(eyeballLeft);
                            material.SetTexture(eyeballLeftTex);
                            material.SetNormal(eyeballLeftNrm);
                            continue;

                        case "eyeballRight":
                        case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_eyeballRight":
                            material.SetColor(eyeballRight);
                            material.SetTexture(eyeballRightTex);
                            material.SetNormal(eyeballRightNrm);
                            continue;

                        case "upTeeth01":
                        case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_upTeeth01":
                            material.SetColor(upTeeth01);
                            material.SetTexture(upTeeth01Tex);
                            material.SetNormal(upTeeth01Nrm);
                            continue;

                        case "upTeeth02":
                        case "downTeeth01":
                        case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_downTeeth01":
                            material.SetColor(upTeeth02);
                            material.SetTexture(upTeeth02Tex);
                            material.SetNormal(upTeeth02Nrm);
                            continue;

                        case "tongue":
                            material.SetColor(tongue);
                            material.SetTexture(tongueTex);
                            material.SetNormal(tongueNrm);
                            continue;

                        case "headMesh":
                        case "headMesh01":
                        case "headMesh02":
                        case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_polySurface51":
                            material.SetColor(head);
                            material.SetTexture(headTex);
                            material.SetNormal(headNrm);
                            continue;

                        case "ponytail":
                        case "mesh_female_kerbalAstronaut01_kerbalGirl_mesh_pCube1":
                            material.SetColor(hair);
                            material.SetTexture(hairTex);
                            material.SetNormal(hairNrm);
                            continue;

                        case "hand_left01":
                        case "hand_right01":
                            material.SetColor(arm);
                            material.SetTexture(armTex);
                            material.SetNormal(armNrm);
                            continue;
                    }
                }
            }
        }
    }
}
