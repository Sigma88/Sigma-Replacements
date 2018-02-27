using Kopernicus;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class KopernicusFixer : MonoBehaviour
        {
            void Awake()
            {
                GameObject[] scenes = FindObjectOfType<MainMenu>().envLogic.areas;
                Templates.kopernicusMainMenu = scenes[1].activeSelf && !(OrbitSceneInfo.DataBase?.Count > 0);
            }
        }
    }
}
