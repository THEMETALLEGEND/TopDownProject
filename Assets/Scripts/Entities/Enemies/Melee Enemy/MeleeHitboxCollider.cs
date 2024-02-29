using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitboxCollider : MonoBehaviour
{
    private TestEnemy parent;
    private void Awake()
    {
        parent = transform.parent.GetComponent<TestEnemy>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) //���� ������������ � ������� (��� ������ �� �������)
        {
            CharacterController2D player = collision.gameObject.GetComponentInParent<CharacterController2D>(); //�� ������� ��� ������
            player.TakeDamage(parent.meleeDamage); //� ����������� ��� ������
            player.currentSpeed = player.currentSpeed / 2;
            parent.isDamaging = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>();
            if (player != null)
                player.currentSpeed = player.walkSpeed;
            parent.isDamaging = false;
            gameObject.SetActive(false);
        }
    }
}
