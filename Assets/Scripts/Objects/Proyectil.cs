using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : Objeto_base
{
    public Base_Con_Vida Lanzador;
    public float speed = 50f;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Calculamos la masa
        float Masa = CalcularMasa();

        // Asignamos la masa al Rigidbody
        rb.mass = Masa; 

        Densidad = 1f;

        CalcularVida();
    }

    public Base_Con_Vida Lanzado_Por()
    {
        return Lanzador;
    }
    public void Lanzar(Vector3 direction, Base_Con_Vida El_Que_Lanza)
    {
        if(rb != null)
        {
            Lanzador = El_Que_Lanza;
            rb.velocity = direction * speed;            
        }
        else
        {
            Debug.LogError(("RigidBody no configurado en lanza"));
            Debug.Log(rb);
        }
    }
}
