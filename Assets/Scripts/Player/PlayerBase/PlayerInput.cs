using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    public Vector2 move;
    public Vector2 look;
    public bool isAttacking;

    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    private void OnAttack(InputValue value)
    {
        isAttacking = value.isPressed;
        //Debug.Log("Ataque Detectado" + isAttacking);
    }
}