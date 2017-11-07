using System.Collections.Generic;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Suits
    {
        internal class SuitInfo : Info
        {
            // Static
            internal static List<Info> List = new List<Info>();
            internal static List<Info> DataBase = new List<Info>();

            // Colors Lists
            internal List<Color> body = new List<Color>();
            internal List<Color> helmet = new List<Color>();
            internal List<Color> visor = new List<Color>();
            internal List<Color> flares = new List<Color>();
            internal List<Color> jetpack = new List<Color>();
            internal List<Color> headset = new List<Color>();
            internal List<Color> mug = new List<Color>();
            internal List<Color> glasses = new List<Color>();
            internal List<Color> backdrop = new List<Color>();

            // Textures Lists
            internal List<Texture> bodyTex = new List<Texture>();
            internal List<Texture> helmetTex = new List<Texture>();
            internal List<Texture> visorTex = new List<Texture>();
            internal List<Texture> flaresTex = new List<Texture>();
            internal List<Texture> jetpackTex = new List<Texture>();
            internal List<Texture> gasjetsTex = new List<Texture>();
            internal List<Texture> headsetTex = new List<Texture>();
            internal List<Texture> mugTex = new List<Texture>();
            internal List<Texture> glassesTex = new List<Texture>();
            internal List<Texture> backdropTex = new List<Texture>();

            // Normals Lists
            internal List<Texture> bodyNrm = new List<Texture>();
            internal List<Texture> helmetNrm = new List<Texture>();
            internal List<Texture> visorNrm = new List<Texture>();
            internal List<Texture> jetpackNrm = new List<Texture>();
            internal List<Texture> headsetNrm = new List<Texture>();
            internal List<Texture> mugNrm = new List<Texture>();
            internal List<Texture> glassesNrm = new List<Texture>();
            internal List<Texture> backdropNrm = new List<Texture>();


            // New SuitInfo
            internal SuitInfo(ConfigNode requirements, ConfigNode info) : base(requirements, info)
            {
                // Parse Requirements
                Parse(requirements, info);

                // Parse SuitInfo Colors
                body = Parse(info.GetValues("body"), body);
                helmet = Parse(info.GetValues("helmet"), helmet);
                visor = Parse(info.GetValues("visor"), visor);
                flares = Parse(info.GetValues("flares"), flares);
                jetpack = Parse(info.GetValues("jetpack"), jetpack);
                headset = Parse(info.GetValues("headset"), headset);
                mug = Parse(info.GetValues("mug"), mug);
                glasses = Parse(info.GetValues("glasses"), glasses);
                backdrop = Parse(info.GetValues("backdrop"), backdrop);

                // Parse SuitInfo Textures
                bodyTex = Parse(info.GetValues("bodyTex"), bodyTex);
                helmetTex = Parse(info.GetValues("helmetTex"), helmetTex);
                visorTex = Parse(info.GetValues("visorTex"), visorTex);
                flaresTex = Parse(info.GetValues("flaresTex"), flaresTex);
                jetpackTex = Parse(info.GetValues("jetpackTex"), jetpackTex);
                gasjetsTex = Parse(info.GetValues("gasjetsTex"), gasjetsTex);
                headsetTex = Parse(info.GetValues("headsetTex"), headsetTex);
                mugTex = Parse(info.GetValues("mugTex"), mugTex);
                glassesTex = Parse(info.GetValues("glassesTex"), glassesTex);
                backdropTex = Parse(info.GetValues("backdropTex"), backdropTex);

                // Parse SuitInfo Normals
                bodyNrm = Parse(info.GetValues("bodyNrm"), bodyNrm);
                helmetNrm = Parse(info.GetValues("helmetNrm"), helmetNrm);
                visorNrm = Parse(info.GetValues("visorNrm"), visorNrm);
                jetpackNrm = Parse(info.GetValues("jetpackNrm"), jetpackNrm);
                headsetNrm = Parse(info.GetValues("headsetNrm"), headsetNrm);
                mugNrm = Parse(info.GetValues("mugNrm"), mugNrm);
                glassesNrm = Parse(info.GetValues("glassesNrm"), glassesNrm);
                backdropNrm = Parse(info.GetValues("backdropNrm"), backdropNrm);

                // Parse Folders
                ParseFolders(info.GetNode("Folders"));
            }

            void ParseFolders(ConfigNode node)
            {
                if (node == null) return;

                // Parse Texture Folders
                bodyTex = ParseFolders(node.GetValues("bodyTex"), bodyTex);
                helmetTex = ParseFolders(node.GetValues("helmetTex"), helmetTex);
                visorTex = ParseFolders(node.GetValues("visorTex"), visorTex);
                flaresTex = ParseFolders(node.GetValues("flaresTex"), flaresTex);
                jetpackTex = ParseFolders(node.GetValues("jetpackTex"), jetpackTex);
                gasjetsTex = ParseFolders(node.GetValues("gasjetsTex"), gasjetsTex);
                headsetTex = ParseFolders(node.GetValues("headsetTex"), headsetTex);
                mugTex = ParseFolders(node.GetValues("mugTex"), mugTex);
                glassesTex = ParseFolders(node.GetValues("glassesTex"), glassesTex);
                backdropTex = ParseFolders(node.GetValues("backdropTex"), backdropTex);

                // Parse Normal Folders
                bodyNrm = ParseFolders(node.GetValues("bodyNrm"), bodyNrm);
                helmetNrm = ParseFolders(node.GetValues("helmetNrm"), helmetNrm);
                visorNrm = ParseFolders(node.GetValues("visorNrm"), visorNrm);
                jetpackNrm = ParseFolders(node.GetValues("jetpackNrm"), jetpackNrm);
                headsetNrm = ParseFolders(node.GetValues("headsetNrm"), headsetNrm);
                mugNrm = ParseFolders(node.GetValues("mugNrm"), mugNrm);
                glassesNrm = ParseFolders(node.GetValues("glassesNrm"), glassesNrm);
                backdropNrm = ParseFolders(node.GetValues("backdropNrm"), backdropNrm);
            }
        }
    }
}
