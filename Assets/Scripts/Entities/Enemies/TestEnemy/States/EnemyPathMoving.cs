using System.Collections.Generic;
using Configs;
using UnityEngine;

public class EnemyPathMoving: BaseState
{
    private TestEnemyStates _sm;
    private bool _timerEnded = false;
    private List<PathConfig> _configs;
    private PathConfig _pathConfig;
    private int _pointID = 0;
    private int _pointMirrorID = 0;
    
    public EnemyPathMoving(TestEnemyStates enemyStateMachine, List<PathConfig> pathConfigs) : base("TestEnemyPathMoving", enemyStateMachine) {
        _sm = (TestEnemyStates)stateMachine;
        _configs = pathConfigs;
        _pathConfig = pathConfigs[0];
    }

    public override void Enter()
    {
        base.Enter();

        _pointID = 0;
        _pointMirrorID = 0;
    
        //_sm.IsAlerted = false;
        _sm.StartingPosition = _sm.transform.position;
        _sm.TargetSetter(_sm.PointTarget);
        _sm.roamingInterval = _pathConfig.points[_pointID].StandDelay;
        _sm.AIDest.target.position = GetPathPoint();
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
        if (_timerEnded == false && _sm.AIPath.reachedEndOfPath)
        {
            _sm.roamingInterval -= 1;
        }

        if (_sm.roamingInterval <= 0f)
        {
            _timerEnded = true;
        }

        if (_timerEnded && _sm.AIPath.reachedEndOfPath)
        {
            _sm.AIDest.target.position = GetPathPoint();
            _timerEnded = false;
        }
    }

    private Vector3 GetPathPoint()
    {
        if (_pathConfig.IsCycle && _pointID == _pathConfig.points.Count)
        {
            _pointID = 0;
        }

        if (_pathConfig.IsMirrorCycle && _pointID == _pathConfig.points.Count)
        {
            if (_pointMirrorID == 0)
            {
                _pointMirrorID = _pathConfig.points.Count - 1;
                _pointID = 0;
                
                _sm.roamingInterval = _pathConfig.points[_pointID].StandDelay;
                return _pathConfig.points[_pointID++].Position;
            }
            
            _sm.roamingInterval = _pathConfig.points[_pointMirrorID].StandDelay;
            return _pathConfig.points[_pointMirrorID--].Position;
        }

        if (_pointID == _pathConfig.points.Count)
        {
            _sm.roamingInterval = _pathConfig.points[^1].StandDelay;
            return _pathConfig.points[^1].Position;
        }

        _sm.roamingInterval = _pathConfig.points[_pointID].StandDelay;
        return _pathConfig.points[_pointID++].Position;
    }

}
