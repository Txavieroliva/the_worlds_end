using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int meleeDamage = 25;
    private bool canDealDmg = false;
    private Base basex;

    private void OnTriggerEnter(Collider other)
    {
        // Hace daño unicamente cuando se activa
        if(canDealDmg)
        {
            basex = other.GetComponent<Base>();

            if( basex != null)
            {
                basex.Golpeado(meleeDamage);
                Debug.Log("Hit");
            }
        }
    }

    public void ActivateTrigger()
    {
        canDealDmg = true;
    }

    public void DeactivateTrigger()
    {
        canDealDmg = false;
    }
}
