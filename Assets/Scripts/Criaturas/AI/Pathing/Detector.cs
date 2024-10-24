using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private Vector3 Hostil_Mas_Cercano = Vector3.zero;
    private List<Collider> HostilesEnRango = new List<Collider>(); // Lista para almacenar hostiles dentro del rango



    void Update()
    {
        if (HostilesEnRango.Count > 0)  // Si hay hostiles en rango
        {
            ActualizarHostilMasCercano(); //Guarda al más cercano
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (EsHostil(collision))
        {
            HostilesEnRango.Add(collision);  // Agregar a la lista
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (HostilesEnRango.Contains(collision))
        {
            HostilesEnRango.Remove(collision);  // Eliminar de la lista
            // Debug.Log("Hostil salió: " + collision.gameObject.name);
            if (HostilesEnRango.Count == 0)
            {
                Hostil_Mas_Cercano = Vector3.zero;  // Reiniciar si no hay más hostiles
            }
        }
    }

    //Se fija cual de los hostiles está más cerca al transform y lo actualiza.
    private void ActualizarHostilMasCercano()
    {
        float distanciaMinima = Mathf.Infinity;
        Vector3 posicionCercana = Vector3.positiveInfinity;

        foreach (Collider hostil in HostilesEnRango) //por cada hostil en rango:
        {
            float distancia = Vector3.Distance(hostil.transform.position, transform.position); // detecta la distancia a sí mismo

            if (distancia < distanciaMinima) //si la distancia es menor a la distancia necesaria o de la ultima detección:
            {
                distanciaMinima = distancia;
                posicionCercana = hostil.transform.position;
            }
        }
            Hostil_Mas_Cercano = posicionCercana; // Actualiza la posición del hostil más cercano
    }

    private bool EsHostil(Collider collision)
    {
        var comportamiento = collision.gameObject.GetComponentInParent<Comportamiento>();
        return comportamiento != null && comportamiento.Hostil;
    }

    public Vector3 Hostil_Cercano()
    {
        if (Hostil_Mas_Cercano == Vector3.zero)
        {
            // Debug.Log("No se ha detectado ningún hostil.");
        }
        return Hostil_Mas_Cercano;
    }
}