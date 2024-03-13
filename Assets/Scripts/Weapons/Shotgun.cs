using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponClass
{
    private void Awake()
    {
        //Если в инвентаре игрока эта пушка уже подобрана, при загрузке добавляем её обратно в Контроллер Пушек
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
