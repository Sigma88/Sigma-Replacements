using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    internal class IVAFinder : MonoBehaviour
    {
        void Start()
        {
            UpdateIVAs();
        }

        internal void UpdateIVAs()
        {
            kerbalExpressionSystem[] kerbalIVAs = Resources.FindObjectsOfTypeAll<kerbalExpressionSystem>();
            Debug.Log("IVAFinder.UpdateIVAs", "IVAs = " + kerbalIVAs?.Length);

            int? n = kerbalIVAs?.Length;
            for (int i = 0; i < n; i++)
            {
                if (kerbalIVAs[i]?.isActiveAndEnabled == true)
                    AddOrGetComponent(kerbalIVAs[i]?.gameObject);
            }
        }

        internal virtual void AddOrGetComponent(GameObject gameObject) { }
    }
}
