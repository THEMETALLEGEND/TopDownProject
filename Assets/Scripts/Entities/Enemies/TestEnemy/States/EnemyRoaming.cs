using UnityEngine;

public class EnemyRoaming : BaseState
{
    private TestEnemyStates _sm;
    private bool timerEnded = false;
    
    public EnemyRoaming(TestEnemyStates enemyStateMachine) : base("TestEnemyRoaming", enemyStateMachine) {
        _sm = (TestEnemyStates)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        //_sm.isAlerted = false;
        _sm.startingPosition = _sm.transform.position;
        _sm.TargetSetter(_sm.pointTarget);
        _sm.aIDest.target.position = GetRoamingPosition();
        _sm.aIPath.maxSpeed = _sm.roamingSpeed;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        if (_sm.CheckPlayerContact(48, 1, 30))
        {
            if (!_sm.isAnNPC && !_sm.debugMode)
                stateMachine.ChangeState(_sm.chasingState);
        }

        if (_sm.isAlerted && !_sm.isAnNPC && _sm.playerObject != null)
            _sm.ChangeState(_sm.chasingState);
        else if (_sm.isAlerted && _sm.isAnNPC)// && _sm.CheckPlayerContact(48, 1, 30))
            _sm.ChangeState(_sm.fleeingState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        if (timerEnded == false && _sm.aIPath.reachedEndOfPath)
        {
            _sm.roamingInterval -= 1;
        }

        if (_sm.roamingInterval <= 0f)
        {
            timerEnded = true;
        }

        if (timerEnded && _sm.aIPath.reachedEndOfPath)
        {
            _sm.aIDest.target.position = GetRoamingPosition();
            _sm.roamingInterval = 60f;
            timerEnded = false;
        }
    }

    private Vector3 GetRoamingPosition()
    {
        return _sm.startingPosition + Random.insideUnitSphere * _sm.roamRadius;
    }

}
