using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponClass
{
    private void Update()
    {
        ammoType.Refresh();

        MagazineReload();
    }
}
