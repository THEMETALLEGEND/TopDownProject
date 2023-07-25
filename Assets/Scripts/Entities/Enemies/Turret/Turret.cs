using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Transform firepoint;

    void Awake()
    {
        firepoint = transform.GetChild(1);
        StartCoroutine(BurstFire());
    }

    IEnumerator BurstFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        // Создаем пулю
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        // Задаем скорость пули вправо
        Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>(); //заходим в рб пулии
        bulletRigidBody.AddForce(firepoint.right * 20, ForceMode2D.Impulse); //задаем силу пули в сторону красного вектора с силой 20f, тип силы - импульс
    }
}