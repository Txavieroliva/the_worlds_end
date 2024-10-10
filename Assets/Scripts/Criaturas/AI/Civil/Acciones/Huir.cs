using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huir : Accion
{
    public Detector MiDetector;
    public Movedor MiMovedor;
    public Accion huir;

private void start()
{
huir = this;
}
    public override float Calculo_Puntaje()  
    {
        return 100;
    }
    
    public override void Ejecutar()
    {
        Vector3 NuevaPos = transform.position - MiDetector.Hostil_Cercano();
        MiMovedor.Mover((transform.position + NuevaPos));
    }

    public override bool Revisar()
    {
        return Completado;
    }

}
