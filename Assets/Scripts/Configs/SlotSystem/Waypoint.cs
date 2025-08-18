using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Waypoints
{
    [System.Serializable]
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private bool _isBusy = false;
        [SerializeField] private bool _multipleUses = false;
        [SerializeField] private uint _timeDelay = 0;
        [SerializeField] private UnityEvent _events;

        public Vector3 Position => gameObject.transform.position;
        public bool IsBusy
        {
            get => _isBusy;
            set => _isBusy = value;
        }

        public bool MultipleUses => _multipleUses;
        public uint TimeDelay => _timeDelay;
    }
}