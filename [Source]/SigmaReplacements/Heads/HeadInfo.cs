using System.Collections.Generic;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Heads
    {
        internal class HeadInfo : Info
        {
            // Static
            internal static List<Info> List = new List<Info>();
            internal static List<Info> DataBase = new List<Info>();

            // Colors Lists
            internal List<Color?> pupilLeft = new List<Color?>();
            internal List<Color?> pupilRight = new List<Color?>();
            internal List<Color?> eyeballLeft = new List<Color?>();
            internal List<Color?> eyeballRight = new List<Color?>();
            internal List<Color?> upTeeth01 = new List<Color?>();
            internal List<Color?> upTeeth02 = new List<Color?>();
            internal List<Color?> tongue = new List<Color?>();
            internal List<Color?> head = new List<Color?>();
            internal List<Color?> hair = new List<Color?>();
            internal List<Color?> arm = new List<Color?>();

            // Textures Lists
            internal List<Texture> pupilLeftTex = new List<Texture>();
            internal List<Texture> pupilRightTex = new List<Texture>();
            internal List<Texture> eyeballLeftTex = new List<Texture>();
            internal List<Texture> eyeballRightTex = new List<Texture>();
            internal List<Texture> upTeeth01Tex = new List<Texture>();
            internal List<Texture> upTeeth02Tex = new List<Texture>();
            internal List<Texture> tongueTex = new List<Texture>();
            internal List<Texture> headTex = new List<Texture>();
            internal List<Texture> hairTex = new List<Texture>();
            internal List<Texture> armTex = new List<Texture>();

            // Normals Lists
            internal List<Texture> pupilLeftNrm = new List<Texture>();
            internal List<Texture> pupilRightNrm = new List<Texture>();
            internal List<Texture> eyeballLeftNrm = new List<Texture>();
            internal List<Texture> eyeballRightNrm = new List<Texture>();
            internal List<Texture> upTeeth01Nrm = new List<Texture>();
            internal List<Texture> upTeeth02Nrm = new List<Texture>();
            internal List<Texture> tongueNrm = new List<Texture>();
            internal List<Texture> headNrm = new List<Texture>();
            internal List<Texture> hairNrm = new List<Texture>();
            internal List<Texture> armNrm = new List<Texture>();


            // New HeadInfo
            internal HeadInfo(ConfigNode requirements, ConfigNode info) : base(requirements, info)
            {
                // Parse Requirements
                Parse(requirements, info);

                // Parse HeadInfo Colors
                pupilLeft = Parse(info.GetValues("pupilLeft"), pupilLeft);
                pupilRight = Parse(info.GetValues("pupilRight"), pupilRight);
                eyeballLeft = Parse(info.GetValues("eyeballLeft"), eyeballLeft);
                eyeballRight = Parse(info.GetValues("eyeballRight"), eyeballRight);
                upTeeth01 = Parse(info.GetValues("upTeeth01"), upTeeth01);
                upTeeth02 = Parse(info.GetValues("upTeeth02"), upTeeth02);
                tongue = Parse(info.GetValues("tongue"), tongue);
                head = Parse(info.GetValues("head"), head);
                hair = Parse(info.GetValues("hair"), hair);
                arm = Parse(info.GetValues("arm"), arm);

                // Parse HeadInfo Textures
                pupilLeftTex = Parse(info.GetValues("pupilLeftTex"), pupilLeftTex);
                pupilRightTex = Parse(info.GetValues("pupilRightTex"), pupilRightTex);
                eyeballLeftTex = Parse(info.GetValues("eyeballLeftTex"), eyeballLeftTex);
                eyeballRightTex = Parse(info.GetValues("eyeballRightTex"), eyeballRightTex);
                upTeeth01Tex = Parse(info.GetValues("upTeeth01Tex"), upTeeth01Tex);
                upTeeth02Tex = Parse(info.GetValues("upTeeth02Tex"), upTeeth02Tex);
                tongueTex = Parse(info.GetValues("tongueTex"), tongueTex);
                headTex = Parse(info.GetValues("headTex"), headTex);
                hairTex = Parse(info.GetValues("hairTex"), hairTex);
                armTex = Parse(info.GetValues("armTex"), armTex);

                // Parse HeadInfo Normals
                pupilLeftNrm = Parse(info.GetValues("pupilLeftNrm"), pupilLeftNrm);
                pupilRightNrm = Parse(info.GetValues("pupilRightNrm"), pupilRightNrm);
                eyeballLeftNrm = Parse(info.GetValues("eyeballLeftNrm"), eyeballLeftNrm);
                eyeballRightNrm = Parse(info.GetValues("eyeballRightNrm"), eyeballRightNrm);
                upTeeth01Nrm = Parse(info.GetValues("upTeeth01Nrm"), upTeeth01Nrm);
                upTeeth02Nrm = Parse(info.GetValues("upTeeth02Nrm"), upTeeth02Nrm);
                tongueNrm = Parse(info.GetValues("tongueNrm"), tongueNrm);
                headNrm = Parse(info.GetValues("headNrm"), headNrm);
                hairNrm = Parse(info.GetValues("hairNrm"), hairNrm);
                armNrm = Parse(info.GetValues("armNrm"), armNrm);

                // Parse Folders
                ParseFolders(info.GetNode("Folders"));
            }

            void ParseFolders(ConfigNode node)
            {
                if (node == null) return;

                // Parse Texture Folders
                pupilLeftTex = ParseFolders(node.GetValues("pupilLeftTex"), pupilLeftTex);
                pupilRightTex = ParseFolders(node.GetValues("pupilRightTex"), pupilRightTex);
                eyeballLeftTex = ParseFolders(node.GetValues("eyeballLeftTex"), eyeballLeftTex);
                eyeballRightTex = ParseFolders(node.GetValues("eyeballRightTex"), eyeballRightTex);
                upTeeth01Tex = ParseFolders(node.GetValues("upTeeth01Tex"), upTeeth01Tex);
                upTeeth02Tex = ParseFolders(node.GetValues("upTeeth02Tex"), upTeeth02Tex);
                tongueTex = ParseFolders(node.GetValues("tongueTex"), tongueTex);
                headTex = ParseFolders(node.GetValues("headTex"), headTex);
                hairTex = ParseFolders(node.GetValues("hairTex"), hairTex);

                // Parse Normal Folders
                pupilLeftNrm = ParseFolders(node.GetValues("pupilLeftNrm"), pupilLeftNrm);
                pupilRightNrm = ParseFolders(node.GetValues("pupilRightNrm"), pupilRightNrm);
                eyeballLeftNrm = ParseFolders(node.GetValues("eyeballLeftNrm"), eyeballLeftNrm);
                eyeballRightNrm = ParseFolders(node.GetValues("eyeballRightNrm"), eyeballRightNrm);
                upTeeth01Nrm = ParseFolders(node.GetValues("upTeeth01Nrm"), upTeeth01Nrm);
                upTeeth02Nrm = ParseFolders(node.GetValues("upTeeth02Nrm"), upTeeth02Nrm);
                tongueNrm = ParseFolders(node.GetValues("tongueNrm"), tongueNrm);
                headNrm = ParseFolders(node.GetValues("headNrm"), headNrm);
                hairNrm = ParseFolders(node.GetValues("hairNrm"), hairNrm);
            }
        }
    }
}
