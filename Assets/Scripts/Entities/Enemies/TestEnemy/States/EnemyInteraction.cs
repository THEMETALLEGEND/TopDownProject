using Waypoints;

namespace Entities.Enemies.TestEnemy.States
{
    public class EnemyInteraction : BaseState
    {
        private TestEnemyStates _sm;
        private SlotController _slotController;
        private int _randomIndexPoint;

        private Waypoint _selecterInteractivePoint;
        private Waypoint _selecterQueuePoint;

        public EnemyInteraction(TestEnemyStates enemyStateMachine, SlotController slotController) : base("EnemyInteraction", enemyStateMachine) {
            _sm = (TestEnemyStates)stateMachine;
            _slotController = slotController;
            _slotController.StartRoute(_sm);
        }

        public override void Enter()
        {
            base.Enter();
        }
    }
}