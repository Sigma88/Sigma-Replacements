using UnityEngine;
using KSP.UI.Screens;


namespace SigmaReplacements
{
    internal class UIKerbalsTrigger : MonoBehaviour
    {
        internal static EventData<GameObject> MissionGene = new EventData<GameObject>("MissionGene");

        void Start()
        {
            Debug.Log("UIKerbalsTrigger", "Start");

            GameObject gene = GetComponent<MissionControl>()?.avatarController?.gameObject?.GetChild("instructor_Gene");

            if (gene != null)
            {
                MissionGene.Fire(gene);
            }
        }
    }

    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    internal class AdminTriggers : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("AdminTriggers", "Start");

            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                UIKerbalStrategy[] strategies = Resources.FindObjectsOfTypeAll<UIKerbalStrategy>();

                Debug.Log("UIKerbalsTrigger", "strategies count = " + strategies?.Length);

                for (int i = 0; i < strategies?.Length; i++)
                {
                    if (strategies[i] != null)
                    {
                        Debug.Log("UIKerbalsTrigger", "Applying UIKerbalStrategy = " + strategies[i]);
                        strategies[i].Apply();
                    }
                }
            }
        }
    }
}
