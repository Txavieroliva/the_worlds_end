using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealler : MonoBehaviour
{
    [SerializeField] private float damageAmount = 20f;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        
        if(player != null)
        {
            player.TakeDamage(damageAmount);
            //Debug.Log("Trigger si");
        }
    }

}
