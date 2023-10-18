using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponClass
{
    private void Awake()
    {
        //���� � ��������� ������ ��� ����� ��� ���������, ��� �������� ��������� � ������� � ���������� �����
        playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();
        if (playerInventory.hasPistol)
        {
            PlayerInventory.weapons[1] = transform;
        }
    }
    private void Update()
    {
        ammoType.Refresh();

        MagazineReload();
    }
}
