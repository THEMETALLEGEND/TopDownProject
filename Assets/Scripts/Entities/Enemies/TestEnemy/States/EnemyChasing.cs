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
        else
            _sm.ChangeState(_sm.roamingState); 
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        //_sm.CheckPlayerInRange(40); //���� ������ 60
        if (!_sm.CheckPlayerInRange(_sm.chasingPlayerDistanceExit)) //���� ������ ���������� ��������
            stateMachine.ChangeState(_sm.roamingState);

        if (_sm.CheckPlayerContact(48, 3, 30)) //���� ����� ���������� ��������
            stateMachine.ChangeState(_sm.shootingState);
    }
}
