using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accion_Soldado : Accion
{
    public Estadisticas_Soldado Stats;
    void Start()
    {
        // Encuentra los componentes en el mismo objeto (si están ahí)
        MiDetector = GetComponentInParent<Detector>();
        MiMovedor = GetComponent<Movedor>();
        Stats = GetComponentInParent<Estadisticas_Soldado>();

        
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