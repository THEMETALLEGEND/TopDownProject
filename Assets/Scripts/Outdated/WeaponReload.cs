using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponReload : MonoBehaviour
{
    /*public int maxAmmo = 40;
    //int currentMaxAmmo;
    public int magCapacity = 7;
    [HideInInspector] public int currentAmmo;

    public float reloadSpeed = 1f;
    public float refillSpeed = 1.5f;

    //public TextMeshProUGUI ammoText;

    [HideInInspector] public bool needReload;
    [HideInInspector] public bool refilling;
    private GameObject player;
    private PlayerInventory playerInventory;
    //private int ammoInInventory;
    Animator anim;
    private void Awake()
    {
        player = GameObject.Find("Player");
        playerInventory = player.GetComponent<PlayerInventory>();
        currentAmmo = magCapacity;
        playerInventory.TestWeaponAmmo = maxAmmo;
        Debug.Log(playerInventory);
        //anim = GetComponent<Animator>();
        needReload = false;
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

    public void RefillAmmo()
    {
        Debug.Log("Refilling Ammo");

        refilling = true;
        playerInventory.TestWeaponAmmo = maxAmmo;
    }

    public void DecreaseAmmo()
    {
        currentAmmo--;
    }*/
}
