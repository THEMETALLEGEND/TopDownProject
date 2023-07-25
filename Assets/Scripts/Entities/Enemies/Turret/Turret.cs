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
        // ������� ����
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        // ������ �������� ���� ������
        Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>(); //������� � �� �����
        bulletRigidBody.AddForce(firepoint.right * 20, ForceMode2D.Impulse); //������ ���� ���� � ������� �������� ������� � ����� 20f, ��� ���� - �������
    }
}