using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accion : Accion_Base
{
    public Estadisticas Stats;
    public float Puntos;
    public bool Completado = false;
    public Detector MiDetector;
    public Movedor MiMovedor;

void Start()
    {
        // Encuentra los componentes en el mismo objeto (si están ahí)
        MiDetector = GetComponentInParent<Detector>();
        MiMovedor = GetComponentInParent<Movedor>();
        
        if (MiDetector == null)
        {
            Debug.LogError("No se encontró Detector en el GameObject");
        }
        if (MiMovedor == null)
        {
            Debug.LogError("No se encontró Movedor en el GameObject");
        }
    }
    public virtual float Calculo_Puntaje()  
    {
        return Puntos;
    }
    
    public override void Ejecutar()
    {
        
    }

    public virtual bool Revisar()
    {
        return Completado;
    }

}
