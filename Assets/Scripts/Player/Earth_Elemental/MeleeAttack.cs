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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(canDealDmg);
        // Intentar obtener el componente Objeto_base, sin importar si es Casa u otro derivado
        Objeto_base base_objeto = other.GetComponentInParent<Objeto_base>();  // Buscamos en padres también si es necesario
        Base base_base = other.GetComponentInParent<Base>();  // Buscamos también Base si fuera necesario

        if(canDealDmg)
        {
            if (base_base != null)
            {
                //Aplica lógica para el componente base (si existe)
                base_base.Golpeado(meleeDamage);  // Ajustar el valor de damageAmount según tu sistema de daño
            }
            else if (base_objeto != null)
            {
                //Aplica lógica para el componente Objeto_base (o derivadas como Casa)
                base_objeto.Golpeado(meleeDamage);  // Ajustar el valor de damageAmount según tu sistema de daño
                Debug.Log("Golpeado: " + base_objeto.gameObject.name + " con " + meleeDamage + " de daño.");
            }
            else
            {
                Debug.Log("No se encontró Objeto_base o Base en " + other.gameObject.name);
            }
        }
        
    }

}
