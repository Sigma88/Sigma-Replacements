using UnityEngine;


namespace SigmaReplacements
{
    namespace Suits
    {
        public class CustomSuit : CustomObject
        {
            // Suit Specific Restrictions
            float? helmetLowPressure = null;
            float? helmetHighPressure = null;
            float? jetpackMaxGravity = null;
            bool jetpackDeployed = true;
            bool helmetHidden = false;
            KerbalEVA eva = null;

            // Colors
            Color? body = null;
            Color? helmet = null;
            Color? visor = null;
            Color? flares = null;
            Color? jetpack = null;
            Color? flag = null;
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
            Texture flagTex = null;
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
            Texture flagNrm = null;
            Texture headsetNrm = null;
            Texture mugNrm = null;
            Texture glassesNrm = null;
            Texture backdropNrm = null;


            void Start()
            {
                ProtoCrewMember kerbal = Apply();
                Debug.Log("CustomSuit.Start", "kerbal = " + kerbal + " (" + kerbal.GetType() + ")");
                if (kerbal == null) return;

                eva = GetComponent<KerbalEVA>();

                LoadFor(kerbal);
                ApplyTo(kerbal);

                if (HighLogic.LoadedScene == GameScenes.FLIGHT && eva != null)
                {
                    if (jetpackMaxGravity != null)
                        TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, JetPack);
                    if (helmetLowPressure != null || helmetHighPressure != null)
                        TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, Helmet);
                }
            }

            void JetPack()
            {
                if (eva.JetpackDeployed != jetpackDeployed && FlightGlobals.ship_geeForce > jetpackMaxGravity)
                {
                    jetpackDeployed = eva.JetpackDeployed;

                    Renderer[] renderers = eva.gameObject.GetChild("jetpack01").GetComponentsInChildren<Renderer>(true);

                    for (int i = 0; i < renderers.Length; i++)
                    {
                        if (renderers[i]?.name?.StartsWith("fx_gasJet") == false)
                            renderers[i].enabled = jetpackDeployed;
                    }

                    eva.gameObject.GetChild("kbEVA_flagDecals").GetComponent<Renderer>().enabled = jetpackDeployed;
                }
            }

            void Helmet()
            {
                if (FlightGlobals.currentMainBody?.atmosphereContainsOxygen != true) return;

                double pressure = FlightGlobals.getStaticPressure();

                if (helmetHidden != !(pressure < helmetLowPressure || pressure > helmetHighPressure || helmetLowPressure == helmetHighPressure))
                {
                    helmetHidden = !helmetHidden;

                    Renderer[] renderers = eva.gameObject.GetChild("helmet01").GetComponentsInChildren<Renderer>(true);

                    for (int i = 0; i < renderers.Length; i++)
                    {
                        if (renderers[i]?.name == "helmet" || renderers[i]?.name == "visor" || renderers[i]?.name == "flare1" || renderers[i]?.name == "flare2")
                            renderers[i].enabled = !helmetHidden;
                    }
                }
            }

            void OnDestroy()
            {
                if (jetpackMaxGravity != null)
                    TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, JetPack);
                if (helmetLowPressure != null || helmetHighPressure != null)
                    TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, Helmet);
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
                        Type type = Type.IVA;
                        if (eva != null) type = Type.EVA;
                        else if (kerbal.GetType() == typeof(CrewMember) && ((CrewMember)kerbal).activity == 0) type = Type.EVA;

                        if (info.type != null && info.type != type) continue;
                        Debug.Log("CustomSuit.LoadFor", "Matched suit type = " + info.type + " to current activity = " + type);

                        if (string.IsNullOrEmpty(collection) || collection == info.collection)
                        {
                            if (info.useChance != 1)
                                useChance = kerbal.Hash(info.useGameSeed) % 100;

                            if (info.useChance == 1 || useChance < info.useChance * 100)
                            {
                                // Collection
                                collection = info.collection;

                                // Suit Specific Requirements
                                helmetLowPressure = helmetLowPressure ?? info.helmetLowPressure;
                                helmetHighPressure = helmetHighPressure ?? info.helmetHighPressure;
                                jetpackMaxGravity = jetpackMaxGravity ?? info.jetpackMaxGravity;

                                // Colors
                                body = body ?? info.body.Pick(kerbal, info.useGameSeed);
                                helmet = helmet ?? info.helmet.Pick(kerbal, info.useGameSeed);
                                visor = visor ?? info.visor.Pick(kerbal, info.useGameSeed);
                                flares = flares ?? info.flares.Pick(kerbal, info.useGameSeed);
                                jetpack = jetpack ?? info.jetpack.Pick(kerbal, info.useGameSeed);
                                flag = flag ?? info.flag.Pick(kerbal, info.useGameSeed);
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
                                flagTex = flagTex ?? info.flagTex.Pick(kerbal, info.useGameSeed);
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
                                flagNrm = flagNrm ?? info.flagNrm.Pick(kerbal, info.useGameSeed);
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
                    }

                    else

                    if (name == "visor" || name == "mesh_female_kerbalAstronaut01_visor" || name == "mesh_hazm_visor")
                    {
                        material.SetColor(visor);
                        material.SetTexture(visorTex);
                        material.SetNormal(visorNrm);
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
                    }

                    else

                    if (material.mainTexture?.name == "EVAjetpack")
                    {
                        material.SetColor(jetpack);
                        material.SetTexture(jetpackTex);
                        material.SetNormal(jetpackNrm);
                    }

                    else

                    if (name == "kbEVA_flagDecals")
                    {
                        material.SetColor(flag);
                        material.SetTexture(flagTex);
                        material.SetNormal(flagNrm);
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

        internal enum Type
        {
            EVA,
            IVA
        }
    }
}
