using System;
using System.Collections.Generic;


namespace SigmaReplacements
{
    namespace SkyBox
    {
        internal static class Extensions
        {
            internal static SkyBoxInfo Pick(this List<SkyBoxInfo> list)
            {
                if (list?.Count > 0)
                {
                    if (HighLogic.LoadedScene == GameScenes.SPACECENTER && HighLogic.CurrentGame != null)
                        return list[Math.Abs(HighLogic.CurrentGame.Seed) % list.Count];
                    else
                        return list[Math.Abs(DateTime.Today.GetHashCode()) % list.Count];
                }

                return null;
            }
        }
    }
}
