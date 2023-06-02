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


    public bool hasMagazine;
    public float reloadSpeed;
    [HideInInspector] public bool needReload; // ����� �� ������������ ������
    [HideInInspector] public bool refilling; // ���������� �� �����������

    // ���������� ��������� � ����������
    public float fireRate; // ����������������
    public GameObject bulletPrefab;
    public float bulletForce; // ���� ��������

    // ���������� ��������� � �������
    public PlayerInventory playerInventory;
    private GameObject player;
    private float alertRadius = 20f; // ������ ����������

    // ���������� ��������� � ��
    private Animator anim;
    private Transform firepoint; // ����� ��������






    public static AmmoContainer ammoContainer = new AmmoContainer(); // ����������� ���������� ��� �������� ������ �� ��������� ������ �mmoContainer
    public AmmoType m_type;

    PistolAmmo pistol = new PistolAmmo(ammoContainer);
    RifleAmmo rifle = new RifleAmmo(ammoContainer);
    EnergyAmmo energy = new EnergyAmmo(ammoContainer);





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
            default:
                ammoType = pistol;
                m_type = AmmoType.Pistol;
                break;
        }
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
        ammoType.Refresh();
        //anim.SetBool("IsReloading", true);
        yield return new WaitForSeconds(reloadSpeed);
        needReload = false;

        if ((magCapacity - ammoInMag) <= ammoType.currentAmmoOfType)
        {
            ammoContainer.ammoTypeValues[m_type] -= magCapacity - ammoInMag;
            ammoInMag += magCapacity - ammoInMag;
            ammoType.Refresh();
        }
        else
        {
            ammoInMag += ammoContainer.ammoTypeValues[m_type];
            ammoContainer.ammoTypeValues[m_type] = 0;
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
    Pistol,
    Rifle,
    Energy
}