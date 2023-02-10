using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TestEnemyStates : MonoBehaviour
{
    private TestEnemy testEnemy;
    private AIPath aIPath;
    private AIDestinationSetter aIDest;
    private Vector3 target;
    private Vector3 startingPosition;

    public float roamRadius = 10f;

    void Start()
    {
        startingPosition = transform.position;
        aIPath = GetComponent<AIPath>();
        aIDest = GetComponent<AIDestinationSetter>();
    }

    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Random.insideUnitSphere * roamRadius;  
    }

    private void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            aIDest.target.position = GetRoamingPosition();
        }
    }
}
