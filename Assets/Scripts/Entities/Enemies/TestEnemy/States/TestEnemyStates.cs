using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TestEnemyStates : StateMachine
{
    [HideInInspector] public EnemyWaiting waitingState;
    [HideInInspector] public EnemyRoaming roamingState;

    private void Awake()
    {
        waitingState = new EnemyWaiting(this);
        roamingState = new EnemyRoaming(this);
    }
    protected override BaseState GetInitialState()
    {
        return waitingState;
    }
}
