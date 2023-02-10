using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoaming : BaseState
{
    public EnemyRoaming(TestEnemyStates enemyStateMachine) : base("TestEnemyRoaming", enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (Input.GetKeyDown("e"))
            stateMachine.ChangeState(((TestEnemyStates)stateMachine).waitingState);
    }













    /*
    private TestEnemy testEnemy;
    private AIPath aIPath;
    private AIDestinationSetter aIDest;
    private Vector3 target;
    private Vector3 startingPosition;

    public float roamRadius = 10f;

    void Start()
    {
        startingPosition = transform.position;
        aIPath = GetComponent<AIPath>();
        aIDest = GetComponent<AIDestinationSetter>();
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Random.insideUnitSphere * roamRadius;
    }

    private void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            aIDest.target.position = GetRoamingPosition();
        }
    }
    */
}
