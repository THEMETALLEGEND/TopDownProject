using Waypoints;

public class EnemyPathMoving: BaseState
{
    private PathController _pathController;
    private TestEnemyStates _sm;
    private bool _timerEnded = false;
    private int _pointID = 0;
    
    public EnemyPathMoving(TestEnemyStates enemyStateMachine, PathController pathController) : base("TestEnemyPathMoving", enemyStateMachine) 
    {
        _sm = (TestEnemyStates)stateMachine;
        _pathController = pathController;
    }

    public override void Enter()
    {
        base.Enter();
        _pathController.StartRoute(_sm);
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
            _sm.roamingInterval = 60f;
            _timerEnded = false;
        }
    }
}
