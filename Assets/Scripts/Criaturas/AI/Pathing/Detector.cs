using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{

    private Vector3 Hostil_Mas_Cercano;

    private void OnCollisionEnter(Collision collision)
    {
        Detectar_Hostil(collision);
    }

    private Vector3 Detectar_Hostil(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (collision.gameObject.GetComponent<Comportamiento>() != null)
                {
                    if (collision.gameObject.GetComponent<Comportamiento>().Hostil == true)
                    {
                        if (Vector3.Distance(collision.gameObject.GetComponent<Transform>().position, transform.position) >= Vector3.Distance(Hostil_Mas_Cercano, transform.position))
                        {
                            Hostil_Mas_Cercano = collision.gameObject.GetComponent<Transform>().position;
                        }
                    }
                }
        }
        return Hostil_Mas_Cercano;
    }
    public Vector3 Hostil_Cercano()
    {
        return Hostil_Mas_Cercano;
    }
}
