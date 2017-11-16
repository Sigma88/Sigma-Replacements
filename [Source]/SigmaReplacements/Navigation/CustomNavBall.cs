using UnityEngine;
using UnityEngine.UI;
using KSP.UI;


namespace SigmaReplacements
{
    namespace Navigation
    {
        public class CustomNavBall : CustomObject
        {
            // Colors
            internal Color? NavBall = null;
            internal Color? Shading = null;
            internal Color? ShadingMask = null;
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


            internal void OnStart()
            {
                ProtoCrewMember kerbal = Apply();
                Debug.Log("CustomNavBall.Start", "kerbal = " + kerbal + " (" + kerbal?.GetType() + ")");
                if (kerbal == null) return;

                LoadFor(kerbal);
                ApplyTo(FlightUIModeController.Instance);
            }

            void LoadFor(ProtoCrewMember kerbal)
            {
                Debug.Log("CustomNavBall.LoadFor", "kerbal = " + kerbal);

                Info.hash = "";
                int? useChance = null;
                string collection = "";

                for (int i = 0; i < NavBallInfo.DataBase?.Count; i++)
                {
                    NavBallInfo info = (NavBallInfo)NavBallInfo.DataBase[i].GetFor(kerbal);

                    if (info != null)
                    {
                        if (string.IsNullOrEmpty(collection) || collection == info.collection)
                        {
                            if (info.useChance != 1)
                                useChance = kerbal.Hash(info.useGameSeed) % 100;

                            if (info.useChance == 1 || useChance < info.useChance * 100)
                            {
                                Debug.Log("CustomNavBall.LoadFor", "Matched suit useChance = " + info.useChance + " to generated chance = " + useChance + " %");
                                Debug.Log("CustomNavBall.LoadFor", "Matched suit collection = " + info.collection + " to current collection = " + collection);

                                // Collection
                                collection = info.collection;

                                // Colors and Textures
                                Pick(info, kerbal);
                            }
                        }
                    }
                }
            }

            internal void Pick(NavBallInfo info, ProtoCrewMember kerbal = null)
            {
                // Colors
                NavBall = NavBall ?? info.NavBall.Pick(kerbal, info.useGameSeed);
                Shading = Shading ?? info.Shading.Pick(kerbal, info.useGameSeed);
                ShadingMask = ShadingMask ?? info.ShadingMask.Pick(kerbal, info.useGameSeed);
                Cursor = Cursor ?? info.Cursor.Pick(kerbal, info.useGameSeed);
                Vectors = Vectors ?? info.Vectors.Pick(kerbal, info.useGameSeed);
                ProgradeVector = ProgradeVector ?? info.ProgradeVector.Pick(kerbal, info.useGameSeed);
                ProgradeWaypoint = ProgradeWaypoint ?? info.ProgradeWaypoint.Pick(kerbal, info.useGameSeed);
                RetrogradeVector = RetrogradeVector ?? info.RetrogradeVector.Pick(kerbal, info.useGameSeed);
                RetrogradeWaypoint = RetrogradeWaypoint ?? info.RetrogradeWaypoint.Pick(kerbal, info.useGameSeed);
                Maneuvers = Maneuvers ?? info.Maneuvers.Pick(kerbal, info.useGameSeed);
                RadialInVector = RadialInVector ?? info.RadialInVector.Pick(kerbal, info.useGameSeed);
                RadialOutVector = RadialOutVector ?? info.RadialOutVector.Pick(kerbal, info.useGameSeed);
                NormalVector = NormalVector ?? info.NormalVector.Pick(kerbal, info.useGameSeed);
                AntiNormalVector = AntiNormalVector ?? info.AntiNormalVector.Pick(kerbal, info.useGameSeed);
                BurnVector = BurnVector ?? info.BurnVector.Pick(kerbal, info.useGameSeed);
                Arrows = Arrows ?? info.Arrows.Pick(kerbal, info.useGameSeed);
                Buttons = Buttons ?? info.Buttons.Pick(kerbal, info.useGameSeed);
                Frame = Frame ?? info.Frame.Pick(kerbal, info.useGameSeed);

                // Textures
                NavBallTex = NavBallTex ?? info.NavBallTex.Pick(kerbal, info.useGameSeed);
                ShadingTex = ShadingTex ?? info.ShadingTex.Pick(kerbal, info.useGameSeed);
                ShadingMaskTex = ShadingMaskTex ?? info.ShadingMaskTex.Pick(kerbal, info.useGameSeed);
                CursorTex = CursorTex ?? info.CursorTex.Pick(kerbal, info.useGameSeed);
                VectorsTex = VectorsTex ?? info.VectorsTex.Pick(kerbal, info.useGameSeed);
                ProgradeVectorTex = ProgradeVectorTex ?? info.ProgradeVectorTex.Pick(kerbal, info.useGameSeed);
                ProgradeWaypointTex = ProgradeWaypointTex ?? info.ProgradeWaypointTex.Pick(kerbal, info.useGameSeed);
                RetrogradeVectorTex = RetrogradeVectorTex ?? info.RetrogradeVectorTex.Pick(kerbal, info.useGameSeed);
                RetrogradeWaypointTex = RetrogradeWaypointTex ?? info.RetrogradeWaypointTex.Pick(kerbal, info.useGameSeed);
                ManeuversTex = ManeuversTex ?? info.ManeuversTex.Pick(kerbal, info.useGameSeed);
                RadialInVectorTex = RadialInVectorTex ?? info.RadialInVectorTex.Pick(kerbal, info.useGameSeed);
                RadialOutVectorTex = RadialOutVectorTex ?? info.RadialOutVectorTex.Pick(kerbal, info.useGameSeed);
                NormalVectorTex = NormalVectorTex ?? info.NormalVectorTex.Pick(kerbal, info.useGameSeed);
                AntiNormalVectorTex = AntiNormalVectorTex ?? info.AntiNormalVectorTex.Pick(kerbal, info.useGameSeed);
                BurnVectorTex = BurnVectorTex ?? info.BurnVectorTex.Pick(kerbal, info.useGameSeed);
                ArrowsTex = ArrowsTex ?? info.ArrowsTex.Pick(kerbal, info.useGameSeed);
                ButtonsTex = ButtonsTex ?? info.ButtonsTex.Pick(kerbal, info.useGameSeed);
                FrameTex = FrameTex ?? info.FrameTex.Pick(kerbal, info.useGameSeed);
            }

