using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : PickableClass
{
    public int ammoAvailable = 15;
    
    public override void CollisionCheck(Collider2D other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

                                                                // Проверяем, есть ли у игрока место для патронов в инвентаре
        if (playerInventory != null && playerInventory.TestWeaponAmmo < playerInventory.TestWeaponMaxAmmo)
        {
                                        // Вычисляем, сколько патронов нужно взять из ящика
            int ammoNeeded = playerInventory.TestWeaponMaxAmmo - playerInventory.TestWeaponAmmo;
            int ammoToTake = Mathf.Min(ammoNeeded, ammoAvailable); //выбираем наименьшее - сколько патронов нужно или сколько патронов осталось

            playerInventory.TestWeaponAmmo += ammoToTake; // Добавляем патроны к инвентарю игрока
            ammoAvailable -= ammoToTake;

                                // Если патроны закончились, удаляем объект ящика
            if (ammoAvailable <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
