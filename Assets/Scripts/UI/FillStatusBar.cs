using UnityEngine;
using UnityEngine.UI;

public class FillStatusBar : MonoBehaviour
{
    private CharacterController2D playerController;  // Assuming the player's health is stored in PlayerInventory
    private Slider slider;

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
        {
            playerController = playerObject.GetComponent<CharacterController2D>();
        }
        else
        {
            Debug.Log("Player GameObject not found in the scene.");
        }
    }

    private void Update()
    {
        // Update only if playerInventory is null
        if (playerController == null)
        {
            FindPlayer();
        }
        else
        {
            // Calculate fill value based on player's health
            float fillValue = playerController.currentHealth / playerController.maxHealth;

            // Update the slider's value
            slider.value = fillValue;
        }
    }
}