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
    [HideInInspector] public Vector3 startingPosition;
    [HideInInspector] public GameObject playerObject;
    public GameObject roamingTarget;
    public float roamingInterval = 30f;
    public float roamRadius = 10f;


    [HideInInspector] public Transform target;
    private GameObject enemyObject;


    public void Awake()
    {
        aIPath = GetComponent<AIPath>();
        aIDest = GetComponent<AIDestinationSetter>();
        enemyObject = gameObject;
        playerObject = GameObject.Find("Player");
        target = aIDest.target;
        roamingTarget = transform.parent.GetChild(1).gameObject; //поиск пустого ГО к которому идет враг во время состояния roaming 
        waitingState = new EnemyWaiting(this); //присваивание состояний к переменным с этой стейт машиной
        roamingState = new EnemyRoaming(this);
        chasingState = new EnemyChasing(this);

    }
    protected override BaseState GetInitialState() //начальное состояние в виде состояния ожидания
    {
        return roamingState;
    }
    public bool CheckPlayerInRange(float alertDistance)     // метод проверяющий расстояние до игрока с переменной которую нужно отдавать с каждого состояния
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
}
