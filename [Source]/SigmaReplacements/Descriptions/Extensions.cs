using System;
using System.Linq;
using System.Reflection;
using UnityEngine.UI;
using KSP.UI;
using KSP.UI.Screens;


namespace SigmaReplacements
{
    namespace Descriptions
    {
        internal static class Extensions
        {
            internal static int crewLimit(this AstronautComplex complex)
            {
                FieldInfo crewLimit = typeof(AstronautComplex).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(k => k.Name == "crewLimit");

                try
                {
                    return (int)crewLimit.GetValue(complex);
                }
                catch
                {
                    return int.MaxValue;
                }
            }

            internal static string PrintFor(this string s, ProtoCrewMember kerbal)
            {
                return s
                    .Replace("&br;", "\n")
                    .Replace("&name;", kerbal.name)
                    .Replace("&trait;", kerbal.trait)
                    .Replace("&seed;", "" + HighLogic.CurrentGame.Seed)
                    .Replace("&visited;", "" + (kerbal?.careerLog?.Entries?.Select(e => e.target)?.Where(t => !string.IsNullOrEmpty(t))?.Distinct()?.Count() ?? 0))
                    .Replace("&missions;", "" + (kerbal?.careerLog?.Entries?.Select(e => e.flight)?.Distinct()?.Count() ?? 0))
                    .GetHashColor();
            }

            internal static string GetHashColor(this string s)
            {
                int start = 0;

                while (s.Substring(start).Contains("&Color"))
                {
                    start = s.IndexOf("&Color");
                    int end = s.Substring(start).IndexOf(";") + 1;
                    if (end > 9)
                    {
                        int add = 0;
                        switch (s.Substring(start + 6, 2))
                        {
                            case "Lo":
                                break;
                            case "Hi":
                                add = 80;
                                break;
                            default:
                                start++;
                                continue;
                        }
                        string text = s.Substring(start, end);
                        int hash = Math.Abs(text.GetHashCode());
                        string color = "#";
                        for (int i = 0; i < 3; i++)
                        {
                            color += (hash % 176 + add).ToString("X");
                            hash = Math.Abs(hash.ToString().GetHashCode());
                        }
                        s = s.Replace(text, "<color=" + color + ">");
                    }
                    else
                    {
                        continue;
                    }
                }
                return s;
            }

            internal static Button suitVariantBtn(this CrewListItem item)
            {
                FieldInfo suitVariantBtn = typeof(CrewListItem).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(f => f.Name == "suitVariantBtn");

                return suitVariantBtn?.GetValue(item) as Button;
            }

            internal static CrewListItem Get_widgetApplicants(this AstronautComplex complex)
            {
                FieldInfo widgetApplicants = typeof(AstronautComplex).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(f => f.Name == "widgetApplicants");
                return widgetApplicants.GetValue(complex) as CrewListItem;
            }

            internal static CrewListItem Get_widgetEnlisted(this AstronautComplex complex)
            {
                FieldInfo widgetEnlisted = typeof(AstronautComplex).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(f => f.Name == "widgetEnlisted");
                return widgetEnlisted.GetValue(complex) as CrewListItem;
            }
        }
    }
}
