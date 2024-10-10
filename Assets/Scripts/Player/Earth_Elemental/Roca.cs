using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roca : Proyectil
{
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Calculamos la masa
        float Masa = CalcularMasa();

        // Asignamos la masa al Rigidbody
        rb.mass = Masa; 

        Densidad = 10f;

        CalcularVida();
    }
}
