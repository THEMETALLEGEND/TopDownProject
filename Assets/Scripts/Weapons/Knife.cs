using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : WeaponClass
{
    private void Update()
    {
        ammoType.Refresh(); //�������. ���������� ���� �������� ������ ����.

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            Debug.Log("knife");
        }
    }
}
