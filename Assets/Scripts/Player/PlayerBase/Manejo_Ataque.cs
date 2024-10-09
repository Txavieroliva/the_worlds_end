using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manejo_Ataque : MonoBehaviour
{
    public List<Ataque_Base> combo; // Lista de ataques (debería contener solo 1 ataque para pruebas)
    private int contadorCombo;       // Contador de combos

    private Animator animator;
    private PlayerInput entradaJugador;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        entradaJugador = GetComponent<PlayerInput>();
        contadorCombo = 0; // Inicializa el contador de combos

        // Establecer la velocidad de la animación (1 es normal, puedes aumentarlo)
        animator.speed = 1.5f; // Aumentar para acelerar las animaciones
    }

    private void Update()
    {
        Atacar();
    }

    private void Atacar()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RealizarAtaque(); // Llama a RealizarAtaque solo si se está atacando
            Debug.Log("Ataque Act: " + combo.Count);
        }
    }
    
    private void RealizarAtaque()
    {
        // Asegúrate de que el contador de combo sea válido
        if (contadorCombo < combo.Count)
        {
            // Asegúrate de que no hay animación de ataque en curso
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                ReproducirAnimacionDeAtaque();
                IncrementarContadorCombo();
            }
        }
    }

    private void ReproducirAnimacionDeAtaque()
    {
        // Reproduce la animación correspondiente al ataque
        animator.runtimeAnimatorController = combo[contadorCombo].animatorOV;
        animator.SetTrigger("AttackTrigger"); // Cambia a la animación de ataque

        // Inicia la coroutine para finalizar el ataque
        StartCoroutine(FinalizarAtaque());
    }

    private void IncrementarContadorCombo()
    {
        contadorCombo++; // Incrementa el contador de combos

        // Reinicia el contador si ha alcanzado el final
        if (contadorCombo >= combo.Count)
        {
            contadorCombo = 0; // Reinicia al primer combo
        }
    }

    private IEnumerator FinalizarAtaque()
    {
        // Espera a que la animación de ataque termine
        AnimatorStateInfo infoEstAtq = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(infoEstAtq.length / animator.speed); // Espera la duración de la animación ajustada por la velocidad

        animator.ResetTrigger("AttackTrigger"); // Reinicia el trigger de ataque

        // Desactiva la entrada de ataque
        entradaJugador.isAttacking = false; // Asegúrate de que la entrada se restablezca
    }
}
