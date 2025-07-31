using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAfraid : BaseState
{
    private TestEnemyStates _sm;
    public EnemyAfraid(TestEnemyStates enemyStateMachine) : base("TestEnemyAfraid", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        _sm.IsAfraid = true;
        _sm.TargetSetter(_sm.PointTarget);
        _sm.PointTarget.transform.position = _sm.transform.position;
        _sm.SpriteRenderer.color = new Color(0.5f, 0.5f, 1f, 1f); //������ ������-����� ���� ������
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _sm.PointTarget.transform.position = _sm.transform.position;
        if (!_sm.CheckPlayerInRange(_sm.fleeingPlayerDistanceExit))
        {
            _sm.ChangeState(_sm.RoamingState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        _sm.IsAfraid = false;
        _sm.SpriteRenderer.color = new Color(1f, 1f, 1f, 1f); //������ ��������� ����
    }
}