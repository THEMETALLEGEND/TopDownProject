using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponClass : MonoBehaviour
{
    public int maxAmmo;
    public int magCapacity;
    public bool hasMagazine;
    public float fireRate;
    //public TextMeshProUGUI ammoText;
    public AmmoType ammoType; // Новое свойство
    public float reloadSpeed = 1f;
    public PlayerInventory playerInventory;
    private GameObject player;

    [HideInInspector] public int currentAmmo;
    [HideInInspector] public bool needReload;
    [HideInInspector] public bool refilling;

    private float nextFireTime = 0f;
    private Animator anim;


    //--------PlayerShooting-----------

    public GameObject bulletPrefab;
    public float bulletForce = 20f;

    //----------------------------------------

    private Transform firepoint;
    private GameObject gunGO;
    private Transform testWeapon;
    private WeaponClass reloadScript;
    private float alertRadius = 20f;


    private void Awake()
    {
        needReload = false;
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        playerInventory = player.GetComponent<PlayerInventory>();
        currentAmmo = magCapacity;
        playerInventory.TestWeaponAmmo = maxAmmo;

        gunGO = GameObject.Find("GunController");
        testWeapon = gunGO.transform.GetChild(0);
        reloadScript = testWeapon.GetComponent<WeaponClass>();
        firepoint = gunGO.transform.GetChild(0).GetChild(0); //достаем точку вылета пули через метод get child 
        //transform.Find("GunController/Weapon/Firepoint"); - второй способ
        Debug.Log(reloadScript);
    }

    private void Update()
    {

        if (currentAmmo <= 0)
            needReload = true;

        if (playerInventory.TestWeaponAmmo > 0 && currentAmmo < magCapacity)
        {
            if (Input.GetKeyDown(KeyCode.R))
                StartCoroutine(Reload());
        }

        if (Input.GetButtonDown("Fire1") && !reloadScript.needReload)
        {
            //reloadScript.DecreaseAmmo();
            Shoot();
        }

        //ammoText.text = currentAmmo + "/" + playerInventory.TestWeaponAmmo;
    }

    IEnumerator Reload()
    {
        Debug.Log("Reload");
        //anim.SetBool("IsReloading", true);
        yield return new WaitForSeconds(reloadSpeed);
        needReload = false;

        if ((magCapacity - currentAmmo) <= playerInventory.TestWeaponAmmo)
        {
            playerInventory.TestWeaponAmmo -= magCapacity - currentAmmo;
            currentAmmo += magCapacity - currentAmmo;
        }
        else
        {
            currentAmmo += playerInventory.TestWeaponAmmo;
            playerInventory.TestWeaponAmmo = 0;
        }

        //anim.SetBool("IsReloading", false);


    }
    private void Shoot()
    {
        SetAlerted(true);
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation); //выщываем префаб пули из точки и ротации пустого ГО firepoint
        Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>(); //заходим в рб пулии
        bulletRigidBody.AddForce(firepoint.right * bulletForce, ForceMode2D.Impulse); //задаем силу пули в сторону красного вектора с силой 20f, тип силы - импульс
        currentAmmo--;
    }

    public void SetAlerted(bool value)
    {
        if (value)
        {
            // Оповещаем всех агентов в радиусе оповещения
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, alertRadius);
            foreach (Collider2D collider in colliders)
            {
                TestEnemyStates agent = collider.GetComponent<TestEnemyStates>();
                if (agent != null && agent != this)
                {
                    agent.isAlerted = true;
                }
            }
        }
    }

    public enum AmmoType
    {
        Pistol,
        Rifle,
        Energy,
        // Другие типы боеприпасов
    }
}