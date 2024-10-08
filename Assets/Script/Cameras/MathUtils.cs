using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{

    public static Vector3 LinearBezier(Vector3 a, Vector3 b, float t) => (1 - t) * a + t * b;

    public static Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t) =>
        (1 - t) * LinearBezier(a, b, t) + t * LinearBezier(b, c, t);
    
    public static Vector3 CubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t) =>
        (1 - t) * QuadraticBezier(a, b, c, t) + t * QuadraticBezier(b, c, d, t);

}

[System.Serializable]
public class Curve
{
    public Vector3 A, B, C, D;

    public Vector3 GetPosition(float t)
    {
        return MathUtils.CubicBezier(A, B, C, D, t);
    }

    public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix)
    {
        Vector3 v = GetPosition(t);
        return localToWorldMatrix.MultiplyPoint(v);
    }

    public void DrawGizmos(Color c, Matrix4x4 localToWorldMatrix)
    {
        Gizmos.color = c;
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(A), 0.1f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(B), 0.1f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(C), 0.1f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(D), 0.1f);
        
        for(float i = 0.01f; i <= 1; i += 0.01f)
        {
            Gizmos.DrawLine(GetPosition(i - 0.01f, localToWorldMatrix), GetPosition(i, localToWorldMatrix));
        }
    }
}
