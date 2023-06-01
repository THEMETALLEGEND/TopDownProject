using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponClass : MonoBehaviour
{
    // Переменные связанные с боеприпасами
    public int magCapacity;
    [HideInInspector] public int ammoInMag; // Текущее количество боеприпасов
    public AmmoType ammoTypeEnum;

    public AmmoClass ammoType;
    
    PistolAmmo pistolAmmo = new();
    RifleAmmo rifleAmmo = new();
    EnergyAmmo energyAmmo = new();


    public bool hasMagazine;
    public float reloadSpeed;
    [HideInInspector] public bool needReload; // Нужно ли перезарядить оружие
    [HideInInspector] public bool refilling; // Происходит ли перезарядка

    // Переменные связанные с выстрелами
    public float fireRate; // Скорострельность
    public GameObject bulletPrefab;
    public float bulletForce; // Сила выстрела

    // Переменные связанные с игроком
    public PlayerInventory playerInventory;
    private GameObject player;
    private float alertRadius = 20f; // Радиус оповещения

    // Переменные связанные с ГО
    private Animator anim;
    private Transform firepoint; // Точка выстрела

    public enum AmmoType
    {
        Pistol,
        Rifle,
        Energy
    }


    private void Awake()
    {
        needReload = false;
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        playerInventory = player.GetComponent<PlayerInventory>();
        firepoint = transform.GetChild(0);

        ammoInMag = magCapacity;

        switch (ammoTypeEnum)
        {
            case AmmoType.Pistol:
                ammoType = pistolAmmo;
                break;
            case AmmoType.Rifle:
                ammoType = rifleAmmo;
                break;
            case AmmoType.Energy:
                ammoType = energyAmmo;
                break;
            default:
                ammoType = pistolAmmo;
                break;
        }

        Debug.Log(ammoTypeEnum);
    }


    private void Update()
    {

        if (ammoInMag <= 0)
            needReload = true;

        if (ammoType.currentAmmoOfType > 0 && ammoInMag < magCapacity)
        {
            if (Input.GetKeyDown(KeyCode.R))
                StartCoroutine(Reload());
        }

        if (Input.GetButtonDown("Fire1") && !needReload)
        {
            Shoot();
        }
    }

    public IEnumerator Reload()
    {
        Debug.Log("Reload" + ammoType.currentAmmoOfType);
        //anim.SetBool("IsReloading", true);
        yield return new WaitForSeconds(reloadSpeed);
        needReload = false;

        if ((magCapacity - ammoInMag) <= ammoType.currentAmmoOfType)
        {
            ammoType.currentAmmoOfType -= magCapacity - ammoInMag;
            ammoInMag += magCapacity - ammoInMag;
        }
        else
        {
            ammoInMag += ammoType.currentAmmoOfType;
            ammoType.currentAmmoOfType = 0;
        }

        //anim.SetBool("IsReloading", false);


    }
    public virtual void Shoot()
    {
        SetAlerted(true);
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation); //выщываем префаб пули из точки и ротации пустого ГО firepoint
        Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>(); //заходим в рб пулии
        bulletRigidBody.AddForce(firepoint.right * bulletForce, ForceMode2D.Impulse); //задаем силу пули в сторону красного вектора с силой 20f, тип силы - импульс
        ammoInMag--;
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
}