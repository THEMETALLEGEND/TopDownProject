using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TestEnemyStates : StateMachine
{
    //-------STATES--------
    [HideInInspector] public EnemyWaiting waitingState;
    [HideInInspector] public EnemyRoaming roamingState;
    [HideInInspector] public EnemyChasing chasingState;
    [HideInInspector] public EnemyShooting shootingState;
    [HideInInspector] public EnemyFleeing fleeingState;
    [HideInInspector] public EnemyAfraid afraidState;

    //-------SCRIPTS--------
    [HideInInspector] public TestEnemy testEnemy;

    //-------PATHFINDING A*--------
    [HideInInspector] public AIPath aIPath;
    [HideInInspector] public AIDestinationSetter aIDest;

    //-------COMPONENTS--------
    [HideInInspector] public Vector3 startingPosition;
    [HideInInspector] public GameObject playerObject;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Transform target;
    [HideInInspector] public GameObject enemyObject;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public GameObject model;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public GameObject pointTarget;
    public GameObject bulletPrefab;

    //-------METHODS--------------
    [HideInInspector] public Stack<BaseState> stateStack = new Stack<BaseState>();

    //-------INSPECTOR VALUES--------
    [Header("State values")]

    [Header("General")]
    [SerializeField] private bool showDebugGizmos = false;
    public float defaultSpeed = 12f;
    public bool isAnNPC = false;

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
    public float shootingInaccuracySize = 0.4f;
    public float shootingPlayerDistanceExit = 35f;
    public float shootingBurstShortTiming = .2f;
    public float shootingBurstLongTiming = 2f;
    [HideInInspector] public float dodgeRandom;

    [Header("Fleeing state")]
    public float fleeingSpeed = 15f;
    public float maxTurnAngle = 45f;
    public float fleeingPlayerDistanceExit = 40f;

    [Header("Afraid state")]
    public float afraidPlayerDistanceExit = 20f;





    public void Awake()
    {
        aIPath = GetComponent<AIPath>();
        aIDest = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody2D>();
        enemyObject = gameObject;
        playerObject = GameObject.Find("Player");
        target = aIDest.target;
        pointTarget = transform.parent.GetChild(1).gameObject; //поиск пустого ГО к которому идет враг во время состояния roaming 
        animator = GetComponent<Animator>();
        model = transform.GetChild(0).gameObject;
        spriteRenderer = model.GetComponent<SpriteRenderer>();
        waitingState = new EnemyWaiting(this); //присваивание состояний к переменным с этой стейт машиной
        roamingState = new EnemyRoaming(this);
        chasingState = new EnemyChasing(this);
        shootingState = new EnemyShooting(this);
        fleeingState = new EnemyFleeing(this);
        afraidState = new EnemyAfraid(this);
        GetGameObject(enemyObject);
        aIPath.maxSpeed = defaultSpeed;
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
        return roamingState;
    }
    public bool CheckPlayerInRange(float alertDistance)     // метод проверяющий расстояние до игрока с кастомной переменной
    {
        if (playerObject != null)
        {
            float distance = Vector3.Distance(enemyObject.transform.position, playerObject.transform.position); //проверка дистанции от объекта а до б
            if (distance <= alertDistance)
                return true;
            else
                return false;
        }

        return false;


    }

    public void TargetSetter(GameObject newTarget)
    {
        aIDest.target = newTarget.transform;
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
