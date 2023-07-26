using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour

{
    public float bulletDamageAmount = 15f; //урон пули
    private Rigidbody2D rb;
    public Vector2 bulletVector;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(BulletVectorCheck());
    }

    IEnumerator BulletVectorCheck()
    {
        yield return new WaitForSeconds(.01f);
        bulletVector = rb.velocity;
    }

    private void Update()
    {
        Destroy(gameObject, 3f); //если пуля не сталкивается ни с чем 3 секунды то она пропадет
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") //если сталкиваемся с игроком
        {
            CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>(); //то достаем его скрипт
            player.TakeDamage(bulletDamageAmount); //и нахуяриваем ему дамага
        }
        Destroy(gameObject); //при любом столкновении дестрой себя
    }
}
