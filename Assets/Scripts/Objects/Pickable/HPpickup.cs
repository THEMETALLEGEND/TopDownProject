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
			CharacterController2D characterController = other.transform.parent.GetComponent<CharacterController2D>();

			if (characterController != null && characterController.currentHealth < 100f)
			{
				characterController.currentHealth += healthAmount;

				// Update the player's health in PlayerInventory
				characterController.UpdatePlayerHealth();

				Destroy(gameObject);
			}
		}
	}
}
