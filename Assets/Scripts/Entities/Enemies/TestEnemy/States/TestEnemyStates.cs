using System;
using System.Collections.Generic;
using Waypoints;
using Entities.Enemies.TestEnemy.States;
using UnityEngine;
using Pathfinding;
using UnityEngine.Serialization;

public class TestEnemyStates : StateMachine
{
	//-------STATES--------
	[HideInInspector] public EnemyWaiting WaitingState;
	[HideInInspector] public EnemyRoaming RoamingState;
	[HideInInspector] public EnemyPathMoving PathMovingState;
	[HideInInspector] public EnemyInteraction InteractionState;
	[HideInInspector] public EnemyChasing ChasingState;
	[HideInInspector] public EnemyShooting ShootingState;
	[HideInInspector] public EnemyHitting HittingState;
	[HideInInspector] public EnemyFleeing FleeingState;
	[HideInInspector] public EnemyAfraid AfraidState;
	[HideInInspector] public EnemyStunned StunnedState;
	[HideInInspector] public OneShotChasing OneShotChasingState;
	[HideInInspector] public OneShotShooting OneShotShootingState;

	//-------SCRIPTS--------
	[HideInInspector] public EnemyClass EnemyClass;

	//-------PATHFINDING A*--------
	[HideInInspector] public AIPath AIPath;
	[HideInInspector] public AIDestinationSetter AIDest;

	//-------COMPONENTS--------
	[HideInInspector] public Vector3 StartingPosition;
	[HideInInspector] public GameObject PlayerObject;
	[HideInInspector] public Animator Animator;
	[HideInInspector] public Transform Target;
	[HideInInspector] public GameObject EnemyObject;
	[HideInInspector] public Rigidbody2D Rigidbody;
	[HideInInspector] public GameObject Model;
	[HideInInspector] public SpriteRenderer SpriteRenderer;
	[HideInInspector] public PlayerRaycast PlayerRaycast;
	[HideInInspector] public GameObject Hitbox;

	//-------LOGIC---------------
	public GameObject PointTarget;
	public GameObject BulletPrefab;
	[HideInInspector] public bool IsAlerted = false;
	[HideInInspector] public bool IsAfraid = false;
	public float AlertRadius = 10f;

	//-------METHODS--------------
	[HideInInspector] public Stack<BaseState> stateStack = new Stack<BaseState>();

	//-------INSPECTOR VALUES--------
	[Header("State values")]

	[Header("General")]
	public bool showDebugGizmos = false;
	public float defaultSpeed = 12f;
	public float slowSpeed = 8f;
	public float meleeSpeed = 20f;
	public bool isAnNPC = false;
	public bool isMelee = false; //ITERATION 2
	public bool isOneShot = false;
	public bool isMinigunner = false;
	public bool debugMode = false;

	[Header("Roaming state")]
	public float roamingSpeed = 10f;
	public float roamingInterval = 30f;
	public float roamRadius = 10f;
	public float roamPlayerDistanceEnter = 20f;

	[Header("Chasing state")]
	public float chasingPlayerDistanceEnter = 28f;
	public float chasingPlayerDistanceExit = 65f;

	[Header("Shooting state")]
	public float dodgeSpeed = 25f;
	public float shootingBulletSpeed = 20f;
	public float shootingInaccuracySize = 0.4f;
	public float shootingPlayerDistanceExit = 35f;
	public float shootingBurstShortTiming = .2f;
	public float shootingBurstLongTiming = 2f;
	[HideInInspector] public float dodgeRandom;

	[Header("Hitting State")]
	public float strikeTiming = .5f;
	public float hittingPlayerDistanceExit = 40f;

	[Header("Fleeing state")]
	public float fleeingSpeed = 15f;
	public float maxTurnAngle = 45f;
	public float fleeingPlayerDistanceExit = 40f;
	public float fleeingTime = 10f;

	[Header("Afraid state")]
	public float afraidPlayerDistanceExit = 20f;

	[Header("Detecting")]
	public bool playerRaycastHit;
	
	[Header("Waypoint Controller")]
	public PathController pathController;
	public InteractionController interactionController;

	[Header("One Shot")]
	public float oneShotTiming = 2f;

	//--------------TEMPORARY------------
	public LayerMask obstacleLayer; // слой, содержащий объекты с коллизией и тегом obstacles
	public LayerMask playerLayer;
	public float rayLength = 28f; // длина лучей
	public int rayCount = 48; // количество лучей
	public float angleStep = 7.5f; // шаг между углами лучей
	public float playerDetectionDistance = 2f; // расстояние, на котором агент определяет игрока

