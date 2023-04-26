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
    public GameObject pointTarget;
    public GameObject bulletPrefab;

    //-------INSPECTOR VALUES--------
    [Header("Roaming state")]
    public float roamingInterval = 30f;
    public float roamRadius = 10f;
    public float roamPlayerDistanceEnter = 30f;

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
    public float maxTurnAngle = 45f;




    public void Awake()
    {
        aIPath = GetComponent<AIPath>();
        aIDest = GetComponent<AIDestinationSetter>();
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
        GetGameObject(enemyObject);
    }
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
}
