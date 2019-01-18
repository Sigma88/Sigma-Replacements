using System;
using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Navigation
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, true)]
        internal class FlightTriggers : MonoBehaviour
        {
            void Start()
            {
                DontDestroyOnLoad(this);

                Debug.Log("FlightTriggers.Start", "Active Vessel = " + FlightGlobals.ActiveVessel);

                GameEvents.onVesselReferenceTransformSwitch.Add(OnControlSwitch);
                GameEvents.onCrewOnEva.Add(OnCrewOnEva);
                GameEvents.onCrewBoardVessel.Add(OnCrewOnIva);
                GameEvents.onVesselChange.Add(OnChangeVessel);

                InternalNavBall[] internals = Resources.FindObjectsOfTypeAll<InternalNavBall>();
                for (int i = 0; i < internals?.Length; i++)
                {
                    if (internals[i] == null) continue;

                    IVAnavball iva = internals[i].gameObject.AddOrGetComponent<IVAnavball>();
                }
            }

            void OnCrewOnEva(GameEvents.FromToAction<Part, Part> action)
            {
                Debug.Log("FlightTriggers.OnCrewOnEva", "Kerbal = " + action.to);

                KerbalNavBall(action.to?.GetComponent<KerbalEVA>());
            }

            void OnCrewOnIva(GameEvents.FromToAction<Part, Part> action)
            {
                Vessel vessel = FlightGlobals.ActiveVessel;

                Debug.Log("FlightTriggers.OnCrewOnIva", "ActiveVessel = " + vessel);

                OnChangeVessel(vessel);
            }

            void OnControlSwitch(Transform from, Transform to)
            {
                TimingManager.UpdateAdd(TimingManager.TimingStage.Normal, OnControlSwitch2);
            }

            void OnControlSwitch2()
            {
                TimingManager.UpdateRemove(TimingManager.TimingStage.Normal, OnControlSwitch2);

                Vessel vessel = FlightGlobals.ActiveVessel;
                Debug.Log("FlightTriggers.OnControlSwitch", "ActiveVessel = " + vessel);

                Part part = vessel?.GetReferenceTransformPart();
                Debug.Log("FlightTriggers.OnControlSwitch", "Part = " + part);

                PartNavBall(part);
            }

            void OnChangeVessel(Vessel vessel)
            {
                Debug.Log("FlightTriggers.OnChangeVessel", "Vessel = " + vessel);

                if (vessel?.isEVA == true)
                {
                    KerbalEVA kerbal = FlightGlobals.ActiveVessel?.evaController;
                    Debug.Log("FlightTriggers.OnChangeVessel", "Kerbal = " + kerbal);

                    KerbalNavBall(kerbal);
                }
                else
                {
                    Part part = vessel?.GetReferenceTransformPart();

                    Debug.Log("FlightTriggers.OnChangeVessel", "Part = " + part);

                    PartNavBall(part);
                }
            }

            void PartNavBall(Part part)
            {
                string name = part?.partInfo?.name;
                Debug.Log("FlightTriggers.PartNavBall", "name = " + name);
                Debug.Log("FlightTriggers.PartNavBall", "Database count = " + ModuleNavBall.DataBase?.Count + ", HasKey = " + ModuleNavBall.DataBase?.ContainsKey(name));

                if (!string.IsNullOrEmpty(name) && ModuleNavBall.DataBase.ContainsKey(name))
                {
                    Debug.Log("FlightTriggers.PartNavBall", "Load CustomNavBall = " + ModuleNavBall.DataBase[name]);
                    ModuleNavBall.DataBase[name]?.ApplyTo(FlightUIModeController.Instance);
                }
                else
                {
                    Debug.Log("FlightTriggers.PartNavBall", "Reset to Stock NavBall = " + DefaultNavBall.Stock);
                    DefaultNavBall.Stock.ApplyTo(FlightUIModeController.Instance);
                }
            }

            void KerbalNavBall(KerbalEVA kerbal)
            {
                if (kerbal != null)
                {
                    Debug.Log("FlightTriggers.KerbalNavBall", "Loading CustomNavBall for Kerbal = " + kerbal);

                    CustomNavBall evaNavBall = kerbal?.gameObject?.AddOrGetComponent<CustomNavBall>();
                    if (evaNavBall != null) evaNavBall.OnStart();
                    DestroyImmediate(evaNavBall);
                }
            }
        }

        [KSPAddon(KSPAddon.Startup.Instantly, true)]
        internal class NyanSettings : MonoBehaviour
        {
            void Start()
            {
                string[] args = Environment.GetCommandLineArgs();

                Nyan.nyan = (DateTime.Now.Month == 4 && DateTime.Now.Day == 1) || (args.Contains("-nyan-nyan") && !args.Contains("-nyan-not") || args.Contains("-nyan-nav"));
                Nyan.forever = Nyan.nyan && (args.Contains("-nyan-4ever") || args.Contains("-nyan-nav"));
            }
        }

        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        internal class NyanTriggers : MonoBehaviour
        {
            void Start()
            {
                if (Nyan.nyan)
                {
                    Texture nyanBall = Nyan.nyanBall;

                    Texture2D white = new Texture2D(1, 1);
                    white.SetPixel(1, 1, new Color(0.5f, 0.5f, 0.5f, 0.5f));
                    white.Apply();

                    Renderer Kerbin1 = GameObject.Find("OrbitScene")?.GetChild("Kerbin")?.GetComponent<Renderer>();
                    if (Kerbin1 != null)
                    {
                        Kerbin1.material.shader = Shader.Find("Terrain/Scaled Planet (Simple)");
                        Kerbin1.material.SetTexture("_MainTex", nyanBall);
                        Kerbin1.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
                        Kerbin1.material.SetTextureOffset("_MainTex", new Vector2(-0.425f, 0));
                        Kerbin1.material.SetTexture("_BumpMap", white);
                    }

                    Renderer Kerbin2 = GameObject.Find("MunScene")?.GetChild("Kerbin")?.GetComponent<Renderer>();
                    if (Kerbin2 != null)
                    {
                        Kerbin2.material.shader = Shader.Find("Terrain/Scaled Planet (Simple)");
                        Kerbin2.material.SetTexture("_MainTex", nyanBall);
                        Kerbin2.material.SetTextureScale("_MainTex", new Vector2(-1, 1));
                        Kerbin2.material.SetTexture("_BumpMap", white);
                    }
                }
            }
        }
    }
}
