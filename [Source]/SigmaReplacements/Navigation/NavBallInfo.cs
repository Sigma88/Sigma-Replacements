using System.Collections.Generic;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Navigation
    {
        internal class NavBallInfo : Info
        {
            // Static
            internal static List<Info> List = new List<Info>();
            internal static List<Info> DataBase = new List<Info>();

            // Colors Lists
            internal List<Color> NavBall = new List<Color>();
            internal List<Color> Shading = new List<Color>();
            internal List<Color> Cursor = new List<Color>();
            internal List<Color> Vectors = new List<Color>();
            internal List<Color> ProgradeVector = new List<Color>();
            internal List<Color> ProgradeWaypoint = new List<Color>();
            internal List<Color> RetrogradeVector = new List<Color>();
            internal List<Color> RetrogradeWaypoint = new List<Color>();
            internal List<Color> Maneuvers = new List<Color>();
            internal List<Color> RadialInVector = new List<Color>();
            internal List<Color> RadialOutVector = new List<Color>();
            internal List<Color> NormalVector = new List<Color>();
            internal List<Color> AntiNormalVector = new List<Color>();
            internal List<Color> BurnVector = new List<Color>();
            internal List<Color> Arrows = new List<Color>();
            internal List<Color> Buttons = new List<Color>();
            internal List<Color> Frame = new List<Color>();

            // Textures Lists
            internal List<Texture> NavBallTex = new List<Texture>();
            internal List<Texture> ShadingTex = new List<Texture>();
            internal List<Texture> CursorTex = new List<Texture>();
            internal List<Texture> VectorsTex = new List<Texture>();
            internal List<Texture> ProgradeVectorTex = new List<Texture>();
            internal List<Texture> ProgradeWaypointTex = new List<Texture>();
            internal List<Texture> RetrogradeVectorTex = new List<Texture>();
            internal List<Texture> RetrogradeWaypointTex = new List<Texture>();
            internal List<Texture> ManeuversTex = new List<Texture>();
            internal List<Texture> RadialInVectorTex = new List<Texture>();
            internal List<Texture> RadialOutVectorTex = new List<Texture>();
            internal List<Texture> NormalVectorTex = new List<Texture>();
            internal List<Texture> AntiNormalVectorTex = new List<Texture>();
            internal List<Texture> BurnVectorTex = new List<Texture>();
            internal List<Texture> ArrowsTex = new List<Texture>();
            internal List<Texture> ButtonsTex = new List<Texture>();
            internal List<Texture> FrameTex = new List<Texture>();


            // New NavBallInfo
            internal NavBallInfo(ConfigNode requirements, ConfigNode info) : base(requirements, info)
            {
                // Parse Requirements
                Parse(requirements, info);

                // Parse NavBallInfo Colors
                NavBall = Parse(info.GetValues("NavBall"), NavBall);
                Shading = Parse(info.GetValues("Shading"), Shading);
                Cursor = Parse(info.GetValues("Cursor"), Cursor);
                Vectors = Parse(info.GetValues("Vectors"), Vectors);
                ProgradeVector = Parse(info.GetValues("ProgradeVector"), ProgradeVector);
                ProgradeWaypoint = Parse(info.GetValues("ProgradeWaypoint"), ProgradeWaypoint);
                RetrogradeVector = Parse(info.GetValues("RetrogradeVector"), RetrogradeVector);
                RetrogradeWaypoint = Parse(info.GetValues("RetrogradeWaypoint"), RetrogradeWaypoint);
                Maneuvers = Parse(info.GetValues("Maneuvers"), Maneuvers);
                RadialInVector = Parse(info.GetValues("RadialInVector"), RadialInVector);
                RadialOutVector = Parse(info.GetValues("RadialOutVector"), RadialOutVector);
                NormalVector = Parse(info.GetValues("NormalVector"), NormalVector);
                AntiNormalVector = Parse(info.GetValues("AntiNormalVector"), AntiNormalVector);
                BurnVector = Parse(info.GetValues("BurnVector"), BurnVector);
                Arrows = Parse(info.GetValues("Arrows"), Arrows);
                Buttons = Parse(info.GetValues("Buttons"), Buttons);
                Frame = Parse(info.GetValues("Frame"), Frame);

                // Parse NavBallInfo Textures
                NavBallTex = Parse(info.GetValues("NavBallTex"), NavBallTex);
                ShadingTex = Parse(info.GetValues("ShadingTex"), ShadingTex);
                CursorTex = Parse(info.GetValues("CursorTex"), CursorTex);
                VectorsTex = Parse(info.GetValues("VectorsTex"), VectorsTex);
                ProgradeVectorTex = Parse(info.GetValues("ProgradeVectorTex"), ProgradeVectorTex);
                ProgradeWaypointTex = Parse(info.GetValues("ProgradeWaypointTex"), ProgradeWaypointTex);
                RetrogradeVectorTex = Parse(info.GetValues("RetrogradeVectorTex"), RetrogradeVectorTex);
                RetrogradeWaypointTex = Parse(info.GetValues("RetrogradeWaypointTex"), RetrogradeWaypointTex);
                ManeuversTex = Parse(info.GetValues("ManeuversTex"), ManeuversTex);
                RadialInVectorTex = Parse(info.GetValues("RadialInVectorTex"), RadialInVectorTex);
                RadialOutVectorTex = Parse(info.GetValues("RadialOutVectorTex"), RadialOutVectorTex);
                NormalVectorTex = Parse(info.GetValues("NormalVectorTex"), NormalVectorTex);
                AntiNormalVectorTex = Parse(info.GetValues("AntiNormalVectorTex"), AntiNormalVectorTex);
                BurnVectorTex = Parse(info.GetValues("BurnVectorTex"), BurnVectorTex);
                ArrowsTex = Parse(info.GetValues("ArrowsTex"), ArrowsTex);
                ButtonsTex = Parse(info.GetValues("ButtonsTex"), ButtonsTex);
                FrameTex = Parse(info.GetValues("FrameTex"), FrameTex);

                // Parse Folders
                ParseFolders(info.GetNode("Folders"));
            }


            // Parsers
            void ParseFolders(ConfigNode node)
            {
                if (node == null) return;

                // Parse Texture Folders
                NavBallTex = ParseFolders(node.GetValues("NavBallTex"), NavBallTex);
                ShadingTex = ParseFolders(node.GetValues("ShadingTex"), ShadingTex);
                CursorTex = ParseFolders(node.GetValues("CursorTex"), CursorTex);
                VectorsTex = ParseFolders(node.GetValues("VectorsTex"), VectorsTex);
                ProgradeVectorTex = ParseFolders(node.GetValues("ProgradeVectorTex"), ProgradeVectorTex);
                ProgradeWaypointTex = ParseFolders(node.GetValues("ProgradeWaypointTex"), ProgradeWaypointTex);
                RetrogradeVectorTex = ParseFolders(node.GetValues("RetrogradeVectorTex"), RetrogradeVectorTex);
                RetrogradeWaypointTex = ParseFolders(node.GetValues("RetrogradeWaypointTex"), RetrogradeWaypointTex);
                ManeuversTex = ParseFolders(node.GetValues("ManeuversTex"), ManeuversTex);
                RadialInVectorTex = ParseFolders(node.GetValues("RadialInVectorTex"), RadialInVectorTex);
                RadialOutVectorTex = ParseFolders(node.GetValues("RadialOutVectorTex"), RadialOutVectorTex);
                NormalVectorTex = ParseFolders(node.GetValues("NormalVectorTex"), NormalVectorTex);
                AntiNormalVectorTex = ParseFolders(node.GetValues("AntiNormalVectorTex"), AntiNormalVectorTex);
                BurnVectorTex = ParseFolders(node.GetValues("BurnVectorTex"), BurnVectorTex);
                ArrowsTex = ParseFolders(node.GetValues("ArrowsTex"), ArrowsTex);
                ButtonsTex = ParseFolders(node.GetValues("ButtonsTex"), ButtonsTex);
                FrameTex = ParseFolders(node.GetValues("FrameTex"), FrameTex);
            }
        }
    }
}
