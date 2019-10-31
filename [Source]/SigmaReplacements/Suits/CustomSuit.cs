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
            float? helmetDelay = null;
            double helmetTime = 1;
            float? jetpackMaxGravity = null;
            bool hideJetPack = false;
            bool jetpackVisible = true;
            bool helmetHidden = false;
            KerbalEVA eva = null;

            // Colors
            Color? body = null;
            Color? neckRing = null;
            Color? helmet = null;
            Color? visor = null;
            Color? flares = null;
            Color? light = null;
            Color? jetpack = null;
            Color? parachute = null;
            Color? canopy = null;
            Color? backpack = null;
            Color? flag = null;
            Color? gasjets = null;
            Color? headset = null;
            Color? mug = null;
            Color? glasses = null;
            Color? backdrop = null;

            // Textures
            Texture bodyTex = null;
            Texture neckRingTex = null;
            Texture helmetTex = null;
            Texture visorTex = null;
            Texture flaresTex = null;
            Texture jetpackTex = null;
            Texture parachuteTex = null;
            Texture canopyTex = null;
            Texture backpackTex = null;
            Texture flagTex = null;
            Texture gasjetsTex = null;
            Texture headsetTex = null;
            Texture mugTex = null;
            Texture glassesTex = null;
            Texture backdropTex = null;

            // Normals
            Texture bodyNrm = null;
            Texture neckRingNrm = null;
            Texture helmetNrm = null;
            Texture visorNrm = null;
            Texture jetpackNrm = null;
            Texture parachuteNrm = null;
            Texture canopyNrm = null;
            Texture backpackNrm = null;
            Texture flagNrm = null;
            Texture headsetNrm = null;
            Texture mugNrm = null;
            Texture glassesNrm = null;
            Texture backdropNrm = null;


            void Start()
            {
                ProtoCrewMember kerbal = Apply();
                Debug.Log("CustomSuit.Start", "kerbal = " + kerbal + " (" + kerbal?.GetType() + ")");
                if (kerbal == null) return;

                eva = GetComponent<KerbalEVA>();

                LoadFor(kerbal);
                ApplyTo(kerbal);

                if (HighLogic.LoadedSceneIsFlight && eva != null)
                {
                    if (Nyan.forever)
                    {
                        TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, RainbowJets);
                        return;
                    }

                    TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, CheckGee);

                    if (helmetLowPressure != null || helmetHighPressure != null)
                    {
                        Helmet();
                        TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, Helmet);
                    }
                }
            }

            void CheckGee()
            {
                if (eva.vessel.geeForce > 0)
                {
                    TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, CheckGee);

                    hideJetPack = (eva.vessel.acceleration_immediate - eva.vessel.graviticAcceleration).magnitude / 9.80665 > jetpackMaxGravity;

                    if (hideJetPack)
                    {
                        TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, JetPack);
                    }
                }
            }

            void JetPack()
            {
                if (jetpackVisible != (eva.JetpackDeployed || eva.IsChuteState))
                {
                    jetpackVisible = !jetpackVisible;

                    Renderer[] jetpackRenderers = eva.gameObject.GetChild("jetpack01").GetComponentsInChildren<Renderer>(true);
                    Renderer[] chuteRenderers = eva.gameObject.GetChild("model").GetComponentsInChildren<Renderer>(true);

                    for (int i = 0; i < jetpackRenderers.Length; i++)
                    {
                        if (jetpackRenderers[i]?.name?.StartsWith("fx_gasJet") == false)
                        {
                            jetpackRenderers[i].enabled = jetpackVisible;
                        }
                    }

                    for (int i = 0; i < chuteRenderers.Length; i++)
                    {
                        if (chuteRenderers[i]?.name?.StartsWith("fx_gasJet") == false)
                        {
                            chuteRenderers[i].enabled = jetpackVisible;
                        }
                    }
                }
            }

            void Helmet()
            {
                if (FlightGlobals.currentMainBody?.atmosphereContainsOxygen != true) return;

                if (helmetTime < (helmetDelay ?? 1))
                {
                    helmetTime += Time.deltaTime;
                }
                else
                {
                    double pressure = FlightGlobals.getStaticPressure();

                    if (helmetHidden != !(pressure < helmetLowPressure || pressure > helmetHighPressure || helmetLowPressure == helmetHighPressure))
                    {
                        if (helmetHidden) helmetTime = 0;

                        helmetHidden = eva.CanSafelyRemoveHelmet() && !helmetHidden;

                        eva.ToggleHelmetAndNeckRing(!helmetHidden, !helmetHidden);
                    }
                }
            }

            void OnDestroy()
            {
                if (Nyan.forever)
                {
                    TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, RainbowJets);
                    return;
                }
                if (jetpackMaxGravity != null)
                {
                    TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, CheckGee);
                    TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, JetPack);
                }
                if (helmetLowPressure != null || helmetHighPressure != null)
                {
                    TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, Helmet);
                }
            }

            internal override void LoadFor(ProtoCrewMember kerbal)
            {
                Debug.Log("CustomSuit.LoadFor", "kerbal = " + kerbal);

                if (kerbal == null) return;

                Info.hash = "";
                int? useChance = null;
                string collection = "";

                for (int i = 0; i < SuitInfo.DataBase?.Count; i++)
                {
                    SuitInfo info = (SuitInfo)SuitInfo.DataBase[i].GetFor(kerbal);

                    Debug.Log("CustomSuit.LoadFor", "SuitInfo.DataBase[" + i + "] = " + info);
                    if (info != null)
                    {
                        Type type = Type.IVA;
                        if (eva != null) type = Type.EVA;
                        else if (kerbal.GetType() == typeof(CrewMember) && ((CrewMember)kerbal).activity == 0) type = Type.EVA;

                        Debug.Log("CustomSuit.LoadFor", "Matching suit type = " + info.type + " to current activity = " + type);
                        if (info.type != null && info.type != type) continue;

                        bool useSuit = true;
                        if (eva != null)
                        {
                            double pressure = FlightGlobals.getStaticPressure();
                            useSuit = !(pressure < info.suitMinPressure) && !(pressure > info.suitMaxPressure);
                            Debug.Log("CustomSuit.LoadFor", "Matching suitMinPressure = " + info.suitMinPressure + ", suitMaxPressure = " + info.suitMaxPressure + " to current atmospheric pressure = " + pressure + ". useSuit = " + useSuit);
                        }

                        Debug.Log("CustomSuit.LoadFor", "Matching suit collection = " + info.collection + " to current collection = " + collection);
                        if (string.IsNullOrEmpty(collection) || collection == info.collection)
                        {
                            if (useChance == null && info.useChance != 1)
                                useChance = kerbal.Hash(info.useGameSeed) % 100;

                            Debug.Log("CustomSuit.LoadFor", "Matching suit useChance = " + info.useChance + " to generated chance = " + useChance + " %");
                            if (info.useChance == 1 || useChance < info.useChance * 100)
                            {
                                Debug.Log("CustomSuit.LoadFor", "Loading SuitInfo.DataBase[" + i + "]");

                                // Collection
                                collection = info.collection;

                                // Suit Specific Requirements
                                if (useSuit)
                                {
                                    helmetLowPressure = helmetLowPressure ?? info.helmetLowPressure;
                                    helmetHighPressure = helmetHighPressure ?? info.helmetHighPressure;
                                    helmetDelay = helmetDelay ?? info.helmetDelay;
                                }
                                jetpackMaxGravity = jetpackMaxGravity ?? info.jetpackMaxGravity;

                                // Colors
                                if (useSuit)
                                {
                                    body = body ?? info.body.Pick(kerbal, info.useGameSeed);
                                    neckRing = neckRing ?? info.neckRing.Pick(kerbal, info.useGameSeed);
                                    helmet = helmet ?? info.helmet.Pick(kerbal, info.useGameSeed);
                                    visor = visor ?? info.visor.Pick(kerbal, info.useGameSeed);
                                    flares = flares ?? info.flares.Pick(kerbal, info.useGameSeed);
                                    light = light ?? info.light.Pick(kerbal, info.useGameSeed);
                                }
                                jetpack = jetpack ?? info.jetpack.Pick(kerbal, info.useGameSeed);
                                parachute = parachute ?? info.parachute.Pick(kerbal, info.useGameSeed);
                                canopy = canopy ?? info.canopy.Pick(kerbal, info.useGameSeed);
                                backpack = backpack ?? info.backpack.Pick(kerbal, info.useGameSeed);
                                flag = flag ?? info.flag.Pick(kerbal, info.useGameSeed);
                                gasjets = gasjets ?? info.gasjets.Pick(kerbal, info.useGameSeed);
                                headset = headset ?? info.headset.Pick(kerbal, info.useGameSeed);
                                mug = mug ?? info.mug.Pick(kerbal, info.useGameSeed);
                                glasses = glasses ?? info.glasses.Pick(kerbal, info.useGameSeed);
                                backdrop = backdrop ?? info.backdrop.Pick(kerbal, info.useGameSeed);

                                // Textures
                                if (useSuit)
                                {
                                    bodyTex = bodyTex ?? info.bodyTex.Pick(kerbal, info.useGameSeed);
                                    neckRingTex = neckRingTex ?? info.neckRingTex.At(bodyTex, info.bodyTex, kerbal, info.useGameSeed);
                                    helmetTex = helmetTex ?? info.helmetTex.At(bodyTex, info.bodyTex, kerbal, info.useGameSeed);
                                    visorTex = visorTex ?? info.visorTex.Pick(kerbal, info.useGameSeed);
                                    flaresTex = flaresTex ?? info.flaresTex.Pick(kerbal, info.useGameSeed);
                                }
                                jetpackTex = jetpackTex ?? info.jetpackTex.Pick(kerbal, info.useGameSeed);
                                parachuteTex = parachuteTex ?? info.parachuteTex.Pick(kerbal, info.useGameSeed);
                                canopyTex = canopyTex ?? info.canopyTex.Pick(kerbal, info.useGameSeed);
                                backpackTex = backpackTex ?? info.backpackTex.Pick(kerbal, info.useGameSeed);
                                flagTex = flagTex ?? info.flagTex.Pick(kerbal, info.useGameSeed);
                                gasjetsTex = gasjetsTex ?? info.gasjetsTex.Pick(kerbal, info.useGameSeed);
                                headsetTex = headsetTex ?? info.headsetTex.Pick(kerbal, info.useGameSeed);
                                mugTex = mugTex ?? info.mugTex.Pick(kerbal, info.useGameSeed);
                                glassesTex = glassesTex ?? info.glassesTex.Pick(kerbal, info.useGameSeed);
                                backdropTex = backdropTex ?? info.backdropTex.Pick(kerbal, info.useGameSeed);

                                // Normals
                                if (useSuit)
                                {
                                    bodyNrm = bodyNrm ?? info.bodyNrm.At(bodyTex, info.bodyTex, kerbal, info.useGameSeed);
                                    neckRingNrm = neckRingNrm ?? info.neckRingNrm.At(neckRingTex, info.neckRingTex, kerbal, info.useGameSeed);
                                    helmetNrm = helmetNrm ?? info.helmetNrm.At(helmetTex, info.helmetTex, kerbal, info.useGameSeed);
                                    visorNrm = visorNrm ?? info.visorNrm.At(visorTex, info.visorTex, kerbal, info.useGameSeed);
                                }
                                jetpackNrm = jetpackNrm ?? info.jetpackNrm.At(jetpackTex, info.jetpackTex, kerbal, info.useGameSeed);
                                parachuteNrm = parachuteNrm ?? info.parachuteNrm.At(parachuteTex, info.parachuteTex, kerbal, info.useGameSeed);
                                canopyNrm = canopyNrm ?? info.canopyNrm.At(canopyTex, info.canopyTex, kerbal, info.useGameSeed);
                                backpackNrm = backpackNrm ?? info.backpackNrm.At(backpackTex, info.backpackTex, kerbal, info.useGameSeed);
                                flagNrm = flagNrm ?? info.flagNrm.At(flagTex, info.flagTex, kerbal, info.useGameSeed);
                                headsetNrm = headsetNrm ?? info.headsetNrm.At(headsetTex, info.headsetTex, kerbal, info.useGameSeed);
                                mugNrm = mugNrm ?? info.mugNrm.At(mugTex, info.mugTex, kerbal, info.useGameSeed);
                                glassesNrm = glassesNrm ?? info.glassesNrm.At(glassesTex, info.glassesTex, kerbal, info.useGameSeed);
                                backdropNrm = backdropNrm ?? info.backdropNrm.At(backdropTex, info.backdropTex, kerbal, info.useGameSeed);

                                continue;
                            }
                        }
                    }

                    Debug.Log("CustomSuit.LoadFor", "Ignoring SuitInfo.DataBase[" + i + "]");
                }

                helmetTime = helmetDelay ?? 1;
            }

            internal override void ApplyTo(ProtoCrewMember kerbal)
            {
                Debug.Log("CustomSuit.ApplyTo", "kerbal = " + kerbal);

                if (kerbal == null) return;

                if (Nyan.nyan)
                {
                    if (HighLogic.LoadedScene == GameScenes.MAINMENU || Nyan.forever)
                    {
                        NyanSuit.ApplyTo(kerbal, this);
                        return;
                    }
                }

                Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
                Debug.Log("CustomSuit.ApplyTo", "renderers.Length = " + renderers?.Length);

                for (int i = 0; i < renderers?.Length; i++)
                {
                    string name = renderers[i]?.name;
                    Debug.Log("CustomSuit.ApplyTo", "renderers[" + i + "].name = " + name + ", mainTex = " + renderers[i]?.sharedMaterial?.mainTexture?.name);

                    if (name == "backdrop")
                    {
                        if (backdropTex != null)
                        {
                            Material material = renderers[i]?.material;

                            material.SetColor(backdrop);
                            material.SetTexture(backdropTex);
                            material.SetNormal(backdropNrm);
                        }
                    }
                    else
                    {
                        Material material = renderers[i]?.material;
                        if (material == null) continue;

                        switch (name)
                        {
                            case "body01":
                            case "mesh_female_kerbalAstronaut01_body01":
                            case "coat01":
                            case "pants01":
                            case "mesh_bowTie01":
                                material.SetColor(body);
                                material.SetTexture(bodyTex);
                                material.SetNormal(bodyNrm);
                                continue;

                            case "helmet":
                            case "mesh_female_kerbalAstronaut01_helmet":
                            case "mesh_backpack":
                            case "mesh_hazm_helmet":
                            case "mesh_helmet_support":
                            case "helmetConstr01":
                                material.SetColor(helmet ?? body);
                                material.SetTexture(helmetTex ?? bodyTex);
                                material.SetNormal(helmetNrm ?? bodyNrm);
                                continue;

                            case "neckRing":
                                material.SetColor(neckRing ?? helmet ?? body);
                                material.SetTexture(neckRingTex ?? helmetTex ?? bodyTex);
                                material.SetNormal(neckRingNrm ?? helmetNrm ?? bodyNrm);
                                continue;

                            case "visor":
                            case "mesh_female_kerbalAstronaut01_visor":
                            case "mesh_hazm_visor":
                                material.SetColor(visor);
                                material.SetTexture(visorTex);
                                material.SetNormal(visorNrm);
                                continue;

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
                                if (flares != null)
                                {
                                    if (material?.shader?.name == "Particles/Alpha Blended Premultiply")
                                        material.shader = Shader.Find("Particles/Alpha Blended");

                                    if (material.HasProperty("_TintColor"))
                                        material.SetTintColor(flares);
                                    else
                                        material.SetColor(flares);
                                }

                                material.SetTexture(flaresTex);

                                if (light.HasValue)
                                {
                                    Light lights = renderers[i]?.transform?.parent?.GetComponentInChildren<Light>();
                                    if (lights != null) lights.color = light.Value;
                                }
                                continue;

                            case "kbEVA_flagDecals":
                                material.SetColor(flag);
                                material.SetTexture(flagTex);
                                material.SetNormal(flagNrm);
                                continue;

                            case "gene_mug_base":
                            case "gene_mug_handle":
                                material.SetColor(mug);
                                material.SetTexture(mugTex);
                                material.SetNormal(mugNrm);
                                continue;
                        }

                        switch (material?.mainTexture?.name)
                        {
                            case "EVAjetpack":
                            case "EVAjetpackscondary":
                            case "ksp_ig_jetpack_diffuse":
                                material.SetColor(jetpack);
                                material.SetTexture(jetpackTex);
                                material.SetNormal(jetpackNrm);
                                continue;

                            case "fairydust":
                                material.SetTintColor(gasjets);
                                material.SetTexture(gasjetsTex);
                                continue;

                            case "backpack_Diff":
                                material.SetColor(parachute);
                                material.SetTexture(parachuteTex);
                                material.SetNormal(parachuteNrm);
                                continue;

                            case "canopy_Diff":
                                material.SetColor(canopy);
                                material.SetTexture(canopyTex);
                                material.SetNormal(canopyNrm);
                                continue;

                            case "cargoContainerPack_diffuse":
                                material.SetColor(backpack);
                                material.SetTexture(backpackTex);
                                material.SetNormal(backpackNrm);
                                continue;

                            case "kbGeneKerman_headset":
                                material.SetColor(headset);
                                material.SetTexture(headsetTex);
                                material.SetNormal(headsetNrm);
                                continue;

                            case "wernerVonKerman_glasses":
                                material.SetColor(glasses);
                                material.SetTexture(glassesTex);
                                material.SetNormal(glassesNrm);
                                continue;
                        }
                    }
                }
            }


            // Nyan
            int index = 0;
            float wait = 0;
            Color[] rainbow = new[] { new Color(1, 0, 0, 0.5f), new Color(1, 0.6f, 0, 0.5f), new Color(1, 1, 0, 0.5f), new Color(0.2f, 1, 0, 0.5f), new Color(0, 0.6f, 1, 0.5f), new Color(0.4f, 0.2f, 1, 0.5f) };

            void RainbowJets()
            {
                if (wait > 0.25)
                {
                    Renderer[] renderers = eva.gameObject.GetChild("jetpack01").GetComponentsInChildren<Renderer>(true);

                    for (int i = 0; i < renderers.Length; i++)
                    {
                        if (renderers[i]?.name?.StartsWith("fx_gasJet") == true)
                        {
                            renderers[i].material.SetTintColor(rainbow[index]);
                        }
                    }

                    index = (index + 1) % 6;
                    wait = 0;
                }
                else
                {
                    wait += Time.deltaTime;
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
