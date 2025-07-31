using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBullet : MonoBehaviour
{
    public float bulletDamageAmount = 40f; // Урон пули

    private Rigidbody2D rb; // Компонент Rigidbody2D пули
    public Vector2 bulletVector;
    public bool isRicochetullet;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Destroy(gameObject, 3f); // Если пуля не сталкивается ни с чем в течение 3 секунд, она пропадет
        if(isRicochetullet && rb.velocity.magnitude < .01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hitbox") // Если этот гейм объект сталкивается с гейм объектом с тэгом "Hitbox"
        {
            EnemyClass enemyClass = collision.gameObject.GetComponentInParent<EnemyClass>(); // Создаем переменную типа EnemyClass
            TestEnemyStates testEnemyStates = collision.gameObject.GetComponentInParent<TestEnemyStates>();
            if (testEnemyStates.IsAlerted == false)
                testEnemyStates.IsAlerted = true;
            enemyClass.TakeDamage(bulletDamageAmount); // Вызываем метод TakeDamage у врага

            Rigidbody2D hitboxRigidbody = collision.gameObject.GetComponentInParent<Rigidbody2D>(); // Получаем Rigidbody2D гейм объекта с тегом "Hitbox"
            Vector2 direction = collision.contacts[0].point - (Vector2)transform.position; // Вычисляем направление от пули до Hitbox
            direction = -direction.normalized; // Инвертируем направление и нормализуем его
            hitboxRigidbody.AddForce(direction * 100, ForceMode2D.Impulse); // Применяем отталкивающую силу к Hitbox

            ParticleSystem particleSystem = collision.gameObject.GetComponentInParent<ParticleSystem>(); // Воспроизводим систему частиц
            particleSystem.Play();

            Destroy(gameObject); // Уничтожаем пулю после столкновения (Код уничтожения пули с другими объектами в BulletRicochet
        }
        else if(isRicochetullet && collision.gameObject.tag == "Player") //если рикошеченная пуля трогает игрока
        {
            CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>(); //то достаем скрипт игрока
            player.TakeDamage(bulletDamageAmount); //и нахуяриваем ему дамага
            Destroy(gameObject);
        }
        else //if (isRicochetullet)
        {
            Destroy(gameObject);
        }
    }
}