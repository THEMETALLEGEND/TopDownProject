using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
    public int AmmoCount { get; private set; }
    public int StuffCollected { get; private set; }



    //---------OBJECTS--------------
    private List<Key> keys = new List<Key>(); //список ключей в инвентаре


    //----------WEAPONS--------------
    public int TestWeaponAmmo;
    public int TestWeaponMaxAmmo = 100;
    public int pistolAmmo;
    public int pistolAmmoMax = 120;
    public int rifleAmmo;
    public int rifleAmmoMax = 240;
    public int energyAmmo;
    public int energyAmmoMax = 300;


    public UnityEvent<PlayerInventory> OnStuffCollected;

    public void StuffCollectedCount()
    {
        StuffCollected++;
        OnStuffCollected.Invoke(this);
    }

    public void AddKey(Key key)
    {
        keys.Add(key);
    }

    public bool HasKey(string keyId)
    {
        return keys.Any(key => key.keyId == keyId);
    }
}
