using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Gender = ProtoCrewMember.Gender;
using Type = ProtoCrewMember.KerbalType;


namespace SigmaReplacements
{
    namespace Heads
    {
        internal class HeadInfo
        {
            // Static
            internal static string hash = "";
            internal static List<HeadInfo> List = new List<HeadInfo>();
            internal static List<HeadInfo> DataBase = new List<HeadInfo>();

            // Identifiers
            internal string name = null;

            // Requirements
            internal bool useGameSeed = false;
            internal float useChance = 1;
            Type? rosterStatus = null;
            Gender? gender = null;
            string[] trait = null;
            bool? veteran = null;
            bool? isBadass = null;
            int minLevel = 0;
            int maxLevel = 5;
            float minCourage = 0;
            float maxCourage = 1;
            float minStupidity = 0;
            float maxStupidity = 1;

            // Colors Lists
            internal List<Color> pupilLeft = new List<Color>();
            internal List<Color> pupilRight = new List<Color>();
            internal List<Color> eyeballLeft = new List<Color>();
            internal List<Color> eyeballRight = new List<Color>();
            internal List<Color> upTeeth01 = new List<Color>();
            internal List<Color> upTeeth02 = new List<Color>();
            internal List<Color> head = new List<Color>();
            internal List<Color> hair = new List<Color>();

            // Textures Lists
            internal List<Texture> pupilLeftTex = new List<Texture>();
            internal List<Texture> pupilRightTex = new List<Texture>();
            internal List<Texture> eyeballLeftTex = new List<Texture>();
            internal List<Texture> eyeballRightTex = new List<Texture>();
            internal List<Texture> upTeeth01Tex = new List<Texture>();
            internal List<Texture> upTeeth02Tex = new List<Texture>();
            internal List<Texture> headTex = new List<Texture>();
            internal List<Texture> hairTex = new List<Texture>();

            // Normals Lists
            internal List<Texture> pupilLeftNrm = new List<Texture>();
            internal List<Texture> pupilRightNrm = new List<Texture>();
            internal List<Texture> eyeballLeftNrm = new List<Texture>();
            internal List<Texture> eyeballRightNrm = new List<Texture>();
            internal List<Texture> upTeeth01Nrm = new List<Texture>();
            internal List<Texture> upTeeth02Nrm = new List<Texture>();
            internal List<Texture> headNrm = new List<Texture>();
            internal List<Texture> hairNrm = new List<Texture>();


