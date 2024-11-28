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

    //SHAKE
    public Transform myCamera;
    public float shakeDuration = 0.3f;
    public float shakeAmount = 2.0f;
    public float decreaseFactor = 1.0f;
    private Vector3 originalPos;
    private bool isShaking = false;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        if (myCamera != null)
        {
            originalPos = myCamera.localPosition; // Guarda la posición inicial al comienzo
        }
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            isShaking = true;
            myCamera.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else if (isShaking)
        {
            isShaking = false;
            myCamera.localPosition = originalPos; // Restaura la posición original al finalizar el shake
        }
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
        Objeto_base base_objeto = other.GetComponentInParent<Objeto_base>();
        Base base_base = other.GetComponentInParent<Base>();

        if (canDealDmg)
        {
            Debug.Log("jeje, god");
            shakeDuration = 0.3f; // Activa el shake durante 0.3 segundos
            if (base_base != null)
            {
                base_base.Golpeado(meleeDamage);
            }
            else if (base_objeto != null)
            {
                base_objeto.Golpeado(meleeDamage);
                Debug.Log("Golpeado: " + base_objeto.gameObject.name + " con " + meleeDamage + " de daño.");
            }
            else
            {
                Debug.Log("No se encontró Objeto_base o Base en " + other.gameObject.name);
            }
        }
    }
}
