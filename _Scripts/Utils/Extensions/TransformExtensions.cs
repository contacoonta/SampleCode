using UnityEngine;

public static class TransformExtensions
{
    public static Transform FindDeepChild(this Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }

            Transform foundChild = child.FindDeepChild(childName);
            if (foundChild != null)
            {
                return foundChild;
            }
        }
        return null;
    }
}