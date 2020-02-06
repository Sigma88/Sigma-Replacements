using System.Collections.Generic;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuButtons
    {
        internal class MenuButtonInfo : Info
        {
            // Static
            internal static List<Info> List = new List<Info>();
            internal static List<Info> DataBase = new List<Info>();

            // Colors
            internal Color? normalColor = null;
            internal Color? hoverColor = null;
            internal Color? downColor = null;

            // Text
            internal string text = null;

            // Vectors
            internal Vector3? position = null;
            internal Quaternion? rotation = null;
            internal Vector3? scale = null;

            // New MenuButtonInfo
            internal MenuButtonInfo(ConfigNode info)
            {
                // Parse Requirements
                name = info.GetValue("name");
                collection = info.GetValue("collection");

                // Parse Colors
                normalColor = Parse(info.GetValue("normalColor"), normalColor);
                hoverColor = Parse(info.GetValue("hoverColor"), hoverColor);
                downColor = Parse(info.GetValue("downColor"), downColor);

                // Parse Text
                text = info.GetValue("text");

                // Parse Vectors
                position = Parse(info.GetValue("position"), position);
                rotation = Parse(info.GetValue("rotation"), rotation);
                scale = Parse(info.GetValue("scale"), scale);
            }
        }
    }
}
