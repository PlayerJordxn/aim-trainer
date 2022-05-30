using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    private Camera cam;
    private CharacterController cc;

    //Input 
    private PlayerControls playerControls;
    private InputAction move;
    private InputAction look;

    private Vector3 playerVelocity;
    private Vector3 moveDirection = Vector3.zero;

    [Header("Movement")]
    //Movement
    [SerializeField] private float movementForce;
    [SerializeField] private float gravity = -9.81f;

    [Header("Jump")]
    //Jump
    [SerializeField] private float jumpHeight = 5f;

    [Header("Look")]
    //Look
    Vector2 lookDirection = Vector2.zero;
    [SerializeField] private float lookSensitivity = 5f;


    void Awake()
    {
        //Input system
        playerControls = new PlayerControls();

        if (!cc) cc = GetComponent<CharacterController>();
        if (!cam) cam = GetComponentInChildren<Camera>();
    }
    
    private void OnEnable()
    {
        //Jump event
        playerControls.Player.Jump.started += DoJump;
        move = playerControls.Player.Move;
        look = playerControls.Player.Look;
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.Jump.started -= DoJump;
        playerControls.Disable();
    }

    private void Update()
    {
        Movement();
        Gravity();
    }

    public void Gravity()
    {
        if (IsGrounded() && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);
    }

    public void Look()
    {
        Vector2 newLookDirection = look.ReadValue<Vector2>() * GetCameraForward(cam) * lookSensitivity;
        lookDirection += newLookDirection;
        cam.transform.Rotate(lookDirection);
    }


    private void Movement()
    {
        moveDirection += move.ReadValue<Vector2>().x * GetCameraForward(cam) * movementForce;
        moveDirection += move.ReadValue<Vector2>().y * GetCameraRight(cam) * movementForce;
        Vector3 newMove = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);
        moveDirection = Vector3.zero;
        cc.Move(newMove);
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (cc.isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    private Vector3 GetCameraForward(Camera cam)
    {
        Vector3 forward = cam.transform.right;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera cam)
    {
        Vector3 right = cam.transform.forward;
        right.y = 0;
        return right.normalized;
    }

    private bool IsGrounded() => cc.isGrounded;
}
