using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentMono : FlockBehaviorMono
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock, Vector2 targetBestPos)
    {
        if (context.Count == 0)
        return agent.transform.up;
            //agent.transform.up
        Vector2 alignmentMove = Vector2.zero;
        foreach (Transform item in context)
        {
            alignmentMove += (Vector2)item.transform.up;
        }
        alignmentMove /= context.Count;

        return alignmentMove;
    }

    
}
