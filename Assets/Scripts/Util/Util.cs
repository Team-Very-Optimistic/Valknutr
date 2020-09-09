using UnityEngine;

public class Util
{
    public int GetRandomWeightedIndex(int[] weights)
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

}