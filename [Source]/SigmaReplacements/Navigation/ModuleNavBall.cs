using System.Collections.Generic;


namespace SigmaReplacements
{
    namespace Navigation
    {
        public class ModuleNavBall : PartModule
        {
            internal static Dictionary<string, CustomNavBall> DataBase = new Dictionary<string, Navigation.CustomNavBall>();

            public override void OnLoad(ConfigNode node)
            {
                if (HighLogic.LoadedScene == GameScenes.LOADING)
                {
                    ConfigNode CustomNavBall = node.GetNode("CustomNavBall");

                    CustomNavBall NavBall = gameObject.AddComponent<CustomNavBall>();

                    NavBallInfo info = new NavBallInfo(new ConfigNode(), CustomNavBall ?? new ConfigNode());

                    NavBall.Pick(info);

                    if (!DataBase.ContainsKey(part.name))
                        DataBase.Add(part.name, NavBall);
                }
            }
        }
    }
}
