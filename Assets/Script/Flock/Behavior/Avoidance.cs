using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class Avoidance : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // if no neighbor
        if (context.Count == 0)
            return Vector2.zero;

        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;
        float distance;
        foreach (Transform item in context)
        {
            if(Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidRadius)
            {
                nAvoid++;
                avoidanceMove += (Vector2)(agent.transform.position - item.position);
            }
            distance = Vector2.SqrMagnitude(agent.transform.position - item.position);
            //Debug.Log(item.ToString());
            //Debug.Log(Vector2.SqrMagnitude(item.position - agent.transform.position));
            //AddRecord(agent.ToString(), item.ToString(), distance, "avoidance50.csv");
        }

        
        //Debug.Log(avoidanceMove);
        if(nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }
        float avoidx = avoidanceMove.x;
        float avoidy = avoidanceMove.y;
        //AddRecord(agent.ToString(), avoidx, avoidy, "avoid.csv");

        return avoidanceMove;
    }

    public void AddRecord(string _agentName, string _flockmate, float _distance, string filepath)
    {

        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
            {
                file.WriteLine(_agentName + "," + _flockmate + "," + _distance);

            }
        }
        catch (Exception ex)
        {
            throw new Exception("oopsie", ex);
        }
    }
}
