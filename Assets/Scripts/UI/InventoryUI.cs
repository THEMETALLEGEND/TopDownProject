using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI stuffCollectedText;

    private void Start()
    {
        stuffCollectedText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateStuffCollectedText(PlayerInventory playerInventory)
    {
        stuffCollectedText.text = playerInventory.StuffCollected.ToString();
        Debug.Log("shit");
    }

}
