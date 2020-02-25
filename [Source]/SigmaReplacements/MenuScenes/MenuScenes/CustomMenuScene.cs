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

            protected GameObject[] GetOrbitKerbals()
            {
                GameObject[] scenes = Object.FindObjectOfType<MainMenu>()?.envLogic?.areas;

                Transform kerbals = scenes[1]?.GetChild("Kerbals")?.transform;

                return new GameObject[] { Instantiate(kerbals.GetChild(0).gameObject), Instantiate(kerbals.GetChild(1).gameObject), Instantiate(kerbals.GetChild(2).gameObject), Instantiate(kerbals.GetChild(3).gameObject) };
            }

            protected GameObject[] GetMunKerbals()
            {
                GameObject[] scenes = Object.FindObjectOfType<MainMenu>()?.envLogic?.areas;

                Transform kerbals = scenes[0]?.GetChild("Kerbals")?.transform;

                return new GameObject[] { Instantiate(kerbals.GetChild(0).gameObject), InstantiateFemale(kerbals.GetChild(0).gameObject) };
            }

            GameObject InstantiateFemale(GameObject maleEVA)
            {
                GameObject[] scenes = Object.FindObjectOfType<MainMenu>()?.envLogic?.areas;

                GameObject femaleEVA = Instantiate(maleEVA);
                femaleEVA.name = "femaleEVA";
                GameObject maleHead = femaleEVA.GetChild("head02");

                GameObject valHead = Object.Instantiate(scenes[1].GetChild("mesh_female_kerbalAstronaut01_kerbalGirl_mesh_kerbalGirl_base"), maleHead.transform.parent);

                valHead.SetActive(true);
                valHead.transform.rotation = maleHead.transform.rotation;
                valHead.transform.position = maleHead.transform.position;
                maleHead.SetActive(false);

                GameObject leftEye = null;
                GameObject rightEye = null;

                foreach (var renderer in valHead.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {
                    if (renderer?.bones?.Length > 0)
                    {
                        var newBones = new Transform[renderer.bones.Length];

                        for (int i = 0; i < renderer.bones.Length; i++)
                        {
                            newBones[i] = femaleEVA?.GetChild(renderer?.bones?[i]?.name)?.transform;

                            if (newBones[i].name == "jntDrv_l_eye01")
                            {
                                if (leftEye == null)
                                {
                                    leftEye = new GameObject("jntDrv_l_eye01_pivot");
                                    leftEye.transform.parent = newBones[i];
                                    leftEye.transform.localPosition = Vector3.zero;
                                    leftEye.transform.localEulerAngles = new Vector3(356, 11, 17);
                                    leftEye.transform.localScale = Vector3.one;
                                }
                                newBones[i] = leftEye.transform;
                            }

                            if (newBones[i].name == "jntDrv_r_eye01")
                            {
                                if (rightEye == null)
                                {
                                    rightEye = new GameObject("jntDrv_r_eye01_pivot");
                                    rightEye.transform.parent = newBones[i];
                                    rightEye.transform.localPosition = Vector3.zero;
                                    rightEye.transform.localEulerAngles = new Vector3(3, 348, 17);
                                    rightEye.transform.localScale = Vector3.one;
                                }
                                newBones[i] = rightEye.transform;
                            }
                        }

                        renderer.rootBone = femaleEVA?.GetChild(renderer?.rootBone?.name)?.transform;

                        if (renderer?.rootBone?.name == "jntDrv_l_eye01")
                            renderer.rootBone = leftEye.transform;

                        if (renderer?.rootBone?.name == "jntDrv_r_eye01")
                            renderer.rootBone = rightEye.transform;

                        renderer.bones = newBones;
                    }
                }

                return femaleEVA;
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
