using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TestEnemy : EnemyClass

{
    private AIPath aIPath;

    public Transform target;
    public float meleeDamage = 1f;

    private void Awake()
    {
        aIPath = GetComponent<AIPath>();
    }

    private void Update()
    {
        aIPath.destination = target.position;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>();
            player.TakeDamage(meleeDamage);
            Debug.Log("Collided");
        }
    }
}
