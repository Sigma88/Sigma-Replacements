using System;
using System.Collections.Generic;


namespace SigmaReplacements
{
    namespace MenuScenes
    {
        internal static class PseudoRandom
        {
            static int max = 3;
            static double type = 0.5;
            static Dictionary<object, double[]> chances = new Dictionary<object, double[]>();

            internal static int Scene(int hash)
            {
                if (Math.Pow(hash, 0.5) % 1 < type)
                {
                    type = Math.Max(0, type - 1d / max);

                    return 0;
                }
                else
                {
                    type = Math.Min(1, type + 1d / max);

                    return 1;
                }
            }

            internal static int Choose(this List<MenuSceneInfo> list, int hash)
            {
                if (!(list?.Count > 1)) return 0;

                if (!chances.ContainsKey(list))
                {
                    Debug.Log("PseudoRandom.Choose", "Adding to dictionary new List<" + list[0].GetType().Name + ">");

                    double[] newChances = new double[list.Count];

                    double maxChance = 100;
                    int adjustedCount = list.Count;

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].useChance > 0)
                        {
                            newChances[i] = 100 * list[i].useChance.Value;
                            maxChance -= 100 * list[i].useChance.Value;
                            adjustedCount--;
                        }
                    }

                    for (int i = 0; i < newChances.Length; i++)
                    {
                        if (!(list[i].useChance > 0))
                        {
                            newChances[i] = maxChance / adjustedCount;
                        }
                    }

                    chances.Add(list, newChances);
                }

                double random = Math.Pow(hash, 0.5) % 100d;
                double sum = 0;

                for (int i = 0; i < chances[list].Length; i++)
                {
                    sum += chances[list][i];
                    if (random < sum)
                    {
                        double penalty = Math.Min(chances[list][i], (100d / max) / chances[list].Length);
                        double bonus = penalty / (chances[list].Length - 1);

                        for (int j = 0; j < chances[list].Length; j++)
                        {
                            chances[list][j] += bonus;
                        }

                        chances[list][i] = chances[list][i] - bonus - penalty;
                        return i;
                    }
                }

                return 0;
            }
        }
    }
}
