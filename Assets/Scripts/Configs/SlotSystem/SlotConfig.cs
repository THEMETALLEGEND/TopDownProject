using System.Collections.Generic;
using UnityEngine;

namespace Waypoints
{
    [CreateAssetMenu(fileName = "SlotConfig", menuName = "Config/Waypoint Config", order = 1)]
    public class SlotConfig : ScriptableObject
    {
        public List<Waypoint> interactionSlots;
        public List<Waypoint> queueSlots;
        public Vector3 spawnPosition = Vector3.zero;
        public Vector3 despawnPosition = Vector3.zero;
    }
}