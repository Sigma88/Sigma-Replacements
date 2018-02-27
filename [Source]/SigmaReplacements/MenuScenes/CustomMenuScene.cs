using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class CustomMenuScene
        {
            internal CustomMenuScene()
            {
            }
            
            MenuObject[] Parse(ConfigNode[] nodes, MenuObject[] array)
            {
                for (int i = 0; i < nodes?.Length; i++)
                {
                    if (array == null) array = new MenuObject[nodes.Length];

                    array[i] = new MenuObject(nodes[i]);
                }

                return array;
            }

            MenuObject[] ParseBoulders(ConfigNode[] input)
            {
                if (input == null) return null;

                MenuObject[] data = Parse(input, null);

                List<MenuObject> output = data.Where(i => i.name == "boulder" && i.index == null).ToList();
                output.AddRange(data.Where(i => i.name == "boulder" && i.index != null).OrderBy(i => i.index));

                return output.ToArray();
            }

            MenuObject[] ParseScatter(ConfigNode[] input)
            {
                MenuObject[] output = null;

                if (input != null)
                {
                    output = Parse(input, output);
                }

                return output?.Where(i => i.name != "boulder")?.ToArray();
            }
        }
    }
}
