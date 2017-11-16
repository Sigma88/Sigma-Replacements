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
                Debug.Log("FlightTriggers.OnCrewOnEva", "Kerbal = " + action.to);

                KerbalNavBall(action.to?.GetComponent<KerbalEVA>());
            }

            void OnCrewOnIva(GameEvents.FromToAction<Part, Part> action)
            {
                Debug.Log("FlightTriggers.OnCrewOnIva", "Part = " + action.to);
                PartNavBall(action.to);
            }

            void OnControlSwitch(Transform from, Transform to)
            {
                Part part = to?.GetComponent<Part>();
                Debug.Log("FlightTriggers.OnControlSwitch", "Part = " + part);
                PartNavBall(part);
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
                if (kerbal != null && kerbal.GetComponent<CustomNavBall>() == null)
                    kerbal.gameObject.AddComponent<CustomNavBall>();
            }
        }
    }
}
