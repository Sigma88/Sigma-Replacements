using UnityEngine;
using UnityEngine.UI;
using KSP.UI;


namespace SigmaReplacements
{
    namespace Navigation
    {
        internal class CustomNavBall : CustomObject
        {
            // Colors
            internal Color? NavBall = null;
            internal Color? Shading = null;
            internal Color? Cursor = null;
            internal Color? Vectors = null;
            internal Color? ProgradeVector = null;
            internal Color? ProgradeWaypoint = null;
            internal Color? RetrogradeVector = null;
            internal Color? RetrogradeWaypoint = null;
            internal Color? Maneuvers = null;
            internal Color? RadialInVector = null;
            internal Color? RadialOutVector = null;
            internal Color? NormalVector = null;
            internal Color? AntiNormalVector = null;
            internal Color? BurnVector = null;
            internal Color? Arrows = null;
            internal Color? Buttons = null;
            internal Color? Frame = null;
            internal Color? IVAbase = null;
            internal Color? IVAprograde = null;

            // Textures
            internal Texture NavBallTex = null;
            internal Texture ShadingTex = null;
            internal Texture ShadingMaskTex = null;
            internal Texture CursorTex = null;
            internal Texture VectorsTex = null;
            internal Texture ProgradeVectorTex = null;
            internal Texture ProgradeWaypointTex = null;
            internal Texture RetrogradeVectorTex = null;
            internal Texture RetrogradeWaypointTex = null;
            internal Texture ManeuversTex = null;
            internal Texture RadialInVectorTex = null;
            internal Texture RadialOutVectorTex = null;
            internal Texture NormalVectorTex = null;
            internal Texture AntiNormalVectorTex = null;
            internal Texture BurnVectorTex = null;
            internal Texture ArrowsTex = null;
            internal Texture ButtonsTex = null;
            internal Texture FrameTex = null;
            internal Vector2? FrameTexRes = null;
            internal Texture IVAbaseTex = null;
            internal Texture IVAemissiveTex = null;
            internal Texture IVAprogradeTex = null;


            internal void OnStart()
            {
                ProtoCrewMember kerbal = Apply();
                Debug.Log("CustomNavBall.Start", "kerbal = " + kerbal + " (" + kerbal?.GetType() + ")");
                if (kerbal == null) return;

                LoadFor(kerbal);
                ApplyTo(FlightUIModeController.Instance);
            }

            internal override void LoadFor(ProtoCrewMember kerbal)
            {
                Debug.Log("CustomNavBall.LoadFor", "kerbal = " + kerbal);

                Info.hash = "";
                int? useChance = null;
                string collection = "";

                for (int i = 0; i < NavBallInfo.DataBase?.Count; i++)
                {
                    NavBallInfo info = (NavBallInfo)NavBallInfo.DataBase[i].GetFor(kerbal);

                    Debug.Log("CustomNavBall.LoadFor", "NavBallInfo.DataBase[" + i + "]");
                    if (info != null)
                    {
                        Debug.Log("CustomNavBall.LoadFor", "Matching navball collection = " + info.collection + " to current collection = " + collection);
                        if (string.IsNullOrEmpty(collection) || collection == info.collection)
                        {
                            if (useChance == null && info.useChance != 1)
                                useChance = kerbal.Hash(info.useGameSeed) % 100;

                            Debug.Log("CustomNavBall.LoadFor", "Matching navball useChance = " + info.useChance + " to generated chance = " + useChance + " %");
                            if (info.useChance == 1 || useChance < info.useChance * 100)
                            {
                                Debug.Log("CustomNavBall.LoadFor", "Loading NavBallInfo.DataBase[" + i + "]");

                                // Collection
                                collection = info.collection;

                                // Colors and Textures
                                Pick(info, kerbal);

                                continue;
                            }
                        }
                    }

                    Debug.Log("CustomNavBall.LoadFor", "Ignoring NavBallInfo.DataBase[" + i + "]");
                }
            }

