using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : WeaponClass
{
    private void Awake()
    {
        //Если в инвентаре игрока эта пушка уже подобрана, при загрузке добавляем её обратно в Контроллер Пушек
        playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();
        if (playerInventory.hasRifle)
        {
            PlayerInventory.weapons[2] = transform;
        }
    }
    void Update()
    {
        MagazineReload();
    }
}
