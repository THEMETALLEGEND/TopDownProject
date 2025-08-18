using System;
using UniRx;
using UnityEngine;

namespace Waypoints
{
    public class SlotController : MonoBehaviour, IDisposable
    {
        [SerializeField] private RouteController _routeController;
        [SerializeField] private bool _checkRoutes;

        private Route _currentRoute;
        private TestEnemyStates _enemyStates;
        
        private CompositeDisposable _disposable;

        public void StartRoute(TestEnemyStates enemyStates)
        {
            _enemyStates = enemyStates;
            if (_checkRoutes)
            {
                CheckingRoutes();
            }

            _currentRoute = _routeController.GetCurrentRoute();
            SettingInitialSettings();
            StartingMovingRoute();

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
                        _enemyStates.AIDest.target.position = _currentRoute.DespawnPoint.Position;
                    }
                }
            }).AddTo(_disposable);
        }

        private void SettingInitialSettings()
        {
            _enemyStates.StartingPosition = _currentRoute.SpawnPoint.Position;
            _enemyStates.TargetSetter(_enemyStates.PointTarget);
            _enemyStates.AIPath.maxSpeed = _enemyStates.roamingSpeed;
        }

        private void CheckingRoutes()
        {
            _routeController.CurrentRoute.Subscribe(_ =>
            {
                ChangingRoute();
            }).AddTo(_disposable);
        }

        private void ChangingRoute()
        {
            
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}