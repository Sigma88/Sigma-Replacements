using UnityEngine;
using Kopernicus.Components.ModularScatter;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal static class Extensions
        {
            internal static Mesh GetModularScatterMesh(this CelestialBody planet, PQSLandControl.LandClassScatter scatter)
            {
                Mesh mesh = null;

                if (scatter != null)
                {
                    if (scatter.baseMesh == null || scatter.baseMesh.name == "Kopernicus-CubeDummy")
                    {
                        ModularScatter[] scatters = planet.GetComponentsInChildren<ModularScatter>(true);

                        for (int i = 0; i < scatters?.Length; i++)
                        {
                            ModularScatter modularScatter = scatters[i];

                            if (scatter == modularScatter?.scatter)
                            {
                                return modularScatter.baseMesh;
                            }
                        }
                    }
                }

                return mesh;
            }
        }
    }
}
