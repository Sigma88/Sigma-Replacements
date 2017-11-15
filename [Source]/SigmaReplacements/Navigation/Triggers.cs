using UnityEngine;


namespace SigmaReplacements
{
    namespace Navigation
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, true)]
        class FlightTriggers : MonoBehaviour
        {
            void Start()
            {
                DontDestroyOnLoad(this);

                Debug.Log("FlightTriggers", "Start");

                GameEvents.onVesselReferenceTransformSwitch.Add(OnControlSwitch);
                GameEvents.onCrewOnEva.Add(OnCrewOnEva);
                GameEvents.onCrewBoardVessel.Add(OnCrewOnIva);
            }

            void OnCrewOnEva(GameEvents.FromToAction<Part, Part> action)
            {
                Debug.Log("FlightTriggers.OnCrewOnEva", "Part = " + action.to);
                /*
                KerbalEVA kerbalEVA = action.to.GetComponent<KerbalEVA>();
                if (kerbalEVA.GetComponent<CustomNavBall>() == null)
                    kerbalEVA.gameObject.AddComponent<CustomNavBall>();*/
            }

            void OnCrewOnIva(GameEvents.FromToAction<Part, Part> action)
            {
                Debug.Log("FlightTriggers.OnCrewOnIva", "Part = " + action.to);
                // DestroyImmediate(FlightUIModeController.Instance);
                // FlightUIModeController.Instance = new FlightUIModeController();
                //VesselNavBall(action.to.vessel);
            }

            void OnControlSwitch(Transform from, Transform to)
            {
                string part = to?.GetComponent<Part>()?.partInfo?.name;
                Debug.Log("FlightTriggers.OnControlSwitch", "Part = " + part);
                Debug.Log("FlightTriggers.OnControlSwitch", "Database count = " + ModuleNavBall.DataBase?.Count + ", HasKey = " + ModuleNavBall.DataBase?.ContainsKey(part));

                if (!string.IsNullOrEmpty(part) && ModuleNavBall.DataBase.ContainsKey(part))
                {
                    ModuleNavBall.DataBase[part]?.ApplyTo(FlightUIModeController.Instance);
                }
                else
                {
                    DefaultNavBall.Stock.ApplyTo(FlightUIModeController.Instance);
                }
            }
        }
    }
}
