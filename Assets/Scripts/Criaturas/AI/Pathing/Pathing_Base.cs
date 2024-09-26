using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Pathing_Base : MonoBehaviour
{
public Transform Jugador;
public NavMeshAgent Agente;
    // Start is called before the first frame update
    void Start()
    {
    
        Agente = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
     Agente.destination = Jugador.position;

    }
}
