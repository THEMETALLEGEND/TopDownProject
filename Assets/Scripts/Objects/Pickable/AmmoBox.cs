using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : PickableClass
{
	public int ammoAvailable = 15; // кол-во патронов в €щике
	public AmmoType ammoType; // тип патронов в €щике

	private WeaponClass weaponClass; //ищем активное оружие в сцене

	public override void CollisionCheck(Collider2D other)
	{
		if (other.name == "Hitbox")
		{
			PlayerInventory playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();

			if (playerInventory != null)
			{
				AmmoContainer ammoContainer = WeaponClass.ammoContainer;
				weaponClass = FindObjectOfType<WeaponClass>();

				if (ammoContainer.ammoTypeValues[ammoType] < ammoContainer.maxAmmoTypeValues[ammoType]) // провер€ем, есть ли у игрока место дл€ патронов в инвентаре
				{
					int ammoToTake = Mathf.Min(ammoAvailable, ammoContainer.maxAmmoTypeValues[ammoType] - ammoContainer.ammoTypeValues[ammoType]); //выбираем наименьшее - сколько патронов нужно или сколько патронов осталось

					ammoContainer.ammoTypeValues[ammoType] += ammoToTake; // ƒобавл€ем патроны в инвентарь игрока
					ammoAvailable -= ammoToTake;
					weaponClass.ammoType.Refresh(); //при подборе патронов игроком обновл€ем пул патронов до актуального

					if (ammoAvailable <= 0) // ≈сли патроны закончились, удал€ем объект €щика
					{
						Destroy(gameObject);
					}
				}
			}
		}
	}
}