using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PathConfig", menuName = "Enemy/Path Config", order = 1)]
    public class PathConfig : ScriptableObject
    {
        public List<Vector3> points;
    }
}