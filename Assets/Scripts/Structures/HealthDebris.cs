using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Debris : MonoBehaviour
{
   [SerializeField] private float healthAmount = 10f;

   public void Dar_Material(Material mater)
   {
    GetComponent<Renderer>().material = mater;
   }
   private void OnCollisionEnter(Collision other)
   {
        
        Player player = other.gameObject.GetComponent<Player>();

        if(player != null)
        {
            player.cureWounds(healthAmount);
            Destroy(gameObject);
        }
   }
}
