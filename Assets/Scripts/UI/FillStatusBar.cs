using UnityEngine;
using UnityEngine.UI;

public class FillStatusBar : MonoBehaviour
{
	private CharacterController2D playerController;  // Assuming the player's health is stored in PlayerInventory
	private Slider slider;
	private float fillValue;

	private void Awake()
	{
		slider = GetComponent<Slider>();
	}

	private void Start()
	{
		// Find the PlayerInventory component
		FindPlayer();
	}

	private void FindPlayer()
	{
		// Look for the GameObject with the PlayerInventory script in the scene
		GameObject playerObject = GameObject.Find("Player");

		if (playerObject != null)
			playerController = playerObject.GetComponent<CharacterController2D>();
	}

	private void Update()
	{
		// Update only if playerInventory is null
		if (playerController == null)
		{
			fillValue = 0f;
		}
		else
		{
			// Calculate fill value based on player's health
			fillValue = playerController.currentHealth / playerController.maxHealth;
		}

		// Update the slider's value
		slider.value = fillValue;
	}
}