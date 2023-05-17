using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : PickableClass
{
    public int ammoAvailable = 15;
    
    public override void CollisionCheck(Collider2D other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

                                                                // ���������, ���� �� � ������ ����� ��� �������� � ���������
        if (playerInventory != null && playerInventory.TestWeaponAmmo < playerInventory.TestWeaponMaxAmmo)
        {
                                        // ���������, ������� �������� ����� ����� �� �����
            int ammoNeeded = playerInventory.TestWeaponMaxAmmo - playerInventory.TestWeaponAmmo;
            int ammoToTake = Mathf.Min(ammoNeeded, ammoAvailable); //�������� ���������� - ������� �������� ����� ��� ������� �������� ��������

            playerInventory.TestWeaponAmmo += ammoToTake; // ��������� ������� � ��������� ������
            ammoAvailable -= ammoToTake;

                                // ���� ������� �����������, ������� ������ �����
            if (ammoAvailable <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
