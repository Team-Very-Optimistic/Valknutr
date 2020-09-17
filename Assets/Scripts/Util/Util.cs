using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Util
{
    public static int GetRandomWeightedIndex(int[] weights)
    {
        int weightSum = 0;
        for (int i = 0; i < weights.Length; ++i)
        {
            weightSum += weights[i];
        }
 
        int index = 0;
        int lastIndex = weights.Length - 1;
        int weightedIndex = Random.Range(0, weightSum);
        while (index < lastIndex)
        {
            if (weightedIndex < weights[index])
            {
                return index;
            }
            weightedIndex -= weights[index++];
        }
        return lastIndex;
    }

    public static T RandomItem<T>(IEnumerable<T> enumerable)
    {
        var list = enumerable.ToList();
        var index = Random.Range(0, list.Count);
        return list[index];
    }

    /// <summary>
    /// Returns a random rotation to the cardinal directions
    /// </summary>
    /// <returns></returns>
    public static Quaternion RandomRotationXZ()
    {
        return Quaternion.Euler(Vector3.up * Random.Range(0, 4) * 90);
    }
}