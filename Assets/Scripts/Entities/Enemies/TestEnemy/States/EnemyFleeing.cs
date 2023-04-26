using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeing : BaseState
{
    private TestEnemyStates _sm;
    private GameObject _fleeTarget; // ������ ������-����
    public EnemyFleeing(TestEnemyStates enemyStateMachine) : base("TestEnemyFleeing", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        // ������� ������ ������-����
        _fleeTarget = new GameObject("FleeTarget");
        _sm.TargetSetter(_fleeTarget); // ������������� ������ ������-���� � �������� ���� ������
    }

    public override void Exit()
    {
        base.Exit();

        // ������� ������ ������-���� ��� ������ �� ���������
        if (_fleeTarget != null)
        {
            GameObject.Destroy(_fleeTarget);
            _fleeTarget = null;
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        Debug.Log(_sm.target);

        // ��������� ������ ����������� �� ������ �� ����
        Vector3 dir = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized;
        Vector3 opDir = dir * -1;
        // �������� ����� �� ������������ ���������� �� ����
        Vector3 targetPoint = _sm.playerObject.transform.position + opDir * 3f;
        // ��������� ��������� ���� ��������
        float randomAngle = Random.Range(-_sm.maxTurnAngle, _sm.maxTurnAngle);
        //targetPoint = Quaternion.Euler(0f, randomAngle, 0f) * targetPoint;
        _fleeTarget.transform.position = targetPoint; // ������������� ������� ������� �������-����

        

        if (Input.GetKeyDown("k"))
        {
            _sm.ChangeState(_sm.roamingState);
        }
    }
}