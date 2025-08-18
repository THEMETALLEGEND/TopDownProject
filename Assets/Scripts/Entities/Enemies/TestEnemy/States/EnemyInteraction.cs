using Waypoints;
using UniRx;

namespace Entities.Enemies.TestEnemy.States
{
    public enum ERoadTracking
    {
        Interaction,
        Queue,
        Despawn
    }
    
    public class EnemyInteraction : BaseState
    {
        private TestEnemyStates _sm;
        private SlotController _slotController;
        private int _randomIndexPoint;

        private Waypoint _selecterInteractivePoint;
        private Waypoint _selecterQueuePoint;

        private ReactiveProperty<bool> _onReachedPoint = new ReactiveProperty<bool>();
        
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