using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] GameObject mainCamera;
    [SerializeField] Transform CameraTarget;
    private PlayerInput input;
    private CharacterController controller;
    float xRot;
    float yRot;

    private void Start() 
    {
        Inputs();
    }

    private void Update() 
    {
        MovePlayer();
    }

    private void LateUpdate() {
        CameraRotation();
    }

    private void Inputs()
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
    }
    private void MovePlayer()
    {
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
}