using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyWaiting : BaseState
{
    private TestEnemyStates _sm;
    public EnemyWaiting(TestEnemyStates enemyStateMachine) : base("TestEnemyWaiting", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        var aiPath = _sm.GetComponent<AIPath>();
        aiPath.canMove = false;
    }
}
