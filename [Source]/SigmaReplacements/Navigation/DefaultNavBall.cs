using UnityEngine;


namespace SigmaReplacements
{
    namespace Navigation
    {
        [KSPAddon(KSPAddon.Startup.Flight, false)]
        class DefaultNavBall : MonoBehaviour
        {
            internal static CustomNavBall Stock = null;
            internal static GameObject Instance = null;

            void Awake()
            {
                Stock = gameObject.AddComponent<CustomNavBall>();
                Instance = Instantiate(FlightUIModeController.Instance.gameObject.GetChild("NavballFrame"));
                Instance.gameObject.SetActive(false);
            }
        }
    }
}