            internal void Pick(NavBallInfo info, ProtoCrewMember kerbal = null, string name = null)
            {
                // Colors
                NavBall = NavBall ?? info.NavBall.Pick(kerbal, info.useGameSeed, name);
                Shading = Shading ?? info.Shading.Pick(kerbal, info.useGameSeed, name);
                Cursor = Cursor ?? info.Cursor.Pick(kerbal, info.useGameSeed, name);
                Vectors = Vectors ?? info.Vectors.Pick(kerbal, info.useGameSeed, name);
                ProgradeVector = ProgradeVector ?? info.ProgradeVector.Pick(kerbal, info.useGameSeed, name);
                ProgradeWaypoint = ProgradeWaypoint ?? info.ProgradeWaypoint.Pick(kerbal, info.useGameSeed, name);
                RetrogradeVector = RetrogradeVector ?? info.RetrogradeVector.Pick(kerbal, info.useGameSeed, name);
                RetrogradeWaypoint = RetrogradeWaypoint ?? info.RetrogradeWaypoint.Pick(kerbal, info.useGameSeed, name);
                Maneuvers = Maneuvers ?? info.Maneuvers.Pick(kerbal, info.useGameSeed, name);
                RadialInVector = RadialInVector ?? info.RadialInVector.Pick(kerbal, info.useGameSeed, name);
                RadialOutVector = RadialOutVector ?? info.RadialOutVector.Pick(kerbal, info.useGameSeed, name);
                NormalVector = NormalVector ?? info.NormalVector.Pick(kerbal, info.useGameSeed, name);
                AntiNormalVector = AntiNormalVector ?? info.AntiNormalVector.Pick(kerbal, info.useGameSeed, name);
                BurnVector = BurnVector ?? info.BurnVector.Pick(kerbal, info.useGameSeed, name);
                Arrows = Arrows ?? info.Arrows.Pick(kerbal, info.useGameSeed, name);
                Buttons = Buttons ?? info.Buttons.Pick(kerbal, info.useGameSeed, name);
                Frame = Frame ?? info.Frame.Pick(kerbal, info.useGameSeed, name);
                IVAbase = IVAbase ?? info.IVAbase.Pick(kerbal, info.useGameSeed, name);
                IVAprograde = IVAprograde ?? info.IVAprograde.Pick(kerbal, info.useGameSeed, name);

                // Textures
                NavBallTex = NavBallTex ?? info.NavBallTex.Pick(kerbal, info.useGameSeed, name);
                ShadingTex = ShadingTex ?? info.ShadingTex.Pick(kerbal, info.useGameSeed, name);
                ShadingMaskTex = ShadingMaskTex ?? info.ShadingMaskTex.Pick(kerbal, info.useGameSeed, name);
                CursorTex = CursorTex ?? info.CursorTex.Pick(kerbal, info.useGameSeed, name);
                VectorsTex = VectorsTex ?? info.VectorsTex.Pick(kerbal, info.useGameSeed, name);
                ProgradeVectorTex = ProgradeVectorTex ?? info.ProgradeVectorTex.Pick(kerbal, info.useGameSeed, name);
                ProgradeWaypointTex = ProgradeWaypointTex ?? info.ProgradeWaypointTex.Pick(kerbal, info.useGameSeed, name);
                RetrogradeVectorTex = RetrogradeVectorTex ?? info.RetrogradeVectorTex.Pick(kerbal, info.useGameSeed, name);
                RetrogradeWaypointTex = RetrogradeWaypointTex ?? info.RetrogradeWaypointTex.Pick(kerbal, info.useGameSeed, name);
                ManeuversTex = ManeuversTex ?? info.ManeuversTex.Pick(kerbal, info.useGameSeed, name);
                RadialInVectorTex = RadialInVectorTex ?? info.RadialInVectorTex.Pick(kerbal, info.useGameSeed, name);
                RadialOutVectorTex = RadialOutVectorTex ?? info.RadialOutVectorTex.Pick(kerbal, info.useGameSeed, name);
                NormalVectorTex = NormalVectorTex ?? info.NormalVectorTex.Pick(kerbal, info.useGameSeed, name);
                AntiNormalVectorTex = AntiNormalVectorTex ?? info.AntiNormalVectorTex.Pick(kerbal, info.useGameSeed, name);
                BurnVectorTex = BurnVectorTex ?? info.BurnVectorTex.Pick(kerbal, info.useGameSeed, name);
                ArrowsTex = ArrowsTex ?? info.ArrowsTex.Pick(kerbal, info.useGameSeed, name);
                ButtonsTex = ButtonsTex ?? info.ButtonsTex.Pick(kerbal, info.useGameSeed, name);
                FrameTex = FrameTex ?? info.FrameTex.Pick(kerbal, info.useGameSeed, name);
                FrameTexRes = FrameTexRes ?? info.FrameTexRes.At(FrameTex, info.FrameTex, kerbal, info.useGameSeed, name);
                IVAbaseTex = IVAbaseTex ?? info.IVAbaseTex.Pick(kerbal, info.useGameSeed, name);
                IVAemissiveTex = IVAemissiveTex ?? info.IVAemissiveTex.At(NavBallTex, info.NavBallTex, kerbal, info.useGameSeed, name);
                IVAprogradeTex = IVAprogradeTex ?? info.IVAprogradeTex.Pick(kerbal, info.useGameSeed, name);
            }

