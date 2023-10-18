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
    private WeaponSwitch weaponSwitch;
    private PlayerInventory playerInventory;
    public WeaponClass weaponClass;

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        weaponSwitch = FindObjectOfType<WeaponSwitch>();
        weaponClass = GetWeaponObject(weaponType);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerInventory != null && other.CompareTag("Player"))
        {
            PlayerInventory.weapons[inventoryPlace] = weaponClass.transform;
            Destroy(gameObject);

            switch (weaponType)
            {
                case WeaponType.Pistol:
                    playerInventory.hasPistol = true;
                    break;
                case WeaponType.Rifle:
                    playerInventory.hasRifle = true;
                    break;
                case WeaponType.Shotgun:
                    playerInventory.hasShotgun = true;
                    break;
            }
        }
    }

    private WeaponClass GetWeaponObject(WeaponType type)
    {
        WeaponClass findWeaponClass;
        GameObject weaponObject = FindInactiveObjectByName(type.ToString());
        switch (weaponType)
        {
            case WeaponType.Pistol:
                findWeaponClass = weaponObject.GetComponent<Pistol>();
                break;
            case WeaponType.Rifle:
                findWeaponClass = weaponObject.GetComponent<Rifle>();
                break;
            case WeaponType.Shotgun:
                findWeaponClass = weaponObject.GetComponent<Shotgun>();
                break;
            default:
                findWeaponClass = null;
                break;
        }

        return findWeaponClass;

        static GameObject FindInactiveObjectByName(string name)
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