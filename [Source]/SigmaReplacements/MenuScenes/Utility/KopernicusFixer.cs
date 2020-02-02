using System.Linq;
using UnityEngine;
using Kopernicus;
using Kopernicus.OnDemand;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class KopernicusFixer : MonoBehaviour
        {
            internal static bool detect = AssemblyLoader.loadedAssemblies.FirstOrDefault(a => a.name == "Kopernicus") != null;

            void Awake()
            {
                GameObject[] scenes = FindObjectOfType<MainMenu>().envLogic.areas;
                Templates.KopernicusMainMenu = scenes[1].activeSelf && OrbitSceneInfo.DataBase == null;
                Debug.Log("KopernicusFixer", "Kopernicus detected => Kopernicus.Templates.KopernicusMainMenu = " + Templates.KopernicusMainMenu);
            }
        }

        internal class OnDemandFixer : MonoBehaviour
        {
            internal static void LoadTextures(GameObject body)
            {
                if (OnDemandStorage.UseOnDemand)
                {
                    body?.GetComponent<ScaledSpaceOnDemand>()?.LoadTextures();
                }
            }
        }
    }
}
