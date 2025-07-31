using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasing : BaseState
{
    private TestEnemyStates _sm;
    public EnemyChasing(TestEnemyStates enemyStateMachine) : base("TestEnemyChasing", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        if (_sm.PlayerObject != null)
        {
            _sm.TargetSetter(_sm.PlayerObject);
            _sm.AIPath.maxSpeed = _sm.defaultSpeed;
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _sm.SetAlerted(true);


        /*if (_sm.EnemyClass.IsDamaging)
            _sm.AIPath.maxSpeed = _sm.defaultSpeed / 3;
        else if (!_sm.EnemyClass.IsDamaging)
            _sm.AIPath.maxSpeed = _sm.defaultSpeed;*/

        if (!_sm.CheckPlayerInRange(_sm.chasingPlayerDistanceExit)) //���� ������ ���������� ��������
            stateMachine.ChangeState(_sm.PathMovingState);

        if (_sm.CheckPlayerContact(100, 3, 30) && !_sm.isMelee) //���� ����� ���������� �������� � �� �������
            stateMachine.ChangeState(_sm.ShootingState);

        if (_sm.EnemyClass.IsDamaging && _sm.isMelee) //���� �������� ������ � �������
            stateMachine.ChangeState(_sm.HittingState);
    }
}
