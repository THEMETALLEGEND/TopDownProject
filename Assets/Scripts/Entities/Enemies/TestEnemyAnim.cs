using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TestEnemyAnim : MonoBehaviour
{
    private TestEnemy testEnemy;
    private Rigidbody2D rb;
    private float animVelocity;
    private Animator animator;
    private AIPath aIPath;
    private AIDestinationSetter aIdest;
    private bool isMoving;
    private bool isFacingRight = true;
    private GameObject enemy;
    private Transform enemyTransform;



    private void Awake()
    {
        testEnemy = GetComponent<TestEnemy>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        aIPath = GetComponent<AIPath>();
        aIdest = GetComponent<AIDestinationSetter>();
        enemy = GameObject.Find("Enemy");
        enemyTransform = enemy.GetComponent<Transform>();
    }

    void Update()
    {
        if (aIPath.desiredVelocity.x > .5 || aIPath.desiredVelocity.x < -.5 || aIPath.desiredVelocity.y > .5 || aIPath.desiredVelocity.y < -.5) //если двигаемся хоть в каком-нибудь направлении
            isMoving = true;
        else
            isMoving = false;


        if (aIdest.target != null && isMoving == true) //если есть цель и двигаемся
        {
            animator.SetBool("IsMoving", true); //включаем анимацию бега

            if (aIPath.desiredVelocity.x > .5 && isFacingRight == false) //и если движемся вправо и не смотрим вправо
            {
                enemyTransform.localScale = Vector3.Scale(new Vector3(enemyTransform.localScale.x, enemyTransform.localScale.y), new Vector3(-1, 1)); //умножаем скейл так чтобы объект отразился по х
                isFacingRight = true; //проверка булом чтобы отражение было только раз
                return;
            }
            else if (aIPath.desiredVelocity.x < -.5 && isFacingRight == true)
            {
                enemyTransform.localScale = Vector3.Scale(new Vector3(enemyTransform.localScale.x, enemyTransform.localScale.y), new Vector3(-1, 1));
                isFacingRight = false;
                return;
            }
        }
        else
            animator.SetBool("IsMoving", false); //анимация бега не включится если нет цели и/или не двигаемся.
    }
}
