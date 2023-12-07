using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Hitbox" && other.transform.parent.name == "Player")
        {
            CharacterController2D characterController2D = other.GetComponent<CharacterController2D>();
            PlayerInventory playerInventory = GameObject.Find("Player Inventory").GetComponent<PlayerInventory>();
            characterController2D.currentHealth = playerInventory.currentHealth;


            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
