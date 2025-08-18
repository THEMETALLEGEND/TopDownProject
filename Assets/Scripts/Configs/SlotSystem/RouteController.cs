using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Waypoints
{
    public class RouteController : MonoBehaviour
    {
        [SerializeField] private List<Route> _routes = new List<Route>();

        [SerializeField] private ReactiveProperty<Route> _currentRoute;

        public ReactiveProperty<Route> CurrentRoute => _currentRoute;
        

        public Route GetCurrentRoute()
        {
            return _currentRoute.Value;
        }
    }
}