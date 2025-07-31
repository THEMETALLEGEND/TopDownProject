using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "SlotConfig", menuName = "Config/Slot Config", order = 1)]
    public class SlotConfig : ScriptableObject
    {
        public List<Slot> interactionSlots;
        public List<Slot> queueSlots;
        public Vector3 spawnPosition = Vector3.zero;
        public Vector3 despawnPosition = Vector3.zero;
    }
}