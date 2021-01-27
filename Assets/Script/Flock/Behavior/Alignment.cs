using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Flock/Behaviour/Alignment")]
public class Alignment : FlockBehavior
{

    static int epoch = 0;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {

        // if no neighbor, keep moving
        if (context.Count == 0)

            return (Vector2)agent.transform.up;
        //Debug.Log("agent ke = " + agent.ToString());
        Vector2 alignmentMove = Vector2.zero;
        float distance;
        
        foreach (Transform item in context)
        {

            alignmentMove += (Vector2)item.transform.up;
            //Debug.Log(item.ToString());
            distance = Vector2.SqrMagnitude(agent.transform.position - item.transform.position);
            //Debug.Log(distance);
            //AddRecord(agent.ToString(), item.ToString(), distance,"AlingmentNew2.csv");
            //AddRecord(agent.ToString(), item.ToString(), distance,(360 - agent.transform.rotation.eulerAngles.z),"Alingmentrot.csv");
            
        }
        alignmentMove /= context.Count;
        
        float alix = alignmentMove.x;
        float aliy = alignmentMove.y;
        
        //AddRecord(agent.ToString(), alix, aliy, "alignment3.csv");
        return alignmentMove;
    }

    public void AddRecord(string _agentName, string _flockmate, float _distancex,float _distancey, string filepath)
    {
        
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
            {
                file.WriteLine(_agentName + "," + _flockmate + "," + _distancex +  "," +  _distancey);
                
            }
        }
        catch (Exception ex)
        {
            throw new Exception("oopsie", ex);
        }
    }
}
