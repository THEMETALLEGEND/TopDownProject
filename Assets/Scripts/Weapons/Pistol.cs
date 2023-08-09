using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponClass
{
    private void Awake()
    {
        if (playerInventory.hasPistol)
        {
            //PlayerInventory.weapons[1] = weaponObject.GetComponent<Pistol>();
        }
    }
    private void Update()
    {
        ammoType.Refresh();

        MagazineReload();
    }
}
