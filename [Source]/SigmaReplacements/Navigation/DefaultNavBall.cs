using UnityEngine;


namespace SigmaReplacements
{
    namespace Navigation
    {
        [KSPAddon(KSPAddon.Startup.Flight, false)]
        class DefaultNavBall : MonoBehaviour
        {
            internal static GameObject Instance = null;

            internal static CustomNavBall Stock = null;

            void Awake()
            {
                Instance = Instantiate(FlightUIModeController.Instance.gameObject);
                Stock = gameObject.AddComponent<CustomNavBall>();
                Instance.gameObject.SetActive(false);
            }
        }
    }
}