            internal void ApplyTo(FlightUIModeController controller)
            {
                if (controller == null) return;

                Transform original = DefaultNavBall.Instance?.transform;


                Material newNavball = controller?.gameObject?.GetChild("NavBall")?.GetComponent<Renderer>()?.material;
                Material stockNavball = original?.gameObject?.GetChild("NavBall")?.GetComponent<Renderer>()?.material;

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

                    if (newShading != null)
                    {
                        newShading.SetColor(Shading, stockShading);
                        newShading.SetTexture(ShadingTex, stockShading);
                    }

                    Image newShadingMask = newShadingObj.GetComponent<Image>();
                    Image stockShadingMask = stockShadingObj.GetComponent<Image>();

                    if (newShadingMask != null)
                    {
                        newShadingMask.SetColor(ShadingMask, stockShadingMask);
                        newShadingMask.SetTexture(ShadingMaskTex, stockShadingMask);
                    }
                }


                Image newCursor = controller?.gameObject?.GetChild("NavBallCursor")?.GetComponent<Image>();
                Image stockCursor = original?.gameObject?.GetChild("NavBallCursor")?.GetComponent<Image>();

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
                        material.SetTintColor(ProgradeVector ?? Vectors, stock);
                        material.SetMainTexture(ProgradeVectorTex ?? VectorsTex, stock);
                    }

                    else

                    if (name == "ProgradeWaypoint")
                    {
                        material.SetTintColor(ProgradeWaypoint ?? Vectors, stock);
                        material.SetMainTexture(ProgradeWaypointTex ?? VectorsTex, stock);
                    }

                    else

                    if (name == "RetrogradeVector")
                    {
                        material.SetTintColor(RetrogradeVector ?? Vectors, stock);
                        material.SetMainTexture(RetrogradeVectorTex ?? VectorsTex, stock);
                    }

                    else

                    if (name == "RetrogradeWaypoint")
                    {
                        material.SetTintColor(RetrogradeWaypoint ?? Vectors, stock);
                        material.SetMainTexture(RetrogradeWaypointTex ?? VectorsTex, stock);
                    }

                    else

                    if (name == "RadialInVector")
                    {
                        material.SetTintColor(RadialInVector ?? Maneuvers, stock);
                        material.SetMainTexture(RadialInVectorTex ?? ManeuversTex, stock);
                    }

                    else

                    if (name == "RadialOutVector")
                    {
                        material.SetTintColor(RadialOutVector ?? Maneuvers, stock);
                        material.SetMainTexture(RadialOutVectorTex ?? ManeuversTex, stock);
                    }

                    else

                    if (name == "NormalVector")
                    {
                        material.SetTintColor(NormalVector ?? Maneuvers, stock);
                        material.SetMainTexture(NormalVectorTex ?? ManeuversTex, stock);
                    }

                    else

                    if (name == "AntiNormalVector")
                    {
                        material.SetTintColor(AntiNormalVector ?? Maneuvers, stock);
                        material.SetMainTexture(AntiNormalVectorTex ?? ManeuversTex, stock);
                    }

                    else

                    if (name == "BurnVector")
                    {
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


                UIStateImage[] newButtons = controller?.gameObject?.GetComponentsInChildren<UIStateImage>();
                UIStateImage[] stockButtons = original?.gameObject?.GetComponentsInChildren<UIStateImage>();

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

                if (newToggle != null)
                {
                    newToggle.SetColor(Buttons, stockToggle);
                    newToggle.SetTexture(ButtonsTex, stockToggle);
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

                if (newFrame != null)
                {
                    newFrame.SetColor(Frame, stockFrame);
                    newFrame.SetTexture(FrameTex, stockFrame);
                }


                Image headingframe = controller?.gameObject?.GetChild("HeadingFrame")?.GetComponent<Image>();

                if (headingframe != null)
                {
                    headingframe.color = new Color(0, 0, 0, 0);
                }
            }
        }
    }
}
