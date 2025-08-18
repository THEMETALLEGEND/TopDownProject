using Waypoints;

namespace Entities.Enemies.TestEnemy.States
{
    public class EnemyInteraction : BaseState
    {
        private TestEnemyStates _sm;
        private PathController _pathController;
        private int _randomIndexPoint;

        private Waypoint _selecterInteractivePoint;
        private Waypoint _selecterQueuePoint;

        public EnemyInteraction(TestEnemyStates enemyStateMachine, PathController pathController) : base("EnemyInteraction", enemyStateMachine) 
        {
            _sm = (TestEnemyStates)stateMachine;
            _pathController = pathController;
            _pathController.StartRoute(_sm);
        }

        public override void Enter()
        {
            base.Enter();
        }
    }
}