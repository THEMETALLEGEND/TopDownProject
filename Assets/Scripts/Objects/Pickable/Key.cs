using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public string keyId; // ID �����

    public bool CanUnlockDoor(Door door)
    {
        return door.keyId == keyId;
    }
}