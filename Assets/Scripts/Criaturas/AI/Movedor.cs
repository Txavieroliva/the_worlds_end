using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Movedor : MonoBehaviour
{
public NavMeshAgent Cuerpo;
    void Start()
    {
        Cuerpo = GetComponentInParent<NavMeshAgent>();
    }

    public void Mover(Vector3 Objetivo)
    {
    Vector3 Punto_En_Navmesh = new Vector3(Objetivo.x, Objetivo.y, Objetivo.z);
    Cuerpo.SetDestination(Punto_En_Navmesh);   
    }
}
