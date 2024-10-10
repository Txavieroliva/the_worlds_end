using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{

    private Vector3 Hostil_Mas_Cercano = Vector3.positiveInfinity;

    private void OnTriggerStay(Collider collision)
    {
        Detectar_Hostil(collision);
    }

    private Vector3 Detectar_Hostil(Collider collision)
    {
            if (collision.gameObject.GetComponent<Comportamiento>() != null && collision.gameObject.GetComponent<Comportamiento>().Hostil == true)
                {
                    Vector3 PosicionHostil = collision.transform.position;
                        if (Vector3.Distance(PosicionHostil, transform.position) < Vector3.Distance(Hostil_Mas_Cercano, transform.position))
                        {
                            Hostil_Mas_Cercano = PosicionHostil;
                        }
        }
        return Hostil_Mas_Cercano;
    }
    public Vector3 Hostil_Cercano()
    {
        if (Hostil_Mas_Cercano == Vector3.positiveInfinity)  // Si no se ha encontrado ningún hostil
        {
            Debug.LogWarning("No se ha detectado ningún hostil.");
            return Vector3.zero;  //returnea 0
        }
        return Hostil_Mas_Cercano;
    }
}
