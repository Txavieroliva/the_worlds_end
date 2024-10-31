using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private Vector3 Hostil_Mas_Cercano = Vector3.zero;
    private List<Collider> HostilesEnRango = new List<Collider>(); // Lista para almacenar hostiles dentro del rango
    private bool necesitaActualizarHostilCercano = false; // Marca si es necesario actualizar el más cercano

    void Update()
    {
        if (necesitaActualizarHostilCercano && HostilesEnRango.Count > 0)  // Solo si es necesario actualizar y hay hostiles
        {
            ActualizarHostilMasCercano();
            necesitaActualizarHostilCercano = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (EsHostil(collision))
        {
            HostilesEnRango.Add(collision); // Agrega el hostil a la lista
            necesitaActualizarHostilCercano = true; // Marca para actualizar el hostil más cercano
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (HostilesEnRango.Remove(collision)) // Elimina si está en la lista y devuelve true
        {
            if (HostilesEnRango.Count == 0)
            {
                Hostil_Mas_Cercano = Vector3.zero;  // Reinicia si no hay más hostiles
                necesitaActualizarHostilCercano = false;
            }
            else
            {
                necesitaActualizarHostilCercano = true; // Marca para actualizar el hostil más cercano
            }
        }
    }

    // Se fija cuál de los hostiles está más cerca al transform y lo actualiza.
    private void ActualizarHostilMasCercano()
    {
        float distanciaMinimaSqr = Mathf.Infinity;
        Vector3 posicionCercana = Vector3.positiveInfinity;
        Vector3 posicionActual = transform.position;

        foreach (Collider hostil in HostilesEnRango) // Recorre solo hostiles en rango
        {
            float distanciaSqr = (hostil.transform.position - posicionActual).sqrMagnitude; // Usa magnitud cuadrada

            if (distanciaSqr < distanciaMinimaSqr) // Si es más cercano
            {
                distanciaMinimaSqr = distanciaSqr;
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
        return Hostil_Mas_Cercano; // Devuelve la posición actual del hostil más cercano
    }
}
