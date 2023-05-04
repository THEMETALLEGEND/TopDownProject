using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAfraid : BaseState
{

    private TestEnemyStates _sm;
    public EnemyAfraid(TestEnemyStates enemyStateMachine) : base("TestEnemyAfraid", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        _sm.TargetSetter(_sm.pointTarget);
        _sm.pointTarget.transform.position = this._sm.transform.position;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_sm.CheckPlayerInRange(20f))
        {
            _sm.ReturnToPreviousState();
        }
    }
}