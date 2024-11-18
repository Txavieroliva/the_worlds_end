using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : Base_Con_Vida
{
    public float moveSpeed;
    [SerializeField] private float correrMultiplicador = 1.5f;
    [SerializeField] private float correrAnimMultiplicador = 1.5f;
    [SerializeField] private KeyCode keyCorrer;
    [SerializeField] float rotSpeed;
    [SerializeField] Transform mainCamera;
    [SerializeField] Transform CameraTarget;
    [SerializeField] UI playerUI;
    [SerializeField] float Densidad;
    private MeleeAttack meleeAttack;
    private PlayerInput input;
    public Rigidbody rb;
    private Animator animator;
    float xRot;
    float yRot;
    bool isAttacking;
    public float Vida;
    public float manaRegen = 1f;
    public float costoManaMelee = 10.0f;
    

    private void Start() 
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        //controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        meleeAttack = GetComponentInChildren<MeleeAttack>();
        Time.timeScale = 0;

        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;  // Asignar la cámara principal si no está asignada
        }

        //HideMouse();

        CalcularMasa();
        CalcularVidaMax();
    }

    private void Update() 
    {
        MovePlayer();
        RotatePlayerWithCamera();
        CalcularSize();
        //Attack();
    }

    private void LateUpdate() {
        CameraRotation();
        RegenerarMana();
    }

    private void HideMouse()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    // Mover al Golem basado en Rigidbody
    private void MovePlayer()
    {
        float currentSpeed = moveSpeed;

        // Verificar si la tecla de correr está presionada
        if (Input.GetKey(keyCorrer))
        {
            currentSpeed *= correrMultiplicador;  // Aumentar la velocidad de movimiento
            animator.speed = correrAnimMultiplicador;  // Aumentar la velocidad de la animación
        }
        else
        {
            animator.speed = 1f;  // Restablecer la velocidad de la animación a la normalidad
        }

        Vector3 movement = new Vector3(input.move.x, 0, input.move.y).normalized;

        if (movement.magnitude >= 0.1f)
        {
            Vector3 moveDirection = GetCameraRelativeMovement(movement);
            Vector3 moveVelocity = moveDirection * currentSpeed * Time.deltaTime;

            rb.MovePosition(rb.position + moveVelocity);
            animator.SetFloat("speed", movement.magnitude * currentSpeed);
        }
        else
        {
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

    // private void Attack()
    // {
    //     if(input.isAttacking && !isAttacking && playerUI.mana >= costoManaMelee)
    //     {
    //         isAttacking = true;
    //         animator.SetBool("attack", true);

    //         playerUI.useMana(costoManaMelee);
    //         meleeAttack.ActivateTrigger();

    //         StartCoroutine(finalDeAtaque());
    //     }

    // }

    private void RegenerarMana()
    {
        if(playerUI.mana < playerUI.maxMana)
        {
            playerUI.mana += manaRegen * Time.deltaTime;
            playerUI.mana = Mathf.Clamp(playerUI.mana, 0, playerUI.maxMana);
        }
    }

    // private IEnumerator finalDeAtaque()
    // {
    //     AnimatorStateInfo infoEstAtq = animator.GetCurrentAnimatorStateInfo(0);
        
    //     yield return new WaitForSeconds(infoEstAtq.length);

    //     animator.SetBool("attack", false);
    //     meleeAttack.DeactivateTrigger();

    //     isAttacking = false;
    //     input.isAttacking = false;
    // }

    public void TakeDamage(float amount)
    {
        playerUI.health -= amount; //actualiza la vida en la UI
        playerUI.maxHealth -= amount;
        

        if(playerUI.health <= 0 || playerUI.maxHealth <= 0)
        {
            playerUI.health = 0;
            playerUI.maxHealth = 0;
            // checkDeath();
        }
        //Debug.Log("Recibo: " + amount + " de daño");
        //CalcularMasa();
    }

    // private void checkDeath()
    // {
    //     if(playerUI.health <= 0 || playerUI.maxHealth <= 0)
    //     {
    //         endGame(); //finaliza el juego si la vida llega a 0 o menos
    //     }
    // }

    // private void endGame()
    // {
    //     Time.timeScale = 0;
    //     //Application.Quit(); // Sale del juego cuando se buildee
    // }

    public void cureWounds(float amount)
    {
        playerUI.health += amount;

        if(playerUI.health > playerUI.maxHealth)
        {
            playerUI.maxHealth = playerUI.health;
        }

        CalcularMasa();

    }

    public float CalcularVolumen()
    {
        Vector3 scale = transform.localScale; // Obtenemos la escala del objeto en la escena
        return scale.x;
    }

    public void CalcularVidaMax()
    {
        Vida = Mathf.RoundToInt(rb.mass);
        playerUI.maxHealth = Vida;
    }

    public void CalcularVidaAct()
    {
        Vida = Mathf.RoundToInt(rb.mass);
        playerUI.health = Vida;
    }

    public void CalcularMasa()
    {
        float volumen = CalcularVolumen();
        rb.mass = volumen * Densidad;
        
        CalcularVidaAct();
    }

    public void CalcularSize()
    {
        float sizer = playerUI.maxHealth * 0.01f;
        rb.mass = playerUI.maxHealth;
        CalcularMasa();
        Vector3 size = new Vector3(sizer, sizer, sizer);
        transform.localScale = size;
    }
}