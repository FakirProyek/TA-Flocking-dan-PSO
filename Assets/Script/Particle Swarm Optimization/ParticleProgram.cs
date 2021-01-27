using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.IO;
using System;

public class ParticleProgram : MonoBehaviour
{
    public static ParticleProgram instance;

    [Header("Settings")]
    // Gameobject prefab
    public GameObject prefab;
    // Update frequency / second
    public float updateFrequency;

    [Header("Target")]
    public Transform targetObject;
    public Vector2 targetCurrentPosition;
    public float holdSpawnTime;

    [Header("Solver")]
    public int dim = 2;
    public int numParticle = 5;
    public float minX = -1;
    public float maxX = 1;
    public int maxEpochs = 50;
    public float exitError = 0f;

    [Header("Result")]
    public double[] bestPosition;
    public double bestError;

    [Header("Debug")]
    public bool useTestReference;

    [Header("PSO Variable")]
    public double w = 0.729; // inertia weight. see http://ieeexplore.ieee.org/stamp/stamp.jsp?arnumber=00870279
    public double c1 = 1.49445; // cognitive/local weight
    public double c2 = 1.49445; // social/global weight
    public double r1 = 0, r2 = 0; // cognitive and social randomizations
    public double probDeath = 0.01;
    public Vector2 resul;
    
     

    #region Test reference
    int dimRef = 2; // problem dimensions
    int numParticlesRef = 5;
    int maxEpochsRef = 1000;
    double exitErrorRef = 0.0; // exit early if reach this error
    double minXRef = -10.0; // problem-dependent
    double maxXRef = 10.0;
    #endregion
    float timer;
    public bool changeRoute;
    public bool swarming;
    Particle[] swarm = new Particle[0];
    public Vector2 targetPosition;
    public Vector2 targetBestPos;
    public Vector2 TargetBestPos { get { return targetBestPos; } }
    public Vector2 targetBestError;
    public Vector2 targetBestGlobalPos;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void getResult()
    {
        Debug.Log(resul);
    }

    public void Init()
    {
        targetCurrentPosition = targetObject.transform.position;
    }

