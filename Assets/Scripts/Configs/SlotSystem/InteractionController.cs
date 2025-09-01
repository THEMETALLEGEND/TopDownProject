using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Waypoints
{
    public class InteractionController : MonoBehaviour, IDisposable
    {
        [SerializeField] private List<Route> routes = new();

        private Route _currentRoute;
        private TestEnemyStates _sm;
        
        private int _pointID = 0;
        private int _pointMirrorID = 0;
        private uint _timeDelayPoint = 0;
        
        private bool isDelay = false;

        private CompositeDisposable _disposable = new CompositeDisposable();

        public void StartRoute(TestEnemyStates enemyStates, int indexRoute = 0)
        {
            if (routes.Count == 0)
                return;

            _sm = enemyStates;
            _currentRoute = routes[indexRoute];

            StartingMovingRoute();
        }

        private void StartingMovingRoute()
        {
            SettingInitialSettings();
            
            _pointID = 0;
            _pointMirrorID = 0;

            var firstPoint = _currentRoute.Waypoints[_pointID].Position;
            _sm.AIDest.target.position = firstPoint;

            CheckWaypoint();
            UpdateReachedEndOfPath();
        }
        private void SettingInitialSettings()
        {
            _sm.StartingPosition = _currentRoute.StartWaypoint.Position;
            _sm.TargetSetter(_sm.PointTarget);
            _sm.AIPath.maxSpeed = _sm.roamingSpeed;
        }

        private void CheckWaypoint()
        {
            if (_currentRoute.Waypoints[_pointID].IsBusy)
            {
                _pointID++;
                _sm.AIDest.target.position = _currentRoute.Waypoints[_pointID].Position;
            }
        }
        
        private void UpdateReachedEndOfPath()
        {
            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(async _ =>
            {
                if (_sm.AIPath.reachedEndOfPath && !isDelay)
                {
                    isDelay = true;
                    _currentRoute.Waypoints[_pointID].Event.Invoke();
                    await UniTask.Delay(TimeSpan.FromSeconds(_timeDelayPoint));
                    _sm.AIDest.target.position = GetFreePosition();
                    isDelay = false;
                }
                else
                {
                    
                }
            }).AddTo(_disposable);
        }

        private Vector3 GetFreePosition()
        {
            if (_pointID != 0 && !_currentRoute.Waypoints[_pointID-1].IsBusy)
            {
                return _currentRoute.Waypoints[--_pointID].Position;
            }

            return _currentRoute.Waypoints[_pointID].Position;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}