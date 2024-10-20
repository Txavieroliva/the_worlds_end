using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int meleeDamage;
    private bool canDealDmg = false;
    private Base base_base;
    private Objeto_base base_objeto;
    public Rigidbody rb;
    private Collider other;
    
    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }
    
    private void Update()
    {
        calcular_dano();
    }

    private void calcular_dano()
    {
        meleeDamage = Mathf.RoundToInt(rb.mass);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        Debug.Log(other);

        base_base = other.GetComponent<Base>();
        base_objeto = other.GetComponent<Objeto_base>();

        // Hace daño unicamente cuando se activa
        if(canDealDmg)
        {
            if( base_base != null)  
            {
                base_base.Golpeado(meleeDamage);
            }
            else if (base_objeto != null)
            {
                base_objeto.Golpeado(meleeDamage);
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
        Debug.Log("Desactivado");
    }
}
