using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : WeaponClass
{
    private void Awake()
    {
        //���� � ��������� ������ ��� ����� ��� ���������, ��� �������� ��������� � ������� � ���������� �����
        playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();
        Debug.Log(playerInventory);
        if (playerInventory.hasRifle)
        {
            PlayerInventory.weapons[2] = transform;
        }
    }
    void Update()
    {
        ammoType.Refresh();

        MagazineReload();
    }
}
