using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float bulletForce = 20f;

    //----------------------------------------

    private Transform firepoint;
    private GameObject gunGO;

    private void Awake()
    {
        gunGO = GameObject.Find("GunController");
        firepoint = gunGO.transform.GetChild(0).GetChild(0);

        //transform.Find("GunController/Weapon/Firepoint");
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
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation); //выщываем префаб пули из точки и ротации пустого ГО firepoint
        Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>(); //заходим в рб пулии
        bulletRigidBody.AddForce(firepoint.right * bulletForce, ForceMode2D.Impulse); //задаем силу пули в сторону красного вектора с силой 20f, тип силы - импульс
    }

}
