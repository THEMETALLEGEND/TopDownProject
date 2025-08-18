using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Waypoints
{
    public class Route : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> _waypoints = new List<Waypoint>();
        [SerializeField] private bool _isLooped;
        [SerializeField] private bool _isLoopedMirrored;
        [SerializeField] private bool _randomiseWaypoints;

        public List<Waypoint> Waypoints => _waypoints;
        public Waypoint StartWaypoint => _waypoints.FirstOrDefault();
        public bool IsLooped => _isLooped;
        public bool IsLoopedMirrored => _isLoopedMirrored;
        public bool RandomiseWaypoints => _randomiseWaypoints;
    }
}