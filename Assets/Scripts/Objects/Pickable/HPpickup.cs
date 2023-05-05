using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPpickup : PickableClass
{
    public override void CollisionCheck(Collider2D other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        CharacterController2D characterController = other.GetComponent<CharacterController2D>();

        if (playerInventory != null)
        {
            playerInventory.StuffCollectedCount();
            characterController.currentHealth += 2f;
            Destroy(gameObject);
        }
    }
}
