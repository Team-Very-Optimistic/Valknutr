using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
    /// Performs a depth-first search of the transforms associated to the given transform, in search
    /// of a descendant with the given name.  Avoid using this method on a frame-by-frame basis, as
    /// it is recursive and quite capable of being slow!
    /// </summary>
    /// <param name="searchTransform">Transform to search within</param>
    /// <param name="descendantName">Name of the descendant to find</param>
    /// <returns>Descendant transform if found, otherwise null.</returns>
    public static Transform FindDescendentTransform(this Transform searchTransform, string descendantName)
    {
        Transform result = null;
 
        int childCount = searchTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = searchTransform.GetChild(i);
 
            // Not it, but has children? Search the children.
            if (childTransform.name != descendantName
                && childTransform.childCount > 0)
            {
                Transform grandchildTransform = FindDescendentTransform(childTransform, descendantName);
                if (grandchildTransform == null)
                    continue;
 
                result = grandchildTransform;
                break;
            }
            // Not it, but has no children?  Go on to the next sibling.
            else if (childTransform.name != descendantName
                     && childTransform.childCount == 0)
            {
                continue;
            }
 
            // Found it.
            result = childTransform;
            break;
        }
 
        return result;
    }

    /// <summary>
    /// Performs a depth-first search of the transforms associated to the given transform, in search
    /// of a descendant with the given name.  Avoid using this method on a frame-by-frame basis, as
    /// it is recursive and quite capable of being slow!
    /// </summary>
    /// <param name="searchTransform">Transform to search within</param>
    /// <param name="predicate">Predicate to match</param>
    /// <returns>Descendant transform if found, otherwise null.</returns>
    public static List<Transform> FindChildrenByPredicate(this Transform searchTransform, Predicate<Transform> predicate)
    {
        List<Transform> result = new List<Transform>();
        
        if (predicate(searchTransform))
        {
            result.Add(searchTransform);
        }
 
        int childCount = searchTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = searchTransform.GetChild(i);
            result.AddRange(FindChildrenByPredicate(childTransform, predicate));
        }
 
        return result;
    }

    /// <summary>
    /// Retrieves the generic component, if it doesnt exist add it instead
    /// </summary>
    public static T GetComponentElseAddIt<T>(this GameObject obj) where T : Component
    {
        T temp;
        temp = obj.GetComponent<T>();
        if (temp == null)
        {
            temp = obj.AddComponent<T>();
        }

        return temp;
    }

    /// <summary>
    /// Returns a random rotation to the cardinal directions
    /// </summary>
    /// <returns></returns>
    public static Quaternion RandomRotationXZ()
    {
        return Quaternion.Euler(Vector3.up * Random.Range(0, 4) * 90);
    }
    
    public static Vector3 GetMousePositionOnWorldPlane(Camera camera)
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        var d = -Vector3.Dot(ray.origin, Vector3.up) / Vector3.Dot(ray.direction, Vector3.up);
        return ray.origin + ray.direction * d;
    }
}