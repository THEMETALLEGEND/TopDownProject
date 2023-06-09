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
    [HideInInspector] public PlayerRaycast playerRaycast;

    //-------LOGIC---------------
    public GameObject pointTarget;
    public GameObject bulletPrefab;
    public bool isAlerted = false;
    public float alertRadius = 10f;

    //-------METHODS--------------
    [HideInInspector] public Stack<BaseState> stateStack = new Stack<BaseState>();

    //-------INSPECTOR VALUES--------
    [Header("State values")]

    [Header("General")]
    public bool showDebugGizmos = false;
    public bool showDebugLogs = false;
    public float defaultSpeed = 12f;
    public bool isAnNPC = false;
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
    public float shootingInaccuracySize = 0.4f;
    public float shootingPlayerDistanceExit = 35f;
    public float shootingBurstShortTiming = .2f;
    public float shootingBurstLongTiming = 2f;
    [HideInInspector] public float dodgeRandom;

    [Header("Fleeing state")]
    public float fleeingSpeed = 15f;
    public float maxTurnAngle = 45f;
    public float fleeingPlayerDistanceExit = 40f;
    public float fleeingTime = 10f;

    [Header("Afraid state")]
    public float afraidPlayerDistanceExit = 20f;

    [Header("Detecting")]
    public bool playerRaycastHit;


    //--------------TEMPORARY------------
    public LayerMask obstacleLayer; // слой, содержащий объекты с коллизией и тегом obstacles
    public LayerMask playerLayer;
    public float rayLength = 28f; // длина лучей
    public int rayCount = 48; // количество лучей
    public float angleStep = 7.5f; // шаг между углами лучей
    public float playerDetectionDistance = 2f; // рассто€ние, на котором агент определ€ет игрока




    public void Awake()
    {
        aIPath = GetComponent<AIPath>();
        aIDest = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody2D>();
        enemyObject = gameObject;
        playerObject = GameObject.Find("Player");
        playerRaycast = playerObject.GetComponent<PlayerRaycast>();
        target = aIDest.target;
        pointTarget = transform.parent.GetChild(1).gameObject; //поиск пустого √ќ к которому идет враг во врем€ состо€ни€ roaming 
        animator = GetComponent<Animator>();
        model = transform.GetChild(0).gameObject;
        spriteRenderer = model.GetComponent<SpriteRenderer>();
        waitingState = new EnemyWaiting(this); //присваивание состо€ний к переменным с этой стейт машиной
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
            stateStack.Push(currentState); // добавл€ем текущее состо€ние в стек перед переходом
        }

        currentState = newState;
        currentState.Enter();
    }

    protected override BaseState GetInitialState() //начальное состо€ние в виде состо€ни€ ожидани€
    {
        return roamingState;
    }

    public bool CheckPlayerContact(int rayCount, int playerRayCount, float rayLength)
    {
        int playerContacts = 0;

        for (int i = 0; i < rayCount; i++)
        {
            // вычисл€ем угол луча
            float angle = i * (360f / rayCount);

            // вычисл€ем направление луча на основе угла
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.right;

            // выпускаем луч и получаем информацию о столкновении с объектами на сло€х obstacleLayer и playerLayer
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, LayerMask.GetMask("Player", "Obstacles"));

            // выбираем цвет дл€ линии на основе столкновени€ луча с объектом
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

            // рисуем линию дл€ луча
            if (showDebugGizmos)
                Debug.DrawLine(transform.position, hit.collider != null ? hit.point : transform.position + direction * rayLength, lineColor);

            // если мы нашли нужное количество лучей, касающихс€ игрока, возвращаем true
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
            // ќповещаем всех агентов в радиусе оповещени€
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, alertRadius);
            foreach (Collider2D collider in colliders)
            {
                TestEnemyStates agent = collider.GetComponent<TestEnemyStates>();
                if (agent != null && agent != this)
                {
                    agent.isAlerted = true;
                }
            }
        }
    }


    public bool CheckPlayerInRange(float alertDistance)     // метод провер€ющий рассто€ние до игрока с кастомной переменной
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

    public void DiceMethod(float successChance, Action methodToRun) //метод который принимает шанс выполнени€ метода и при успехе выполн€ет его.
                                                                            //дл€ использовани€ метода Action нужно указывать ссылку на сборку System
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
            BaseState previousState = stateStack.Pop(); // »звлекаем предыдущее состо€ние из стека
            if (previousState.GetType() == typeof(EnemyAfraid)) // ѕровер€ем, €вл€етс€ ли предыдущее состо€ние "afraidState"
            {
                if (stateStack.Count > 0) // ѕровер€ем, есть ли еще состо€ние в стеке
                {
                    //previousState = stateStack.Pop(); // ≈сли есть, извлекаем еще одно состо€ние из стека
                   // Debug.Log("Returned state other than afraidState");

                    //fleeing, в который должен переключатьс€ агент, сейчас сразу же переключаетс€ обратно. fleeing нужны доп. проверки
                    previousState = new EnemyRoaming(this);
                }
                else
                {
                    // ≈сли больше нет состо€ний в стеке, не делаем ничего, так как мы не можем вернутьс€ к состо€нию до "afraidState"
                    //Debug.Log("Ќет предыдущего состо€ни€ дл€ возврата");
                    //ChangeState(roamingState);

                    // ≈сли больше нет состо€ний в стеке, переключаемс€ на EnemyRoaming
                    previousState = new EnemyRoaming(this);
                }
            }
            ChangeState(previousState); // ѕереходим в предыдущее состо€ние
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
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, fleeingPlayerDistanceExit);
    }
}