            internal void ApplyTo(FlightUIModeController controller)
            {
                Debug.Log("CustomNavBall.ApplyTo", "controller = " + controller);

                if (Nyan.forever)
                {
                    NyanNavBall.ApplyTo(controller);
                    return;
                }

                if (controller == null) return;

                Transform original = DefaultNavBall.Instance?.transform;


                Material newNavball = controller?.gameObject?.GetChild("NavBall")?.GetComponent<Renderer>()?.material;
                Material stockNavball = original?.gameObject?.GetChild("NavBall")?.GetComponent<Renderer>()?.material;
                Debug.Log("StockTexturesAndColors", "NavBall = " + stockNavball?.GetColor("_TintColor") + "NavBallTex = " + stockNavball?.GetTexture("_MainTexture"));
                if (newNavball != null)
                {
                    newNavball.SetTintColor(NavBall, stockNavball);
                    newNavball.SetMainTexture(NavBallTex, stockNavball);
                }


                GameObject newShadingObj = controller?.gameObject?.GetChild("NavBall_OverlayMask");
                GameObject stockShadingObj = original?.gameObject?.GetChild("NavBall_OverlayMask");

                if (newShadingObj != null)
                {
                    Image newShading = newShadingObj?.GetChild("shadingOverlay")?.GetComponent<Image>();
                    Image stockShading = stockShadingObj?.GetChild("shadingOverlay")?.GetComponent<Image>();
                    Debug.Log("StockTexturesAndColors", "Shading = " + stockShading?.color + "ShadingTex = " + stockShading?.sprite?.texture);
                    if (newShading != null)
                    {
                        newShading.SetColor(Shading, stockShading);
                        newShading.SetTexture(ShadingTex, stockShading, true);
                    }

                    Image newShadingMask = newShadingObj.GetComponent<Image>();
                    Image stockShadingMask = stockShadingObj.GetComponent<Image>();
                    Debug.Log("StockTexturesAndColors", "ShadingMaskTex = " + stockShadingMask?.sprite?.texture);
                    if (newShadingMask != null)
                    {
                        newShadingMask.SetTexture(ShadingMaskTex, stockShadingMask, true);
                    }
                }


                Image newCursor = controller?.gameObject?.GetChild("NavBallCursor")?.GetComponent<Image>();
                Image stockCursor = original?.gameObject?.GetChild("NavBallCursor")?.GetComponent<Image>();
                Debug.Log("StockTexturesAndColors", "Cursor = " + stockCursor?.color + "CursorTex = " + stockCursor?.sprite?.texture);
                if (newCursor != null)
                {
                    newCursor.SetColor(Cursor, stockCursor);
                    newCursor.SetTexture(CursorTex, stockCursor);
                }


                Renderer[] newVectors = controller?.gameObject?.GetChild("NavBallVectorsPivot")?.GetComponentsInChildren<Renderer>(true);
                Renderer[] stockVectors = original?.gameObject?.GetChild("NavBallVectorsPivot")?.GetComponentsInChildren<Renderer>(true);

                for (int i = 0; i < newVectors?.Length; i++)
                {
                    string name = newVectors[i]?.name;

                    Material material = newVectors[i]?.material;
                    if (material == null) continue;

                    Material stock = stockVectors?.Length > i ? stockVectors[i]?.material : null;

                    if (name == "ProgradeVector")
                    {
                        Debug.Log("StockTexturesAndColors", "ProgradeVector = " + stock?.GetColor("_TintColor") + "ProgradeVectorTex = " + stock?.GetTexture("_MainTexture"));
                        material.SetTintColor(ProgradeVector ?? Vectors, stock);
                        material.SetMainTexture(ProgradeVectorTex ?? VectorsTex, stock);
                    }

                    else

                    if (name == "ProgradeWaypoint")
                    {
                        Debug.Log("StockTexturesAndColors", "ProgradeWaypoint = " + stock?.GetColor("_TintColor") + "ProgradeWaypointTex = " + stock?.GetTexture("_MainTexture"));
                        material.SetTintColor(ProgradeWaypoint ?? Vectors, stock);
                        material.SetMainTexture(ProgradeWaypointTex ?? VectorsTex, stock);
                    }

                    else

                    if (name == "RetrogradeVector")
                    {
                        Debug.Log("StockTexturesAndColors", "RetrogradeVector = " + stock?.GetColor("_TintColor") + "RetrogradeVectorTex = " + stock?.GetTexture("_MainTexture"));
                        material.SetTintColor(RetrogradeVector ?? Vectors, stock);
                        material.SetMainTexture(RetrogradeVectorTex ?? VectorsTex, stock);
                    }

                    else

                    if (name == "RetrogradeWaypoint")
                    {
                        Debug.Log("StockTexturesAndColors", "RetrogradeWaypoint = " + stock?.GetColor("_TintColor") + "RetrogradeWaypointTex = " + stock?.GetTexture("_MainTexture"));
                        material.SetTintColor(RetrogradeWaypoint ?? Vectors, stock);
                        material.SetMainTexture(RetrogradeWaypointTex ?? VectorsTex, stock);
                    }

                    else

                    if (name == "RadialInVector")
                    {
                        Debug.Log("StockTexturesAndColors", "RadialInVector = " + stock?.GetColor("_TintColor") + "RadialInVectorTex = " + stock?.GetTexture("_MainTexture"));
                        material.SetTintColor(RadialInVector ?? Maneuvers, stock);
                        material.SetMainTexture(RadialInVectorTex ?? ManeuversTex, stock);
                    }

                    else

                    if (name == "RadialOutVector")
                    {
                        Debug.Log("StockTexturesAndColors", "RadialOutVector = " + stock?.GetColor("_TintColor") + "RadialOutVectorTex = " + stock?.GetTexture("_MainTexture"));
                        material.SetTintColor(RadialOutVector ?? Maneuvers, stock);
                        material.SetMainTexture(RadialOutVectorTex ?? ManeuversTex, stock);
                    }

                    else

                    if (name == "NormalVector")
                    {
                        Debug.Log("StockTexturesAndColors", "NormalVector = " + stock?.GetColor("_TintColor") + "NormalVectorTex = " + stock?.GetTexture("_MainTexture"));
                        material.SetTintColor(NormalVector ?? Maneuvers, stock);
                        material.SetMainTexture(NormalVectorTex ?? ManeuversTex, stock);
                    }

                    else

                    if (name == "AntiNormalVector")
                    {
                        Debug.Log("StockTexturesAndColors", "AntiNormalVector = " + stock?.GetColor("_TintColor") + "AntiNormalVectorTex = " + stock?.GetTexture("_MainTexture"));
                        material.SetTintColor(AntiNormalVector ?? Maneuvers, stock);
                        material.SetMainTexture(AntiNormalVectorTex ?? ManeuversTex, stock);
                    }

                    else

                    if (name == "BurnVector")
                    {
                        Debug.Log("StockTexturesAndColors", "BurnVector = " + stock?.GetColor("_TintColor") + "BurnVectorTex = " + stock?.GetTexture("_MainTexture"));
                        material.SetTintColor(BurnVector ?? Maneuvers, stock);
                        material.SetMainTexture(BurnVectorTex ?? ManeuversTex, stock);
                    }

                    else

                    if (name == "BurnVectorArrow")
                    {
                        material.SetTintColor(Arrows, stock);
                        material.SetMainTexture(ArrowsTex, stock);
                    }
                }


                UIStateImage[] newButtons = controller?.gameObject?.GetChild("IVAEVACollapseGroup")?.GetComponentsInChildren<UIStateImage>(true);
                UIStateImage[] stockButtons = original?.gameObject?.GetChild("IVAEVACollapseGroup")?.GetComponentsInChildren<UIStateImage>(true);

                for (int i = 0; i < newButtons?.Length; i++)
                {
                    string name = newButtons[i]?.name;
                    if (name == "RCS" || name == "SAS")
                    {
                        UIStateImage button = newButtons[i];
                        UIStateImage stock = stockButtons?.Length > i ? stockButtons[i] : null;

                        button.SetColor(Buttons, stock);
                        button.SetTexture(ButtonsTex, stock);
                    }
                }


                Image newToggle = controller?.gameObject?.GetChild("ButtonTabToggle")?.GetComponent<Image>();
                Image stockToggle = original?.gameObject?.GetChild("ButtonTabToggle")?.GetComponent<Image>();
                Debug.Log("StockTexturesAndColors", "ButtonsTex = " + stockToggle?.sprite?.texture);
                if (newToggle != null)
                {
                    newToggle.SetColor(Buttons, stockToggle);
                    newToggle.SetTexture(ButtonsTex, stockToggle);
                    newToggle.rectTransform.SetAsLastSibling();
                }


                Image[] newGauges = controller?.gameObject?.GetChild("SideGauges")?.GetComponentsInChildren<Image>(true);
                Image[] stockGauges = original?.gameObject?.GetChild("SideGauges")?.GetComponentsInChildren<Image>(true);

                for (int i = 0; i < newGauges?.Length; i++)
                {
                    string name = newGauges[i]?.name;

                    if (name == "ThrottleGaugePointerImage" || name == "GeeGaugePointerImage")
                    {
                        Image stock = stockGauges?.Length > i ? stockGauges[i] : null;
                        newGauges[i].SetColor(Buttons, stock);
                        newGauges[i].SetTexture(ButtonsTex, stock);
                    }
                }


                UIStateToggleButton[] newModes = controller?.gameObject?.GetChild("AutopilotModes")?.GetComponentsInChildren<UIStateToggleButton>(true);
                UIStateToggleButton[] stockModes = original?.gameObject?.GetChild("AutopilotModes")?.GetComponentsInChildren<UIStateToggleButton>(true);

                for (int i = 0; i < newModes?.Length; i++)
                {
                    UIStateToggleButton mode = newModes[i];
                    UIStateToggleButton stock = stockModes?.Length > i ? stockModes[i] : null;

                    mode.SetColor(Buttons, stock);
                    mode.SetTexture(ButtonsTex, stock);
                }


                Image newFrame = controller?.gameObject?.GetChild("Frame")?.GetComponent<Image>();
                Image stockFrame = original?.gameObject?.GetChild("Frame")?.GetComponent<Image>();
                Debug.Log("StockTexturesAndColors", "Frame = " + stockFrame?.color + ", FrameTex = " + stockFrame?.sprite?.texture);
                if (newFrame != null)
                {
                    newFrame.SetColor(Frame, stockFrame);
                    newFrame.SetTexture(FrameTex, stockFrame, true, true, FrameTexRes);
                }


                Image headingframe = controller?.gameObject?.GetChild("HeadingFrame")?.GetComponent<Image>();

                if (headingframe != null)
                {
                    headingframe.color = new Color(0, 0, 0, 0);
                }
            }

