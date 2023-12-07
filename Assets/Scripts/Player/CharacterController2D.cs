using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : EntityClass
{
    public float walkSpeed = 7f;  //публичное значение скорости
    public float runSpeed = 10f;
    public float currentSpeed;
    private float moveVariable;

    private Rigidbody2D rb;
    private Vector3 moveDir;

    private GameObject model;
    private Animator animator;
    [HideInInspector]public Vector3 playerVelocity;
    [HideInInspector]public float moveX;
    [HideInInspector]public float moveY;
    private PlayerAnimation playerAnimation;
    public PlayerInventory inventory;
    private Door currentDoor;
    

    private void Awake()
    {
        currentSpeed = walkSpeed;
        playerAnimation = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody2D>();
        model = GameObject.Find("Model");
        animator = model.GetComponent<Animator>();
        inventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();
    }


    private void Update()
    {
        HealthCheck(); //!!пофиксить чтобы было только при восполнении хп

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

        if (Input.GetKeyDown(KeyCode.E) && currentDoor != null)
        {
            TryOpenDoor(currentDoor);
        }


    }

    private void FixedUpdate()
    {
        rb.velocity = playerVelocity;
        playerVelocity = moveDir * currentSpeed;  //вектор умножается на скорость. физика обрабатывается в fixed
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            // Если игрок сталкивается с дверью, сохраняем ее
            currentDoor = other.gameObject.GetComponent<Door>();
        }

        if (other.gameObject.CompareTag("Key"))
        {
            // Получаем компонент Key
            Key key = other.gameObject.GetComponent<Key>();

            // Если игрок не имеет этот ключ, добавляем его в инвентарь
            if (!inventory.HasKey(key.keyId))
            {
                inventory.AddKey(key);
                Debug.Log("You picked up the key!");
                Destroy(other.gameObject); // Удаляем объект ключа из сцены
            }
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            // Если игрок перестал сталкиваться с дверью, очищаем текущую дверь
            currentDoor = null;
        }
    }

    public void TryOpenDoor(Door door)
    {
        if (string.IsNullOrEmpty(door.keyId) || inventory.HasKey(door.keyId)) //если ключ пустой или уже есть в инвентаре открываем дверь
        {
            door.Open();
        }
        else
        {
            // Показываем сообщение о том, что невозможно открыть дверь
            Debug.Log("You don't have the key to open this door.");
        }
    }
}
