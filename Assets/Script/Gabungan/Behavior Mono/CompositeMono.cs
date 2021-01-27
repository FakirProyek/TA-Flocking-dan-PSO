using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeMono : FlockBehaviorMono
{

    public FlockBehaviorMono[] behaviors;
    public float[] weights;

    public CompositeMono()
    {

        behaviors = new FlockBehaviorMono[5];
        weights = new float[5];
        behaviors[0] = new AlignmentMono();
        behaviors[1] = new AvoidMono();
        behaviors[2] = new CohesionMono();
        behaviors[3] = new MoveTowardsMono();
        behaviors[4] = new StatInRadiusMono();

        weights[0] = 1;
        weights[1] = 2;
        weights[2] = 4;
        weights[3] = 0;
        weights[4] = 1;
    }
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock, Vector2 targetBestPos)
    {
        //Debug.Log("composite best Pos" + targetBestPos);
        if (weights.Length != behaviors.Length)
        {
            Debug.Log("kurang Behavior" +  this);
            return Vector2.zero;
        }

        Vector2 move = Vector2.zero;

        for (int i = 0; i < behaviors.Length; i++)
        {
            Vector2 partialMove = behaviors[i].CalculateMove(agent, context, flock, targetBestPos) * weights[i];
            
            if (partialMove != Vector2.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    //partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }

        return move;
    }



    
}