            internal void FixIVA(Renderer[] renderers)
            {
                Debug.Log("CustomNavBall.FixIVA", "renderers count = " + renderers?.Length);

                for (int i = 0; i < renderers?.Length; i++)
                {
                    string name = renderers[i]?.name;

                    Material material = renderers[i]?.material;
                    if (material == null) continue;


                    if (name == "NavSphere")
                    {
                        material.SetColor(NavBall);
                        material.SetTexture(NavBallTex, true);
                        material.SetEmissive(IVAemissiveTex, true);
                    }

                    else

                    if (name == "IVAbase")
                    {
                        material.SetColor(IVAbase);
                        material.SetTexture(IVAbaseTex);
                    }

                    else

                    if (name == "IVAprograde")
                    {
                        material.SetColor(IVAprograde);
                        material.SetTexture(IVAprogradeTex);
                    }

                    else

                    if (name == "progradeVector")
                    {
                        material.SetColor(ProgradeVector ?? Vectors);
                        material.SetMainTexture(ProgradeVectorTex ?? VectorsTex);
                    }

                    else

                    if (name == "progradeWaypoint")
                    {
                        material.SetColor(ProgradeWaypoint ?? Vectors);
                        material.SetMainTexture(ProgradeWaypointTex ?? VectorsTex);
                    }

                    else

                    if (name == "retrogradeVector")
                    {
                        material.SetColor(RetrogradeVector ?? Vectors);
                        material.SetMainTexture(RetrogradeVectorTex ?? VectorsTex);
                    }

                    else

                    if (name == "retrogradeWaypoint" || name == "retrogradeDeltaV")
                    {
                        material.SetColor(RetrogradeWaypoint ?? Vectors);
                        material.SetMainTexture(RetrogradeWaypointTex ?? VectorsTex);
                    }

                    else

                    if (name == "RadialInVector")
                    {
                        material.SetColor(RadialInVector ?? Maneuvers);
                        material.SetMainTexture(RadialInVectorTex ?? ManeuversTex);
                    }

                    else

                    if (name == "RadialOutVector")
                    {
                        material.SetColor(RadialOutVector ?? Maneuvers);
                        material.SetMainTexture(RadialOutVectorTex ?? ManeuversTex);
                    }

                    else

                    if (name == "NormalVector")
                    {
                        material.SetColor(NormalVector ?? Maneuvers);
                        material.SetMainTexture(NormalVectorTex ?? ManeuversTex);
                    }

                    else

                    if (name == "antiNormalVector")
                    {
                        material.SetColor(AntiNormalVector ?? Maneuvers);
                        material.SetMainTexture(AntiNormalVectorTex ?? ManeuversTex);
                    }

                    else

                    if (name == "NavWaypointVector")
                    {
                        material.SetColor(BurnVector ?? Maneuvers);
                        material.SetTexture(BurnVectorTex ?? ManeuversTex);
                    }

                    else

                    if (name == "ManeuverArrow")
                    {
                        material.SetColor(Arrows);
                        material.SetMainTexture(ArrowsTex);
                    }
                }
            }
        }
    }
}
