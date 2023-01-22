using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TestEnemy : EnemyClass

{
    private AIPath aIPath;

    public Transform target;

    private void Awake()
    {
        aIPath = GetComponent<AIPath>();
    }

    private void Update()
    {
        aIPath.destination = target.position;
    }
}
