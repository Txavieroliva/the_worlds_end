using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public Vector3 Hostil_Mas_Cercano = Vector3.zero;
    private List<Collider> HostilesEnRango = new List<Collider>();
    private bool necesitaActualizarHostilCercano = false;

    void Update()
    {
        if (necesitaActualizarHostilCercano && HostilesEnRango.Count > 0)
        {
            ActualizarHostilMasCercano();
            necesitaActualizarHostilCercano = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (EsHostil(collision))
        {
            HostilesEnRango.Add(collision);
            necesitaActualizarHostilCercano = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (HostilesEnRango.Remove(collision))
        {
            necesitaActualizarHostilCercano = HostilesEnRango.Count > 0;
            if (!necesitaActualizarHostilCercano)
            {
                Hostil_Mas_Cercano = Vector3.zero;
            }
        }
    }

    private void ActualizarHostilMasCercano()
    {
        float distanciaMinimaSqr = Mathf.Infinity;
        Vector3 posicionActual = transform.position;

        foreach (Collider hostil in HostilesEnRango)
        {
            float distanciaSqr = (hostil.transform.position - posicionActual).sqrMagnitude;

            if (distanciaSqr < distanciaMinimaSqr)
            {
                distanciaMinimaSqr = distanciaSqr;
                Hostil_Mas_Cercano = hostil.transform.position;
            }
        }
    }

    private bool EsHostil(Collider collision)
    {
        var comportamiento = collision.gameObject.GetComponentInParent<Comportamiento>();
        return comportamiento != null && comportamiento.Hostil;
    }

    public Vector3 Hostil_Cercano()
    {
        if (necesitaActualizarHostilCercano)
        {
            ActualizarHostilMasCercano();
            necesitaActualizarHostilCercano = false;
        }
        return HostilesEnRango.Count > 0 ? Hostil_Mas_Cercano : Vector3.zero;
    }
}

