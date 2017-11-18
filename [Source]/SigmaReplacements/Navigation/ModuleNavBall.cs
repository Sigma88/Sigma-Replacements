using System.Collections.Generic;


namespace SigmaReplacements
{
    namespace Navigation
    {
        public class ModuleNavBall : PartModule
        {
            internal static Dictionary<string, CustomNavBall> DataBase = new Dictionary<string, CustomNavBall>();

            public override void OnLoad(ConfigNode node)
            {
                if (HighLogic.LoadedScene == GameScenes.LOADING)
                {
                    ConfigNode NavBallNode = node.GetNode("NavBall");

                    CustomNavBall NavBall = gameObject.AddComponent<CustomNavBall>();

                    NavBallInfo info = new NavBallInfo(new ConfigNode(), NavBallNode ?? new ConfigNode());

                    NavBall.Pick(info, name: part.name);

                    if (!DataBase.ContainsKey(part.name))
                        DataBase.Add(part.name, NavBall);
                }
            }
        }
    }
}
