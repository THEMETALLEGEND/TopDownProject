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
    public GameObject pointTarget;
    public GameObject bulletPrefab;

    //-------INSPECTOR VALUES--------
    [Header("Roaming state")]
    public float roamingInterval = 30f;
    public float roamRadius = 10f;
    public float roamPlayerDistanceEnter = 20f;

    [Header("Chasing state")]
    public float chasingPlayerDistanceEnter = 15f;
    public float chasingPlayerDistanceExit = 40f;

    [Header("Shooting state")]
    public float shootingInaccuracySize = 0.4f;
    public float shootingPlayerDistanceExit = 20f;
    public float shootingBurstShortTiming = .2f;
    public float shootingBurstLongTiming = 2f;
    [HideInInspector] public float dodgeRandom;

    [Header("Fleeing state")]
    public float fleeingSpeed = 12f;
    public float maxTurnAngle = 45f;
    public float fleeingPlayerDistanceExit = 40f;

    [Header("Obstacle check")]
    [HideInInspector] public bool reachedObstacleRetreat = true;


    public float defaultSpeed = 8f;



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
        waitingState = new EnemyWaiting(this); //присваивание состояний к переменным с этой стейт машиной
        roamingState = new EnemyRoaming(this);
        chasingState = new EnemyChasing(this);
        shootingState = new EnemyShooting(this);
        fleeingState = new EnemyFleeing(this);
        GetGameObject(enemyObject);
        aIPath.maxSpeed = defaultSpeed;
    }
    protected override BaseState GetInitialState() //начальное состояние в виде состояния ожидания
    {
        return roamingState;
    }
    public bool CheckPlayerInRange(float alertDistance)     // метод проверяющий расстояние до игрока с кастомной переменной
    {
        float distance = Vector3.Distance(enemyObject.transform.position, playerObject.transform.position); //проверка дистанции от объекта а до б
        if (distance <= alertDistance)
            return true;
        else
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

    public void ObstacleCheck(GameObject baseTarget)
    {
        reachedObstacleRetreat = false;
        if (aIDest.target != null && aIPath.velocity.magnitude <= .1f && !reachedObstacleRetreat)
        {
            Vector2 desiredDir = aIPath.desiredVelocity.normalized;
            // Создаем новую цель в обратном направлении от агента на расстоянии retreatDistance
            Vector2 newTargetPos = (Vector2)transform.position - desiredDir * 5f;
            pointTarget.transform.position = newTargetPos;
        }
    }
}
