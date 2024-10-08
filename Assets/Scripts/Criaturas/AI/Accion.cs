using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accion : Accion_Base
{
    public Estadisticas Stats;
    public float Puntos;
    public bool Completado;

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
