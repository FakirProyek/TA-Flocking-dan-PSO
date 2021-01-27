using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsMono : FlockBehaviorMono
{
    
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock,Vector2 targetBestPos)
    {
        //ParticleProgram partic = new ParticleProgram();
        // if no neighbor, keep moving
        if (context.Count == 0)
            return agent.transform.up;

        //ParticleProgram partic = new ParticleProgram();
        Vector2 PSOBestPos = targetBestPos;
        //Debug.Log("MoveToward Best Pos" + PSOBestPos);
        //Vector2 PSOBestPos = new Vector2(posX,posY);
        //Debug.Log("Target = " + PSOBestPos);
        PSOBestPos -= (Vector2)agent.transform.position;
        //foreach (Transform item in context)
        //{

        //    PSOBestPos -= (((Vector2)agent.transform.position - (Vector2)item.transform.position));

        //}
        PSOBestPos /= context.Count ;



        return PSOBestPos;
    }

    
}
