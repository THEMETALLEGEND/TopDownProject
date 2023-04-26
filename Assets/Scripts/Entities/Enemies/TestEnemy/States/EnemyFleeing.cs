using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeing : BaseState
{
    private TestEnemyStates _sm;
    private GameObject _fleeTarget; // пустой объект-цель
    public EnemyFleeing(TestEnemyStates enemyStateMachine) : base("TestEnemyFleeing", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        // создаем пустой объект-цель
        _fleeTarget = new GameObject("FleeTarget");
        _sm.TargetSetter(_fleeTarget); // устанавливаем пустой объект-цель в качестве цели агента
    }

    public override void Exit()
    {
        base.Exit();

        // удаляем пустой объект-цель при выходе из состояния
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

        // вычисляем вектор направления от агента до цели
        Vector3 dir = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized;
        Vector3 opDir = dir * -1;
        // получаем точку на максимальном расстоянии от цели
        Vector3 targetPoint = _sm.playerObject.transform.position + opDir * 3f;
        // добавляем случайный угол поворота
        float randomAngle = Random.Range(-_sm.maxTurnAngle, _sm.maxTurnAngle);
        //targetPoint = Quaternion.Euler(0f, randomAngle, 0f) * targetPoint;
        _fleeTarget.transform.position = targetPoint; // устанавливаем позицию пустого объекта-цели

        

        if (Input.GetKeyDown("k"))
        {
            _sm.ChangeState(_sm.roamingState);
        }
    }
}