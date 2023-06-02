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
    private List<Key> keys = new List<Key>(); //������ ������ � ���������


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
public class AmmoContainer
{
    public Dictionary<AmmoType, int> maxAmmoTypeValues = new Dictionary<AmmoType, int>();
    public Dictionary<AmmoType, int> ammoTypeValues = new Dictionary<AmmoType, int>();

    public AmmoContainer()
    {
        maxAmmoTypeValues.Add(AmmoType.Pistol, 120);
        maxAmmoTypeValues.Add(AmmoType.Rifle, 240);
        maxAmmoTypeValues.Add(AmmoType.Energy, 400);

        ammoTypeValues.Add(AmmoType.Pistol, 24);
        ammoTypeValues.Add(AmmoType.Rifle, 60);
        ammoTypeValues.Add(AmmoType.Energy, 100);
    }
}
