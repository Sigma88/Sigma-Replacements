using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace SkyBox
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        class NyanMenu : MonoBehaviour
        {
            static GameObject nyanMenu;
            static int frame = 0;
            static float wait = 0;
            static float angle = 0;
            static Texture[] frames;
            static Texture2D none = new Texture2D(1, 1);
            static Material XP;

            void Start()
            {
                if (Nyan.nyan)
                {
                    GameObject galaxy = GameObject.Find("MainMenuGalaxy");
                    nyanMenu = nyanMenu ?? Instantiate(galaxy);

                    Debug.Log("NyanMenuSkyBox.Start", "Applying to skybox = " + nyanMenu);
                    if (nyanMenu == null) return;

                    nyanMenu.transform.SetParent(galaxy.transform.parent);
                    nyanMenu.transform.position = galaxy.transform.position;
                    nyanMenu.transform.localScale = galaxy.transform.localScale * 0.9f;
                    //nyanMenu.transform.rotation = Quaternion.Euler(0, -5, 60);

                    frames = Nyan.nyanSkyBox;
                    none.SetPixel(1, 1, new Color(0, 0, 0, 0));
                    none.Apply();

                    // Set Textures
                    Renderer[] renderers = nyanMenu?.GetComponentsInChildren<Renderer>();

                    for (int i = 0; i < renderers?.Length; i++)
                    {
                        Debug.Log("NyanMenuSkyBox.Start", "Renderer = " + renderers[i]);
                        string name = renderers[i]?.name;
                        Material material = renderers[i]?.material;

                        if (material == null) continue;

                        material.shader = Shader.Find("Unlit/Transparent");

                        if (name == "XP")
                            XP = material;
                        else if (name == "XN")
                            material.SetTexture(none);
                        else if (name == "YP")
                            material.SetTexture(none);
                        else if (name == "YN")
                            material.SetTexture(none);
                        else if (name == "ZN")
                            material.SetTexture(none);
                        else if (name == "ZP")
                            material.SetTexture(none);
                    }
                }
            }

            void Update()
            {
                nyanMenu.transform.rotation = Quaternion.Euler(0, -5, 60);
                nyanMenu.transform.localRotation *= Quaternion.Euler(0, angle, 0);
                angle = (angle + 30f * Time.deltaTime) % 360;

                if (wait > 0.025)
                {
                    XP.SetTexture(frames[frame]);

                    frame = (frame + 1) % 4;
                    wait = 0;
                }
                else
                {
                    wait += Time.deltaTime;
                }

                /*
                if (Nyan.nyan && wait == 0)
                {
                    XP.SetTexture(frames[frame]);
                    nyanMenu.transform.localRotation *= Quaternion.Euler(0, 1, 0);

                    frame = frame == 3 ? 0 : frame + 1;
                    wait = 1;
                }
                else if (Nyan.nyan)
                {
                    wait--;
                }
                */
            }
        }

        [KSPAddon(KSPAddon.Startup.EveryScene, false)]
        class NyanSkyBox : MonoBehaviour
        {
            static GameObject nyanMenu;
            static int frame = 0;
            static float wait = 0;
            static float angle = 0;
            static Texture[] frames;
            static Texture2D none = new Texture2D(1, 1);
            static Material XP = null;
            static bool initialized = false;

            void Start()
            {
                if (Nyan.forever)
                {
                    TimingManager.LateUpdateAdd(TimingManager.TimingStage.BetterLateThanNever, UpdateBox);

                    GameObject cube = GalaxyCubeControl.Instance?.gameObject?.GetComponentsInChildren<Renderer>()?.FirstOrDefault(r => r.name == "XP")?.gameObject?.transform?.parent?.gameObject;

                    nyanMenu = nyanMenu ?? Instantiate(cube);

                    if (nyanMenu == null) return;

                    nyanMenu.transform.SetParent(cube.transform.parent);
                    nyanMenu.transform.position = cube.transform.position;
                    nyanMenu.transform.localScale = cube.transform.localScale * 0.9f;

                    frames = Nyan.nyanSkyBox;
                    none.SetPixel(1, 1, new Color(0, 0, 0, 0));
                    none.Apply();

                    // Set Textures
                    Renderer[] renderers = nyanMenu?.GetComponentsInChildren<Renderer>();

                    for (int i = 0; i < renderers?.Length; i++)
                    {
                        string name = renderers[i]?.name;
                        Material material = renderers[i]?.material;
                        if (material == null) continue;

                        material.shader = Shader.Find("Unlit/Transparent");

                        if (name == "XP")
                            XP = material;
                        else if (name == "XN")
                            material.SetTexture(none);
                        else if (name == "YP")
                            material.SetTexture(none);
                        else if (name == "YN")
                            material.SetTexture(none);
                        else if (name == "ZN")
                            material.SetTexture(none);
                        else if (name == "ZP")
                            material.SetTexture(none);
                    }
                }
            }

            void OnDestroy()
            {
                if (initialized && Nyan.forever)
                    TimingManager.LateUpdateRemove(TimingManager.TimingStage.BetterLateThanNever, UpdateBox);
            }

            void UpdateBox()
            {
                if (Nyan.forever && HighLogic.LoadedScene != GameScenes.MAINMENU)
                {
                    nyanMenu.transform.rotation = Quaternion.Euler(0, -5, 60);
                    nyanMenu.transform.localRotation *= Quaternion.Euler(0, angle, 0);
                    angle = (angle + 6f * Time.deltaTime) % 360;

                    if (wait > 0.1)
                    {
                        XP.SetTexture(frames[frame]);

                        frame = (frame + 1) % 4;
                        wait = 0;
                    }
                    else
                    {
                        wait += Time.deltaTime;
                    }
                }
            }
        }
    }
}
