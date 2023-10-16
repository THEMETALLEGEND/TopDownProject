using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitting : BaseState
{

    private TestEnemyStates _sm;
    private Coroutine _strikeCoroutine; //����� ��������� ���������� �������� ShootBurst 
    public EnemyHitting(TestEnemyStates enemyStateMachine) : base("TestEnemyHitting", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }


    public override void Enter()
    {
        base.Enter();

        //_sm.aIPath.canMove = false;
        Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized;
        _sm.pointTarget.transform.position = _sm.enemyObject.transform.position + (new Vector3(direction.x, direction.y, 0f) * 2f); //������ ������ �� ��� ���� ������ �� ������� ������ � ������� ������ ����� ��
                                                                                                                                    //����������� � ������� desiredVelocity � ��� ��� ��� �������� � ������ �����
        _sm.TargetSetter(_sm.pointTarget);

        _strikeCoroutine = _sm.StartCoroutine(StrikeEnum()); //��������� �������� � �������� ��
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_sm.CheckPlayerInRange(_sm.shootingPlayerDistanceExit)) //���� ������ ���������� ��������
            stateMachine.ChangeState(_sm.chasingState);
    }

    IEnumerator StrikeEnum()
    {
        while (true)
        {
            // ���������, ������������ �� ����� � ����� ������
            if (!_sm.CheckPlayerContact(48, 1, 30))
            {
                stateMachine.ChangeState(_sm.chasingState);
                yield break;
            }

            // ���� ����� ������������ � �����, �� ���������� ��������
            Strike();
            yield return new WaitForSeconds(_sm.strikeTiming);
        }
    }

    private void Strike()
    {
        _sm.aIPath.maxSpeed = _sm.dodgeSpeed; //�� ����� ����� ������ ����������� ��������
        Vector2 direction = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized; //����������� ��������������� ������ �� ������ �� ������

        Vector3 dodgePoint = direction * 20; //������ ����� �� �������
        _sm.pointTarget.transform.position = _sm.enemyObject.transform.position + dodgePoint; //������ ������ �� �� ����� �� �������
    }

    public override void Exit()
    {
        base.Exit();

        if (_strikeCoroutine != null) //���� �������� ������ �������������
        {
            _sm.StopCoroutine(_strikeCoroutine); //�� ������������� �
            _strikeCoroutine = null;  //� ��������� ���������� null (�� ������� �������� �� ���������������)
        }
    }
}