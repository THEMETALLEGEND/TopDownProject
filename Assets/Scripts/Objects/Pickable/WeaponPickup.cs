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
    public WeaponClass weaponClass;

    private void Start()
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
        WeaponClass findWeaponClass = null;
        GameObject weaponObject = FindInactiveObjectByName(type.ToString());
        findWeaponClass = weaponType switch
        {
            WeaponType.Pistol => weaponObject.GetComponent<Pistol>(),
            WeaponType.Rifle => weaponObject.GetComponent<Rifle>(),
            WeaponType.Shotgun => weaponObject.GetComponent<Shotgun>(),
            _ => null
        };

        return findWeaponClass;

        GameObject FindInactiveObjectByName(string name)
        {
            Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].hideFlags == HideFlags.None)
                {
                    if (objs[i].name == name)
                    {
                        return objs[i].gameObject;
                    }
                }
            }
            return null;
        }
    }
}