    public void UpdateParticleProgram()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !swarming)
        {
            timer += Time.deltaTime;
            swarming = true;
            if (useTestReference)
            {
                StartCoroutine(Solve(dimRef, numParticlesRef, minXRef, maxXRef, maxEpochsRef, exitErrorRef));
            }
            else
            {
               StartCoroutine(Solve(dim, numParticle, minX, maxX, maxEpochs, exitError));
            }
        }

        if ((Vector2)targetObject.position != targetCurrentPosition)
        {
            targetCurrentPosition = targetObject.position;
            changeRoute = true;
        }
    }
    //error untuk pencarian posisi player
    double TargetPos(double[] x)
    {
        //double z = 100 * ((x[1] - (x[0] * x[0])) * (x[1] - (x[0] * x[0]))) + (1 - x[0]) * (1 - x[0]);
        Vector2 position = targetObject.position;
        Vector2 swarmPosition = new Vector2((float)x[0], (float)x[1]);
        double z = Vector2.Distance(position, swarmPosition);
        return z;
    }

    //error untuk double dip func
    double DoubleDip(double[] x)
    {
        double trueMin = -0.42888194;
        double z = x[0] * Mathf.Exp(-((float)(x[0] * x[0]) + (float)(x[1] * x[1])));
        return (z - trueMin) * (z - trueMin);
    }


    //error untuk rosenbrock func
    double Rosenbrock(double[] x)
    {
        double z = 100 * ((x[1] - (x[0] * x[0])) * (x[1] - (x[0] * x[0]))) + (1 - x[0]) * (1 - x[0]);
        return z;
    }

    double Error(double[] x)
    {
        return TargetPos(x);
        //return Rosenbrock(x);
        //return DoubleDip(x);
    }

    public Particle SpawnParticle()
    {
        Particle a = Instantiate(prefab).GetComponent<Particle>();
        return a;
    }

    public IEnumerator Solve(int dim, int numParticles, double minX, double maxX, int maxEpochs, double exitError)
    {
        yield return null;
        double[] bestGlobalPosition = new double[dim]; // best solution found by any particle in the swarm
        double bestGlobalError = double.MaxValue; // smaller values better

        // buat dynamic
        bool spawn = false;
        if (swarm.Length == 0)
        {
            swarm = new Particle[numParticles];
            spawn = true;
            holdSpawnTime = 2f;
        }
        else {
            holdSpawnTime = 0;
        }

        if (spawn)
        {
            for (int i = 0; i < swarm.Length; ++i)
            {
                double[] randomPosition = new double[dim];
                for (int j = 0; j < randomPosition.Length; ++j)
                    randomPosition[j] = (maxX - minX) * UnityEngine.Random.Range(0.0f, 1.0f) + minX; // 

                double error = Error(randomPosition);
                double[] randomVelocity = new double[dim];

                for (int j = 0; j < randomVelocity.Length; ++j)
                {
                    double lo = minX * 0.1;
                    double hi = maxX * 0.1;
                    randomVelocity[j] = (hi - lo) * UnityEngine.Random.Range(0.0f, 1.0f) + lo;
                }


                // buat dynamic
                swarm[i] = SpawnParticle();
                swarm[i].SetValue(randomPosition, error, randomVelocity, randomPosition, error);

                // does current Particle have global best position/solution?
                if (swarm[i].error < bestGlobalError)
                {
                    bestGlobalError = swarm[i].error;
                    swarm[i].position.CopyTo(bestGlobalPosition, 0);
                }

                Vector3 targetPosition = new Vector3((float)swarm[i].position[0], (float)swarm[i].position[1]);
                swarm[i].transform.position = targetPosition;
            }
        }
        // swarm initialization
        // initialization

        // prepare
        int epoch = 0;

        double[] newVelocity = new double[dim];
        double[] newPosition = new double[dim];
        double newError = 0;

        // Swarming
        StartCoroutine(Swarming(dim, epoch, maxEpochs, swarm, r1, r2, newVelocity, w, c1, c2, bestGlobalPosition, newPosition, minX,
            maxX, newError, bestGlobalError, probDeath));

    } // Solve

    public IEnumerator Swarming(int dim, int epoch, int maxEpochs, Particle[] swarm, double r1, double r2,
        double[] newVelocity, double w, double c1, double c2, double[] bestGlobalPosition, double[] newPosition,
        double minX, double maxX, double newError, double bestGlobalError, double probDeath)
    {
        yield return new WaitForSeconds(holdSpawnTime);

        while (epoch < maxEpochs)
        {
            for (int i = 0; i < swarm.Length; ++i) // each Particle
            {
                Particle currP = swarm[i]; // for clarity

                // new velocity
                for (int j = 0; j < currP.velocity.Length; ++j) // each component of the velocity
                {
                    r1 = UnityEngine.Random.Range(0.0f, 1.0f);
                    r2 = UnityEngine.Random.Range(0.0f, 1.0f);

                    newVelocity[j] = (w * currP.velocity[j]) +
                      (c1 * r1 * (currP.bestPosition[j] - currP.position[j])) +
                      (c2 * r2 * (bestGlobalPosition[j] - currP.position[j]));
                }
                newVelocity.CopyTo(currP.velocity, 0);

                // new position
                for (int j = 0; j < currP.position.Length; ++j)
                {
                    newPosition[j] = currP.position[j] + newVelocity[j];
                    if (newPosition[j] < minX)
                        newPosition[j] = minX;
                    else if (newPosition[j] > maxX)
                        newPosition[j] = maxX;
                }
                newPosition.CopyTo(currP.position, 0);

                newError = Error(newPosition);
                currP.error = newError;

                if (newError < currP.bestError)
                {
                    newPosition.CopyTo(currP.bestPosition, 0);
                    currP.bestError = newError;
                }

                if (newError < bestGlobalError)
                {
                    newPosition.CopyTo(bestGlobalPosition, 0);
                    bestGlobalError = newError;
                }

                // death?
                double die = UnityEngine.Random.Range(0.0f, 1.0f);
                if (die < probDeath)
                {
                    // new position, leave velocity, update error
                    for (int j = 0; j < currP.position.Length; ++j)
                        currP.position[j] = (maxX - minX) * UnityEngine.Random.Range(0.0f, 1.0f) + minX;
                    currP.error = Error(currP.position);
                    currP.position.CopyTo(currP.bestPosition, 0);
                    currP.bestError = currP.error;

                    if (currP.error < bestGlobalError) // global best by chance?
                    {
                        bestGlobalError = currP.error;
                        currP.position.CopyTo(bestGlobalPosition, 0);
                    }
                }
                targetPosition = new Vector3((float)currP.position[0], (float)currP.position[1]);
                targetBestPos = new Vector2((float)currP.bestPosition[0], (float)currP.bestPosition[1]);
                targetBestGlobalPos = new Vector2((float)bestGlobalPosition[0], (float)bestGlobalPosition[1]);
                targetBestError = new Vector2((float)currP.bestError, (float)currP.bestError);
                currP.transform.DOMove(targetPosition, updateFrequency);
                //System.Threading.Thread.Sleep(2000);
                //Debug.Log(targetBestGlobalPos);
                //Debug.Log("posisi = "+ targetPosition);
                //Debug.Log("best pos = " + targetBestPos);
                //Debug.Log(currP.bestError);
                //Debug.Log(bestGlobalError);
                //Debug.Log( targetBestPos + "");
                if (changeRoute)
                    break;
                yield return new WaitForSeconds(updateFrequency);
            } // each Particle
            if (changeRoute)
                break;
            ++epoch;
            //Debug.Log("iterasi =" + epoch);
            //Debug.Log(" error =" + (100 - bestGlobalError));
            //AddRecord((100 - bestGlobalError), epoch, "errorPercobaanstresstest.csv");

            //Debug.Log(epoch);
        } // while



        if (changeRoute)
        {
            changeRoute = false;
            if (useTestReference)
            {
                StartCoroutine(Solve(dimRef, numParticlesRef, minXRef, maxXRef, maxEpochsRef, exitErrorRef));
            }
            else
            {
                StartCoroutine(Solve(dim, numParticle, minX, maxX, maxEpochs, exitError));
            }
        }
        else{
            double[] result = new double[dim];
            bestGlobalPosition.CopyTo(result, 0);
            bestPosition = result;
            bestError = Error(bestPosition);
            resul = new Vector2((float)result[0], (float)result[1]);
            getResult();
            Debug.Log(resul);
            Debug.Log("Processing complete");
            Debug.Log("Final swarm");
            
        }
    }

    public static void AddRecord(double _error,int _epoch,string filepath)
    {
        try
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, true))
            {
                file.WriteLine(_epoch + "," + _error);
            }
        }
        catch(Exception ex)
        {
            throw new Exception("oopsie", ex);
        }
    }
}
