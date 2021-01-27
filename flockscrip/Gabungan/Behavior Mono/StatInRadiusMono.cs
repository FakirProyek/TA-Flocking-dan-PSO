using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatInRadiusMono : FlockBehaviorMono
{
    public Vector2 center;
    public float radius = 2f;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock, Vector2 targetBestPos)
    {

        Vector2 centerOffset = targetBestPos - (Vector2)agent.transform.position;
        float t = centerOffset.magnitude / radius;
        if (t < 0.9f)
        {
            return Vector2.zero;
        }

        return centerOffset * t * t;
    }
}
