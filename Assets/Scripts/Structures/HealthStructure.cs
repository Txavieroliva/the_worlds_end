using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBuilding : MonoBehaviour
{
   [SerializeField] private float healthAmount = 10f;

   private void OnTriggerEnter(Collider other)
   {
        Player player = other.GetComponent<Player>();

        if(player != null)
        {
            player.cureWounds(healthAmount);
            Destroy(gameObject);
        }
   }
}
