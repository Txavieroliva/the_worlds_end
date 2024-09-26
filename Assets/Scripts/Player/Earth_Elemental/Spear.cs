using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spear : Objeto_base
{
    public float speed = 50f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Calculamos la masa
        float Masa = CalcularMasa();

        // Asignamos la masa al Rigidbody
        rb.mass = Masa; 

        Densidad = 0.1f;

        CalcularVida();
    }

    public void Lanzar(Vector3 direction)
    {
        if(rb != null)
        {
            rb.velocity = direction * speed;
            Debug.Log("La lanza tiene velocidad: " + rb.velocity);
            
        }
        else
        {
            Debug.LogError(("RigidBody no configurado en lanza"));
            Debug.Log(rb);
        }
    }
}
