using UnityEngine;

public static class Extensions
{
    public static float DistanceXZ(this Vector3 v1, Vector3 v2)
    {
        Vector3 v = v1 - v2;

        v.y = 0;

        return v.magnitude;
    }
}
