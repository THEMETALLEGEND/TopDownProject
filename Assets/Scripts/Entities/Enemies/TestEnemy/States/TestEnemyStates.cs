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
    [HideInInspector] public EnemyStuck stuckState;

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

    //-------METHODS--------------
    private Stack<BaseState> stateStack = new Stack<BaseState>();

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
    public float retreatDistance = 10f;


    public float defaultSpeed = 8f;



    public void Awake()
    {
        aIPath = GetComponent<AIPath>();
        aIDest = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody2D>();
        enemyObject = gameObject;
        playerObject = GameObject.Find("Player");
        target = aIDest.target;
        pointTarget = transform.parent.GetChild(1).gameObject; //����� ������� �� � �������� ���� ���� �� ����� ��������� roaming 
        animator = GetComponent<Animator>();
        waitingState = new EnemyWaiting(this); //������������ ��������� � ���������� � ���� ����� �������
        roamingState = new EnemyRoaming(this);
        chasingState = new EnemyChasing(this);
        shootingState = new EnemyShooting(this);
        fleeingState = new EnemyFleeing(this);
        stuckState = new EnemyStuck(this);
        GetGameObject(enemyObject);
        aIPath.maxSpeed = defaultSpeed;
    }

    /*private void Update()
    {
        Debug.Log(aIPath.velocity.magnitude);
    }*/

    protected override BaseState GetInitialState() //��������� ��������� � ���� ��������� ��������
    {
        return roamingState;
    }
    public bool CheckPlayerInRange(float alertDistance)     // ����� ����������� ���������� �� ������ � ��������� ����������
    {
        float distance = Vector3.Distance(enemyObject.transform.position, playerObject.transform.position); //�������� ��������� �� ������� � �� �
        if (distance <= alertDistance)
            return true;
        else
            return false;


    }

    public void TargetSetter(GameObject newTarget)
    {
        aIDest.target = newTarget.transform;
    }

    public void DiceMethod(float successChance, Action methodToRun) //����� ������� ��������� ���� ���������� ������ � ��� ������ ��������� ���.
                                                                            //��� ������������� ������ Action ����� ��������� ������ �� ������ System
    {
        float randomValue = UnityEngine.Random.Range(0f, 1f);
        if (randomValue <= successChance / 100f)
        {
            methodToRun.Invoke();
        }
    }

    public void ObstacleCheck(GameObject baseTarget)
    {
        // ���������, ��� ���� ������ � ����� �� ��������
        if (aIDest.target != null && aIPath.velocity.magnitude <= 0.1f && !aIPath.reachedEndOfPath)
        {
            reachedObstacleRetreat = false;
            Vector2 agentPos = transform.position; // �������� ������� ������� ������
            Vector2 targetPos = aIDest.target.position; // �������� ������� ������� ���� ������
            Vector2 dirToAgent = (agentPos - targetPos).normalized;   // �������� ����������� �� ���� � ������
            Vector2 newTargetPos = targetPos + dirToAgent * -retreatDistance; // �������� ����� ������� ����
            baseTarget.transform.position = newTargetPos;    // ������������� ����� ������� ����
            aIDest.target = baseTarget.transform;  // ������������� ����� ���� ��� ������
        }
        // ���������, ��� ���� ������ � ����� ������ ����� ����
        else if (!reachedObstacleRetreat)
        {
            if (aIPath.reachedEndOfPath)
            {
                reachedObstacleRetreat = true;
            }
        }
    }

    public void EntityStuck(BaseState baseState)
    {
        baseState.Exit(); // ����������� ����� ������ � ������ ���������
        stateStack.Push(baseState); // ��������� ������ ��������� � ����
        ChangeState(stuckState);
    }

    public void ReturnToPreviousState()
    {
        if (stateStack.Count > 0)
        {
            BaseState previousState = stateStack.Pop(); // ��������� ���������� ��������� �� �����
            ChangeState(previousState); // ��������� � ���������� ���������
        }
    }
}
