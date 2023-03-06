using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoaming : BaseState
{

    private TestEnemyStates _sm;
    public EnemyRoaming(TestEnemyStates enemyStateMachine) : base("TestEnemyRoaming", enemyStateMachine) {
        _sm = (TestEnemyStates)stateMachine;
    }

    public float roamRadius = 10f;

    public override void Enter()
    {
        base.Enter();

        _sm.startingPosition = _sm.transform.position;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (Input.GetKeyDown("f"))
            stateMachine.ChangeState(_sm.waitingState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        if (Input.GetKey("x"))
        {
            _sm.aIDest.target.position = GetRoamingPosition();
        }
    }


    private Vector3 GetRoamingPosition()
    {
        return _sm.startingPosition + Random.insideUnitSphere * roamRadius;
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
