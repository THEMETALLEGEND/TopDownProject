using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Transform gunGO;
    private Camera cam;
    Vector2 mousePos;
    void Start()
    {
        gunGO = GameObject.Find("GunController").transform;
        transform.rotation = gunGO.rotation;

        cam = FindObjectOfType<Camera>();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //������� ���� � ������� �����������
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            TestEnemyStates _sm = collision.GetComponent<TestEnemyStates>();
            _sm.ChangeState(_sm.stunnedState);
            Rigidbody2D enemyRB = collision.GetComponent<Rigidbody2D>(); // �������� Rigidbody2D ����������
            Vector2 playerToEnemy = collision.transform.position - transform.position; // ������ �� ������ �� ����������
            Vector2 playerToMouse = mousePos - (Vector2)transform.position; // ������ �� ������ �� ������� ����
            Vector2 knockbackDirection = playerToEnemy.normalized + playerToMouse.normalized; // ���������� ������� � �����������
            Debug.Log("Vector 1 " + playerToEnemy + "Vector 2 " + playerToMouse + "Comvined vector " + knockbackDirection);

            enemyRB.AddForce(knockbackDirection.normalized * 15, ForceMode2D.Impulse); // ��������� ���� ������������ � ����������
        }
    }

    private void Update()
    {
    }
}
