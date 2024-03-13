using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : PickableClass
{
	public int ammoAvailable = 15; // ���-�� �������� � �����
	public AmmoType ammoType; // ��� �������� � �����

	private WeaponClass weaponClass; //���� �������� ������ � �����

	public override void CollisionCheck(Collider2D other)
	{
		if (other.name == "Hitbox")
		{
			PlayerInventory playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();

			if (playerInventory != null)
			{
				AmmoContainer ammoContainer = WeaponClass.ammoContainer;
				weaponClass = FindObjectOfType<WeaponClass>();

				if (ammoContainer.ammoTypeValues[ammoType] < ammoContainer.maxAmmoTypeValues[ammoType]) // ���������, ���� �� � ������ ����� ��� �������� � ���������
				{
					int ammoToTake = Mathf.Min(ammoAvailable, ammoContainer.maxAmmoTypeValues[ammoType] - ammoContainer.ammoTypeValues[ammoType]); //�������� ���������� - ������� �������� ����� ��� ������� �������� ��������

					ammoContainer.ammoTypeValues[ammoType] += ammoToTake; // ��������� ������� � ��������� ������
					ammoAvailable -= ammoToTake;
					weaponClass.ammoType.Refresh(); //��� ������� �������� ������� ��������� ��� �������� �� �����������

					if (ammoAvailable <= 0) // ���� ������� �����������, ������� ������ �����
					{
						Destroy(gameObject);
					}
				}
			}
		}
	}
}