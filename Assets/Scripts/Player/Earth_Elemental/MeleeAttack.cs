using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float meleeDamage = 25f;
    private bool canDealDmg = false;

    private void OnTriggerEnter(Collider other)
    {
        //Hace daño unicamente cuando se activa
        if(canDealDmg)
        {
            StructureBase structure = other.GetComponent<StructureBase>();

            if(structure != null)
            {
                structure.TakeDamage(meleeDamage);
                Debug.Log("Hit");
            }
        }
    }

    public void ActivateTrigger()
    {
        canDealDmg = true;
        Debug.Log("Activado");
    }

    public void DeactivateTrigger()
    {
        canDealDmg = false;
        Debug.Log("Nao Nao");
    }
}
