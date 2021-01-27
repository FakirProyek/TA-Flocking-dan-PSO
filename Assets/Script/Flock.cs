using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behaviors;
    private FlockBehaviorMono behaviorMono;
    public static int epoch = 0;
    //public float[] weights = { alignval, cohesionval, avoidval };

    [Range(10, 500)]
    public int startingFlock = 200;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(0f, 10f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidRadiusMultiplier = 0.5f;

    //[Header("behavior weights")]
    //public static float cohesionval, alignval, avoidval;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidRadius;
    public float SquareAvoidRadius { get { return squareAvoidRadius; } }
    public ParticleProgram partic;
    public bool PSO = false;

    public void PSOInit()
    {
        partic.Init();
    }
    public void PSOUpdate()
    {
        partic.UpdateParticleProgram();
    }
    // Start is called before the first frame update
    void Start() //Init Flock
    {
        if (PSO == true)//Kalo PSO dinyalain
        {
            PSOInit();
        }
        behaviorMono  = new CompositeMono();
        //behaviorMono.behaviors = new FlockBehaviorMono[4];
        

       //behaviorMono.weights = new float[4];
       
        //partic =  ParticleProgram();

        
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidRadius = squareNeighborRadius * avoidRadiusMultiplier * avoidRadiusMultiplier;
        
        for (int i = 0; i < startingFlock; i++)// instantiate agent buat agent ke game
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                UnityEngine.Random.insideUnitCircle * startingFlock * AgentDensity,
                Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0f, 360f)),
                transform);
            newAgent.name = "Agent" + i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PSO ==  true)
        {
            partic.UpdateParticleProgram();
        }

       
            foreach (FlockAgent agent in agents)// update ke seluruh agent
            {
            List<Transform> context = GetNearByObjects(agent);
                
            Vector2 move;
            if (PSO == true)
            {
                move = behaviorMono.CalculateMove(agent, context, this, partic.targetBestGlobalPos);
            }
            else
            {
                move = behaviors.CalculateMove(agent, context, this);
            }

            move *= driveFactor;

            if (move.sqrMagnitude > squareMaxSpeed)
            {

                move = move.normalized * maxSpeed;
            }
            agent.Move(move);

                
            //Debug.Log(Vector2.SqrMagnitude(this.transform.position - agent.transform.position));
            //Debug.Log(this.ToString() + "  " + agent.ToString());

            //AddRecord(this.ToString(), agent.ToString(), Vector2.SqrMagnitude(this.transform.position - agent.transform.position), "floflo.csv");
            //for (int i = 0; i < moveArray.Length; i++)
            //{
            //    if (moveArray[i].sqrMagnitude > squareMaxSpeed)
            //    {
            //        moveArray[i] = moveArray[i].normalized * maxSpeed;
            //    }
            //    agent.Move(moveArray[i]);
            //}
            }
            epoch++;
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter("AlingmentNew2.csv", true))


            //    Debug.Log((int)Time.fixedTime);
        
        



    }

    List<Transform> GetNearByObjects(FlockAgent agent)// inisiasi Collider
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach(Collider2D c in contextColliders)
        {
            if(c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }

        }
        
        return context;
        
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


