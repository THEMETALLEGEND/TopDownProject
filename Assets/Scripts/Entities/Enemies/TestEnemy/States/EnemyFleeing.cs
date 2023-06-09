using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeing : BaseState
{
    private TestEnemyStates _sm;
    private Vector3 targetpoint;
    private float startTime;
    private float waitTime = 1f; // задержка в 1 секунду
    private bool waiting = false;

    private GameObject player; 
    private Coroutine fleeTimerCoroutine;

    public EnemyFleeing(TestEnemyStates enemyStateMachine) : base("TestEnemyFleeing", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player");
        _sm.aIPath.maxSpeed = _sm.fleeingSpeed;
        _sm.TargetSetter(_sm.pointTarget);

        // запускаем таймер на отступление
        fleeTimerCoroutine = null;
        fleeTimerCoroutine = _sm.StartCoroutine(FleeToRoamTimer());
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!_sm.isAlerted)
            fleeTimerCoroutine = _sm.StartCoroutine(FleeToRoamTimer());

        if (_sm.playerObject != null)
        {
            Vector3 dir = (player.transform.position - _sm.enemyObject.transform.position).normalized;
            Vector3 opDir = dir * -1;
            targetpoint = _sm.enemyObject.transform.position + opDir * 5f;

            _sm.pointTarget.transform.position = targetpoint;

            if (_sm.CheckPlayerInRange(_sm.fleeingPlayerDistanceExit))
            {
                // начинаем таймер заново, если игрок слишком близко
                if (fleeTimerCoroutine != null)
                {
                    _sm.StopCoroutine(fleeTimerCoroutine);
                }
                fleeTimerCoroutine = _sm.StartCoroutine(FleeToRoamTimer());
            }
        }
        else
        {
            _sm.ChangeState(_sm.roamingState);
        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (_sm.aIPath.velocity.magnitude <= .2f && !_sm.CheckPlayerInRange(_sm.fleeingPlayerDistanceExit))
        {
            fleeTimerCoroutine = _sm.StartCoroutine(FleeToRoamTimer());
        }

        if (_sm.aIDest.target != null && _sm.aIPath.velocity.magnitude <= .2f && _sm.aIPath.reachedEndOfPath)
        {
            if (!waiting)
            {
                startTime = Time.time;
                waiting = true;
            }
            else if (Time.time - startTime >= waitTime)
            {
                _sm.ChangeState(_sm.afraidState);
            }
        }
        else
        {
            waiting = false;
        }


        if (_sm.aIPath.velocity.magnitude <= 1f)
        {
            _sm.StartCoroutine(FleeToAfraidTimer());
        }

        if (_sm.showDebugLogs)
            Debug.Log(_sm.aIPath.velocity.magnitude);

    }

    public override void Exit()
    {
        base.Exit();

        _sm.StopCoroutine(fleeTimerCoroutine);
        fleeTimerCoroutine = null;

        _sm.isAlerted = false;
    }

    // корутина для таймера на отступление
    private IEnumerator FleeToRoamTimer()
    {
        yield return new WaitForSeconds(3f);

        _sm.ChangeState(_sm.roamingState);
    }

    private IEnumerator FleeToAfraidTimer()
    {
        yield return new WaitForSeconds(1f);
        _sm.ChangeState(_sm.afraidState);
    }
}

