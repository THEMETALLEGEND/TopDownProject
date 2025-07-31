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

    private float startTime2;
    private float waitTime2 = 4f; 
    private bool waiting2 = false;
    private GameObject player;
    private float checkOthersAfraid = 3; //радиус проверки есть ли агенты в состонии afraid

    public EnemyFleeing(TestEnemyStates enemyStateMachine) : base("TestEnemyFleeing", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _sm.IsAlerted = true;
        player = GameObject.Find("Player");
        _sm.AIPath.maxSpeed = _sm.fleeingSpeed; //назначаем скорость больше дефолтной на момент отступления
        //_fleeTarget = new GameObject("FleeTarget"); // создаем пустой объект-цель
        _sm.TargetSetter(_sm.PointTarget); // устанавливаем пустой объект-цель в качестве цели агента
    }



    public override void UpdateLogic()
    {
        base.UpdateLogic();


        if (!_sm.IsAlerted)
            _sm.ChangeState(_sm.RoamingState);


        if (_sm.PlayerObject != null)
        {
            Vector3 dir = (player.transform.position - _sm.EnemyObject.transform.position).normalized; // вычисляем вектор направления от агента до цели
            Vector3 opDir = dir * -1; //обращаем этот вектор
            targetpoint = _sm.EnemyObject.transform.position + opDir * 3f; //назначаем целевую точку от агента в противоположную от игрока сторону на 3

            _sm.PointTarget.transform.position = targetpoint; // устанавливаем позицию пустого объекта-цели
        }
        else
        {
            _sm.ChangeState(_sm.RoamingState);
        }

        //если дальше указанного значения
        if (!_sm.CheckPlayerInRange(_sm.fleeingPlayerDistanceExit))
        {
            if (!waiting2)
            {
                startTime2 = Time.time;
                waiting2 = true;
            }
            else if (Time.time - startTime2 >= waitTime2)
            {
                stateMachine.ChangeState(_sm.RoamingState);
            }
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_sm.transform.position, checkOthersAfraid);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                TestEnemyStates enemyStates = collider.GetComponent<TestEnemyStates>();

                if (enemyStates != null && enemyStates.IsAfraid)
                {
                    _sm.ChangeState(_sm.AfraidState);
                }
            }
        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (_sm.AIPath.velocity.magnitude <= .2f && !_sm.CheckPlayerInRange(_sm.fleeingPlayerDistanceExit))
        {
            _sm.ChangeState(_sm.RoamingState);
        }

            if (_sm.AIDest.target != null && _sm.AIPath.velocity.magnitude <= .5f && _sm.AIPath.reachedEndOfPath) //если цель не нулл и двигаемся и достигли конца пути (AIPath.velocity.magnitude работает только в updatephysics)
        {
            if (!waiting) //таймер который отсчитывает секунду прежде чем уходить в состояние страха
            {
                startTime = Time.time; // сохраняем текущее время
                waiting = true;
            }
            else if (Time.time - startTime >= waitTime)
            {
                _sm.ChangeState(_sm.AfraidState);
            }
        }
        else
        {
            waiting = false; // сбрасываем таймер, если условие больше не выполняется
        }

    }
    public override void Exit()
    {
        base.Exit();

        _sm.IsAlerted = false;
        waiting2 = false;
    }
}