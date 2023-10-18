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

        if (_sm.playerObject != null)
        {
            _sm.TargetSetter(_sm.playerObject);
            _sm.aIPath.maxSpeed = _sm.defaultSpeed;
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _sm.SetAlerted(true);


        /*if (_sm.testEnemy.isDamaging)
            _sm.aIPath.maxSpeed = _sm.defaultSpeed / 3;
        else if (!_sm.testEnemy.isDamaging)
            _sm.aIPath.maxSpeed = _sm.defaultSpeed;*/

        if (!_sm.CheckPlayerInRange(_sm.chasingPlayerDistanceExit)) //если дальше указанного значения
            stateMachine.ChangeState(_sm.roamingState);

        if (_sm.CheckPlayerContact(100, 5, 30) && !_sm.isMelee) //если ближе указанного значения и не ближник
            stateMachine.ChangeState(_sm.shootingState);

        if (_sm.testEnemy.isDamaging && _sm.isMelee) //если касаемся игрока и ближник
            stateMachine.ChangeState(_sm.hittingState);
    }
}
