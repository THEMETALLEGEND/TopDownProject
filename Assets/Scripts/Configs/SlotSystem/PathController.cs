using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Waypoints
{
    public class PathController : MonoBehaviour, IDisposable
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

            SettingInitialSettings();
            StartingMovingRoute();
        }

        public void SetRoute(Route route)
        {
            _currentRoute = route;
        }

        private void StartingMovingRoute()
        {
            _pointID = 0;
            _pointMirrorID = 0;
            
            var firstPoint = _currentRoute.Waypoints[_pointID].Position;
            _sm.AIDest.target.position = firstPoint;
            UpdateReachedEndOfPath();
        }

        private void SettingInitialSettings()
        {
            _sm.StartingPosition = _currentRoute.StartWaypoint.Position;
            _sm.TargetSetter(_sm.PointTarget);
            _sm.AIPath.maxSpeed = _sm.roamingSpeed;
        }

        private void UpdateReachedEndOfPath()
        {
            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(async _ =>
            {
                if (_sm.AIPath.reachedEndOfPath && !isDelay)
                {
                    isDelay = true;
                    InvokePointEvent();
                    await UniTask.Delay(TimeSpan.FromSeconds(_timeDelayPoint));
                    _sm.AIDest.target.position = GetPathPoint();
                    isDelay = false;
                }
                else
                {
                    
                }
            }).AddTo(_disposable);
        }

        private Vector3 GetPathPoint()
        {
            if (_currentRoute.IsLooped && _pointID == _currentRoute.Waypoints.Count)
            {
                _pointID = 0;
            }

            if (_currentRoute.IsLoopedMirrored && _pointID == _currentRoute.Waypoints.Count)
            {
                if (_pointMirrorID == 0)
                {
                    _pointMirrorID = _currentRoute.Waypoints.Count - 1;
                    _pointID = 0;

                    _timeDelayPoint = _currentRoute.Waypoints[_pointID].TimeDelay;
                    return _currentRoute.Waypoints[_pointID++].Position;
                }

                _timeDelayPoint = _currentRoute.Waypoints[_pointMirrorID].TimeDelay;
                return _currentRoute.Waypoints[_pointMirrorID--].Position;
            }

            if (_pointID == _currentRoute.Waypoints.Count)
            {
                _timeDelayPoint = _currentRoute.Waypoints[^1].TimeDelay;
                return _currentRoute.Waypoints[^1].Position;
            }

            _timeDelayPoint = _currentRoute.Waypoints[_pointID].TimeDelay;
            return _currentRoute.Waypoints[_pointID++].Position;
        }

        private void InvokePointEvent()
        {
            if (_pointID == _currentRoute.Waypoints.Count)
            {
                _currentRoute.Waypoints[^1].Event.Invoke();
            }
            else
            {
                _currentRoute.Waypoints[_pointID].Event.Invoke();
            }
            
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}