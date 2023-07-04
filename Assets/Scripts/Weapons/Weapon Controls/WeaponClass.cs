using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponClass : MonoBehaviour
{
    // ���������� ��������� � ������������
    public int magCapacity;
    [HideInInspector] public int ammoInMag; // ������� ���������� �����������
    public AmmoType ammoTypeEnum;

    public AmmoClass ammoType;

    public bool isAutomatic;
    public bool hasMagazine;
    private bool allowedShooting = true;
    public float reloadSpeed;
    [HideInInspector] public bool needReload; // ����� �� ������������ ������
    [HideInInspector] public bool refilling; // ���������� �� �����������

    // ���������� ��������� � ����������
    public GameObject bulletPrefab;
    public float bulletForce; // ���� ��������
    public float fireRate = .1f;
    private float nextFire = 0f;

    // ���������� ��������� � �������
    public PlayerInventory playerInventory;
    private GameObject player;
    public float alertRadius = 20f; // ������ ����������

    // ���������� ��������� � ��
    private Animator anim;
    private Transform firepoint; // ����� ��������

    //Camera
    public Camera cam;







    public static AmmoContainer ammoContainer = new AmmoContainer(); // ����������� ���������� ��� �������� ������ �� ��������� ������ �mmoContainer
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
        ammoType.Refresh(); //���������� ���� �������� ������ ����.
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
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation); //�������� ������ ���� �� ����� � ������� ������� �� firepoint
        Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>(); //������� � �� �����
        bulletRigidBody.AddForce(firepoint.right * bulletForce, ForceMode2D.Impulse); //������ ���� ���� � ������� �������� ������� � ����� 20f, ��� ���� - �������
        ammoInMag--;
    }

    public virtual void ShotgunShoot()
    {
        SetAlerted(true);

        // ���������� ����, ������� ����� �������� �� ���������
        int numBullets = 8;

        for (int i = 0; i < numBullets; i++)
        {
            // ������� ����
            GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);

            // �������� Rigidbody2D ����
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();

            // ���������� ��������� �������� ��� ��������
            float scatterAngle = Random.Range(-10f, 10f);

            // ��������� ������� � ����������� ����
            Vector3 bulletDirection = Quaternion.Euler(0f, 0f, scatterAngle) * firepoint.right;

            // ������ ���� ���� � �������������� ���������� ��������
            float modifiedBulletForce = bulletForce + Random.Range(-2f, 2f);
            bulletRigidBody.AddForce(bulletDirection * modifiedBulletForce, ForceMode2D.Impulse);
        }

        ammoInMag--;
    }

    public virtual void AttackMelee()
    {
        SetAlerted(true);

        // �������� ������� ������� � ������� �����������
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //������� ���� � ������� �����������

        // ����������� ������� ���������� � Vector2
        Vector2 attackerPos = new Vector2(transform.position.x, transform.position.y);

        var direction = mousePos - attackerPos;

        var distance = 7f;

        // ��������� ������� � �������� ����������� � �� �������� �����
        RaycastHit2D hit = Physics2D.Raycast(attackerPos, direction, distance, LayerMask.GetMask("Enemies"));
        if (hit.collider != null)
        {
            // ���� ������� ���������� � �������� �� ���� "Enemies", �� ������������ ������������
            var enemy = hit.collider.GetComponent<TestEnemy>();
            var enemyStates = hit.collider.GetComponent<TestEnemyStates>();
            if (enemy != null)
            {
                if (enemyStates.isAlerted == false)
                    enemyStates.isAlerted = true;
                enemy.TakeDamage(20);
            }
        }

        // ���������� �������
        Debug.DrawRay(attackerPos, direction.normalized * distance, Color.red);
    }


    public void SetAlerted(bool value)
    {
        if (value)
        {
            // ��������� ���� ������� � ������� ����������
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