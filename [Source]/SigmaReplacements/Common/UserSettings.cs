using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace SigmaReplacements
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    internal class UserSettings : MonoBehaviour
    {
        internal static ConfigNode ConfigNode
        {
            get
            {
                return GameDatabase.Instance?.GetConfigs(nodeName)?.FirstOrDefault(n => n.url == (folder.Substring(9) + file + "/" + nodeName))?.config;
            }
        }

        static string folder = "GameData/Sigma/Replacements/";
        static string file = "Settings";
        internal static string nodeName = "SigmaReplacements";

        void Awake()
        {
            string path = Assembly.GetExecutingAssembly().Location;

            if (!folder.StartsWith(Path.GetFullPath(folder)))
            {
                UnityEngine.Debug.Log(Debug.Tag + " WARNING: Incorrect plugin location => " + path);
            }

            if (!Directory.Exists(folder))
            {
                UnityEngine.Debug.Log(Debug.Tag + " WARNING: Missing folder => " + folder);
                Directory.CreateDirectory(folder);
            }

            if (!File.Exists(folder + file + ".cfg"))
            {
                UnityEngine.Debug.Log(Debug.Tag + " WARNING: Missing file => " + folder + file + ".cfg");

                File.WriteAllLines(folder + file + ".cfg", new[] { nodeName + " {}" });
                return;
            }

            if (ConfigNode.Load(folder + file + ".cfg")?.HasNode(nodeName) != true)
            {
                UnityEngine.Debug.Log(Debug.Tag + " WARNING: Missing node => " + folder + file + "/" + nodeName);

                File.AppendAllText(folder + file + ".cfg", "\r\n" + nodeName + " {}");
            }
        }

        void Start()
        {
            var configs = GameDatabase.Instance.GetConfigs(nodeName);

            for (int i = 0; i < configs?.Length; i++)
            {
                if (configs[i].url != (folder.Substring(9) + file + "/" + nodeName))
                    configs[i].parent.configs.Remove(configs[i]);
            }
        }
    }
}