            // Get
            internal HeadInfo GetFor(ProtoCrewMember kerbal)
            {
                if (name == null || name == kerbal.name)
                {
                    Debug.Log("HeadInfo.GetFor", "Matched name = " + name + " to kerbal name = " + kerbal.name);
                    if (rosterStatus == null || rosterStatus == kerbal.type)
                    {
                        Debug.Log("HeadInfo.GetFor", "Matched rosterStatus = " + rosterStatus + " to kerbal rosterStatus = " + kerbal.type);
                        if (gender == null || gender == kerbal.gender)
                        {
                            Debug.Log("HeadInfo.GetFor", "Matched gender = " + gender + " to kerbal gender = " + kerbal.gender);
                            if (trait == null || trait.Contains(kerbal.trait))
                            {
                                Debug.Log("HeadInfo.GetFor", "Matched trait = " + trait + " to kerbal trait = " + kerbal.trait);
                                if (veteran == null || veteran == kerbal.veteran)
                                {
                                    Debug.Log("HeadInfo.GetFor", "Matched veteran = " + veteran + " to kerbal veteran = " + kerbal.veteran);
                                    if (isBadass == null || isBadass == kerbal.isBadass)
                                    {
                                        Debug.Log("HeadInfo.GetFor", "Matched isBadass = " + isBadass + " to kerbal isBadass = " + kerbal.isBadass);
                                        if (minLevel <= kerbal.experienceLevel && maxLevel >= kerbal.experienceLevel)
                                        {
                                            Debug.Log("HeadInfo.GetFor", "Matched minLevel = " + minLevel + ", maxLevel = " + maxLevel + " to kerbal level = " + kerbal.experienceLevel);
                                            if (minCourage <= kerbal.courage && maxCourage >= kerbal.courage)
                                            {
                                                Debug.Log("HeadInfo.GetFor", "Matched minCourage = " + minCourage + ", maxCourage = " + maxCourage + " to kerbal courage = " + kerbal.courage);
                                                if (minStupidity <= kerbal.stupidity && maxStupidity >= kerbal.stupidity)
                                                {
                                                    Debug.Log("HeadInfo.GetFor", "Matched minStupidity = " + minStupidity + ", maxStupidity = " + maxStupidity + " to kerbal stupidity = " + kerbal.stupidity);
                                                    Debug.Log("Information.GetText", "Return this HeadInfo");
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

                Debug.Log("Information.GetText", "Return null");
                return null;
            }


            // New HeadInfo
            internal HeadInfo(ConfigNode requirements, ConfigNode info)
            {
                // Parse HeadInfo Requirements
                useGameSeed = Parse(requirements.GetValue("useGameSeed"), useGameSeed);
                useChance = Parse(requirements.GetValue("useChance"), useChance);
                name = requirements.GetValue("name");
                rosterStatus = Parse(requirements.GetValue("rosterStatus"), rosterStatus);
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


                // Parse HeadInfo Colors
                pupilLeft = Parse(info.GetValues("pupilLeft"), pupilLeft);
                pupilRight = Parse(info.GetValues("pupilRight"), pupilRight);
                eyeballLeft = Parse(info.GetValues("eyeballLeft"), eyeballLeft);
                eyeballRight = Parse(info.GetValues("eyeballRight"), eyeballRight);
                upTeeth01 = Parse(info.GetValues("upTeeth01"), upTeeth01);
                upTeeth02 = Parse(info.GetValues("upTeeth02"), upTeeth02);
                head = Parse(info.GetValues("head"), head);
                hair = Parse(info.GetValues("hair"), hair);

                // Parse HeadInfo Textures
                pupilLeftTex = Parse(info.GetValues("pupilLeftTex"), pupilLeftTex);
                pupilRightTex = Parse(info.GetValues("pupilRightTex"), pupilRightTex);
                eyeballLeftTex = Parse(info.GetValues("eyeballLeftTex"), eyeballLeftTex);
                eyeballRightTex = Parse(info.GetValues("eyeballRightTex"), eyeballRightTex);
                upTeeth01Tex = Parse(info.GetValues("upTeeth01Tex"), upTeeth01Tex);
                upTeeth02Tex = Parse(info.GetValues("upTeeth02Tex"), upTeeth02Tex);
                headTex = Parse(info.GetValues("headTex"), headTex);
                hairTex = Parse(info.GetValues("hairTex"), hairTex);

                // Parse HeadInfo Normals
                pupilLeftNrm = Parse(info.GetValues("pupilLeftNrm"), pupilLeftNrm);
                pupilRightNrm = Parse(info.GetValues("pupilRightNrm"), pupilRightNrm);
                eyeballLeftNrm = Parse(info.GetValues("eyeballLeftNrm"), eyeballLeftNrm);
                eyeballRightNrm = Parse(info.GetValues("eyeballRightNrm"), eyeballRightNrm);
                upTeeth01Nrm = Parse(info.GetValues("upTeeth01Nrm"), upTeeth01Nrm);
                upTeeth02Nrm = Parse(info.GetValues("upTeeth02Nrm"), upTeeth02Nrm);
                headNrm = Parse(info.GetValues("headNrm"), headNrm);
                hairNrm = Parse(info.GetValues("hairNrm"), hairNrm);
            }


            // Parsers
            float Parse(string s, float defaultValue) { return float.TryParse(s, out float f) ? f : defaultValue; }
            float? Parse(string s, float? defaultValue) { return float.TryParse(s, out float f) ? f : defaultValue; }
            bool Parse(string s, bool defaultValue) { return bool.TryParse(s, out bool b) ? b : defaultValue; }
            bool? Parse(string s, bool? defaultValue) { return bool.TryParse(s, out bool b) ? b : defaultValue; }
            int Parse(string s, int defaultValue) { return int.TryParse(s, out int b) ? b : defaultValue; }
            int? Parse(string s, int? defaultValue) { return int.TryParse(s, out int b) ? b : defaultValue; }

            Type? Parse(string s, Type? defaultValue)
            {
                try { return (Type)Enum.Parse(typeof(Type), s); }
                catch { return defaultValue; }
            }

            Gender? Parse(string s, Gender? defaultValue)
            {
                try { return (Gender)Enum.Parse(typeof(Gender), s); }
                catch { return defaultValue; }
            }

            Texture Parse(string s, Texture defaultValue)
            {
                return Resources.FindObjectsOfTypeAll<Texture>().FirstOrDefault(t => t.name == s) ?? defaultValue;
            }

            List<Texture> Parse(string[] s, List<Texture> defaultValue)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    Texture tex = null;
                    tex = Parse(s[i], tex);
                    if (tex != null && !defaultValue.Contains(tex))
                        defaultValue.Add(tex);
                }
                return defaultValue;
            }

            Color? Parse(string s, Color? defaultValue)
            {
                try { return ConfigNode.ParseColor(s); }
                catch { return defaultValue; }
            }

            List<Color> Parse(string[] s, List<Color> defaultValue)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    Color? col = null;
                    col = Parse(s[i], col);
                    if (!defaultValue.Contains((Color)col))
                        defaultValue.Add((Color)col);
                }

                return defaultValue;
            }


            // Order DataBase
            internal static void OrderDB()
            {
                DataBase.AddRange(List.Where(h => !string.IsNullOrEmpty(h?.name)));
                DataBase.AddRange(List.Where(h => h != null && string.IsNullOrEmpty(h?.name)));
            }
        }
    }
}
