using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float bulletForce = 20f;

    //----------------------------------------

    private Transform firepoint;

    private void Awake()
    {
        firepoint = transform.Find("GunController/Weapon/Firepoint");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidBody.AddForce(firepoint.right * bulletForce, ForceMode2D.Impulse);
    }

}
