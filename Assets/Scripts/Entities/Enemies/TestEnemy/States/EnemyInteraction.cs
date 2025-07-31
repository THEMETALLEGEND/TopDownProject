using System;
using System.Linq;
using Configs;
using Cysharp.Threading.Tasks;
using Entities.Enemies.TestEnemy.Enums;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

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
        private SlotConfig _slotConfig;
        private int _randomIndexPoint;

        private Slot _selecterInteractivePoint;
        private Slot _selecterQueuePoint;

        private ReactiveProperty<bool> _onReachedPoint = new ReactiveProperty<bool>();
        
        public EnemyInteraction(TestEnemyStates enemyStateMachine, SlotConfig slotConfig) : base("EnemyInteraction", enemyStateMachine) {
            _sm = (TestEnemyStates)stateMachine;
            _slotConfig = slotConfig;
        }

        public override void Enter()
        {
            base.Enter();
            
            _sm.StartingPosition = _slotConfig.spawnPosition;
            _sm.TargetSetter(_sm.PointTarget);
            _sm.AIDest.target.position = _slotConfig.despawnPosition;
            _sm.AIPath.maxSpeed = _sm.roamingSpeed;
            SelectingInteractivePoint();
            
        }

        private void SelectingInteractivePoint()
        {
            _randomIndexPoint = GetRandomIndex(_slotConfig.interactionSlots.Count);
            _selecterInteractivePoint = _slotConfig.interactionSlots[_randomIndexPoint];
            if (_selecterInteractivePoint.IsBusy)
            {
                SelectingInteractivePoint();
                return;
            }
            else
            {
                _selecterInteractivePoint.IsBusy = true;
                _sm.AIDest.target.position = _selecterInteractivePoint.Position;
                SubscribeOnRide(ERoadTracking.Interaction);
            }
        }

        private void SubscribeOnRide(ERoadTracking road)
        {
            Observable.EveryUpdate()
                .Where(_ => _sm.AIPath.reachedEndOfPath)
                .First()
                .Subscribe(
                    _ =>
                    {
                        switch (road)
                        {
                            case ERoadTracking.Interaction:
                                InteractionWithPoint();
                                break;
                            case ERoadTracking.Queue:
                                SelectingQueuePoint();
                                break;
                            case ERoadTracking.Despawn:
                                SelectingDespawnPoint();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(road), road, null);
                        }
                    },
                    () => Debug.Log("Подписка завершена (один раз)")
                );
        }

        private void SelectingQueuePoint()
        {
            _selecterQueuePoint = GetFreeQueueSlot();
            if (_selecterQueuePoint == null)
            {
                SelectingInteractivePoint();
            }
            else
            {
                _selecterInteractivePoint.IsBusy = false;
                _selecterQueuePoint.IsBusy = true;
                _sm.AIDest.target.position = _selecterQueuePoint.Position;
                _sm.EnemyClass.SetPathState(EPathStates.QueuePoint);
                //_onReachedPoint.Subscribe()
//                SubscribeOnRide(ERoadTracking.Despawn);
            }
        }

        private void SelectingDespawnPoint()
        {
            _selecterInteractivePoint.IsBusy = false;
            _selecterQueuePoint.IsBusy = false;
            _sm.AIDest.target.position = _slotConfig.despawnPosition;
            _sm.EnemyClass.SetPathState(EPathStates.Despawn);
        }

        private async UniTask InteractionWithPoint()
        {
            _sm.EnemyClass.SetPathState(EPathStates.InteractablePoint);
            Debug.Log($"Взаимодействую");
            await UniTask.WaitForSeconds(4);
            Debug.Log($"Иду в очередь");
            SelectingQueuePoint();
        }

        private int GetRandomIndex(int maxSize)
        {
            return Random.Range(0, maxSize);
        }

        private Slot GetFreeQueueSlot()
        {
            return _slotConfig.queueSlots.FirstOrDefault(slot => !slot.IsBusy);
        }
    }
}