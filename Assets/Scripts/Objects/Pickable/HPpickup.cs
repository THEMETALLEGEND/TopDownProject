using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPpickup : PickableClass
{
    public float healthAmount = 10f;
    public override void CollisionCheck(Collider2D other)
    {
        if (other.name == "Hitbox")
        {
            PlayerInventory playerInventory = other.transform.parent.GetComponent<PlayerInventory>();
            CharacterController2D characterController = other.transform.parent.GetComponent<CharacterController2D>();

            if (playerInventory != null)
            {
                characterController.currentHealth += healthAmount;
                Destroy(gameObject);
            }
        }
    }
}
