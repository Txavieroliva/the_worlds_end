using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atacar : Accion_Soldado
{
    public override float Calculo_Puntaje()  
    {

        if (MiDetector.Hostil_Cercano() == Vector3.zero || Vector3.Distance(Stats.Mi_Patrulla.position, transform.position)>(Stats.Mi_Patrulla.localScale.x /2)+10f)
        {
            return 0;
        } else 
        {
        return 100;
        }
    }
    
    public override void Ejecutar()
    {
        Completado = false;

        if (MiDetector.Hostil_Cercano() == Vector3.zero)
        {
            Completado = true;
            // MiMovedor.Mover(transform.position);
        } else 
        {
        MiMovedor.Mover(MiDetector.Hostil_Cercano());
        }
        
    }

    public override bool Revisar()
    {
      //si no esta completado, se ejecuta nuevamente, returnea que no está completado (si es que no lo está).
        if (Completado == false)
        {
            Ejecutar();
        } else
        {
            //Lo mismo, pero lo deja quieto si ya está completado.
            MiMovedor.Mover(transform.position);
        }
    return true;


    }
}
