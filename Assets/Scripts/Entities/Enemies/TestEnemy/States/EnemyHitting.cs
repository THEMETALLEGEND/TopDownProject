using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitting : BaseState
{

    private TestEnemyStates _sm;
    public EnemyHitting(TestEnemyStates enemyStateMachine) : base("TestEnemyHitting", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();

        _sm.TargetSetter(_sm.playerObject);
        //_sm.aIPath.maxSpeed = _sm.defaultSpeed / 3;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_sm.testEnemy.isDamaging)
            stateMachine.ChangeState(_sm.chasingState);
        else if (_sm.playerObject == null)
            stateMachine.ChangeState(_sm.roamingState);
    }

    public override void Exit()
    {
        base.Exit();

    }
}