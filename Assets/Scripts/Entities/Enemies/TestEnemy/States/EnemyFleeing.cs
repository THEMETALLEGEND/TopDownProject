using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeing : BaseState
{
    private TestEnemyStates _sm;
    private GameObject _fleeTarget; // пустой объект-цель
    private Vector3 targetpoint;
    private float startTime;
    private float waitTime = 1f; // задержка в 1 секунду
    private bool waiting = false;
    public EnemyFleeing(TestEnemyStates enemyStateMachine) : base("TestEnemyFleeing", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        _sm.aIPath.maxSpeed = _sm.fleeingSpeed;
        // создаем пустой объект-цель
        _fleeTarget = new GameObject("FleeTarget");
        _sm.TargetSetter(_fleeTarget); // устанавливаем пустой объект-цель в качестве цели агента
    }



    public override void UpdateLogic()
    {
        base.UpdateLogic();

        // вычисляем вектор направления от агента до цели
        Vector3 dir = (_sm.playerObject.transform.position - _sm.enemyObject.transform.position).normalized;
        Vector3 opDir = dir * -1;

        if (_sm.CheckPlayerInRange(40f))
        {
            // получаем точку на максимальном расстоянии от цели
            targetpoint = _sm.enemyObject.transform.position + opDir * 3f;
        }

        _fleeTarget.transform.position = targetpoint; // устанавливаем позицию пустого объекта-цели

        if (!_sm.CheckPlayerInRange(_sm.fleeingPlayerDistanceExit) && _sm.aIPath.reachedEndOfPath == true) //если дальше указанного значения
                stateMachine.ChangeState(_sm.roamingState);

        

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (_sm.aIDest.target != null && _sm.aIPath.velocity.magnitude <= .2f && _sm.aIPath.reachedEndOfPath)
        {
            if (!waiting)
            {
                startTime = Time.time; // сохраняем текущее время
                waiting = true;
            }
            else if (Time.time - startTime >= waitTime)
            {
                _sm.ChangeState(_sm.afraidState);
            }
        }
        else
        {
            waiting = false; // сбрасываем таймер, если условие больше не выполняется
        }
        Debug.Log(_sm.aIDest.target);
        Debug.Log(_sm.aIPath.velocity.magnitude);
        Debug.Log(_sm.aIPath.reachedEndOfPath);
    }
    public override void Exit()
    {
        base.Exit();

        _sm.aIPath.maxSpeed = _sm.defaultSpeed;

        if (_fleeTarget != null)    // удаляем пустой объект-цель при выходе из состояния
        {
            GameObject.Destroy(_fleeTarget);
            _fleeTarget = null;
        }
    }
}