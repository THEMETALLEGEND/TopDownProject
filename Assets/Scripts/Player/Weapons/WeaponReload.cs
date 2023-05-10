using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponReload : MonoBehaviour
{
    public int maxAmmo = 40;
    int currentMaxAmmo;
    public int magCapacity = 7;
    [HideInInspector] public int currentAmmo;

    public float reloadSpeed = 1f;
    public float refillSpeed = 1.5f;

    public TextMeshProUGUI ammoText;

    [HideInInspector] public bool needReload;
    [HideInInspector] public bool refilling;
    Animator anim;
    private void Awake()
    {
        currentAmmo = magCapacity;
        currentMaxAmmo = maxAmmo;
        //anim = GetComponent<Animator>();
        needReload = false;
    }

    private void Update()
    {
        if (currentAmmo <= 0)
            needReload = true;

        if (currentMaxAmmo > 0 && currentAmmo < magCapacity)
        {
            if (Input.GetKeyDown(KeyCode.R))
                StartCoroutine(Reload());
        }

        ammoText.text = currentAmmo + "/" + currentMaxAmmo;
    }

    IEnumerator Reload()
    {
        Debug.Log("Reload");
        //anim.SetBool("IsReloading", true);
        yield return new WaitForSeconds(reloadSpeed);
        needReload = false;

        if ((magCapacity - currentAmmo) <= currentMaxAmmo)
        {
            currentMaxAmmo -= magCapacity - currentAmmo;
            currentAmmo += magCapacity - currentAmmo;
        }
        else
        {
            currentAmmo += currentMaxAmmo;
            currentMaxAmmo = 0;
        }

        //anim.SetBool("IsReloading", false);


    }

    public void RefillAmmo()
    {
        Debug.Log("Refilling Ammo");

        refilling = true;
        currentMaxAmmo = maxAmmo;
    }

    public void DecreaseAmmo()
    {
        currentAmmo--;
    }
}
