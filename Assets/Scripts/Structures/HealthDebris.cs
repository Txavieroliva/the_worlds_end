using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBuilding : MonoBehaviour
{
   [SerializeField] private float healthAmount = 10f;

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
