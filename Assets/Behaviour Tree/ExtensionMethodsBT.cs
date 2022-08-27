using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethodsBT
{
   public static float GetDistance(Vector2 p1,Vector2 p2)
    {
        return Mathf.Sqrt(Mathf.Pow((p2.x - p1.x), 2) + Mathf.Pow((p2.y - p1.y), 2));
    }

    public static Vector2 GetXZVector(Vector3 vector) => new Vector2(vector.x, vector.z);
}
