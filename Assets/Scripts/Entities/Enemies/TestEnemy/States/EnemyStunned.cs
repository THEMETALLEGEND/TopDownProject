using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyStunned : BaseState
{

    private TestEnemyStates _sm;
    private Coroutine stunnedCoroutine;
    public EnemyStunned(TestEnemyStates enemyStateMachine) : base("TestEnemyStunned", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        _sm.aIDest.enabled = false;
        _sm.aIPath.canMove = false;
        stunnedCoroutine = _sm.StartCoroutine(StunnedForSeconds());
    }

    IEnumerator StunnedForSeconds()
    {
        yield return new WaitForSeconds(1);
        if(_sm.isAnNPC)
            _sm.ChangeState(_sm.fleeingState);
        else
            _sm.ChangeState(_sm.chasingState);
    }

    public override void Exit()
    {
        base.Exit();
        _sm.aIDest.enabled = true; 
        _sm.aIPath.canMove = true;
        _sm.StopCoroutine(StunnedForSeconds());
        stunnedCoroutine = null;
    }
}