using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : WeaponClass
{
	private void Awake()
	{
		//Нож всегда подобран, при загрузке добавляем его обратно в Контроллер Пушек
		playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();
		PlayerInventory.weapons[0] = transform;
	}
	private void Update()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire && allowedShooting)
		{
			nextFire = Time.time + fireRate;
			AttackMelee();
		}
	}
}
