using System.Collections.Generic;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Textures
    {
        class TextureInfo : Info
        {
            internal static Dictionary<Texture, Texture> Database = new Dictionary<Texture, Texture>();

            internal TextureInfo(ConfigNode[] InfoNodes)
            {
                List<Texture> bulkReplace = new List<Texture>();

                for (int i = 0; i < InfoNodes?.Length; i++)
                {
                    Texture newTex = null;
                    newTex = Parse(InfoNodes[i].GetValue("replacement"), newTex);

                    if (newTex != null)
                    {
                        if (!Database.ContainsKey(newTex))
                        {
                            string[] originals = InfoNodes[i].GetValues("original");
                            for (int j = 0; j < originals?.Length; j++)
                            {
                                Texture oldTex = null;
                                oldTex = Parse(originals[j], oldTex);
                                if (oldTex != null)
                                {
                                    if (!Database.ContainsKey(oldTex))
                                    {
                                        Database.Add(oldTex, newTex);
                                        Debug.Log("SettingsLoader", "Added definition to replacements database.");
                                        Debug.Log("SettingsLoader", "[ " + newTex.name + " ] == replaces ==> [ " + oldTex.name + " ]");
                                    }
                                    else
                                    {
                                        Debug.Log("SettingsLoader", "Original texture already in the database = " + oldTex.name);
                                    }
                                }
                                else
                                {
                                    Debug.Log("SettingsLoader", "Original texture not found = " + originals[j]);
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("SettingsLoader", "Replacement texture already in the database = " + newTex.name);
                        }
                    }
                    else
                    {
                        Debug.Log("SettingsLoader", "Replacement texture not found = " + InfoNodes[i].GetValue("replacement"));
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
