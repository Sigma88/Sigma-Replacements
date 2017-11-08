using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    public class CustomObject : MonoBehaviour
    {
        internal ProtoCrewMember Apply()
        {
            Debug.Log(GetType().Name + ".Apply", "In gameObject = " + gameObject);

            ProtoCrewMember kerbal = GetComponent<KerbalEVA>()?.part?.protoModuleCrew?.FirstOrDefault();
            if (kerbal == null) kerbal = GetComponent<kerbalExpressionSystem>()?.protoCrewMember;
            if (kerbal == null) kerbal = GetComponent<UIKerbalMenu>()?.crewMember;
            if (kerbal == null) kerbal = GetComponent<UIKerbalWerner>()?.crewMember;
            if (kerbal == null) kerbal = GetComponent<UIKerbalGene>()?.crewMember;
            if (kerbal == null) kerbal = GetComponent<UIKerbalStrategy>()?.crewMember;

            return kerbal;
        }
    }
}
