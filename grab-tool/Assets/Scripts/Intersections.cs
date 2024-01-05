using UnityEngine;

public static class Intersections
{
    public static bool RayPlane(Ray ray, Vector3 point, Vector3 planeNormal, out RaycastHit hit)
    {
        var denom = Vector3.Dot(planeNormal, ray.direction);
        if (Mathf.Abs(denom) > Mathf.Epsilon)
        {
            var t = Vector3.Dot(point - ray.origin, planeNormal) / denom;
            if (t >= 0)
            {
                hit = new RaycastHit
                {
                    point = ray.GetPoint(t)
                };
                return true;
            }
        }
        
        hit = new RaycastHit();
        return false;
    }
}