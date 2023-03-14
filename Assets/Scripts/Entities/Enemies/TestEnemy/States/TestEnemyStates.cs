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
    [HideInInspector] public TestEnemy testEnemy;
    [HideInInspector] public AIPath aIPath;
    [HideInInspector] public AIDestinationSetter aIDest;
    [HideInInspector] public Vector3 target;
    [HideInInspector] public Vector3 startingPosition;
    [HideInInspector] public GameObject playerObject;
    public float roamingInterval = 30f;
    public float roamRadius = 10f;



    private GameObject enemyObject;
    public float alertDistance = 10;

    public void Awake()
    {
        aIPath = GetComponent<AIPath>();
        aIDest = GetComponent<AIDestinationSetter>();
        enemyObject = gameObject;
        playerObject = GameObject.Find("Player");
        waitingState = new EnemyWaiting(this); //присваивание состояний к переменным с этой стейт машиной
        roamingState = new EnemyRoaming(this);
        chasingState = new EnemyChasing(this);
    }
    protected override BaseState GetInitialState() //начальное состояние в виде состояния ожидания
    {
        return waitingState;
    }
    public bool CheckPlayerInRange()
    {
        float distance = Vector3.Distance(enemyObject.transform.position, playerObject.transform.position);
        if (distance <= alertDistance)
            return true;
        else
            return false;

        
    }
}
