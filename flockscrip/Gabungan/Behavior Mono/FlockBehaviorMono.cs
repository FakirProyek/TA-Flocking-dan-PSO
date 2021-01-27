using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehaviorMono // constructor flocking
{

    public FlockBehaviorMono[] behaviors;
    public float[] weights;
    public Vector2 targetBestPos;
    public abstract Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock, Vector2 targetBestPos);
}
