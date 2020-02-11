using System.Collections.Generic;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuButtons
    {
        internal class MenuButtonInfo : Info
        {
            // Template
            internal string template = null;

            // Static
            internal static List<Info> List = new List<Info>();
            internal static List<Info> DataBase = new List<Info>();
            internal static Dictionary<string, string> Templates = new Dictionary<string, string>();

            // Colors
            internal Color? normalColor = null;
            internal Color? hoverColor = null;
            internal Color? downColor = null;
            internal Color? disabledColor = null;

            // Border
            internal float? borderSize = null;
            internal Color? borderColor = null;

            // Text
            internal string text = null;
            internal string font = null;
            internal float? fontSize = null;

            // Vectors
            internal Vector3? position = null;
            internal Quaternion? rotation = null;
            internal Vector3? scale = null;

            // New MenuButtonInfo
            internal MenuButtonInfo(ConfigNode info)
            {
                // Parse Requirements
                name = info.GetValue("name");
                template = info.GetValue("template");
                collection = info.GetValue("collection");

                // Parse Colors
                normalColor = Parse(info.GetValue("normalColor"), normalColor);
                hoverColor = Parse(info.GetValue("hoverColor"), hoverColor);
                downColor = Parse(info.GetValue("downColor"), downColor);
                disabledColor = Parse(info.GetValue("disabledColor"), disabledColor);

                // Parse Border
                borderSize = Parse(info.GetValue("borderSize"), borderSize);
                borderColor = Parse(info.GetValue("borderColor"), borderColor);

                // Parse Text
                text = info.GetValue("text");
                font = info.GetValue("font");
                fontSize = Parse(info.GetValue("fontSize"), fontSize);

                // Parse Vectors
                position = Parse(info.GetValue("position"), position);
                rotation = Parse(info.GetValue("rotation"), rotation);
                scale = Parse(info.GetValue("scale"), scale);

                // Instantiate Template
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(template))
                    return;

                if (Templates.ContainsKey(name) || Templates.ContainsValue(template))
                    return;

                Templates.Add(template, name);
            }
        }
    }
}
