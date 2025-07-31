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

        //_sm.IsAlerted = false;
        _sm.StartingPosition = _sm.transform.position;
        _sm.TargetSetter(_sm.PointTarget);
        _sm.AIDest.target.position = GetRoamingPosition();
        _sm.AIPath.maxSpeed = _sm.roamingSpeed;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        if (_sm.CheckPlayerContact(48, 1, 30))
        {
            if (!_sm.isAnNPC && !_sm.debugMode)
                stateMachine.ChangeState(_sm.ChasingState);
        }

        if (_sm.IsAlerted && !_sm.isAnNPC && _sm.PlayerObject != null)
            _sm.ChangeState(_sm.ChasingState);
        else if (_sm.IsAlerted && _sm.isAnNPC)// && _sm.CheckPlayerContact(48, 1, 30))
            _sm.ChangeState(_sm.FleeingState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        if (timerEnded == false && _sm.AIPath.reachedEndOfPath)
        {
            _sm.roamingInterval -= 1;
        }

        if (_sm.roamingInterval <= 0f)
        {
            timerEnded = true;
        }

        if (timerEnded && _sm.AIPath.reachedEndOfPath)
        {
            _sm.AIDest.target.position = GetRoamingPosition();
            _sm.roamingInterval = 60f;
            timerEnded = false;
        }
    }

    private Vector3 GetRoamingPosition()
    {
        return _sm.StartingPosition + Random.insideUnitSphere * _sm.roamRadius;
    }

}
