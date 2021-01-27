using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CohesionMono : FlockBehaviorMono
{
    Vector2 currentVelocity;
    public float agentSmoothTime = 0.6f;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock, Vector2 targetBestPos)
    {
        // if no neighbor
        if (context.Count == 0)
            return Vector2.zero;

        Vector2 cohesionMove = Vector2.zero;
        foreach (Transform item in context)
        {
            cohesionMove += (Vector2)item.position;
        }
        cohesionMove /= context.Count;


        cohesionMove -= (Vector2)agent.transform.position;
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime);
        return cohesionMove;
    }

    
}
