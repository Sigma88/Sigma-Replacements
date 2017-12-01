using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Textures
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        class NyanMenu : MonoBehaviour
        {
            static int frame = 0;
            static float wait = 0;
            static Texture[] frames;
            static Material logo;

            void Start()
            {
                if (Nyan.nyan)
                {
                    logo = null;
                }
            }

            void Update()
            {
                if (Nyan.nyan)
                {
                    if (frames == null)
                        frames = Nyan.nyanLogo;

                    if (logo == null)
                        logo = Resources.FindObjectsOfTypeAll<Material>().FirstOrDefault(m => m?.name == "logofullred (Instance)");

                    if (wait > 0.025)
                    {
                        logo.SetTexture(frames[frame]);

                        frame = (frame + 1) % 4;
                        wait = 0;
                    }
                    else
                    {
                        wait += Time.deltaTime;
                    }
                }
            }
        }
    }
}
