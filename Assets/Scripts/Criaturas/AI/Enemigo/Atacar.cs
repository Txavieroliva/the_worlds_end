using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atacar : Accion
{
    public override float Calculo_Puntaje()  
    {
        if (MiDetector.Hostil_Cercano() == Vector3.zero)
        {
            return 100;
        } else 
        {
        return 0;
        }
    }
    
    public override void Ejecutar()
    {
        Completado = false;

        if (MiDetector.Hostil_Cercano() == Vector3.zero)
        {
            Completado = true;
            Debug.Log("Completado");
        } else 
        {
        MiMovedor.Mover(MiDetector.Hostil_Cercano());
        }
        
    }

    public override bool Revisar()
    {
        if (Completado == false)
        {
            Ejecutar();
            return Completado;
        } else
        {
            MiMovedor.Mover(transform.position);
            return Completado;
        }
    }

}
