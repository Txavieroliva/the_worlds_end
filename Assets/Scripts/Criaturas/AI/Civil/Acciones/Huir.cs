using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huir : Accion_Granjero
{
    public override float Calculo_Puntaje()  
    {
        if (MiDetector.Hostil_Cercano() == Vector3.zero)
        {
            return 0;
        } else 
        {
        return 100;
        }
    }
    
    public override void Ejecutar()
    {
        //corrije que aun no está completado
        Completado = false;
        Vector3 direccionHuida  = transform.position - MiDetector.Hostil_Cercano();

        // si está al lado del enemigo o lo suificientemente lejos, entonces está completado.
        if (MiDetector.Hostil_Cercano() == Vector3.zero)
        {
            Completado = true;
        } else 
        
        //lo mueve en la dirección contraria al enemigo más cercano.
        {
        MiMovedor.Mover((transform.position + direccionHuida));
        }
        
    }

    public override bool Revisar()
    {
        //si no esta completado, se ejecuta nuevamente, returnea que no está completado (si es que no lo está).
        if (Completado == false)
        {
            Ejecutar();
            return false;
        } else
        {
            //Lo mismo, pero lo deja quieto si ya está completado.
            MiMovedor.Mover(transform.position);
                return Completado;
        }
    }

}
