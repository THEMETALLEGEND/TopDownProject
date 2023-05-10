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
    private Transform testWeapon;
    private WeaponReload reloadScript;

    private void Awake()
    {
        gunGO = GameObject.Find("GunController");
        testWeapon = gunGO.transform.GetChild(0);
        reloadScript = testWeapon.GetComponent<WeaponReload>();
        firepoint = gunGO.transform.GetChild(0).GetChild(0); //достаем точку вылета пули через метод get child 
        //transform.Find("GunController/Weapon/Firepoint"); - второй способ
        Debug.Log(reloadScript);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !reloadScript.needReload)
        {
            reloadScript.DecreaseAmmo();
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
