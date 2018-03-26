using System.IO;
using UnityEngine;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class LiveDebug : MonoBehaviour
        {
            static double time = 0.1;
            static float moveby = 0.05f;

            void Update()
            {
                time += Time.deltaTime;

                if (time < 0.1) return;
                else time = 0;

                // POSITION
                if (Input.GetKey(KeyCode.LeftArrow))
                    transform.localPosition = new Vector3(transform.localPosition.x - moveby, transform.localPosition.y, transform.localPosition.z);
                else if (Input.GetKey(KeyCode.RightArrow))
                    transform.localPosition = new Vector3(transform.localPosition.x + moveby, transform.localPosition.y, transform.localPosition.z);
                else if (Input.GetKey(KeyCode.DownArrow))
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - moveby, transform.localPosition.z);
                else if (Input.GetKey(KeyCode.UpArrow))
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + moveby, transform.localPosition.z);
                else if (Input.GetKey(KeyCode.RightControl))
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - moveby);
                else if (Input.GetKey(KeyCode.RightShift))
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + moveby);

                // ROTATION
                else if (Input.GetKey(KeyCode.W))
                    transform.localRotation = Quaternion.Euler((transform.localEulerAngles.x + 1) % 360, transform.localEulerAngles.y, transform.localEulerAngles.z);
                else if (Input.GetKey(KeyCode.S))
                    transform.localRotation = Quaternion.Euler((transform.localEulerAngles.x + 359) % 360, transform.localEulerAngles.y, transform.localEulerAngles.z);
                else if (Input.GetKey(KeyCode.A))
                    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, (transform.localEulerAngles.y + 1) % 360, transform.localEulerAngles.z);
                else if (Input.GetKey(KeyCode.D))
                    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, (transform.localEulerAngles.y + 359) % 360, transform.localEulerAngles.z);
                else if (Input.GetKey(KeyCode.Q))
                    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, (transform.localEulerAngles.z + 1) % 360);
                else if (Input.GetKey(KeyCode.E))
                    transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, (transform.localEulerAngles.z + 359) % 360);

                // SCALE
                else if (Input.GetKey(KeyCode.KeypadMinus))
                    transform.localScale = transform.localScale * 0.95f;
                else if (Input.GetKey(KeyCode.KeypadPlus))
                    transform.localScale = transform.localScale * 1.05f;

                // NOTHING
                else return;

                Save();
            }

            void Save()
            {
                Directory.CreateDirectory("GameData/Sigma/Replacements/MenuScenes/Debug/");

                string[] data = new string[3];

                data[0] = "position = " + (Vector3d)transform.position;
                data[1] = "rotation = " + (Vector3d)transform.eulerAngles;
                data[2] = "scale = " + (Vector3d)transform.localScale;

                File.WriteAllLines("GameData/Sigma/Replacements/MenuScenes/Debug/" + name + ".txt", data);
            }
        }
    }
}
