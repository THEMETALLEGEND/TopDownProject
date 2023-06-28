using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : WeaponClass
{
    void Update()
    {
        ammoType.Refresh();

        MagazineReload();
    }
}
