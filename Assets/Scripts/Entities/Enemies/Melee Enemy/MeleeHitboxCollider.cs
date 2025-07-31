using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitboxCollider : MonoBehaviour
{
    private EnemyClass parent;
    private void Awake()
    {
        parent = transform.parent.GetComponent<EnemyClass>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) //если сталкиваемс€ с игроком (нпс дамага не наносит)
        {
            CharacterController2D player = collision.gameObject.GetComponentInParent<CharacterController2D>(); //то достаем его скрипт
            player.TakeDamage(parent.MeleeDamage); //и наху€риваем ему дамага
            player.currentSpeed = player.currentSpeed / 2;
            parent.IsDamaging = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>();
            if (player != null)
                player.currentSpeed = player.walkSpeed;
            parent.IsDamaging = false;
            gameObject.SetActive(false);
        }
    }
}
