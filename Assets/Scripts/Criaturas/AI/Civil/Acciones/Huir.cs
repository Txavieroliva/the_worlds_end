using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huir : Accion
{
    public override float Calculo_Puntaje()  
    {
        return 100;
    }
    
    public override void Ejecutar()
    {
        Completado = false;
        Vector3 direccionHuida  = transform.position - MiDetector.Hostil_Cercano();

        if (MiDetector.Hostil_Cercano() == Vector3.zero)
        {
            Completado = true;
        } else 
        {
        MiMovedor.Mover((transform.position + direccionHuida));
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
