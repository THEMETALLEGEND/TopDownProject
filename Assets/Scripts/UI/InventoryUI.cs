using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
	private TextMeshProUGUI stuffCollectedText;
	private PlayerInventory playerInventoryLocal;

	private void Start()
	{
		stuffCollectedText = GetComponent<TextMeshProUGUI>();
		playerInventoryLocal = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();

		// Register UpdateStuffCollectedText as a listener to OnStuffCollected event
		playerInventoryLocal.OnStuffCollected.AddListener(UpdateStuffCollectedText);

		// Update the text initially
		UpdateStuffCollectedText(playerInventoryLocal);
	}

	// This method will be automatically called when OnStuffCollected event is triggered
	public void UpdateStuffCollectedText(PlayerInventory playerInventory)
	{
		stuffCollectedText.text = playerInventory.StuffCollected.ToString();
	}
}