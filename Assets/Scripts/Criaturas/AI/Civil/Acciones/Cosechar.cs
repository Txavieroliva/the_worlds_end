using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosechar : Accion
{
    public float terrainSize = 10f; // Tamaño del terreno (10x10)   
    public override float Calculo_Puntaje()
    {
        return 10;
    }

    public override void Ejecutar()
    {

        // Verificar si ha pasado el tiempo de espera
        if (Completado)
        {
            // Generar una posición aleatoria en el terreno de 10x10
            Vector3 randomPos = new Vector3
            (
                Random.Range(-terrainSize / 2, terrainSize / 2), 
                0, 
                Random.Range(-terrainSize / 2, terrainSize / 2)
            );

            // Mover usando el Movedor
            MiMovedor.Mover(randomPos);
            Completado = false;
        }
    }

    public override bool Revisar()
    {
        // Revisar si el agente ha llegado a su destino
        if (!MiMovedor.Cuerpo.pathPending && MiMovedor.Cuerpo.remainingDistance <= MiMovedor.Cuerpo.stoppingDistance)
        {
            Completado = true;
        }

        return Completado;
    }
}
