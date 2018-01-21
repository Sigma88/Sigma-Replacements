using System.Collections.Generic;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Textures
    {
        class TextureInfo : Info
        {
            internal static Dictionary<Texture, Texture> Database = new Dictionary<Texture, Texture>();

            internal TextureInfo(ConfigNode[] InfoNodes) : base(new ConfigNode(), new ConfigNode())
            {
                List<Texture> bulkReplace = new List<Texture>();

                for (int i = 0; i < InfoNodes?.Length; i++)
                {
                    Texture newTex = null;
                    newTex = Parse(InfoNodes[i].GetValue("replacement"), newTex);

                    if (newTex != null)
                    {
                        string[] originals = InfoNodes[i].GetValues("original");
                        for (int j = 0; j < originals?.Length; j++)
                        {
                            Texture oldTex = null;
                            oldTex = Parse(originals[j], oldTex);
                            if (oldTex != null && !Database.ContainsKey(oldTex) && !Database.ContainsKey(newTex))
                            {
                                Database.Add(oldTex, newTex);
                            }
                        }
                    }

                    bulkReplace = ParseFolders(InfoNodes[i]?.GetNode("Folders")?.GetValues("path"), bulkReplace);
                }

                Debug.Log("SettingsLoader", "Total textures found in Folders = " + bulkReplace?.Count);

                for (int i = 0; i < bulkReplace?.Count; i++)
                {
                    Texture newTex = bulkReplace[i];
                    string name = newTex.name.Substring(newTex.name.LastIndexOf('/') + 1);
                    Texture oldTex = null;
                    oldTex = Parse(name, oldTex);
                    if (oldTex != null && !Database.ContainsKey(oldTex) && !Database.ContainsKey(newTex))
                    {
                        Database.Add(oldTex, newTex);
                    }
                }

                Debug.Log("SettingsLoader", "Valid Textures Replacements found = " + Database?.Count);
            }
        }
    }
}
