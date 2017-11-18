using UnityEngine;


namespace SigmaReplacements
{
    namespace Navigation
    {
        class IVAnavball : MonoBehaviour
        {
            void Start()
            {
                string part = GetComponent<InternalNavBall>()?.part?.partInfo?.name;
                if (!string.IsNullOrEmpty(part) && ModuleNavBall.DataBase?.ContainsKey(part) == true)
                    ModuleNavBall.DataBase[part]?.FixIVA(GetComponentsInChildren<Renderer>(true));
            }
        }
    }
}
