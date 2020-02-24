using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class FlareFixer : MonoBehaviour
        {
            internal MenuObject info;
            internal CelestialBody template;

            void Awake()
            {
                StartCoroutine(CallbackUtil.DelayedCallback(1, () => info.AddFlare(gameObject, template)));
            }
        }

        internal class FlareCamera : MonoBehaviour
        {
            internal LensFlare flare;
            internal float maxBrightness;

            bool start = true;
            bool hidden = false;
            static int layerMask = 1 << 01 | 1 << 02 | 1 << 03 | 1 << 04 | 1 << 05 | 1 << 06 | 1 << 07 | 1 << 08 | 1 << 09 |
                                   1 << 10 | 1 << 11 | 1 << 12 | 1 << 13 | 1 << 14 | 1 << 15 | 1 << 16 | 1 << 17 | 1 << 18 | 1 << 19 |
                                   1 << 20 | 1 << 21 | 1 << 22 | 1 << 23 | 1 << 24 | 1 << 25 | 1 << 26 | 1 << 27 | 1 << 28 | 1 << 29 |
                                   1 << 30 | 1 << 31;

            void Start()
            {
                GameEvents.onGameSceneSwitchRequested.Add(SceneSwitch);
            }

            void SceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scenes)
            {
                DestroyImmediate(flare.gameObject);
                Destroy(this);
            }

            void Update()
            {
                flare.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
            }

            void LateUpdate()
            {
                CheckHidden();

                if (start)
                {
                    start = false;
                    flare.brightness = 0;
                    return;
                }

                if (hidden)
                {
                    Hide();
                }
                else
                {
                    Show();
                }

                Track();
            }

            void CheckHidden()
            {
                hidden = Physics.Raycast(Camera.main.transform.position, flare.transform.forward.normalized * -20000, Mathf.Infinity, layerMask);
            }

            void Show()
            {
                if (flare.brightness < maxBrightness)
                {
                    flare.brightness += flare.fadeSpeed * Time.deltaTime;
                }

                if (flare.brightness > maxBrightness)
                {
                    flare.brightness = maxBrightness;
                }
            }

            void Hide()
            {
                if (flare.brightness > 0)
                {
                    flare.brightness -= flare.fadeSpeed * Time.deltaTime;
                }

                if (flare.brightness < 0)
                {
                    flare.brightness = 0;
                }
            }

            LineRenderer line;

            void Track()
            {
                if (Debug.debug)
                {
                    if (line == null)
                    {
                        GameObject myLine = new GameObject("lineRenderer");
                        line = myLine.AddOrGetComponent<LineRenderer>();
                        line.transform.position = Camera.main.transform.position + Camera.main.transform.forward.normalized;
                        line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
                        line.startWidth = 0.02f;
                        line.endWidth = 5f;
                    }

                    if (Camera.main?.transform != null)
                        line.SetPosition(0, Camera.main.transform.position + Camera.main.transform.forward.normalized);

                    if (flare.transform != null)
                        line.SetPosition(1, flare.transform.forward.normalized * -5000);

                    line.startColor = line.endColor = hidden ? Color.red : Color.green;
                }
            }
        }

        internal class FlareRemover : MonoBehaviour
        {
            LensFlare flare;
            bool skip = true;

            void Awake()
            {
                flare = GetComponent<LensFlare>();
                Debug.Log("FlareRemover.Awake", "flare = " + flare);

                if (flare?.enabled != true)
                {
                    DestroyImmediate(this);
                }
            }

            void Start()
            {
                TimingManager.LateUpdateAdd(TimingManager.TimingStage.BetterLateThanNever, LaterUpdate);
            }

            void LaterUpdate()
            {
                if (HighLogic.LoadedScene == GameScenes.MAINMENU)
                {
                    flare.enabled = skip = false;

                    Track();
                }

                else

                if (!skip)
                {
                    flare.enabled = skip = true;
                    DestroyImmediate(this);
                }
            }

            LineRenderer line;

            void Track()
            {
                if (Debug.debug)
                {
                    if (line == null)
                    {
                        GameObject myLine = new GameObject("lineRenderer");
                        line = myLine.AddOrGetComponent<LineRenderer>();
                        line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
                        line.startColor = line.endColor = Color.black;
                        line.startWidth = 0.02f;
                        line.endWidth = 5f;
                    }

                    if (Camera.main?.transform != null)
                        line.SetPosition(0, Camera.main.transform.position + Camera.main.transform.forward.normalized);

                    if (transform != null)
                        line.SetPosition(1, transform.forward * -5000);
                }
            }

            void OnDestroy()
            {
                TimingManager.LateUpdateRemove(TimingManager.TimingStage.BetterLateThanNever, LaterUpdate);
            }
        }
    }
}
