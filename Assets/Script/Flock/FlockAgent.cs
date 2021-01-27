using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }
    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }
    Vector2 position;
    public Vector2 AgentPosition { get { return position; } }
    
    // Start is called before the first frame update
    void Start()
    {
        
        agentCollider = GetComponent<Collider2D>();
        
    }
    public void Initialize(Flock flock)
    {
        agentFlock = flock;
        
    }
    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obs")
        {
            Debug.Log("hit");
        }
    }
}
