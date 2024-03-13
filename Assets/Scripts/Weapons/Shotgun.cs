using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponClass
{
    private void Awake()
    {
        //���� � ��������� ������ ��� ����� ��� ���������, ��� �������� ��������� � ������� � ���������� �����
        playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();
        if (playerInventory.hasShotgun)
        {
            PlayerInventory.weapons[3] = transform;
        }
    }
    void Update()
    {
        ShotgunReload();
    }
}
