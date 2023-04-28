using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeing : BaseState
{
    private TestEnemyStates _sm;
    private GameObject _fleeTarget; // ������ ������-����
    private Vector3 targetpoint;
    public EnemyFleeing(TestEnemyStates enemyStateMachine) : base("TestEnemyFleeing", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        _sm.aIPath.maxSpeed = _sm.fleeingSpeed;
        // ������� ������ ������-����
        _fleeTarget = new GameObject("FleeTarget");
        _sm.TargetSetter(_fleeTarget); // ������������� ������ ������-���� � �������� ���� ������
    }



    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // ��������� ������ ����������� �� ������ �� ����
        Vector3 dir = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized;
        Vector3 opDir = dir * -1;

        if (_sm.CheckPlayerInRange(40f))
        {
            // �������� ����� �� ������������ ���������� �� ����
            targetpoint = _sm.enemyObject.transform.position + opDir * 3f;
        }

        _fleeTarget.transform.position = targetpoint; // ������������� ������� ������� �������-����

        if (!_sm.CheckPlayerInRange(_sm.fleeingPlayerDistanceExit) && _sm.aIPath.reachedDestination == true) //���� ������ ���������� ��������
                stateMachine.ChangeState(_sm.roamingState);
        _sm.ObstacleCheck();
    }
    public override void Exit()
    {
        base.Exit();

        _sm.aIPath.maxSpeed = _sm.defaultSpeed;

        if (_fleeTarget != null)    // ������� ������ ������-���� ��� ������ �� ���������
        {
            GameObject.Destroy(_fleeTarget);
            _fleeTarget = null;
        }
    }
}