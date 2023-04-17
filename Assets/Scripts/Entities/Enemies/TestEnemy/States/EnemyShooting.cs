using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : BaseState
{

    private TestEnemyStates _sm;
    public EnemyShooting(TestEnemyStates enemyStateMachine) : base("TestEnemyShooting", enemyStateMachine)
    {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        _sm.TargetSetter(_sm.playerObject);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        
    }
}
