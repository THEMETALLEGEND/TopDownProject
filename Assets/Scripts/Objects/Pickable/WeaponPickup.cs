using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public PlayerInventory playerInventory;
    public WeaponClass weaponClass;
    public int inventoryPlace;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            PlayerInventory.weapons[inventoryPlace] = weaponClass.transform;
            Destroy(gameObject);

        }
    }
}
