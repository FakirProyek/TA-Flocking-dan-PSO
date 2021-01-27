using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
[CreateAssetMenu(menuName = "Flock/Behaviour/Steered Cohesion")]
public class SteeredCohesion : FlockBehavior
{

    Vector2 currentVelocity;
    public float agentSmoothTime = 0.6f;
    

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // if no neighbor
        if (context.Count == 0)
            return Vector2.zero;

        Vector2 cohesionMove = Vector2.zero;
        float distance;
        foreach (Transform item in context)
        {
            cohesionMove += (Vector2)item.position;
            distance = Vector2.SqrMagnitude(agent.transform.position - item.position);
            //AddRecord(agent.ToString(), item.ToString(), distance, "cohesion10.csv");
        }
        cohesionMove /= context.Count;
        //Debug.Log(agent.ToString());
        //Debug.Log(cohesionMove);
        float cohex = cohesionMove.x;
        float cohey = cohesionMove.y;
        //AddRecord(cohex, cohey, agent.ToString(), "cohesion.csv");

        cohesionMove -= (Vector2)agent.transform.position;
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove,ref currentVelocity, agentSmoothTime);
        return cohesionMove;
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
