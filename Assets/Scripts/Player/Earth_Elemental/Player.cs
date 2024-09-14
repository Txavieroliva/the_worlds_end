using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] GameObject mainCamera;
    [SerializeField] Transform CameraTarget;
    [SerializeField] UI playerUI;
    private PlayerInput input;
    private CharacterController controller;
    private Animator animator;
    float xRot;
    float yRot;
    bool isAttacking;

    private void Start() 
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();


        HideMouse();
    }

    private void Update() 
    {
        MovePlayer();
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

    private void MovePlayer()
    {
        if(isAttacking)
        {
            return;
        }

        float speed = 0;
        Vector3 inputDir = new Vector3(input.move.x, 0, input.move.y);
        float playerRot = 0;
        if(input.move != Vector2.zero)
        {
            speed = moveSpeed;
            playerRot = Quaternion.LookRotation(inputDir).eulerAngles.y + mainCamera.transform.rotation.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, playerRot, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
        }
        
        Vector3 playerDir = Quaternion.Euler(0, playerRot, 0) * Vector3.forward;
        animator.SetFloat("speed", input.move.magnitude);
        controller.Move(playerDir * speed * Time.deltaTime);
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
}