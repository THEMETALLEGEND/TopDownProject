using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableClass : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        CollisionCheck(other);
    }

    public virtual void CollisionCheck(Collider2D other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.StuffCollectedCount();
            Destroy(gameObject);
        }
    }
}
