using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    private PlayerInput input;
    private CharacterController controller;

    private void Start() 
    {
        Inputs();
    }

    private void Update() 
    {
        MovePlayer();
    }

    private void Inputs()
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
    }
    private void MovePlayer()
    {
        Vector3 moveDir = new Vector3(input.move.x, 0, input.move.y);
        transform.rotation = Quaternion.LookRotation(moveDir);
        controller.Move(moveDir * speed * Time.deltaTime);
    }
}