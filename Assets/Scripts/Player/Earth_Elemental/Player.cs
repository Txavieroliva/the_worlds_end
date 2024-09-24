using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] Transform mainCamera;
    [SerializeField] Transform CameraTarget;
    [SerializeField] UI playerUI;
    private PlayerInput input;
    private Rigidbody rb;
    private Animator animator;
    float xRot;
    float yRot;
    bool isAttacking;

    private void Start() 
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        //controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;  // Asignar la cámara principal si no está asignada
        }

        HideMouse();
    }

    private void Update() 
    {
        MovePlayer();
        RotatePlayerWithCamera();
        Attack();
    }

    private void LateUpdate() {
        CameraRotation();
    }

    private void HideMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Mover al Golem basado en Rigidbody
    private void MovePlayer()
    {
        if(isAttacking)
        {
            return;
        }
        
        // Obtener la dirección de movimiento en función del input del jugador
        Vector3 movement = new Vector3(input.move.x, 0, input.move.y).normalized;

        // Si hay movimiento, aplicamos la velocidad
        if (movement.magnitude >= 0.1f)
        {
            Vector3 moveDirection = GetCameraRelativeMovement(movement);  // Convertir el movimiento relativo a la cámara
            Vector3 moveVelocity = moveDirection * moveSpeed;

            // Aplicar la velocidad al Rigidbody
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);

            // Controlar la animación de movimiento
            animator.SetFloat("speed", movement.magnitude);
        }
        else
        {
            // Si no hay movimiento, detener la animación
            animator.SetFloat("speed", 0f);
        }
    }

    // Girar al jugador en la dirección de la cámara
    private void RotatePlayerWithCamera()
    {
        // Obtener la dirección de movimiento en función de la cámara
        Vector3 movement = new Vector3(input.move.x, 0, input.move.y).normalized;

        // Solo rotar si hay movimiento
        if (movement.magnitude >= 0.1f)
        {
            // Convertir el movimiento a la dirección relativa a la cámara
            Vector3 moveDirection = GetCameraRelativeMovement(movement);
        
            // Crear la rotación objetivo basándonos en la dirección de movimiento
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // Fijar la rotación en el eje Y, manteniendo los ejes X y Z actuales
            targetRotation.x = 0;
            targetRotation.z = 0;

            // Suavizar la rotación hacia la rotación objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
    }

    // Método para obtener el movimiento relativo a la cámara
    private Vector3 GetCameraRelativeMovement(Vector3 inputMovement)
    {
        // Obtener la dirección de la cámara en el plano XZ (horizontal)
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        // Anular la componente Y de la dirección de la cámara para mantener el movimiento horizontal
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Convertir el movimiento en función de la cámara
        Vector3 moveDirection = (cameraForward * inputMovement.z + cameraRight * inputMovement.x);
        return moveDirection;
    }

    private void CameraRotation()
    {
        xRot += input.look.y;
        yRot += input.look.x;
        xRot = Mathf.Clamp(xRot, -30, 70);
        Quaternion rotation = Quaternion.Euler(xRot, yRot, 0);
        CameraTarget.rotation = rotation;
    }

    private void Attack()
    {
        if(input.isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("attack");
        }



        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAttacking = false;
        input.isAttacking = false;
    }

    public void TakeDamage(float amount)
    {
        playerUI.health -= amount; //actualiza la vida en la UI
        if(playerUI.health < 0)
        {
            playerUI.health = 0;
            checkDeath();
        }
        //Debug.Log("Recibo: " + amount + " de daño");

        
    }

    private void checkDeath()
    {
        if(playerUI.health <= 0)
        {
            endGame(); //finaliza el juego si la vida llega a 0 o menos
            Debug.Log("muelte");
        }
    }

    private void endGame()
    {
        Time.timeScale = 0;
        //Application.Quit(); // Sale del juego cuando se buildee
        Debug.Log("muelte diablo");
    }

    public void cureWounds(float amount)
    {
        playerUI.health += amount;
    }
}