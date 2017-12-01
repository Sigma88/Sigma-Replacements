using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Gender = ProtoCrewMember.Gender;
using Type = ProtoCrewMember.KerbalType;
using Roster = ProtoCrewMember.RosterStatus;


namespace SigmaReplacements
{
    public enum Status
    {
        Crew = 0,
        Applicant = 1,
        Unowned = 2,
        Tourist = 3,
        Available = 4,
        Assigned = 5,
        Dead = 6,
        Missing = 7
    }

    internal class Info
    {
        // Static
        internal static string hash = "";

        // Identifiers
        internal string name = null;

        // Requirements
        internal bool useGameSeed = false;
        internal float useChance = 1;
        internal Status? status = null;
        internal Gender? gender = null;
        internal string[] trait = null;
        internal bool? veteran = null;
        internal bool? isBadass = null;
        int minLevel = 0;
        int maxLevel = 5;
        float minCourage = 0;
        float maxCourage = 1;
        float minStupidity = 0;
        float maxStupidity = 1;
        // For MainMenuKerbals
        internal float? courage = null;
        internal float? stupidity = null;
        internal int? experienceLevel = null;

        // Collection
        internal string collection = "";

        // Get
        internal Info GetFor(ProtoCrewMember kerbal)
        {
            if (name == null || name == kerbal.name)
            {
                Debug.Log(GetType().Name + ".GetFor", "Matched name = " + name + " to kerbal name = " + kerbal.name);
                if (status == null || (Type)status == kerbal.type || (int?)status > 3 && kerbal.type == 0)
                {
                    Debug.Log(GetType().Name + ".GetFor", "Matched status = " + status + " to kerbal type = " + kerbal.type);
                    if (!((int?)status > 3 && kerbal.type == 0 && (int?)status - 4 != (int)kerbal.rosterStatus))
                    {
                        if ((int?)status > 3)
                            Debug.Log(GetType().Name + ".GetFor", "Matched status = " + status + " to kerbal rosterStatus = " + kerbal.rosterStatus);

                        if (gender == null || gender == kerbal.gender)
                        {
                            Debug.Log(GetType().Name + ".GetFor", "Matched gender = " + gender + " to kerbal gender = " + kerbal.gender);
                            if (trait == null || trait.Contains(kerbal.trait))
                            {
                                Debug.Log(GetType().Name + ".GetFor", "Matched trait = " + trait + " to kerbal trait = " + kerbal.trait);
                                if (veteran == null || veteran == kerbal.veteran)
                                {
                                    Debug.Log(GetType().Name + ".GetFor", "Matched veteran = " + veteran + " to kerbal veteran = " + kerbal.veteran);
                                    if (isBadass == null || isBadass == kerbal.isBadass)
                                    {
                                        Debug.Log(GetType().Name + ".GetFor", "Matched isBadass = " + isBadass + " to kerbal isBadass = " + kerbal.isBadass);
                                        if (minLevel <= kerbal.experienceLevel && maxLevel >= kerbal.experienceLevel)
                                        {
                                            Debug.Log(GetType().Name + ".GetFor", "Matched minLevel = " + minLevel + ", maxLevel = " + maxLevel + " to kerbal level = " + kerbal.experienceLevel);
                                            if (minCourage <= kerbal.courage && maxCourage >= kerbal.courage)
                                            {
                                                Debug.Log(GetType().Name + ".GetFor", "Matched minCourage = " + minCourage + ", maxCourage = " + maxCourage + " to kerbal courage = " + kerbal.courage);
                                                if (minStupidity <= kerbal.stupidity && maxStupidity >= kerbal.stupidity)
                                                {
                                                    Debug.Log(GetType().Name + ".GetFor", "Matched minStupidity = " + minStupidity + ", maxStupidity = " + maxStupidity + " to kerbal stupidity = " + kerbal.stupidity);
                                                    Debug.Log(GetType().Name + ".GetFor", "Return this Info");
                                                    return this;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Debug.Log(GetType().Name + ".GetFor", "Return null");
            return null;
        }


        // New Info
        internal Info(ConfigNode requirements, ConfigNode info)
        {
            Parse(requirements, info);
        }

        // Parse from ConfigNode
        internal void Parse(ConfigNode requirements, ConfigNode info)
        {
            Debug.Log("Info", "new Info from:");
            Debug.Log("Info", "Requirements node = " + requirements);
            Debug.Log("Info", "Info node = " + info);

            // Parse Info Requirements
            useGameSeed = Parse(requirements.GetValue("useGameSeed"), useGameSeed);
            useChance = Parse(requirements.GetValue("useChance"), useChance);
            name = requirements.GetValue("name");
            status = Parse(requirements.GetValue("status"), status);
            gender = Parse(requirements.GetValue("gender"), gender);
            trait = requirements.HasValue("trait") ? requirements.GetValues("trait") : null;
            veteran = Parse(requirements.GetValue("veteran"), veteran);
            isBadass = Parse(requirements.GetValue("isBadass"), isBadass);
            minLevel = Parse(requirements.GetValue("minLevel"), minLevel);
            maxLevel = Parse(requirements.GetValue("maxLevel"), maxLevel);
            minCourage = Parse(requirements.GetValue("minCourage"), minCourage);
            maxCourage = Parse(requirements.GetValue("maxCourage"), maxCourage);
            minStupidity = Parse(requirements.GetValue("minStupidity"), minStupidity);
            maxStupidity = Parse(requirements.GetValue("maxStupidity"), maxStupidity);
            // For MainMenuKerbals
            experienceLevel = Parse(requirements.GetValue("level"), experienceLevel);
            courage = Parse(requirements.GetValue("courage"), courage);
            stupidity = Parse(requirements.GetValue("stupidity"), stupidity);


            // Parse Collection
            collection = info.GetValue("collection");
        }


        // Parsers
        internal float Parse(string s, float defaultValue) { return float.TryParse(s, out float f) ? f : defaultValue; }
        internal float? Parse(string s, float? defaultValue) { return float.TryParse(s, out float f) ? f : defaultValue; }
        internal bool Parse(string s, bool defaultValue) { return bool.TryParse(s, out bool b) ? b : defaultValue; }
        internal bool? Parse(string s, bool? defaultValue) { return bool.TryParse(s, out bool b) ? b : defaultValue; }
        internal int Parse(string s, int defaultValue) { return int.TryParse(s, out int b) ? b : defaultValue; }
        internal int? Parse(string s, int? defaultValue) { return int.TryParse(s, out int b) ? b : defaultValue; }

        internal Status? Parse(string s, Status? defaultValue)
        {
            try { return (Status)Enum.Parse(typeof(Status), s); }
            catch { return defaultValue; }
        }

        internal Roster Parse(string s, Roster defaultValue)
        {
            try { return (Roster)Enum.Parse(typeof(Roster), s); }
            catch { return defaultValue; }
        }

        internal Gender? Parse(string s, Gender? defaultValue)
        {
            try { return (Gender)Enum.Parse(typeof(Gender), s); }
            catch { return defaultValue; }
        }

        internal Color? Parse(string s, Color? defaultValue)
        {
            try { return ConfigNode.ParseColor(s); }
            catch { return defaultValue; }
        }

        internal Texture Parse(string s, Texture defaultValue)
        {
            return Resources.FindObjectsOfTypeAll<Texture>().FirstOrDefault(t => t.name == s) ?? defaultValue;
        }

        internal Vector2? Parse(string s, Vector2? defaultValue)
        {
            try { return ConfigNode.ParseVector2(s); }
            catch { return defaultValue; }
        }

        internal List<Color?> Parse(string[] s, List<Color?> defaultValue)
        {
            for (int i = 0; i < s.Length; i++)
            {
                Color? col = null;
                defaultValue.Add(Parse(s[i], col));
            }
            return defaultValue;
        }

        internal List<Texture> Parse(string[] s, List<Texture> defaultValue)
        {
            for (int i = 0; i < s.Length; i++)
            {
                Texture tex = null;
                defaultValue.Add(Parse(s[i], tex));
            }
            return defaultValue;
        }

        internal List<Vector2?> Parse(string[] s, List<Vector2?> defaultValue)
        {
            for (int i = 0; i < s.Length; i++)
            {
                Vector2? v = null;
                defaultValue.Add(Parse(s[i], v));
            }
            return defaultValue;
        }

        // Parse Folders

        internal List<Texture> ParseFolders(string[] paths, List<Texture> list)
        {
            for (int i = 0; i < paths?.Length; i++)
            {
                list.AddUniqueRange(ParseFolder(paths[i]));
            }

            return list;
        }

        internal List<Texture> ParseFolder(string path)
        {
            if (!path.EndsWith("/")) path += "/";

            Texture[] textures = Resources.FindObjectsOfTypeAll<Texture>();
            List<Texture> list = new List<Texture>();

            if (Directory.Exists("GameData/" + path))
            {
                string[] files = Directory.GetFiles("GameData/" + path)?.Where(f => Path.GetExtension(f) == ".dds" || Path.GetExtension(f) == ".png")?.ToArray();

                for (int i = 0; i < files?.Length; i++)
                {
                    if (!string.IsNullOrEmpty(files[i]))
                    {
                        string name = path + Path.GetFileNameWithoutExtension(files[i]);
                        Texture texture = textures.FirstOrDefault(t => t?.name == name);
                        list.Add(texture);
                    }
                }
            }

            return list;
        }
    }
}
