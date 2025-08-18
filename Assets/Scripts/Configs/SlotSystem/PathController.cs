using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Waypoints
{
    public class PathController : MonoBehaviour, IDisposable
    {
        [SerializeField] private List<Route> routes = new();
        
        private Route _currentRoute;
        private TestEnemyStates _enemyStates;
        
        private CompositeDisposable _disposable;

        public void StartRoute(TestEnemyStates enemyStates, int indexRoute = 0)
        {
            if (routes.Count == 0)
                return;
            
            _enemyStates = enemyStates;
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
            var currentIndex = 0;
            var firstPoint = _currentRoute.Waypoints[currentIndex].Position;
            _enemyStates.AIDest.target.position = firstPoint;
            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ =>
            {
                if (_enemyStates.AIPath.reachedEndOfPath)
                {
                    if (_currentRoute.Waypoints.Count > currentIndex + 1)
                    {
                        currentIndex++;
                        var nextPoint = _currentRoute.Waypoints[currentIndex].Position;
                        _enemyStates.AIDest.target.position = nextPoint;
                    }
                    else
                    {
                        // end path
                    }
                }
            }).AddTo(_disposable);
        }

        private void SettingInitialSettings()
        {
            _enemyStates.StartingPosition = _currentRoute.StartWaypoint.Position;
            _enemyStates.TargetSetter(_enemyStates.PointTarget);
            _enemyStates.AIPath.maxSpeed = _enemyStates.roamingSpeed;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}