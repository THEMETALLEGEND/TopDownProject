using Waypoints;

namespace Entities.Enemies.TestEnemy.States
{
    public class EnemyInteraction : BaseState
    {
        private TestEnemyStates _sm;
        private InteractionController _pathController;
        private int _randomIndexPoint;

        private Waypoint _selecterInteractivePoint;
        private Waypoint _selecterQueuePoint;

        public EnemyInteraction(TestEnemyStates enemyStateMachine, InteractionController interactionController) : base("EnemyInteraction", enemyStateMachine) 
        {
            _sm = (TestEnemyStates)stateMachine;
            _pathController = interactionController;
            _pathController.StartRoute(_sm);
        }

        public override void Enter()
        {
            base.Enter();
        }
    }
}