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

        _sm.AIDest.enabled = false;
        _sm.AIPath.canMove = false;
        stunnedCoroutine = _sm.StartCoroutine(StunnedForSeconds());
    }

    IEnumerator StunnedForSeconds()
    {
        yield return new WaitForSeconds(1);
        if(_sm.isAnNPC)
            _sm.ChangeState(_sm.FleeingState);
        else
            _sm.ChangeState(_sm.ChasingState);
    }

    public override void Exit()
    {
        base.Exit();
        _sm.AIDest.enabled = true; 
        _sm.AIPath.canMove = true;
        _sm.StopCoroutine(StunnedForSeconds());
        stunnedCoroutine = null;
    }
}