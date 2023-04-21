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

        //_sm.CheckPlayerInRange(40); //если дальше 60
        if (!_sm.CheckPlayerInRange(_sm.chasingPlayerDistanceExit)) //если дальше указанного значения
            stateMachine.ChangeState(_sm.roamingState);

        if (_sm.CheckPlayerInRange(_sm.chasingPlayerDistanceEnter)) //если ближе указанного значения
            stateMachine.ChangeState(_sm.shootingState);
    }
}
