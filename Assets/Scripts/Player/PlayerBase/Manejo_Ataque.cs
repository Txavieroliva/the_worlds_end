using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manejo_Ataque : MonoBehaviour
{
    public List<Ataque_Base> Combo;
    private float tiempoUltimoClick;
    private float finUltimoCombo;
    private int contadorCombo;

    private Animator animator;
    private PlayerInput input;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if(input.isAttacking)
        {
            Ataque();
        }
        finAtaque();
    }

    private void Ataque()
    {
        if(Time.time - finUltimoCombo > 0.5f && contadorCombo < Combo.Count)
        {
            CancelInvoke("EndCombo");

            ManejoCombo();
        }
    }

    private void ManejoCombo()
    {
        if(Time.time - tiempoUltimoClick >= 0.2f)
        {
            animator.runtimeAnimatorController = Combo[contadorCombo].animatorOV;
            animator.Play("Attack", 0,0);
            contadorCombo++;
            tiempoUltimoClick = Time.time;

            resetContadorCombo();
        }
    }

    private void resetContadorCombo()
    {
        if(contadorCombo >= Combo.Count)
        {
            contadorCombo = 0;
        }
    }

    private void finAtaque()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("finCombo", 1);
        }
    }

    private void finCombo()
    {
        contadorCombo = 0;
        finUltimoCombo = Time.time;
    }
}
