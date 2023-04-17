using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasing : BaseState
{
    // Start is called before the first frame update

    private TestEnemyStates _sm;
    public EnemyChasing(TestEnemyStates enemyStateMachine) : base("TestEnemyChasing", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        _sm.TargetSetter(_sm.playerObject);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _sm.CheckPlayerInRange(60); //���� ������ 60
        if (!_sm.CheckPlayerInRange(60))
            stateMachine.ChangeState(_sm.roamingState);

        if (_sm.CheckPlayerInRange(25)) //���� ����� 25
            stateMachine.ChangeState(_sm.shootingState);
    }
}
