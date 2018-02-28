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

            protected MenuObject Parse(ConfigNode node, MenuObject defaultValue)
            {
                if (node != null)
                {
                    defaultValue = new MenuObject(node);
                }

                return defaultValue;
            }

            protected MenuObject[] Parse(ConfigNode[] nodes, MenuObject[] array)
            {
                for (int i = 0; i < nodes?.Length; i++)
                {
                    if (array == null) array = new MenuObject[nodes.Length];

                    array[i] = Parse(nodes[i], array[i]);
                }

                return array;
            }

            protected GameObject Instantiate(GameObject original)
            {
                GameObject clone = null;

                if (original != null)
                {
                    clone = Object.Instantiate(original);
                    if (original?.transform?.parent != null)
                        clone.transform.SetParent(original.transform.parent);
                }

                return clone;
            }
        }
    }
}
