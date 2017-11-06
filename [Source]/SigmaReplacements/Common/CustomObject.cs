using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    public class CustomObject : MonoBehaviour
    {
        void Start()
        {
            Apply();
        }

        internal void Apply()
        {
            Debug.Log("CustomHead.Apply", "In gameObject = " + gameObject);

            ProtoCrewMember kerbal = GetComponent<KerbalEVA>()?.part?.protoModuleCrew?.FirstOrDefault();
            if (kerbal == null) kerbal = GetComponent<kerbalExpressionSystem>()?.protoCrewMember;
            if (kerbal == null) kerbal = GetComponent<UIKerbalMenu>()?.crewMember;
            if (kerbal == null) kerbal = GetComponent<UIKerbalWerner>()?.crewMember;
            if (kerbal == null) kerbal = GetComponent<UIKerbalGene>()?.crewMember;
            if (kerbal == null) kerbal = GetComponent<UIKerbalStrategy>()?.crewMember;
            Debug.Log("CustomHead.Apply", "kerbal = " + kerbal);
            if (kerbal == null) return;

            LoadFor(kerbal);

            ApplyTo(kerbal);
        }

        void LoadFor(ProtoCrewMember kerbal)
        {
        }

        void ApplyTo(ProtoCrewMember kerbal)
        {
        }
    }
}
