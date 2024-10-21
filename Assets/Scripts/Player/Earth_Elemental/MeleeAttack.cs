using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int meleeDamage;
    private bool canDealDmg = false;
    private Base basex;
    public Rigidbody rb;

private void calcular_dano()
    {
        meleeDamage = (Mathf.RoundToInt(rb.mass));
    }
    private void OnTriggerEnter(Collider other)
    {
        calcular_dano();
        // Debug.Log(other);

        // Hace daño unicamente cuando se activa
        if(canDealDmg)
        {
            basex = other.GetComponentInParent<Base>();
            ///other.GetComponentInParent.<Base>();


            if( basex != null)  
            {
                basex.Golpeado(meleeDamage);
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
