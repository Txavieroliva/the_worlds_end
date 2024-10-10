using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Estadisticas : MonoBehaviour
{
    public Comportamiento Mi_Comportamiento;

    void Start()
    {
        Mi_Comportamiento = GetComponent<Comportamiento>();
    }
 
}
