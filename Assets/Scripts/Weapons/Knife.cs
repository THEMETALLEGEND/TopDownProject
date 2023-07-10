using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : WeaponClass
{
    private void Update()
    {
        ammoType.Refresh(); //�������. ���������� ���� �������� ������ ����.

        if (Input.GetButton("Fire1") && Time.time > nextFire && allowedShooting)
        {
            nextFire = Time.time + fireRate;
            AttackMelee();
        }
    }
}
