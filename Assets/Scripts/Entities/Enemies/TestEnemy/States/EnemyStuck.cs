using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyStuck : BaseState
{

    private TestEnemyStates _sm;
    private Transform transform;
    public EnemyStuck(TestEnemyStates enemyStateMachine) : base("TestEnemyStuck", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    private Vector3 targetPosition;

    public override void Enter()
    {
        base.Enter();

        // находим ближайшую доступную точку
        targetPosition = AstarPath.active.GetNearest(transform.position, NNConstraint.Default).position;

        // ищем путь до этой точки
        _sm.aIPath.destination = targetPosition;
        _sm.aIPath.SearchPath();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_sm.aIPath.reachedEndOfPath)
        {
            _sm.ReturnToPreviousState();
        }
    }
}