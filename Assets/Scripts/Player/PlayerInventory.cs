using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int AmmoCount { get; private set; }
    public int StuffCollected { get; private set; }

    public UnityEvent<PlayerInventory> OnStuffCollected;

    public void StuffCollectedCount()
    {
        StuffCollected++;
        OnStuffCollected.Invoke(this);
    }
}
