using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PathConfig", menuName = "Enemy/Path Config", order = 1)]
    public class PathConfig : ScriptableObject
    {
        public List<Point> points;
        public bool IsCycle;
        public bool IsMirrorCycle;
    }

    [Serializable]
    public class Point
    {
        public Vector3 Position;
        public float StandDelay = 0f;
        //Event System []
    }
}