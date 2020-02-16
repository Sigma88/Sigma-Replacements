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

            protected void AddCoronas(GameObject template, GameObject body)
            {
                SunCoronas[] coronas = template.GetComponentsInChildren<SunCoronas>();

                for (int i = 0; i < coronas.Length; i++)
                {
                    GameObject corona = coronas[i].gameObject;
                    GameObject cloneCorona = Instantiate(corona.gameObject);
                    
                    cloneCorona.name = corona.name;
                    cloneCorona.transform.SetParent(body.transform);
                    cloneCorona.transform.localPosition = corona.transform.localPosition;
                    cloneCorona.transform.localRotation = corona.transform.localRotation;
                    cloneCorona.transform.localScale = corona.transform.localScale;
                    cloneCorona.SetLayerRecursive(15);
                }
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
                    clone = Object.Instantiate(original, original?.transform?.parent, true);
                }

                return clone;
            }
        }
    }
}
