using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flock/Behaviour/Move To")]
public class MoveTowards : FlockBehavior
{

    public float posX = 100;
    public float posY = 0;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {

        //ParticleProgram partic = new ParticleProgram();
        // if no neighbor, keep moving
        if (context.Count == 0)
        {
            return (Vector2)agent.transform.up;
        } 
            

        //ParticleProgram partic = new ParticleProgram();
        Vector2 PSOBestPos = new Vector2(posX,posY);
        //Debug.Log("MoveToward Best Pos" + PSOBestPos);
        //Vector2 PSOBestPos = new Vector2(posX,posY);
        //Debug.Log("Target = " + PSOBestPos);
        foreach (Transform item in context)
        {

            PSOBestPos -= (((Vector2)agent.transform.position - (Vector2)item.transform.position));

        }
        PSOBestPos /= (context.Count) ;
        


        return PSOBestPos;
    }


}
