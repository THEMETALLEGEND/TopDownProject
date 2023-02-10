using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaiting : BaseState
{
    public EnemyWaiting(TestEnemyStates enemyStateMachine) : base("TestEnemyWaiting", enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (Input.GetKeyDown("q"))
            stateMachine.ChangeState(((TestEnemyStates)stateMachine).roamingState);
    }

}
