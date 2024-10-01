using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Debris_Piedra : Debris
{
      protected override void Awake()
   {
    float  a = Random.Range(1f,4f);
    Vector3 vecty = new Vector3(a, a, a);
    transform.localScale = vecty; // Obtenemos la escala del objeto en la escena
   }
   
}
