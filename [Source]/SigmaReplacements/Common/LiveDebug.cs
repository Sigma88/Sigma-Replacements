using System.IO;
using UnityEngine;


namespace SigmaReplacements
{
    internal class LiveDebug : MonoBehaviour
    {
        // Debug
        bool debug = false;

        // Original values
        internal int? index = null;
        Vector3 originalPosition;
        Quaternion originalRotation;
        Vector3 originalScale;

        // Settings
        float moveby;
        float rotateby;
        float scaleby;

        void Start()
        {
            // Original values
            originalPosition = transform.position;
            originalRotation = transform.localRotation;
            originalScale = transform.localScale;

            // Settings
            moveby = 500;
            rotateby = 50;
            scaleby = 0.5f;

            // SAVE
            StartCoroutine(CallbackUtil.DelayedCallback(1, Save));
        }

        void Update()
        {
            if (Input.anyKey)
            {
                if (debug)
                {
                    // POSITION
                    if (Input.GetKey(KeyCode.LeftArrow))
                        transform.position += Vector3.left * moveby * Time.deltaTime;
                    if (Input.GetKey(KeyCode.RightArrow))
                        transform.position += Vector3.right * moveby * Time.deltaTime;
                    if (Input.GetKey(KeyCode.DownArrow))
                        transform.position += Vector3.down * moveby * Time.deltaTime;
                    if (Input.GetKey(KeyCode.UpArrow))
                        transform.position += Vector3.up * moveby * Time.deltaTime;
                    if (Input.GetKey(KeyCode.RightControl))
                        transform.position += Vector3.back * moveby * Time.deltaTime;
                    if (Input.GetKey(KeyCode.RightShift))
                        transform.position += Vector3.forward * moveby * Time.deltaTime;

                    // ROTATION
                    if (Input.GetKey(KeyCode.S))
                        transform.Rotate(Vector3.left, rotateby * Time.deltaTime);
                    if (Input.GetKey(KeyCode.W))
                        transform.Rotate(Vector3.right, rotateby * Time.deltaTime);
                    if (Input.GetKey(KeyCode.D))
                        transform.Rotate(Vector3.down, rotateby * Time.deltaTime);
                    if (Input.GetKey(KeyCode.A))
                        transform.Rotate(Vector3.up, rotateby * Time.deltaTime);
                    if (Input.GetKey(KeyCode.E))
                        transform.Rotate(Vector3.back, rotateby * Time.deltaTime);
                    if (Input.GetKey(KeyCode.Q))
                        transform.Rotate(Vector3.forward, rotateby * Time.deltaTime);

                    // SCALE
                    if (Input.GetKey(KeyCode.Period))
                        transform.localScale *= 1 + scaleby * Time.deltaTime;
                    if (Input.GetKey(KeyCode.Comma))
                        transform.localScale /= 1 + scaleby * Time.deltaTime;
                }
            }
            else
            {
                // SAVE
                if (Input.GetKeyUp(KeyCode.F5))
                {
                    Save();
                }

                // LOAD
                else if (Input.GetKeyUp(KeyCode.F9))
                {
                    Load();
                }

                // RESET
                else if (Input.GetKeyUp(KeyCode.R))
                {
                    Reset();
                }
            }
        }

        void Save()
        {
            Directory.CreateDirectory("GameData/Sigma/Replacements/MenuScenes/Debug/");

            string[] data = new string[]
            {
                    "position = " + transform.position.x + ", "+ transform.position.y + ", "+ transform.position.z,
                    "rotation = " + transform.localEulerAngles.x + ", "+ transform.localEulerAngles.y + ", "+ transform.localEulerAngles.z,
                    "scale = " + transform.localScale.x + ", "+ transform.localScale.y + ", "+ transform.localScale.z,
                    "moveby = " + moveby,
                    "rotateby = " + rotateby,
                    "scaleby = " + scaleby,
                    "enabled = " + debug
            };

            File.WriteAllLines("GameData/Sigma/Replacements/MenuScenes/Debug/" + name + index + ".txt", data);
        }

        void Load()
        {
            string path = "GameData/Sigma/Replacements/MenuScenes/Debug/";

            if (Directory.Exists(path))
            {
                if (File.Exists(path + name + index + ".txt"))
                {
                    string[] data = File.ReadAllLines(path + name + index + ".txt");

                    if (bool.TryParse(data[6].Replace("enabled", "").Replace("=", ""), out debug) && debug)
                    {
                        transform.position = ConfigNode.ParseVector3(data[0].Replace("position", "").Replace("=", ""));
                        transform.localEulerAngles = ConfigNode.ParseVector3(data[1].Replace("rotation", "").Replace("=", ""));
                        transform.localScale = ConfigNode.ParseVector3(data[2].Replace("scale", "").Replace("=", ""));

                        if (float.TryParse(data[3].Replace("moveby", "").Replace("=", ""), out float move))
                        {
                            moveby = move;
                        }
                        if (float.TryParse(data[4].Replace("rotateby", "").Replace("=", ""), out float rotate))
                        {
                            rotateby = rotate;
                        }
                        if (float.TryParse(data[5].Replace("scaleby", "").Replace("=", ""), out float scale))
                        {
                            scaleby = scale;
                        }
                    }
                }
            }
        }

        void Reset()
        {
            if (debug)
            {
                transform.position = originalPosition;
                transform.localRotation = originalRotation;
                transform.localScale = originalScale;
            }
        }
    }
}
