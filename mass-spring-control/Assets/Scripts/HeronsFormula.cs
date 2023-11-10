using System;
using UnityEngine;

public static class HeronsFormula
{
    public static float GetArea(Vector3 pos0, Vector3 pos1, Vector3 pos2)
    {
        var a = pos0 - pos1;
        var b = pos1 - pos2;
        var c = pos2 - pos0;

        return Herons(a.magnitude, b.magnitude, c.magnitude);
    }

    private static float Herons(float a, float b, float c)
    {
        float p = 0.5f * (a + b + c);
        return MathF.Sqrt(p * (p - a) * (p - b) * (p - c));
    }
}
