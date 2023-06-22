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

        _sm.isAfraid = true;
        _sm.TargetSetter(_sm.pointTarget);
        _sm.pointTarget.transform.position = _sm.transform.position;
        _sm.spriteRenderer.color = new Color(0.5f, 0.5f, 1f, 1f); //ставим светло-синий цвет медели
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _sm.pointTarget.transform.position = _sm.transform.position;
        if (!_sm.CheckPlayerInRange(_sm.fleeingPlayerDistanceExit))
        {
            _sm.ChangeState(_sm.roamingState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        _sm.isAfraid = false;
        _sm.spriteRenderer.color = new Color(1f, 1f, 1f, 1f); //ставим дефолтный цвет
    }
}