	public void Awake()
	{
		AIPath = GetComponent<AIPath>();
		AIDest = GetComponent<AIDestinationSetter>();
		Rigidbody = GetComponent<Rigidbody2D>();
		EnemyObject = gameObject;
		//PlayerObject = GameObject.Find("Player");
		//PlayerRaycast = PlayerObject.GetComponent<PlayerRaycast>();
		Target = AIDest.target;
		PointTarget = transform.parent.GetChild(1).gameObject; //поиск пустого ГО к которому идет враг всегда - так называемая цель
		Hitbox = transform.GetChild(1).gameObject;
		Animator = GetComponent<Animator>();
		Model = transform.GetChild(0).gameObject;
		SpriteRenderer = Model.GetComponent<SpriteRenderer>();
		EnemyClass = GetComponent<EnemyClass>();
		WaitingState = new EnemyWaiting(this); //присваивание состояний к переменным с этой стейт машиной
		RoamingState = new EnemyRoaming(this);
		PathMovingState = new EnemyPathMoving(this, pathController);
		InteractionState = new EnemyInteraction(this, interactionController);
		ChasingState = new EnemyChasing(this);
		ShootingState = new EnemyShooting(this);
		HittingState = new EnemyHitting(this);
		FleeingState = new EnemyFleeing(this);
		AfraidState = new EnemyAfraid(this);
		StunnedState = new EnemyStunned(this);
		OneShotChasingState = new OneShotChasing(this);
		OneShotShootingState = new OneShotShooting(this);
		GetGameObject(EnemyObject);

		if (isMelee)
			defaultSpeed = meleeSpeed;
		AIPath.maxSpeed = defaultSpeed;
	}

	public override void ChangeState(BaseState newState)
	{
		if (currentState != null)
		{
			currentState.Exit();
			stateStack.Push(currentState); // добавляем текущее состояние в стек перед переходом
		}

		currentState = newState;
		currentState.Enter();
	}

	protected override BaseState GetInitialState() //начальное состояние в виде состояния ожидания
	{
		if (true)
		{
			return InteractionState;
		}
		return RoamingState;
	}

	public bool CheckPlayerContact(int rayCount, int playerRayCount, float rayLength)
	{
		int playerContacts = 0;

		for (int i = 0; i < rayCount; i++)
		{
			// вычисляем угол луча
			float angle = i * (360f / rayCount);

			// вычисляем направление луча на основе угла
			Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.right;

			// выпускаем луч и получаем информацию о столкновении с объектами на слоях obstacleLayer и playerLayer
			RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, LayerMask.GetMask("Player", "Obstacles"));

			// выбираем цвет для линии на основе столкновения луча с объектом
			Color lineColor = Color.green;
			if (hit.collider != null)
			{
				if (hit.collider.CompareTag("Player"))
				{
					playerContacts++;
					lineColor = Color.red;
				}
				else if (hit.collider.CompareTag("Obstacles"))
				{
					lineColor = Color.yellow;
				}
			}

			// рисуем линию для луча
			if (showDebugGizmos)
				Debug.DrawLine(transform.position, hit.collider != null ? hit.point : transform.position + direction * rayLength, lineColor);

			// если мы нашли нужное количество лучей, касающихся игрока, возвращаем true
			if (playerContacts >= playerRayCount)
			{
				return true;
			}
		}

		// если не нашли нужное количество лучей, возвращаем false
		return false;
	}

	public void SetAlerted(bool value)
	{
		if (value)
		{
			// Оповещаем всех агентов в радиусе оповещения
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, AlertRadius);
			foreach (Collider2D collider in colliders)
			{
				TestEnemyStates agent = collider.GetComponent<TestEnemyStates>();
				if (agent != null && agent != this)
				{
					agent.IsAlerted = true;
				}
			}
		}
	}


	public bool CheckPlayerInRange(float alertDistance)     // метод проверяющий расстояние до игрока с кастомной переменной
	{
		if (PlayerObject != null)
		{
			float distance = Vector3.Distance(EnemyObject.transform.position, PlayerObject.transform.position); //проверка дистанции от объекта а до б
			if (distance <= alertDistance)
				return true;
			else
				return false;
		}

		return false;


	}

	public void TargetSetter(GameObject newTarget)
	{
		AIDest.target = newTarget.transform;
	}

	public void DiceMethod(float successChance, Action methodToRun) //метод который принимает шанс выполнения метода и при успехе выполняет его.
																	//для использования метода Action нужно указывать ссылку на сборку System
	{
		float randomValue = UnityEngine.Random.Range(0f, 1f);
		if (randomValue <= successChance / 100f)
		{
			methodToRun.Invoke();
		}
	}

	public void ReturnToPreviousState()
	{
		if (stateStack.Count > 0)
		{
			BaseState previousState = stateStack.Pop(); // Извлекаем предыдущее состояние из стека
			ChangeState(previousState); // Переходим в предыдущее состояние
		}
		else
			Debug.Log("стак пуст");
	}


	private void OnDrawGizmos()
	{
		if (showDebugGizmos)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, chasingPlayerDistanceExit);

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, chasingPlayerDistanceEnter);
		}
	}
}
