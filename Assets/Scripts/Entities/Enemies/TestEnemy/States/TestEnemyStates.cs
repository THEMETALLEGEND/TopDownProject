using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TestEnemyStates : StateMachine
{
    //------ROAMING---------
    [HideInInspector] public EnemyWaiting waitingState;
    [HideInInspector] public EnemyRoaming roamingState;
    [HideInInspector] public EnemyChasing chasingState;
    [HideInInspector] public EnemyShooting shootingState;
    [HideInInspector] public TestEnemy testEnemy;
    [HideInInspector] public AIPath aIPath;
    [HideInInspector] public AIDestinationSetter aIDest;
    [HideInInspector] public Vector3 startingPosition;
    [HideInInspector] public GameObject playerObject;
    [HideInInspector] public Animator animator;
    public GameObject pointTarget;
    public float roamingInterval = 30f;
    public float roamRadius = 10f;
    public GameObject bulletPrefab;


    [HideInInspector] public Transform target;
    [HideInInspector] public GameObject enemyObject;


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
}
