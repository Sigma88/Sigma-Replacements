using System.Linq;
using UnityEngine;


namespace SigmaReplacements
{
    namespace Textures
    {
        [KSPAddon(KSPAddon.Startup.MainMenu, false)]
        internal class NyanMenu : MonoBehaviour
        {
            static int frame = 0;
            static float wait = 0;
            static Texture[] frames;
            static Renderer nyan;

            void Start()
            {
                if (Nyan.nyan)
                {
                    nyan = null;
                }
            }

            void Update()
            {
                if (Nyan.nyan)
                {
                    if (frames == null)
                        frames = Nyan.nyanLogo;

                    if (nyan == null)
                    {
                        Renderer logo = Resources.FindObjectsOfTypeAll<Renderer>().FirstOrDefault(r => r?.material?.name == "logofullred (Instance)");

                        if (logo != null)
                        {
                            GameObject nyanObj = Instantiate(logo.gameObject);
                            nyan = nyanObj.GetComponent<Renderer>();
                            nyan.material = new Material(logo.material.shader);

                            nyan.transform.SetParent(logo.transform);
                            nyan.transform.localScale = new Vector3(0.5f, 0, 0.16f);
                            nyan.transform.localPosition = new Vector3(-0.05f, 0.15f, 0.47f);
                        }
                    }

                    if (wait > 0.025)
                    {
                        nyan.material.SetTexture(frames[frame]);

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
