using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosechar : Accion
{

    public override float Calculo_Puntaje()  
    {
        return Puntos;
    }
    
    public override void Ejecutar()
    {
        
    }

    public override bool Revisar()
    {
        return Completado;
    }

}
