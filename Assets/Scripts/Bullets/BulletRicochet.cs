using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRicochet : MonoBehaviour
{
    public GameObject bulletPrefab; // Префаб пули
    public float angleDeviancy = 20f; // Погрешность в градусах
    public int bulletForce = 50; // Сила пули (значение от 0 до 100)
    private Vector2 bullet1Velocity;

    private Rigidbody2D bullet1Rigidbody;
    private Vector2 initialPosition;

    private EnemyBullet bullet2Script;

    void Start()
    {
        bullet1Rigidbody = GetComponent<Rigidbody2D>();
        initialPosition = bullet1Rigidbody.position;
        StartCoroutine(CalculateBulletVelocity());
    }

    private IEnumerator CalculateBulletVelocity()
    {
        yield return new WaitForFixedUpdate();

        Vector2 currentPosition = bullet1Rigidbody.position;
        bullet1Velocity = currentPosition - initialPosition;
        Debug.Log("Вектор движения пули: " + bullet1Velocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.layer == LayerMask.NameToLayer("Multiprojectile"))
        {
            bullet2Script = collision.gameObject.GetComponent<EnemyBullet>();

            // Вектор второй пули, который высчитывается через 0.01 секунду после выстрела
            Vector2 bullet2Velocity = bullet2Script.bulletVector;

            // Добавляем погрешность в направление нового вектора
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(-angleDeviancy, angleDeviancy));

            // Вычисляем направление рикошета с учетом bulletForce
            Vector2 ricochetDirection;
            if (bulletForce >= 100)
            {
                ricochetDirection = bullet1Velocity.normalized;
            }
            else if (bulletForce < 100 && bulletForce >= 0)
            {
                Vector2 midpoint = (bullet1Velocity.normalized + bullet2Velocity.normalized).normalized;
                ricochetDirection = Vector2.Lerp(midpoint, bullet1Velocity.normalized, bulletForce / 100f).normalized;
            }
            else if (bulletForce > -100)
            {
                Vector2 midpoint = (bullet1Velocity.normalized + bullet2Velocity.normalized).normalized;
                ricochetDirection = Vector2.Lerp(midpoint, -bullet1Velocity.normalized, -bulletForce / 100f).normalized;
            }
            else
            {
                ricochetDirection = -bullet1Velocity.normalized;
            }

            // Проверяем, образует ли направление рикошета прямой угол с bullet1Velocity
            if (Vector2.Angle(ricochetDirection, bullet1Velocity.normalized) <= 95f && Vector2.Angle(ricochetDirection, bullet1Velocity.normalized) >= 85f)
            {
                // Вектор отражается, если образуется прямой угол
                float randomValue = Random.Range(0f, 1f);
                if (randomValue <= 50 / 100f)
                {
                    ricochetDirection *= -1;
                }
            }

            // Вычисляем вектор рикошета
            Vector2 ricochetVector = ricochetDirection * bullet1Velocity.magnitude;

            // Запускаем корутину для задержки спавна пули
            SpawnRicochetBullet(ricochetVector, randomRotation);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SpawnRicochetBullet(Vector2 velocity, Quaternion rotation)
    {
        //yield return new WaitForSeconds(0f);

        // Создаем новую пулю
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, rotation);

        // Получаем Rigidbody2D новой пули
        Rigidbody2D bulletRigidbody = newBullet.GetComponent<Rigidbody2D>();

        // Задаем скорость новой пули с помощью AddForce
        bulletRigidbody.AddForce(velocity * 100f, ForceMode2D.Impulse);

        // Поворачиваем пулю в направлении ее полета
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        newBullet.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        // Уничтожаем текущую пулю
        Destroy(gameObject);
    }
}