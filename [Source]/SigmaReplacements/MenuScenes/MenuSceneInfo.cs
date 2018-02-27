namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal class MenuSceneInfo
        {
            // Requirements
            internal string name = "";
            internal bool enabled = true;

            // New MenuSceneInfo
            internal MenuSceneInfo()
            {
            }

            // New MenuScenesInfo From Name
            internal MenuSceneInfo(string name)
            {
                this.name = name;
            }
        }
    }
}
