using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Debris_Madera : Debris
{
   protected override void Awake()
   {
    float a = Random.Range(1f , 2f);
    Vector3 vecty = new Vector3(a, Random.Range(1f , 2f), a);
    transform.localScale = vecty; // Obtenemos la escala del objeto en la escena
   }
   
}
