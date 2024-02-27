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
	[HideInInspector] public Vector3 playerVelocity;
	[HideInInspector] public float moveX;
	[HideInInspector] public float moveY;
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

	protected override void InitializeEntity()
	{
		// Retrieve player health from PlayerInventory
		float savedHealth = PlayerInventory.Instance != null ? PlayerInventory.Instance.GetPlayerHealth() : -1f;

		if (savedHealth > 0)
		{
			// Use the saved health value
			SetCurrentHealth(savedHealth);
			Debug.Log("set saved health:" + savedHealth);
		}
		else
		{
			// Use default max health if no saved health is available
			SetCurrentHealth(maxHealth);
			PlayerInventory.Instance.SetPlayerHealth(currentHealth);
			Debug.Log("no health available - setting max health and saving to inventory");
		}

		// Call the base class method for any common initialization behavior
		//base.InitializeEntity();
	}

	public override void TakeDamage(float damageAmount)
	{
		SetCurrentHealth(currentHealth - damageAmount);

		if (currentHealth <= 0f)
		{
			DestroyEntity();
		}

		// Update the player's health in PlayerInventory when it changes
		UpdatePlayerHealth();
	}

	public void UpdatePlayerHealth()
	{
		if (PlayerInventory.Instance != null)
		{
			// Ensure that currentHealth is within the valid range [0, maxHealth]
			float clampedHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

			// Set the clamped health value in the PlayerInventory
			PlayerInventory.Instance.SetPlayerHealth(clampedHealth);

			// Update currentHealth to match the value in PlayerInventory
			currentHealth = PlayerInventory.Instance.GetPlayerHealth();
		}
	}



	private void Update()
	{
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
