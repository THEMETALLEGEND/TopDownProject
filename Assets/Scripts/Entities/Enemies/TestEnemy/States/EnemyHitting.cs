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

        _sm.TargetSetter(_sm.PlayerObject);
        _sm.AIPath.maxSpeed = _sm.defaultSpeed / 3;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_sm.EnemyClass.IsDamaging)
            stateMachine.ChangeState(_sm.ChasingState);
        else if (_sm.PlayerObject == null)
            stateMachine.ChangeState(_sm.RoamingState);
    }

    public override void Exit()
    {
        base.Exit();

    }
}