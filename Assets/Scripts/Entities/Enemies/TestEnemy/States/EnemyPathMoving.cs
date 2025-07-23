using Configs;
using UnityEngine;

public class EnemyPathMoving: BaseState
{
    private TestEnemyStates _sm;
    private bool _timerEnded = false;
    private PathConfig _pathConfig;
    private int _pointID = 0;
    
    public EnemyPathMoving(TestEnemyStates enemyStateMachine, PathConfig pathConfig) : base("TestEnemyPathMoving", enemyStateMachine) {
        _sm = (TestEnemyStates)stateMachine;
        _pathConfig = pathConfig;
    }

    public override void Enter()
    {
        base.Enter();

        //_sm.isAlerted = false;
        _sm.startingPosition = _sm.transform.position;
        _sm.TargetSetter(_sm.pointTarget);
        _sm.aIDest.target.position = GetPathPoint();
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
        if (_timerEnded == false && _sm.aIPath.reachedEndOfPath)
        {
            _sm.roamingInterval -= 1;
        }

        if (_sm.roamingInterval <= 0f)
        {
            _timerEnded = true;
        }

        if (_timerEnded && _sm.aIPath.reachedEndOfPath)
        {
            _sm.aIDest.target.position = GetPathPoint();
            _sm.roamingInterval = 60f;
            _timerEnded = false;
        }
    }

    private Vector3 GetPathPoint()
    {
        if (_pointID == _pathConfig.points.Count)
        {
            _pointID = 0;
        }
            
        return _pathConfig.points[_pointID++];
    }

}
