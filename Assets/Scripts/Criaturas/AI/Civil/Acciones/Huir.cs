using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Huir : Accion
{
    public Detector MiDetector;
    public Movedor MiMovedor;
    public Accion huir;

private void Start()
{
huir = this;
}
    public override float Calculo_Puntaje()  
    {
        return 100;
    }
    
    public override void Ejecutar()
    {
        Vector3 direccionHuida  = transform.position - MiDetector.Hostil_Cercano().normalized;
        MiMovedor.Mover((transform.position + direccionHuida));
    }

    public override bool Revisar()
    {
        return Completado;
    }

}
