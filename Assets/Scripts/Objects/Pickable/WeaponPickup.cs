using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public enum WeaponType
    {
        Pistol,
        Rifle,
        Shotgun
    }

    public WeaponType weaponType;
    public int inventoryPlace;
    private PlayerInventory playerInventory;
    private WeaponClass weaponClass;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();

        weaponClass = GetWeaponObject(weaponType);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerInventory != null)
        {
            Debug.Log("weaponclass not null");
            PlayerInventory.weapons[inventoryPlace] = weaponClass.transform;
            Destroy(gameObject);
        }
    }

    private WeaponClass GetWeaponObject(WeaponType type)
    {
        GameObject weaponObject = GameObject.Find(type.ToString());
        if (weaponObject != null)
        {
            return weaponObject.GetComponent<WeaponClass>();
        }
        else
        {
            Debug.LogError("GameObject named '" + type.ToString() + "' not found in the scene");
        }

        return null;
    }
}