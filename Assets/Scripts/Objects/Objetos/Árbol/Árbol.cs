using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbol : Objeto_base
{
protected override void	Destruccion_Porcentual()
    {
        if (Vida < Mathf.RoundToInt(rb.mass) / 2){
        rb.isKinematic = false;

        }
    }
        
}

