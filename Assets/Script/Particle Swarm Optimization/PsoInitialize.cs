using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsoInitialize : MonoBehaviour
{

    public ParticleProgram partic;
    // Start is called before the first frame update
    void Start()
    {
        partic.Init();
    }

    // Update is called once per frame
    void Update()
    {
        partic.UpdateParticleProgram();
    }
}
