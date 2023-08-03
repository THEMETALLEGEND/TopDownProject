using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : PickableClass
{
    public int ammoAvailable = 15; // ���-�� �������� � �����
    public AmmoType ammoType; // ��� �������� � �����
    public override void CollisionCheck(Collider2D other)
    {
        if (other.name == "Hitbox")
        {
            PlayerInventory playerInventory = other.transform.parent.GetComponent<PlayerInventory>();

            if (playerInventory != null)
            {
                AmmoContainer ammoContainer = WeaponClass.ammoContainer;

                if (ammoContainer.ammoTypeValues[ammoType] < ammoContainer.maxAmmoTypeValues[ammoType]) // ���������, ���� �� � ������ ����� ��� �������� � ���������
                {
                    int ammoToTake = Mathf.Min(ammoAvailable, ammoContainer.maxAmmoTypeValues[ammoType] - ammoContainer.ammoTypeValues[ammoType]); //�������� ���������� - ������� �������� ����� ��� ������� �������� ��������

                    ammoContainer.ammoTypeValues[ammoType] += ammoToTake; // ��������� ������� � ��������� ������
                    ammoAvailable -= ammoToTake;

                    if (ammoAvailable <= 0) // ���� ������� �����������, ������� ������ �����
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}