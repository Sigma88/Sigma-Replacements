using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class LightTracker : MonoBehaviour
        {
            internal Transform target;
            internal bool invert = false;

            void Update()
            {
                if (invert)
                {
                    Quaternion.LookRotation(transform.position - target.position);
                }
                else
                {
                    Quaternion.LookRotation(target.position - transform.position);
                }
            }
        }
    }
}
