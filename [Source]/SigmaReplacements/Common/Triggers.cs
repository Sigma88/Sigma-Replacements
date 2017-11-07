using UnityEngine;


namespace SigmaReplacements
{
    internal class AdminTrigger : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("AdminTrigger", "Start");

            UIKerbalStrategy[] strategies = GetComponentsInChildren<UIKerbalStrategy>();

            for (int i = 0; i < strategies?.Length; i++)
            {
                if (strategies[i] != null)
                    strategies[i].Apply();
            }
        }
    }
}
