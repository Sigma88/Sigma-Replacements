using System.Collections.Generic;
using UnityEngine;


namespace SigmaReplacements
{
    internal class IVAFinder : MonoBehaviour
    {
        void Start()
        {
            Vessel vessel = GetComponent<Vessel>();
            Debug.Log("IVAFinder.Start", "vessel = " + vessel);

            kerbalExpressionSystem[] kerbalIVAs = GetIVAs(vessel);
            int? n = kerbalIVAs?.Length;
            Debug.Log("IVAFinder.Start", "IVAs = " + n);

            for (int i = 0; i < n; i++)
            {
                AddOrGetComponent(kerbalIVAs[i]?.gameObject);
            }
        }

        internal virtual void AddOrGetComponent(GameObject gameObject) { }

        kerbalExpressionSystem[] GetIVAs(Vessel vessel)
        {
            List<kerbalExpressionSystem> list = new List<kerbalExpressionSystem>();

            int? n = vessel?.parts?.Count;

            for (int i = 0; i < n; i++)
            {
                kerbalExpressionSystem[] array = vessel?.parts?[i]?.internalModel?.GetComponentsInChildren<kerbalExpressionSystem>(true);
                if (array?.Length > 0)
                    list.AddRange(array);
            }

            return list.ToArray();
        }
    }
}
