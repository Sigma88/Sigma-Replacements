using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class LightTracker : MonoBehaviour
        {
            void Update()
            {
                transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
            }
        }
    }
}
