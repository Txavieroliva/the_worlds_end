using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spear : Proyectil
{    
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Calculamos la masa
        float Masa = CalcularMasa();

        // Asignamos la masa al Rigidbody
        rb.mass = Masa; 

        Densidad = 0.1f;

        CalcularVida();
    }
}