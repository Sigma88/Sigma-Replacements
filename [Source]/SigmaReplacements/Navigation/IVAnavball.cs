using UnityEngine;


namespace SigmaReplacements
{
    namespace Navigation
    {
        class IVAnavball : MonoBehaviour
        {
            void Start()
            {
                if (Nyan.forever)
                {
                    NyanNavBall.FixIVA(GetComponentsInChildren<Renderer>(true));
                    return;
                }

                string part = GetComponent<InternalNavBall>()?.part?.partInfo?.name;
                if (!string.IsNullOrEmpty(part) && ModuleNavBall.DataBase?.ContainsKey(part) == true)
                    ModuleNavBall.DataBase[part]?.FixIVA(GetComponentsInChildren<Renderer>(true));
            }
        }
    }
}
