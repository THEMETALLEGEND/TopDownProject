using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeing : BaseState
{
    private TestEnemyStates _sm;
    private GameObject _fleeTarget; // ������ ������-����
    private Vector3 targetpoint;
    private float startTime;
    private float waitTime = 1f; // �������� � 1 �������
    private bool waiting = false;
    public EnemyFleeing(TestEnemyStates enemyStateMachine) : base("TestEnemyFleeing", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        _sm.aIPath.maxSpeed = _sm.fleeingSpeed; //��������� �������� ������ ��������� �� ������ �����������
        // ������� ������ ������-����
        _fleeTarget = new GameObject("FleeTarget");
        _sm.TargetSetter(_fleeTarget); // ������������� ������ ������-���� � �������� ���� ������
    }



    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_sm.playerObject != null)
        {
            Vector3 dir = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized; // ��������� ������ ����������� �� ������ �� ����
            Vector3 opDir = dir * -1; //�������� ���� ������

            if (_sm.CheckPlayerInRange(40f))
            {
                targetpoint = _sm.enemyObject.transform.position + opDir * 3f; //��������� ������� ����� �� ������ � ��������������� �� ������ ������� �� 3
            }

            _fleeTarget.transform.position = targetpoint; // ������������� ������� ������� �������-����
        }
        else
            _sm.ChangeState(_sm.roamingState);

        if (!_sm.CheckPlayerInRange(_sm.fleeingPlayerDistanceExit) && _sm.aIPath.reachedEndOfPath == true) //���� ������ ���������� ��������
                stateMachine.ChangeState(_sm.roamingState);

        

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (_sm.aIDest.target != null && _sm.aIPath.velocity.magnitude <= .2f && _sm.aIPath.reachedEndOfPath) //���� ���� �� ���� � ��������� � �������� ����� ���� (aIPath.velocity.magnitude �������� ������ � updatephysics)
        {
            if (!waiting) //������ ������� ����������� ������� ������ ��� ������� � ��������� ������
            {
                startTime = Time.time; // ��������� ������� �����
                waiting = true;
            }
            else if (Time.time - startTime >= waitTime)
            {
                _sm.ChangeState(_sm.afraidState);
            }
        }
        else
        {
            waiting = false; // ���������� ������, ���� ������� ������ �� �����������
        }
    }
    public override void Exit()
    {
        base.Exit();


        if (_fleeTarget != null)    // ������� ������ ������-���� ��� ������ �� ���������
        {
            GameObject.Destroy(_fleeTarget);
            _fleeTarget = null;
        }
    }
}