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

    [Header("Afraid state")]
    public float afraidPlayerDistanceExit = 20f;

    [Header("Detecting")]
    public bool playerRaycastHit;


    //--------------TEMPORARY------------
    public LayerMask obstacleLayer; // ����, ���������� ������� � ��������� � ����� obstacles
    public LayerMask playerLayer;
    public float rayLength = 28f; // ����� �����
    public int rayCount = 48; // ���������� �����
    public float angleStep = 7.5f; // ��� ����� ������ �����
    public float playerDetectionDistance = 2f; // ����������, �� ������� ����� ���������� ������




    public void Awake()
    {
        aIPath = GetComponent<AIPath>();
        aIDest = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody2D>();
        enemyObject = gameObject;
        playerObject = GameObject.Find("Player");
        playerRaycast = playerObject.GetComponent<PlayerRaycast>();
        target = aIDest.target;
        pointTarget = transform.parent.GetChild(1).gameObject; //����� ������� �� � �������� ���� ���� �� ����� ��������� roaming 
        animator = GetComponent<Animator>();
        model = transform.GetChild(0).gameObject;
        spriteRenderer = model.GetComponent<SpriteRenderer>();
        waitingState = new EnemyWaiting(this); //������������ ��������� � ���������� � ���� ����� �������
        roamingState = new EnemyRoaming(this);
        chasingState = new EnemyChasing(this);
        shootingState = new EnemyShooting(this);
        fleeingState = new EnemyFleeing(this);
        afraidState = new EnemyAfraid(this);
        GetGameObject(enemyObject);
        aIPath.maxSpeed = defaultSpeed;
        // ��������� ����� � �������
        //playerRaycast.raycastHitEnemies.Add(gameObject, false);
    }

    public override void ChangeState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
            stateStack.Push(currentState); // ��������� ������� ��������� � ���� ����� ���������
        }

        currentState = newState;
        currentState.Enter();
    }

    protected override BaseState GetInitialState() //��������� ��������� � ���� ��������� ��������
    {
        return roamingState;
    }

    public bool CheckPlayerContact(int rayCount, int playerRayCount, float rayLength)
    {
        int playerContacts = 0;

        for (int i = 0; i < rayCount; i++)
        {
            // ��������� ���� ����
            float angle = i * (360f / rayCount);

            // ��������� ����������� ���� �� ������ ����
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.right;

            // ��������� ��� � �������� ���������� � ������������ � ��������� �� ����� obstacleLayer � playerLayer
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, LayerMask.GetMask("Player", "Obstacles"));

            // �������� ���� ��� ����� �� ������ ������������ ���� � ��������
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

            // ������ ����� ��� ����
            if (showDebugGizmos)
                Debug.DrawLine(transform.position, hit.collider != null ? hit.point : transform.position + direction * rayLength, lineColor);

            // ���� �� ����� ������ ���������� �����, ���������� ������, ���������� true
            if (playerContacts >= playerRayCount)
            {
                return true;
            }
        }

        // ���� �� ����� ������ ���������� �����, ���������� false
        return false;
    }

    public void SetAlerted(bool value)
    {
        if (value)
        {
            // ��������� ���� ������� � ������� ����������
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


    public bool CheckPlayerInRange(float alertDistance)     // ����� ����������� ���������� �� ������ � ��������� ����������
    {
        if (playerObject != null)
        {
            float distance = Vector3.Distance(enemyObject.transform.position, playerObject.transform.position); //�������� ��������� �� ������� � �� �
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

    public void DiceMethod(float successChance, Action methodToRun) //����� ������� ��������� ���� ���������� ������ � ��� ������ ��������� ���.
                                                                            //��� ������������� ������ Action ����� ��������� ������ �� ������ System
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
            BaseState previousState = stateStack.Pop(); // ��������� ���������� ��������� �� �����
            ChangeState(previousState); // ��������� � ���������� ���������
        }
        else
            Debug.Log("���� ����");
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
