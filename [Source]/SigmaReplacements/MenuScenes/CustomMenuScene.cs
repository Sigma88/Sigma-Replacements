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

            protected MenuObject[] Parse(ConfigNode[] nodes, MenuObject[] array)
            {
                for (int i = 0; i < nodes?.Length; i++)
                {
                    if (array == null) array = new MenuObject[nodes.Length];

                    array[i] = new MenuObject(nodes[i]);
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
