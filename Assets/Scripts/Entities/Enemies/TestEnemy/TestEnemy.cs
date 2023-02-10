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
        if (!(target == null)) //проверка на то что цель существует
        {
            aIPath.destination = target.position; //и если так то строить до неё путь
        }
        else
            return;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player") //если сталкиваемся с игроком
        {
            CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>(); //то достаем его скрипт
            player.TakeDamage(meleeDamage); //и нахуяриваем ему дамага
        }
    }
}
