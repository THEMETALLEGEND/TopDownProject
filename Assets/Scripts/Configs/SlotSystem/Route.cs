using System.Collections.Generic;
using UnityEngine;

namespace Waypoints
{
    public class Route : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> _waypoints = new List<Waypoint>();
        [SerializeField] private Waypoint _spawnpoint;
        [SerializeField] private Waypoint _despawnpoint;
        [SerializeField] private bool _isLooped;
        [SerializeField] private bool _isLoopedMirrored;
        [SerializeField] private bool _randomiseWaypoints;

        public List<Waypoint> Waypoints => _waypoints;
        public Waypoint SpawnPoint => _spawnpoint;
        public Waypoint DespawnPoint => _spawnpoint;
        public bool IsLooped => _isLooped;
        public bool IsLoopedMirrored => _isLoopedMirrored;
        public bool RandomiseWaypoints => _randomiseWaypoints;
    }
}