using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.UI.Image;

public static class MathUtils
{
    public static Vector3 GetNearestPointOnSegment(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 n = (b-a).normalized;
        float p = Vector3.Dot(c - a, n);
        p = Mathf.Clamp(p, 0, Vector3.Distance(a,b));
        return a + n * p;
    }
}
