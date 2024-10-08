using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Movedor : MonoBehaviour
{
public NavMeshAgent Cuerpo;
    void Start()
    {
        Cuerpo = GetComponent<NavMeshAgent>();
    }

    public void Mover(Vector3 Objetivo)
    {
     Cuerpo.destination = Objetivo;
    }
}
