using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponClass
{
    // Update is called once per frame
    void Update()
    {
        ammoType.Refresh();

        ShotgunReload();
    }
}
