using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoaming : BaseState
{

    private TestEnemyStates _sm;
    public EnemyRoaming(TestEnemyStates enemyStateMachine) : base("TestEnemyRoaming", enemyStateMachine) {
        _sm = (TestEnemyStates)stateMachine;
    }
    //���������� enemyroaming �� ����� ������� ������� �������� enemystatemachine �� �������� (�������� ���������) � ��������� ��������� � ��� ��������� ����� �������.
    //����� ����������� ������ ����������� � ���������� ����� �� ������� �� ������ ���
    
    private bool timerEnded = false;

    public override void Enter()
    {
        base.Enter();

        _sm.startingPosition = _sm.transform.position; //��������� ������� ��� ������� ��������� roaming
        _sm.TargetSetter(_sm.roamingTarget);    //���������� ���� 
        _sm.aIDest.target.position = GetRoamingPosition();      //��������� ������������ ������� ���� �� ��������� �����
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (Input.GetKeyDown("f"))
            stateMachine.ChangeState(_sm.waitingState); //������ ��������� �� ������

        _sm.CheckPlayerInRange(45); //��������� ��������� �� ������ (����� � �����������)
        if (_sm.CheckPlayerInRange(45))   
        {
            stateMachine.ChangeState(_sm.chasingState);
        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        if (timerEnded == false && _sm.aIPath.reachedDestination == true) //���� ������ �� �������� � ������ � ����
        {
            _sm.roamingInterval -= 1; //�������� ������ �������
        }

        if (_sm.roamingInterval <= 0f) //���� ������ ����� ��� ������ ����, �� �� ��������
        {
            timerEnded = true;
        }



        if (timerEnded == true && _sm.aIPath.reachedDestination == true) //���� ������ �������� � ������ � ����
        {
            _sm.aIDest.target.position = GetRoamingPosition(); //����� ��������� ���� � ��������� �������
            _sm.roamingInterval = 60f; //������
            timerEnded = false; //������ ������ �� ��������, ��������� ������
        }
    }


    private Vector3 GetRoamingPosition()
    {
        return _sm.startingPosition + Random.insideUnitSphere * _sm.roamRadius; //�� ��������� ������� ������ ����� ������� ��� ������������
    }
}
