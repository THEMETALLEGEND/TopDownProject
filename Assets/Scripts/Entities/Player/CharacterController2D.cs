using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 60f;  //публичное значение скорости

    private Rigidbody2D rb;
    private Vector3 moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        var moveX = Input.GetAxisRaw("Horizontal"); //управление по Х (A, D)
        var moveY = Input.GetAxisRaw("Vertical");   //управление по Y (W, S)

        moveDir = new Vector3(moveX, moveY).normalized; //вектор движения игрока, всегда равный 1
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * moveSpeed;  //вектор умножается на скорость. физика обрабатывается в fixed
    }
}
