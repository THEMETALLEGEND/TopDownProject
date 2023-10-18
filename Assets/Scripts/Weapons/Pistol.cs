using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponClass
{
    private void Awake()
    {
        //Если в инвентаре игрока эта пушка уже подобрана, при загрузке добавляем её обратно в Контроллер Пушек
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
