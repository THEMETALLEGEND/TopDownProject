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
        _sm.TargetSetter(_sm.pointTarget);    //���������� ���� 
        _sm.aIDest.target.position = GetRoamingPosition();      //��������� ������������ ������� ���� �� ��������� �����
        _sm.aIPath.maxSpeed = _sm.roamingSpeed;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // ������� ����� � ������������ ���������
        for (int i = 0; i < _sm.rayCount; i++)
        {
            // ��������� ���� ����
            float angle = i * _sm.angleStep;

            // ��������� ����������� ���� �� ������ ����
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.right;

            // ��������� ��� � �������� ���������� � ������������ � ��������� �� ����� obstacleLayer � playerLayer
            RaycastHit2D hit = Physics2D.Raycast(_sm.transform.position, direction, _sm.rayLength, _sm.obstacleLayer | _sm.playerLayer);

            // �������� ���� ��� ����� �� ������ ������������ ���� � ��������
            Color lineColor = hit.collider != null ? hit.collider.CompareTag("Player") ? Color.red : Color.yellow : Color.green;

            // ������ ����� ��� ����
            if (_sm.showDebugGizmos)
                Debug.DrawLine(_sm.transform.position, hit.collider != null ? hit.point : _sm.transform.position + direction * _sm.rayLength, lineColor);

            // ���� ��� ���������� � �������, ��������� � ������ ���������
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                // ������������� � ������ ��������� � ����������� �� ����, �������� �� ����� NPC
                if (_sm.isAnNPC && !_sm.debugMode)
                    stateMachine.ChangeState(_sm.fleeingState);
                else if (!_sm.debugMode)
                    stateMachine.ChangeState(_sm.chasingState);
            }
        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        if (timerEnded == false && _sm.aIPath.reachedEndOfPath == true) //���� ������ �� �������� � ������ � ����
        {
            _sm.roamingInterval -= 1; //�������� ������ �������
        }

        if (_sm.roamingInterval <= 0f) //���� ������ ����� ��� ������ ����, �� �� ��������
        {
            timerEnded = true;
        }



        if (timerEnded == true && _sm.aIPath.reachedEndOfPath == true) //���� ������ �������� � ������ � ����
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
