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

    public bool isAutomatic;
    public bool hasMagazine;
    private bool allowedShooting = true;
    public float reloadSpeed;
    [HideInInspector] public bool needReload; // Нужно ли перезарядить оружие
    [HideInInspector] public bool refilling; // Происходит ли перезарядка

    // Переменные связанные с выстрелами
    public GameObject bulletPrefab;
    public float bulletForce; // Сила выстрела
    public float fireRate = .1f;
    private float nextFire = 0f;

    // Переменные связанные с игроком
    public PlayerInventory playerInventory;
    private GameObject player;
    public float alertRadius = 20f; // Радиус оповещения

    // Переменные связанные с ГО
    private Animator anim;
    private Transform firepoint; // Точка выстрела

    //Camera
    public Camera cam;







    public static AmmoContainer ammoContainer = new AmmoContainer(); // Статическая переменная для хранения ссылки на экземпляр класса АmmoContainer
    [HideInInspector] public AmmoType m_type;

    Melee melee = new(ammoContainer);
    PistolAmmo pistol = new(ammoContainer);
    RifleAmmo rifle = new(ammoContainer);
    ShellsAmmo shells = new(ammoContainer);
    EnergyAmmo energy = new(ammoContainer);





    private void Awake()
    {
        needReload = false;
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        playerInventory = player.GetComponent<PlayerInventory>();
        firepoint = transform.GetChild(0);
        cam = FindObjectOfType<Camera>();

        ammoInMag = magCapacity;

        switch (ammoTypeEnum)
        {
            case AmmoType.Melee:
                ammoType = melee;
                m_type = AmmoType.Melee;
                break;
            case AmmoType.Pistol:
                ammoType = pistol;
                m_type = AmmoType.Pistol;
                break;
            case AmmoType.Rifle:
                ammoType = rifle;
                m_type = AmmoType.Rifle;
                break;
            case AmmoType.Energy:
                ammoType = energy;
                m_type = AmmoType.Energy;
                break;
            case AmmoType.Shells:
                ammoType = shells;
                m_type = AmmoType.Shells;
                break;
            default:
                ammoType = pistol;
                m_type = AmmoType.Pistol;
                break;
        }
    }


    private void Update()
    {
        ammoType.Refresh(); //Обновление пула патронов каждый кадр.
    }

    public void MagazineReload()
    {
        if (ammoInMag <= 0)
            needReload = true;

        if (ammoType.currentAmmoOfType > 0 && ammoInMag < magCapacity)
        {
            if (Input.GetKeyDown(KeyCode.R))
                StartCoroutine(Reload());
        }
        if (isAutomatic)
        {
            if (Input.GetButton("Fire1") && !needReload && Time.time > nextFire && allowedShooting)
            {
                nextFire = Time.time + fireRate;
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && !needReload && Time.time > nextFire && allowedShooting)
            {
                nextFire = Time.time + fireRate;
                Shoot();
            }
        }
    }

    public void ShotgunReload()
    {
        if (ammoInMag <= 0)
            needReload = true;

        if (ammoType.currentAmmoOfType > 0 && ammoInMag < magCapacity)
        {
            if (Input.GetKeyDown(KeyCode.R))
                StartCoroutine(Reload());
        }
        if (isAutomatic)
        {
            if (Input.GetButton("Fire1") && !needReload && Time.time > nextFire && allowedShooting)
            {
                nextFire = Time.time + fireRate;
                ShotgunShoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && !needReload && Time.time > nextFire && allowedShooting)
            {
                nextFire = Time.time + fireRate;
                ShotgunShoot();
            }
        }
    }

    public IEnumerator Reload()
    {
        allowedShooting = false;
        ammoType.Refresh();
        //anim.SetBool("IsReloading", true);
        yield return new WaitForSeconds(reloadSpeed);
        allowedShooting = true;
        needReload = false;

        int ammoToAdd = Mathf.Min(magCapacity - ammoInMag, ammoContainer.ammoTypeValues[m_type]);

        if (ammoToAdd > 0)
        {
            ammoInMag += ammoToAdd;
            ammoContainer.ammoTypeValues[m_type] -= ammoToAdd;
            ammoType.Refresh();
        }

        Debug.Log("Reload" + ammoContainer.ammoTypeValues[m_type]);

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

    public virtual void ShotgunShoot()
    {
        SetAlerted(true);

        // Количество пуль, которые будут выпущены из дробовика
        int numBullets = 8;

        for (int i = 0; i < numBullets; i++)
        {
            // Создаем пулю
            GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);

            // Получаем Rigidbody2D пули
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();

            // Генерируем случайное смещение для разброса
            float scatterAngle = Random.Range(-10f, 10f);

            // Применяем разброс к направлению пули
            Vector3 bulletDirection = Quaternion.Euler(0f, 0f, scatterAngle) * firepoint.right;

            // Задаем силу пули с незначительным изменением скорости
            float modifiedBulletForce = bulletForce + Random.Range(-2f, 2f);
            bulletRigidBody.AddForce(bulletDirection * modifiedBulletForce, ForceMode2D.Impulse);
        }

        ammoInMag--;
    }

    public virtual void AttackMelee()
    {
        SetAlerted(true);

        // Получаем позицию курсора в мировых координатах
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //позиция мыши в мировых координатах

        // Преобразуем позицию атакующего в Vector2
        Vector2 attackerPos = new Vector2(transform.position.x, transform.position.y);

        var direction = mousePos - attackerPos;

        var distance = 7f;

        // Выполняем рейкаст в заданном направлении и на заданную длину
        RaycastHit2D hit = Physics2D.Raycast(attackerPos, direction, distance, LayerMask.GetMask("Enemies"));
        if (hit.collider != null)
        {
            // Если рейкаст столкнулся с объектом на слое "Enemies", то обрабатываем столкновение
            var enemy = hit.collider.GetComponent<TestEnemy>();
            var enemyStates = hit.collider.GetComponent<TestEnemyStates>();
            if (enemy != null)
            {
                if (enemyStates.isAlerted == false)
                    enemyStates.isAlerted = true;
                enemy.TakeDamage(20);
            }
        }

        // Отобразить рейкаст
        Debug.DrawRay(attackerPos, direction.normalized * distance, Color.red);
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

public enum AmmoType
{
    Melee,
    Pistol,
    Rifle,
    Shells,
    Energy
}