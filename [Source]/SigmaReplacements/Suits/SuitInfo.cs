using System;
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

            // Suit Specific Requirements
            internal Type? type = null;
            internal float? suitMinPressure = null;
            internal float? suitMaxPressure = null;
            internal float? helmetLowPressure = null;
            internal float? helmetHighPressure = null;
            internal float? helmetDelay = null;
            internal float? jetpackMaxGravity = null;
            internal float? jetpackDelay = null;

            // Colors Lists
            internal List<Color?> body = new List<Color?>();
            internal List<Color?> helmet = new List<Color?>();
            internal List<Color?> visor = new List<Color?>();
            internal List<Color?> flares = new List<Color?>();
            internal List<Color?> light = new List<Color?>();
            internal List<Color?> jetpack = new List<Color?>();
            internal List<Color?> flag = new List<Color?>();
            internal List<Color?> gasjets = new List<Color?>();
            internal List<Color?> headset = new List<Color?>();
            internal List<Color?> mug = new List<Color?>();
            internal List<Color?> glasses = new List<Color?>();
            internal List<Color?> backdrop = new List<Color?>();

            // Textures Lists
            internal List<Texture> bodyTex = new List<Texture>();
            internal List<Texture> helmetTex = new List<Texture>();
            internal List<Texture> visorTex = new List<Texture>();
            internal List<Texture> flaresTex = new List<Texture>();
            internal List<Texture> jetpackTex = new List<Texture>();
            internal List<Texture> flagTex = new List<Texture>();
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
            internal List<Texture> flagNrm = new List<Texture>();
            internal List<Texture> headsetNrm = new List<Texture>();
            internal List<Texture> mugNrm = new List<Texture>();
            internal List<Texture> glassesNrm = new List<Texture>();
            internal List<Texture> backdropNrm = new List<Texture>();


            // New SuitInfo
            internal SuitInfo(ConfigNode requirements, ConfigNode info) : base(requirements, info)
            {
                // Parse Requirements
                Parse(requirements, info);

                // Suit Specific Requirements
                type = Parse(info.GetValue("type"), type);
                suitMinPressure = Parse(info.GetValue("suitMinPressure"), suitMinPressure);
                suitMaxPressure = Parse(info.GetValue("suitMaxPressure"), suitMaxPressure);
                helmetLowPressure = Parse(info.GetValue("helmetLowPressure"), helmetLowPressure);
                helmetHighPressure = Parse(info.GetValue("helmetHighPressure"), helmetHighPressure);
                helmetDelay = Parse(info.GetValue("helmetDelay"), helmetDelay);
                jetpackMaxGravity = Parse(info.GetValue("jetpackMaxGravity"), jetpackMaxGravity);
                jetpackDelay = Parse(info.GetValue("jetpackDelay"), jetpackDelay);

                // Parse SuitInfo Colors
                body = Parse(info.GetValues("body"), body);
                helmet = Parse(info.GetValues("helmet"), helmet);
                visor = Parse(info.GetValues("visor"), visor);
                flares = Parse(info.GetValues("flares"), flares);
                light = Parse(info.GetValues("light"), light);
                jetpack = Parse(info.GetValues("jetpack"), jetpack);
                flag = Parse(info.GetValues("flag"), flag);
                gasjets = Parse(info.GetValues("gasjets"), gasjets);
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
                flagTex = Parse(info.GetValues("flagTex"), flagTex);
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
                flagNrm = Parse(info.GetValues("flagNrm"), flagNrm);
                headsetNrm = Parse(info.GetValues("headsetNrm"), headsetNrm);
                mugNrm = Parse(info.GetValues("mugNrm"), mugNrm);
                glassesNrm = Parse(info.GetValues("glassesNrm"), glassesNrm);
                backdropNrm = Parse(info.GetValues("backdropNrm"), backdropNrm);

                // Parse Folders
                ParseFolders(info.GetNode("Folders"));
            }


            // Parsers
            internal Type? Parse(string s, Type? defaultValue)
            {
                try { return (Type)Enum.Parse(typeof(Type), s); }
                catch { return defaultValue; }
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
                flagTex = ParseFolders(node.GetValues("flagTex"), flagTex);
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
                flagNrm = ParseFolders(node.GetValues("flagNrm"), flagNrm);
                headsetNrm = ParseFolders(node.GetValues("headsetNrm"), headsetNrm);
                mugNrm = ParseFolders(node.GetValues("mugNrm"), mugNrm);
                glassesNrm = ParseFolders(node.GetValues("glassesNrm"), glassesNrm);
                backdropNrm = ParseFolders(node.GetValues("backdropNrm"), backdropNrm);
            }
        }
    }
}
