using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace SkyBox
    {
        internal enum Mode
        {
            SANDBOX = 0,
            Sandbox = 0,
            CAREER = 1,
            Career = 1,
            SCENARIO = 2,
            SCENARIO_NON_RESUMABLE = 3,
            SCIENCE_SANDBOX = 4,
            Science_Sandbox = 4,
            SCIENCE = 4,
            Science = 4,
            MAINMENU = 5,
            MainMenu = 5
        }

        internal class SkyBoxInfo : Info
        {
            // CubeMap sides
            static string[] sides = new[] { "XP", "XN", "YP", "YN", "ZP", "ZN" };

            // Static
            internal static List<Info> List = new List<Info>();
            internal static List<Info> DataBase = new List<Info>();

            // SkyBox Specific Restrictions
            List<Mode> mode = new List<Mode>();

            // SkyBox Settings
            internal bool? rotate = null;
            internal bool? mirror = null;

            // Textures Lists
            internal List<Texture[]> SkyBox = new List<Texture[]>();


            // Get
            internal SkyBoxInfo GetFor(Mode? gameMode)
            {
                if (gameMode != null)
                {
                    Debug.Log("SkyBoxInfo.GetFor", "Mode = " + gameMode + ", SkyBoxInfo mode count = " + mode?.Count);

                    if (!(mode?.Count > 0) || mode.Contains((Mode)gameMode))
                    {
                        Debug.Log(GetType().Name + ".GetFor", "Matched mode list to game mode = " + gameMode);
                        Debug.Log(GetType().Name + ".GetFor", "Return this " + GetType().Name);
                        return this;
                    }
                }

                Debug.Log(GetType().Name + ".GetFor", "Return null");
                return null;
            }

            // New SkyBoxInfo
            internal SkyBoxInfo(ConfigNode requirements, ConfigNode info) : base(requirements, info)
            {
                // Parse Requirements
                Parse(requirements, info);

                // SkyBox Specific Requirements
                mode = Parse(requirements.GetValues("mode"), mode);

                // Parse Settings
                rotate = Parse(info.GetValue("rotate"), rotate);
                mirror = Parse(info.GetValue("mirror"), mirror);

                // Parse Textures
                SkyBox = Parse(info.GetValues("SkyBox"), SkyBox);

                // Parse Folders
                SkyBox = ParseFolders(info?.GetNode("Folders")?.GetValues("SkyBox"), SkyBox);
            }


            // SkyBox Specific Parsers
            internal List<Mode> Parse(string[] s, List<Mode> defaultValue)
            {
                if (defaultValue == null) return null;

                for (int i = 0; i < s?.Length; i++)
                {
                    Mode? mode = Parse(s[i], (Mode?)null);
                    if (mode != null && defaultValue?.Contains((Mode)mode) == false)
                        defaultValue.Add((Mode)mode);
                }

                return defaultValue;
            }

            internal Mode? Parse(string s, Mode? defaultValue)
            {
                try { return (Mode)Enum.Parse(typeof(Mode), s); }
                catch { return defaultValue; }
            }

            internal List<Texture[]> Parse(string[] s, List<Texture[]> defaultValue)
            {
                defaultValue = defaultValue ?? new List<Texture[]>();

                for (int i = 0; i < s?.Length; i++)
                {
                    Texture[] SkyBox = Parse(s[i], (Texture[])null);
                    if (SkyBox != null)
                        defaultValue.Add(SkyBox);
                }

                return defaultValue;
            }

            internal Texture[] Parse(string s, Texture[] defaultValue)
            {
                Texture[] SkyBox = new Texture[] { null, null, null, null, null, null };

                for (int i = 0; i < sides?.Length; i++)
                {
                    SkyBox[i] = Parse(s + sides[i], SkyBox[i]);
                }

                return SkyBox.Contains(null) ? defaultValue : SkyBox;
            }


            // Parse Folders
            List<Texture[]> ParseFolders(string[] paths, List<Texture[]> list)
            {
                if (paths == null) return list;
                list = list ?? new List<Texture[]>();

                for (int i = 0; i < paths?.Length; i++)
                {
                    list = ParseFolder(paths[i], list);
                }

                return list;
            }

            List<Texture[]> ParseFolder(string path, List<Texture[]> list)
            {
                if (string.IsNullOrEmpty(path)) return list;
                list = list ?? new List<Texture[]>();

                List<Texture> files = ParseFolder(path);
                List<string> names = new List<string>();

                for (int i = 0; i < files?.Count; i++)
                {
                    string name = files[i]?.name;

                    if (name?.Length > 3 && sides.Contains(name.Substring(name.Length - 2)))
                    {
                        name = name.TrimEnd(new[] { 'P', 'N', 'X', 'Y', 'Z' });
                        names.AddUnique(name);
                    }
                }

                list = Parse(names.ToArray(), list);

                return list;
            }
        }
    }
}