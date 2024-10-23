using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accion_Granjero : Accion
{
    public Estadisticas_Civil Stats;

void Start()
    {
        // Encuentra los componentes en el mismo objeto (si están ahí)
        MiDetector = GetComponentInParent<Detector>();
        MiMovedor = GetComponentInParent<Movedor>();
        Stats = GetComponent<Estadisticas_Civil>();

        
        if (MiDetector == null)
        {
            Debug.LogError("No se encontró Detector en el GameObject");
        }
        if (MiMovedor == null)
        {
            Debug.LogError("No se encontró Movedor en el GameObject");
        }
    }
}

