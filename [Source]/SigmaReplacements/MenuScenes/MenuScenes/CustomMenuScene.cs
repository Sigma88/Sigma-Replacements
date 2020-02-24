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

            protected void KerbalColliders(GameObject kerbal, GameObject helmet)
            {
                GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                capsule.name = "joints01_capsule_collider";
                capsule.layer = 15;
                capsule.transform.SetParent(kerbal.GetChild("joints01").transform, true);
                capsule.transform.localPosition = new Vector3(0, 0.42f, -0.025f);
                capsule.transform.localScale = new Vector3(0.25f, 0.42f, 0.35f);
                capsule.transform.localRotation = Quaternion.identity;
                capsule.transform.SetParent(kerbal.GetChild("bn_spA01").transform);
                capsule.GetComponent<Collider>().isTrigger = true;

                if (Debug.debug)
                {
                    capsule.GetComponent<Renderer>().material.color = Color.magenta;
                }
                else
                {
                    Object.DestroyImmediate(capsule.GetComponent<Renderer>());
                }

                if (helmet?.activeSelf == true)
                {
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.name = "bn_helmet01_sphere_collider";
                    sphere.layer = 15;
                    sphere.transform.SetParent(kerbal.GetChild("bn_helmet01").transform, true);
                    sphere.transform.localPosition = new Vector3(-0.25f, 0.05f, 0);
                    sphere.transform.localScale = new Vector3(0.58f, 0.58f, 0.58f);
                    sphere.transform.localRotation = Quaternion.identity;
                    sphere.GetComponent<Collider>().isTrigger = true;

                    if (Debug.debug)
                    {
                        sphere.GetComponent<Renderer>().material.color = Color.magenta;
                    }
                    else
                    {
                        Object.DestroyImmediate(sphere.GetComponent<Renderer>());
                    }
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

            protected MenuLight Parse(ConfigNode node, MenuLight defaultValue)
            {
                if (node != null)
                {
                    defaultValue = new MenuLight(node);
                }

                return defaultValue;
            }

            protected MenuLight[] Parse(ConfigNode[] nodes, MenuLight[] array)
            {
                for (int i = 0; i < nodes?.Length; i++)
                {
                    if (array == null) array = new MenuLight[nodes.Length];

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
