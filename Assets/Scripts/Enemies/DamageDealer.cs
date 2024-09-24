using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealler : MonoBehaviour
{
    [SerializeField] private float damageAmount = 20f;

    private void OnCollisionEnter(Collision other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if(player != null)
        {
            player.TakeDamage(damageAmount);
        }
   }
}

