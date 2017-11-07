using UnityEngine;


namespace SigmaReplacements
{
    namespace Suits
    {
        public class CustomSuit : CustomObject
        {
            // Colors
            Color? body = null;
            Color? helmet = null;
            Color? visor = null;
            Color? flares = null;
            Color? jetpack = null;
            Color? headset = null;
            Color? mug = null;
            Color? glasses = null;
            Color? backdrop = null;

            // Textures
            Texture bodyTex = null;
            Texture helmetTex = null;
            Texture visorTex = null;
            Texture flaresTex = null;
            Texture jetpackTex = null;
            Texture gasjetsTex = null;
            Texture headsetTex = null;
            Texture mugTex = null;
            Texture glassesTex = null;
            Texture backdropTex = null;

            // Normals
            Texture bodyNrm = null;
            Texture helmetNrm = null;
            Texture visorNrm = null;
            Texture jetpackNrm = null;
            Texture headsetNrm = null;
            Texture mugNrm = null;
            Texture glassesNrm = null;
            Texture backdropNrm = null;


            void Start()
            {
                ProtoCrewMember kerbal = Apply();
                LoadFor(kerbal);
                ApplyTo(kerbal);
            }

            void LoadFor(ProtoCrewMember kerbal)
            {
                Debug.Log("CustomSuit.LoadFor", "kerbal = " + kerbal);

                Info.hash = "";
                int? useChance = null;

                for (int i = 0; i < SuitInfo.DataBase?.Count; i++)
                {
                    SuitInfo info = (SuitInfo)SuitInfo.DataBase[i].GetFor(kerbal);
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
                                body = body ?? info.body.Pick(kerbal, info.useGameSeed);
                                helmet = helmet ?? info.helmet.Pick(kerbal, info.useGameSeed);
                                visor = visor ?? info.visor.Pick(kerbal, info.useGameSeed);
                                flares = flares ?? info.flares.Pick(kerbal, info.useGameSeed);
                                jetpack = jetpack ?? info.jetpack.Pick(kerbal, info.useGameSeed);
                                headset = headset ?? info.headset.Pick(kerbal, info.useGameSeed);
                                mug = mug ?? info.mug.Pick(kerbal, info.useGameSeed);
                                glasses = glasses ?? info.glasses.Pick(kerbal, info.useGameSeed);
                                backdrop = backdrop ?? info.backdrop.Pick(kerbal, info.useGameSeed);

                                // Textures
                                bodyTex = bodyTex ?? info.bodyTex.Pick(kerbal, info.useGameSeed);
                                helmetTex = helmetTex ?? info.helmetTex.Pick(kerbal, info.useGameSeed);
                                visorTex = visorTex ?? info.visorTex.Pick(kerbal, info.useGameSeed);
                                flaresTex = flaresTex ?? info.flaresTex.Pick(kerbal, info.useGameSeed);
                                jetpackTex = jetpackTex ?? info.jetpackTex.Pick(kerbal, info.useGameSeed);
                                gasjetsTex = gasjetsTex ?? info.gasjetsTex.Pick(kerbal, info.useGameSeed);
                                headsetTex = headsetTex ?? info.headsetTex.Pick(kerbal, info.useGameSeed);
                                mugTex = mugTex ?? info.mugTex.Pick(kerbal, info.useGameSeed);
                                glassesTex = glassesTex ?? info.glassesTex.Pick(kerbal, info.useGameSeed);
                                backdropTex = backdropTex ?? info.backdropTex.Pick(kerbal, info.useGameSeed);

                                // Normals
                                bodyNrm = bodyNrm ?? info.bodyNrm.Pick(kerbal, info.useGameSeed);
                                helmetNrm = helmetNrm ?? info.helmetNrm.Pick(kerbal, info.useGameSeed);
                                visorNrm = visorNrm ?? info.visorNrm.Pick(kerbal, info.useGameSeed);
                                jetpackNrm = jetpackNrm ?? info.jetpackNrm.Pick(kerbal, info.useGameSeed);
                                headsetNrm = headsetNrm ?? info.headsetNrm.Pick(kerbal, info.useGameSeed);
                                mugNrm = mugNrm ?? info.mugNrm.Pick(kerbal, info.useGameSeed);
                                glassesNrm = glassesNrm ?? info.glassesNrm.Pick(kerbal, info.useGameSeed);
                                backdropNrm = backdropNrm ?? info.backdropNrm.Pick(kerbal, info.useGameSeed);
                            }
                        }
                    }
                }
            }

            void ApplyTo(ProtoCrewMember kerbal)
            {
                Debug.Log("CustomSuit.ApplyTo", "kerbal = " + kerbal);

                Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

                for (int i = 0; i < renderers?.Length; i++)
                {
                    string name = renderers[i]?.name;
                    Material material = renderers[i]?.material;
                    if (material == null) continue;

                    if (name == "body01" || name == "mesh_female_kerbalAstronaut01_body01" || name == "coat01" || name == "pants01" || name == "mesh_bowTie01")
                    {
                        material.SetColor(body);
                        material.SetTexture(bodyTex);
                        material.SetNormal(bodyNrm);
                    }

                    else

                    if (name == "helmet" || name == "mesh_female_kerbalAstronaut01_helmet" || name == "mesh_backpack" || name == "mesh_hazm_helmet" || name == "mesh_helmet_support" || name == "helmetConstr01")
                    {
                        material.SetColor(helmet);
                        material.SetTexture(helmetTex);
                        material.SetNormal(helmetNrm);

                        if (HighLogic.LoadedScene == GameScenes.FLIGHT && FlightGlobals.getStaticPressure() > 50)
                            renderers[i].enabled = false;
                    }

                    else

                    if (name == "visor" || name == "mesh_female_kerbalAstronaut01_visor" || name == "mesh_hazm_visor")
                    {
                        material.SetColor(visor);
                        material.SetTexture(visorTex);
                        material.SetNormal(visorNrm);

                        if (HighLogic.LoadedScene == GameScenes.FLIGHT && FlightGlobals.getStaticPressure() > 50)
                            renderers[i].enabled = false;
                    }

                    else

                    if (name == "flare1" || name == "flare2")
                    {
                        material.SetTexture(flaresTex);

                        if (flares != null)
                        {
                            Light lights = renderers[i].GetComponentInParent<Light>();
                            lights.color = (Color)flares;
                        }

                        if (HighLogic.LoadedScene == GameScenes.FLIGHT && FlightGlobals.getStaticPressure() > 50)
                            renderers[i].enabled = false;
                    }

                    else

                    if (material.mainTexture?.name == "EVAjetpack")
                    {
                        material.SetColor(jetpack);
                        material.SetTexture(jetpackTex);
                        material.SetNormal(jetpackNrm);
                    }

                    else

                    if (material.mainTexture?.name == "fairydust")
                    {
                        material.SetTexture(gasjetsTex);
                    }

                    else

                    if (material.mainTexture?.name == "kbGeneKerman_headset")
                    {
                        material.SetColor(headset);
                        material.SetTexture(headsetTex);
                        material.SetNormal(headsetNrm);
                    }

                    else

                    if (name == "gene_mug_base" || name == "gene_mug_handle")
                    {
                        material.SetColor(mug);
                        material.SetTexture(mugTex);
                        material.SetNormal(mugNrm);
                    }

                    else

                    if (material.mainTexture?.name == "wernerVonKerman_glasses")
                    {
                        material.SetColor(glasses);
                        material.SetTexture(glassesTex);
                        material.SetNormal(glassesNrm);
                    }

                    else

                    if (name == "backdrop")
                    {
                        material.SetColor(backdrop);
                        material.SetTexture(backdropTex);
                        material.SetNormal(backdropNrm);
                    }
                }
            }
        }
    }
}
