using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : EntityClass
{
    public float walkSpeed = 7f;  //публичное значение скорости
    public float runSpeed = 10f;
    private float currentSpeed;
    private float moveVariable;

    private Rigidbody2D rb;
    private Vector3 moveDir;

    private GameObject model;
    private Animator animator;
    [HideInInspector]public Vector3 playerVelocity;
    [HideInInspector]public float moveX;
    [HideInInspector]public float moveY;
    private PlayerAnimation playerAnimation;
    

    private void Awake()
    {
        currentSpeed = walkSpeed;
        playerAnimation = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody2D>();
        model = GameObject.Find("Model");
        animator = model.GetComponent<Animator>();
    }



    private void Update()
    {
        HealthCheck();

        moveX = Input.GetAxisRaw("Horizontal"); //управление по Х (A, D)
        moveY = Input.GetAxisRaw("Vertical");   //управление по Y (W, S)

        moveDir = new Vector3(moveX, moveY).normalized; //вектор движения игрока, не превышающий 1


        if (Input.GetButtonDown("Sprint") && playerAnimation.isMoving)
        {
            currentSpeed = runSpeed;
        }
        else if (Input.GetButtonUp("Sprint") || !playerAnimation.isMoving)
        {
            currentSpeed = walkSpeed;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = playerVelocity;
        playerVelocity = moveDir * currentSpeed;  //вектор умножается на скорость. физика обрабатывается в fixed
    }
}
