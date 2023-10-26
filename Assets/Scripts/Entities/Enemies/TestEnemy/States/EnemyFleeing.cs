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

    private float startTime2;
    private float waitTime2 = 4f; 
    private bool waiting2 = false;
    private GameObject player;
    private float checkOthersAfraid = 3; //������ �������� ���� �� ������ � �������� afraid

    public EnemyFleeing(TestEnemyStates enemyStateMachine) : base("TestEnemyFleeing", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _sm.isAlerted = true;
        player = GameObject.Find("Player");
        _sm.aIPath.maxSpeed = _sm.fleeingSpeed; //��������� �������� ������ ��������� �� ������ �����������
        //_fleeTarget = new GameObject("FleeTarget"); // ������� ������ ������-����
        _sm.TargetSetter(_sm.pointTarget); // ������������� ������ ������-���� � �������� ���� ������
    }



    public override void UpdateLogic()
    {
        base.UpdateLogic();


        if (!_sm.isAlerted)
            _sm.ChangeState(_sm.roamingState);


        if (_sm.playerObject != null)
        {
            Vector3 dir = (player.transform.position - _sm.enemyObject.transform.position).normalized; // ��������� ������ ����������� �� ������ �� ����
            Vector3 opDir = dir * -1; //�������� ���� ������
            targetpoint = _sm.enemyObject.transform.position + opDir * 3f; //��������� ������� ����� �� ������ � ��������������� �� ������ ������� �� 3

            _sm.pointTarget.transform.position = targetpoint; // ������������� ������� ������� �������-����
        }
        else
        {
            _sm.ChangeState(_sm.roamingState);
        }

        //���� ������ ���������� ��������
        if (!_sm.CheckPlayerInRange(_sm.fleeingPlayerDistanceExit))
        {
            if (!waiting2)
            {
                startTime2 = Time.time;
                waiting2 = true;
            }
            else if (Time.time - startTime2 >= waitTime2)
            {
                stateMachine.ChangeState(_sm.roamingState);
            }
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_sm.transform.position, checkOthersAfraid);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                TestEnemyStates enemyStates = collider.GetComponent<TestEnemyStates>();

                if (enemyStates != null && enemyStates.isAfraid)
                {
                    _sm.ChangeState(_sm.afraidState);
                }
            }
        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (_sm.aIPath.velocity.magnitude <= .2f && !_sm.CheckPlayerInRange(_sm.fleeingPlayerDistanceExit))
        {
            _sm.ChangeState(_sm.roamingState);
        }

            if (_sm.aIDest.target != null && _sm.aIPath.velocity.magnitude <= .5f && _sm.aIPath.reachedEndOfPath) //���� ���� �� ���� � ��������� � �������� ����� ���� (aIPath.velocity.magnitude �������� ������ � updatephysics)
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

        _sm.isAlerted = false;
        waiting2 = false;
    }
}