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
    private Animator childModelAnimator;
    private AIPath aIPath;
    private AIDestinationSetter aIdest;
    private bool isMoving;
    private bool isFacingRight = true;
    private Transform childModel;
    private GameObject enemy;
    private Transform enemyTransform;



    private void Awake()
    {
        testEnemy = GetComponent<TestEnemy>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        aIPath = GetComponent<AIPath>();
        aIdest = GetComponent<AIDestinationSetter>();
        //enemy = GameObject.Find("Enemy");
        enemyTransform = this.GetComponent<Transform>();
        childModel = this.transform.GetChild(0);
        childModelAnimator = childModel.GetComponent<Animator>();
    }

    void Update()
    {
        if (aIPath.desiredVelocity.x > .1 || aIPath.desiredVelocity.x < -.1 || aIPath.desiredVelocity.y > .1 || aIPath.desiredVelocity.y < -.1) //���� ��������� ���� � �����-������ �����������
            isMoving = true;
        else
            isMoving = false;


        if (aIdest.target != null && isMoving == true) //���� ���� ���� � ���������
        {
            childModelAnimator.SetBool("IsMoving", true); //�������� �������� ����

            if (aIPath.desiredVelocity.x > .5 && isFacingRight == false) //� ���� �������� ������ � �� ������� ������
            {
                new WaitForSeconds(.3f);
                childModel.localScale = Vector3.Scale(new Vector3(childModel.localScale.x, childModel.localScale.y), new Vector3(-1, 1)); //�������� ����� ��� ����� ������ ��������� �� �
                isFacingRight = true; //�������� ����� ����� ��������� ���� ������ ���
                return;
            }
            else if (aIPath.desiredVelocity.x < -.5 && isFacingRight == true)
            {
                new WaitForSeconds(.3f);
                childModel.localScale = Vector3.Scale(new Vector3(childModel.localScale.x, childModel.localScale.y), new Vector3(-1, 1));
                isFacingRight = false;
                return;
            }
        }
        else
            childModelAnimator.SetBool("IsMoving", false); //�������� ���� �� ��������� ���� ��� ���� �/��� �� ���������.
    }
}
