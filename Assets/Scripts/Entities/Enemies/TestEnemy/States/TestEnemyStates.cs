using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TestEnemyStates : StateMachine
{
    [HideInInspector] public EnemyWaiting waitingState;
    [HideInInspector] public EnemyRoaming roamingState;
    [HideInInspector] public TestEnemy testEnemy;
    [HideInInspector] public AIPath aIPath;
    [HideInInspector] public AIDestinationSetter aIDest;
    [HideInInspector] public Vector3 target;
    [HideInInspector] public Vector3 startingPosition;

    public float roamRadius = 10f;

    public void Awake()
    {
        aIPath = GetComponent<AIPath>();
        aIDest = GetComponent<AIDestinationSetter>();
        waitingState = new EnemyWaiting(this);
        roamingState = new EnemyRoaming(this);
    }
    protected override BaseState GetInitialState()
    {
        return waitingState;
    }
}
