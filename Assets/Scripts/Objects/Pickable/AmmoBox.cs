using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : PickableClass
{
    public int ammoAvailable = 15; // кол-во патронов в €щике
    public AmmoType ammoType; // тип патронов в €щике
    public override void CollisionCheck(Collider2D other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            AmmoContainer ammoContainer = WeaponClass.ammoContainer;

            if (ammoContainer.ammoTypeValues[ammoType] < ammoContainer.maxAmmoTypeValues[ammoType]) // провер€ем, есть ли у игрока место дл€ патронов в инвентаре
            {
                int ammoToTake = Mathf.Min(ammoAvailable, ammoContainer.maxAmmoTypeValues[ammoType] - ammoContainer.ammoTypeValues[ammoType]); //выбираем наименьшее - сколько патронов нужно или сколько патронов осталось

                ammoContainer.ammoTypeValues[ammoType] += ammoToTake; // ƒобавл€ем патроны в инвентарь игрока
                ammoAvailable -= ammoToTake;

                if (ammoAvailable <= 0) // ≈сли патроны закончились, удал€ем объект €щика
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}